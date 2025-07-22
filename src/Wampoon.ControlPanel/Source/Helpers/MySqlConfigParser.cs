using System;
using System.IO;
using System.Text.RegularExpressions;
using Wampoon.ControlPanel.Enums;

namespace Wampoon.ControlPanel.Helpers
{
    public static class MySqlConfigParser
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
                // Log the exception but don't throw - return default as fallback
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
    }
}