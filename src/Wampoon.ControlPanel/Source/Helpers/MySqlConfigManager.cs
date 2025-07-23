using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Wampoon.ControlPanel.Enums;

namespace Wampoon.ControlPanel.Helpers
{
    public static class MySqlConfigManager
    {
        private static readonly Regex PortRegex = new Regex(
            @"^\s*port\s*=\s*(?<port>\d+)\s*(?:#.*)?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex SectionRegex = new Regex(
            @"^\s*\[(?<section>[^\]]+)\]\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static int ParsePort(string configFilePath, Action<string, LogType> logAction = null)
        {
            if (!File.Exists(configFilePath))
            {
                return AppConstants.Ports.MYSQL_DEFAULT;
            }

            try
            {
                var lines = File.ReadAllLines(configFilePath);
                string currentSection = null;
                
                foreach (var line in lines)
                {
                    // Check for section headers.
                    var sectionMatch = SectionRegex.Match(line);
                    if (sectionMatch.Success)
                    {
                        currentSection = sectionMatch.Groups["section"].Value.ToLower();
                        continue;
                    }

                    // Look for port setting in relevant sections.
                    if (IsRelevantSection(currentSection))
                    {
                        var portMatch = PortRegex.Match(line);
                        if (portMatch.Success && int.TryParse(portMatch.Groups["port"].Value, out int port))
                        {
                            // Validate port range.
                            if (port > 0 && port <= 65535)
                            {
                                return port;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception but don't throw - return default as fallback.
                var errorMsg = $"Error parsing MySQL config: {ex.Message}";
                logAction?.Invoke(errorMsg, LogType.Error);
                System.Diagnostics.Debug.WriteLine(errorMsg);
            }

            return AppConstants.Ports.MYSQL_DEFAULT;
        }

        public static string ParseBindAddress(string configFilePath, Action<string, LogType> logAction = null)
        {
            var bindAddressRegex = new Regex(
                @"^\s*bind-address\s*=\s*(?<address>[^\s#]+)\s*(?:#.*)?$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (!File.Exists(configFilePath))
            {
                return "127.0.0.1";
            }

            try
            {
                var lines = File.ReadAllLines(configFilePath);
                string currentSection = null;
                
                foreach (var line in lines)
                {
                    var sectionMatch = SectionRegex.Match(line);
                    if (sectionMatch.Success)
                    {
                        currentSection = sectionMatch.Groups["section"].Value.ToLower();
                        continue;
                    }

                    if (IsRelevantSection(currentSection))
                    {
                        var bindMatch = bindAddressRegex.Match(line);
                        if (bindMatch.Success)
                        {
                            return bindMatch.Groups["address"].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error parsing MySQL bind address: {ex.Message}";
                logAction?.Invoke(errorMsg, LogType.Error);
                System.Diagnostics.Debug.WriteLine(errorMsg);
            }

            return "127.0.0.1";
        }

        public static bool IsValidConfigFile(string configFilePath)
        {
            if (!File.Exists(configFilePath))
                return false;

            try
            {
                // Basic validation - check if file contains MySQL/MariaDB directives.
                var content = File.ReadAllText(configFilePath);
                return content.Contains("[mysqld]") || 
                       content.Contains("[mariadb]") || 
                       content.Contains("port=") ||
                       content.Contains("datadir");
            }
            catch
            {
                return false;
            }
        }

        private static bool IsRelevantSection(string section)
        {
            if (string.IsNullOrEmpty(section))
                return false;

            // Check for sections that contain server configuration.
            return section == "mysqld" || 
                   section == "mariadb" || 
                   section == "server" ||
                   section == "mysql";
        }

        /// <summary>
        /// Updates the port number in MySQL configuration file.
        /// </summary>
        /// <param name="configFilePath">Path to the MySQL config file</param>
        /// <param name="newPort">New port number to set</param>
        /// <param name="logAction">Optional logging action</param>
        /// <returns>True if update was successful, false otherwise</returns>
        public static bool UpdatePort(string configFilePath, int newPort, Action<string, LogType> logAction = null)
        {
            if (!File.Exists(configFilePath))
            {
                logAction?.Invoke($"MySQL config file not found: {configFilePath}", LogType.Error);
                return false;
            }

            if (newPort <= 0 || newPort > 65535)
            {
                logAction?.Invoke($"Invalid port number: {newPort}. Must be between 1 and 65535.", LogType.Error);
                return false;
            }

            try
            {
                var lines = File.ReadAllLines(configFilePath);
                var modifiedLines = new List<string>();
                string currentSection = null;
                bool foundPortSetting = false;
                string targetSection = null;

                // First pass: find existing port setting and update it.
                foreach (var line in lines)
                {
                    var sectionMatch = SectionRegex.Match(line);
                    if (sectionMatch.Success)
                    {
                        currentSection = sectionMatch.Groups["section"].Value.ToLower();
                        modifiedLines.Add(line);
                        continue;
                    }

                    if (IsRelevantSection(currentSection))
                    {
                        var portMatch = PortRegex.Match(line);
                        if (portMatch.Success)
                        {
                            // Update existing port setting
                            var updatedLine = PortRegex.Replace(line, $"port = {newPort}");
                            modifiedLines.Add(updatedLine);
                            foundPortSetting = true;
                            targetSection = currentSection;
                            logAction?.Invoke($"Updated port setting: {line.Trim()} → {updatedLine.Trim()}", LogType.Info);
                            continue;
                        }
                    }

                    modifiedLines.Add(line);
                }

                // If no port setting was found, add one to the [mysqld] section.
                if (!foundPortSetting)
                {
                    var insertIndex = -1;
                    var mysqldSectionIndex = -1;

                    // Find [mysqld] section.
                    for (int i = 0; i < modifiedLines.Count; i++)
                    {
                        var sectionMatch = SectionRegex.Match(modifiedLines[i]);
                        if (sectionMatch.Success && sectionMatch.Groups["section"].Value.ToLower() == "mysqld")
                        {
                            mysqldSectionIndex = i;
                            // Find the end of this section or end of file.
                            for (int j = i + 1; j < modifiedLines.Count; j++)
                            {
                                if (SectionRegex.IsMatch(modifiedLines[j]))
                                {
                                    insertIndex = j;
                                    break;
                                }
                            }
                            if (insertIndex == -1)
                            {
                                insertIndex = modifiedLines.Count;
                            }
                            break;
                        }
                    }

                    // If [mysqld] section not found, create it.
                    if (mysqldSectionIndex == -1)
                    {
                        modifiedLines.Add("");
                        modifiedLines.Add("[mysqld]");
                        modifiedLines.Add($"port = {newPort}");
                        logAction?.Invoke($"Added new [mysqld] section with port = {newPort}", LogType.Info);
                    }
                    else
                    {
                        // Add port setting to existing [mysqld] section.
                        modifiedLines.Insert(insertIndex, $"port = {newPort}");
                        logAction?.Invoke($"Added port setting to [mysqld] section: port = {newPort}", LogType.Info);
                    }
                    
                    foundPortSetting = true;
                }

                if (foundPortSetting)
                {
                    // Create backup of original file
                    var backupPath = configFilePath + ".backup." + DateTime.Now.ToString("yyyyMMddHHmmss");
                    File.Copy(configFilePath, backupPath);
                    logAction?.Invoke($"Created backup: {Path.GetFileName(backupPath)}", LogType.Info);

                    // Write the modified configuration
                    File.WriteAllLines(configFilePath, modifiedLines, Encoding.UTF8);
                    logAction?.Invoke($"MySQL configuration updated successfully", LogType.Info);
                    
                    return true;
                }
                else
                {
                    logAction?.Invoke("No suitable section found to add port setting", LogType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error updating MySQL config: {ex.Message}";
                logAction?.Invoke(errorMsg, LogType.Error);
                System.Diagnostics.Debug.WriteLine(errorMsg);
                return false;
            }
        }

        /// <summary>
        /// Validates if a port number is available and suitable for MySQL.
        /// </summary>
        /// <param name="port">Port number to validate</param>
        /// <param name="logAction">Optional logging action</param>
        /// <returns>True if port is valid and available, false otherwise</returns>
        public static bool ValidatePort(int port, Action<string, LogType> logAction = null)
        {
            // Check port range
            if (port <= 0 || port > 65535)
            {
                logAction?.Invoke($"Port {port} is out of valid range (1-65535)", LogType.Error);
                return false;
            }

            // Check if port is in commonly reserved range
            if (port < 1024 && port != 3306)
            {
                logAction?.Invoke($"Port {port} is in reserved range. Consider using ports 1024 or higher.", LogType.Warning);
            }

            // Check if port is available using existing NetworkPortHelper.
            try
            {
                if (NetworkPortHelper.IsPortInUse(port))
                {
                    logAction?.Invoke($"Port {port} is already in use by another application", LogType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logAction?.Invoke($"Error checking port availability: {ex.Message}", LogType.Warning);
                // Continue validation even if port check fails.
            }

            logAction?.Invoke($"Port {port} is available", LogType.Info);
            return true;
        }
    }
}