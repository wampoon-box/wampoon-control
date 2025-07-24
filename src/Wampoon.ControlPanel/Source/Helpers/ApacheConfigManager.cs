using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Wampoon.ControlPanel.Enums;

namespace Wampoon.ControlPanel.Helpers
{
    public static class ApacheConfigManager
    {
        private static readonly Regex ListenDirectiveRegex = new Regex(
            @"^\s*Listen\s+(?:(?<ip>\d+\.\d+\.\d+\.\d+):)?(?<port>\d+)(?:\s+ssl)?\s*(?:#.*)?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex VirtualHostRegex = new Regex(
            @"^\s*<VirtualHost\s+(?<host>[^:>]+):(?<port>\d+)\s*>\s*(?:#.*)?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static List<int> ParsePorts(string configFilePath, Action<string, LogType> logAction = null)
        {
            var ports = new List<int>();

            if (!File.Exists(configFilePath))
            {
                return ports;
            }

            try
            {
                var lines = File.ReadAllLines(configFilePath);
                
                foreach (var line in lines)
                {
                    var match = ListenDirectiveRegex.Match(line);
                    if (match.Success && int.TryParse(match.Groups["port"].Value, out int port))
                    {
                        if (!ports.Contains(port))
                        {
                            ports.Add(port);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception but don't throw - return empty list as fallback.
                var errorMsg = $"Error parsing Apache config: {ex.Message}";
                logAction?.Invoke(errorMsg, LogType.Error);
                System.Diagnostics.Debug.WriteLine(errorMsg);
            }

            return ports;
        }

        public static int GetPrimaryHttpPort(string configFilePath, Action<string, LogType> logAction = null)
        {
            var ports = ParsePorts(configFilePath, logAction);

            // Return the first HTTP port (typically 80 or 8080).
            // SSL ports (443, 8443) are usually secondary.
            var httpPort = ports.FirstOrDefault(p => p != 443 && p != 8443);
            
            return httpPort != 0 ? httpPort : AppConstants.Ports.APACHE_DEFAULT;
        }

        public static int GetPrimaryHttpsPort(string configFilePath, Action<string, LogType> logAction = null)
        {
            var ports = ParsePorts(configFilePath, logAction);

            // Look for common HTTPS ports.
            var httpsPort = ports.FirstOrDefault(p => p == 443 || p == 8443);
            
            return httpsPort != 0 ? httpsPort : AppConstants.Ports.HTTPS_DEFAULT;
        }

        public static bool IsValidConfigFile(string configFilePath)
        {
            if (!File.Exists(configFilePath))
                return false;

            try
            {
                // Basic validation - check if file contains Apache directives.
                var content = File.ReadAllText(configFilePath);
                return content.Contains("Listen") || 
                       content.Contains("ServerRoot") || 
                       content.Contains("DocumentRoot");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Updates virtual host port numbers in Apache virtual host configuration file.
        /// </summary>
        /// <param name="vhostConfigFilePath">Path to the virtual host config file</param>
        /// <param name="oldPort">Current port number to replace</param>
        /// <param name="newPort">New port number to set</param>
        /// <param name="logAction">Optional logging action</param>
        /// <returns>True if update was successful, false otherwise</returns>
        public static bool UpdateVirtualHostPorts(string vhostConfigFilePath, int oldPort, int newPort, Action<string, LogType> logAction = null)
        {
            if (!File.Exists(vhostConfigFilePath))
            {
                logAction?.Invoke($"Virtual host config file not found: {vhostConfigFilePath}", LogType.Warning);
                return true; // Not finding vhost file shouldn't fail the operation.
            }

            if (newPort <= 0 || newPort > 65535)
            {
                logAction?.Invoke($"Invalid port number: {newPort}. Must be between 1 and 65535.", LogType.Error);
                return false;
            }

            try
            {
                var lines = File.ReadAllLines(vhostConfigFilePath);
                var modifiedLines = new List<string>();
                bool foundVirtualHost = false;

                foreach (var line in lines)
                {
                    var match = VirtualHostRegex.Match(line);
                    if (match.Success)
                    {
                        var currentPort = int.Parse(match.Groups["port"].Value);
                        var host = match.Groups["host"].Value;

                        // Only update if it matches the old port we're replacing.
                        if (currentPort == oldPort)
                        {
                            var updatedLine = line.Replace($"{host}:{currentPort}", $"{host}:{newPort}");
                            modifiedLines.Add(updatedLine);
                            foundVirtualHost = true;
                            logAction?.Invoke($"Updated VirtualHost: {line.Trim()} → {updatedLine.Trim()}", LogType.Info);
                        }
                        else
                        {
                            // Keep other ports unchanged.
                            modifiedLines.Add(line);
                        }
                    }
                    else
                    {
                        modifiedLines.Add(line);
                    }
                }

                if (!foundVirtualHost)
                {
                    logAction?.Invoke($"No virtual hosts found with port {oldPort} in {Path.GetFileName(vhostConfigFilePath)}", LogType.Info);
                    return true; // Not finding matching virtual hosts is not an error
                }

                // Write the modified configuration.
                File.WriteAllLines(vhostConfigFilePath, modifiedLines, Encoding.UTF8);
                logAction?.Invoke($"Virtual host configuration updated successfully", LogType.Info);
                
                return true;
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error updating virtual host config: {ex.Message}";
                logAction?.Invoke(errorMsg, LogType.Error);
                System.Diagnostics.Debug.WriteLine(errorMsg);
                return false;
            }
        }

        /// <summary>
        /// Updates both the main Apache configuration and virtual host configurations atomically.
        /// </summary>
        /// <param name="mainConfigPath">Path to the main Apache config file (httpd.conf)</param>
        /// <param name="vhostConfigPath">Path to the virtual host config file</param>
        /// <param name="newPort">New port number to set</param>
        /// <param name="logAction">Optional logging action</param>
        /// <returns>True if update was successful, false otherwise</returns>
        public static bool UpdatePortWithVirtualHosts(string mainConfigPath, string vhostConfigPath, int newPort, Action<string, LogType> logAction = null)
        {
            string mainBackupPath = null;
            string vhostBackupPath = null;

            try
            {
                // Get current port for virtual host updates.
                var currentPort = GetPrimaryHttpPort(mainConfigPath, logAction);
                
                logAction?.Invoke($"Starting atomic port update: {currentPort} → {newPort}", LogType.Info);

                // Create backups of both files.
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                
                if (File.Exists(mainConfigPath))
                {
                    mainBackupPath = mainConfigPath + ".backup." + timestamp;
                    File.Copy(mainConfigPath, mainBackupPath);
                    logAction?.Invoke($"Created main config backup: {Path.GetFileName(mainBackupPath)}", LogType.Info);
                }

                if (File.Exists(vhostConfigPath))
                {
                    vhostBackupPath = vhostConfigPath + ".backup." + timestamp;
                    File.Copy(vhostConfigPath, vhostBackupPath);
                    logAction?.Invoke($"Created vhost config backup: {Path.GetFileName(vhostBackupPath)}", LogType.Info);
                }

                // Update main configuration
                bool mainConfigUpdated = UpdatePortInternal(mainConfigPath, newPort, logAction);
                if (!mainConfigUpdated)
                {
                    logAction?.Invoke("Failed to update main Apache configuration", LogType.Error);
                    RollbackFiles(mainBackupPath, vhostBackupPath, mainConfigPath, vhostConfigPath, logAction);
                    return false;
                }

                // Update virtual host configuration
                bool vhostConfigUpdated = UpdateVirtualHostPorts(vhostConfigPath, currentPort, newPort, logAction);
                if (!vhostConfigUpdated)
                {
                    logAction?.Invoke("Failed to update virtual host configuration", LogType.Error);
                    RollbackFiles(mainBackupPath, vhostBackupPath, mainConfigPath, vhostConfigPath, logAction);
                    return false;
                }

                logAction?.Invoke("=== Atomic port update completed successfully ===", LogType.Info);
                return true;
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error during atomic port update: {ex.Message}";
                logAction?.Invoke(errorMsg, LogType.Error);
                RollbackFiles(mainBackupPath, vhostBackupPath, mainConfigPath, vhostConfigPath, logAction);
                return false;
            }
        }

        /// <summary>
        /// Rolls back both configuration files to their backup versions.
        /// </summary>
        private static void RollbackFiles(string mainBackupPath, string vhostBackupPath, string mainConfigPath, string vhostConfigPath, Action<string, LogType> logAction)
        {
            try
            {
                if (!string.IsNullOrEmpty(mainBackupPath) && File.Exists(mainBackupPath))
                {
                    File.Copy(mainBackupPath, mainConfigPath, true);
                    logAction?.Invoke("Restored main config from backup", LogType.Warning);
                }

                if (!string.IsNullOrEmpty(vhostBackupPath) && File.Exists(vhostBackupPath))
                {
                    File.Copy(vhostBackupPath, vhostConfigPath, true);
                    logAction?.Invoke("Restored virtual host config from backup", LogType.Warning);
                }

                logAction?.Invoke("Configuration rollback completed", LogType.Warning);
            }
            catch (Exception rollbackEx)
            {
                logAction?.Invoke($"Error during rollback: {rollbackEx.Message}", LogType.Error);
            }
        }

        /// <summary>
        /// Internal method for updating main Apache configuration without backup creation.
        /// Used by UpdatePortWithVirtualHosts for atomic operations.
        /// </summary>
        private static bool UpdatePortInternal(string configFilePath, int newPort, Action<string, LogType> logAction)
        {
            if (!File.Exists(configFilePath))
            {
                logAction?.Invoke($"Apache config file not found: {configFilePath}", LogType.Error);
                return false;
            }

            try
            {
                // Read all lines from the config file.
                var lines = File.ReadAllLines(configFilePath);
                var modifiedLines = new List<string>();
                bool foundHttpListen = false;

                foreach (var line in lines)
                {
                    var match = ListenDirectiveRegex.Match(line);
                    if (match.Success)
                    {
                        var currentPort = int.Parse(match.Groups["port"].Value);

                        // Only update non-SSL ports (not 443, 8443).
                        if (currentPort != 443 && currentPort != 8443)
                        {
                            // Replace the port in the Listen directive.
                            var updatedLine = line;
                            if (match.Groups["ip"].Success)
                            {
                                // Has IP address: Listen 192.168.1.1:80.
                                updatedLine = line.Replace($":{currentPort}", $":{newPort}");
                            }
                            else
                            {
                                // No IP address: Listen 80.
                                updatedLine = line.Replace(currentPort.ToString(), newPort.ToString());
                            }
                            
                            modifiedLines.Add(updatedLine);
                            foundHttpListen = true;
                            logAction?.Invoke($"Updated Listen directive: {line.Trim()} → {updatedLine.Trim()}", LogType.Info);
                        }
                        else
                        {
                            // Keep SSL ports unchanged.
                            modifiedLines.Add(line);
                        }
                    }
                    else
                    {
                        modifiedLines.Add(line);
                    }
                }

                // If no HTTP Listen directive was found, add one.
                if (!foundHttpListen)
                {
                    // Find a good place to insert the Listen directive (after ServerRoot or at the beginning)
                    var insertIndex = 0;
                    for (int i = 0; i < modifiedLines.Count; i++)
                    {
                        if (modifiedLines[i].TrimStart().StartsWith("ServerRoot", StringComparison.OrdinalIgnoreCase))
                        {
                            insertIndex = i + 1;
                            break;
                        }
                    }
                    
                    modifiedLines.Insert(insertIndex, $"Listen {newPort}");
                    logAction?.Invoke($"Added new Listen directive: Listen {newPort}", LogType.Info);
                }

                // Write the modified configuration
                File.WriteAllLines(configFilePath, modifiedLines, Encoding.UTF8);
                
                return true;
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error updating Apache config: {ex.Message}";
                logAction?.Invoke(errorMsg, LogType.Error);
                System.Diagnostics.Debug.WriteLine(errorMsg);
                return false;
            }
        }

        /// <summary>
        /// Updates the primary HTTP port in Apache configuration file.
        /// </summary>
        /// <param name="configFilePath">Path to the Apache config file</param>
        /// <param name="newPort">New port number to set</param>
        /// <param name="logAction">Optional logging action</param>
        /// <returns>True if update was successful, false otherwise</returns>
        public static bool UpdatePort(string configFilePath, int newPort, Action<string, LogType> logAction = null)
        {
            if (!File.Exists(configFilePath))
            {
                logAction?.Invoke($"Apache config file not found: {configFilePath}", LogType.Error);
                return false;
            }

            if (newPort <= 0 || newPort > 65535)
            {
                logAction?.Invoke($"Invalid port number: {newPort}. Must be between 1 and 65535.", LogType.Error);
                return false;
            }

            try
            {
                // Read all lines from the config file.
                var lines = File.ReadAllLines(configFilePath);
                var modifiedLines = new List<string>();
                bool foundHttpListen = false;

                foreach (var line in lines)
                {
                    var match = ListenDirectiveRegex.Match(line);
                    if (match.Success)
                    {
                        var currentPort = int.Parse(match.Groups["port"].Value);
                        
                        // Only update non-SSL ports (not 443, 8443)
                        if (currentPort != 443 && currentPort != 8443)
                        {
                            // Replace the port in the Listen directive.
                            var updatedLine = line;
                            if (match.Groups["ip"].Success)
                            {
                                // Has IP address: Listen 192.168.1.1:80
                                updatedLine = line.Replace($":{currentPort}", $":{newPort}");
                            }
                            else
                            {
                                // No IP address: Listen 80
                                updatedLine = line.Replace(currentPort.ToString(), newPort.ToString());
                            }
                            
                            modifiedLines.Add(updatedLine);
                            foundHttpListen = true;
                            logAction?.Invoke($"Updated Listen directive: {line.Trim()} → {updatedLine.Trim()}", LogType.Info);
                        }
                        else
                        {
                            // Keep SSL ports unchanged.
                            modifiedLines.Add(line);
                        }
                    }
                    else
                    {
                        modifiedLines.Add(line);
                    }
                }

                // If no HTTP Listen directive was found, add one.
                if (!foundHttpListen)
                {
                    // Find a good place to insert the Listen directive (after ServerRoot or at the beginning)
                    var insertIndex = 0;
                    for (int i = 0; i < modifiedLines.Count; i++)
                    {
                        if (modifiedLines[i].TrimStart().StartsWith("ServerRoot", StringComparison.OrdinalIgnoreCase))
                        {
                            insertIndex = i + 1;
                            break;
                        }
                    }
                    
                    modifiedLines.Insert(insertIndex, $"Listen {newPort}");
                    logAction?.Invoke($"Added new Listen directive: Listen {newPort}", LogType.Info);
                }

                // Create backup of original file.
                var backupPath = configFilePath + ".backup." + DateTime.Now.ToString("yyyyMMddHHmmss");
                File.Copy(configFilePath, backupPath);
                logAction?.Invoke($"Created backup: {Path.GetFileName(backupPath)}", LogType.Info);

                // Write the modified configuration.
                File.WriteAllLines(configFilePath, modifiedLines, Encoding.UTF8);
                logAction?.Invoke($"Apache configuration updated successfully", LogType.Info);
                
                return true;
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error updating Apache config: {ex.Message}";
                logAction?.Invoke(errorMsg, LogType.Error);
                System.Diagnostics.Debug.WriteLine(errorMsg);
                return false;
            }
        }

        /// <summary>
        /// Validates if a port number is available and suitable for Apache.
        /// </summary>
        /// <param name="port">Port number to validate</param>
        /// <param name="logAction">Optional logging action</param>
        /// <param name="currentPort">Current Apache port to exclude from in-use check</param>
        /// <returns>True if port is valid and available, false otherwise</returns>
        public static bool ValidatePort(int port, Action<string, LogType> logAction = null, int currentPort = 0)
        {
            // Check port range.
            if (port <= 0 || port > 65535)
            {
                logAction?.Invoke($"Port {port} is out of valid range (1-65535)", LogType.Error);
                return false;
            }

            // Check if port is in commonly reserved range.
            if (port < 1024 && port != 80 && port != 443)
            {
                logAction?.Invoke($"Port {port} is in reserved range. Consider using ports 1024 or higher.", LogType.Warning);
            }

            // Check if port is available using existing NetworkPortHelper.
            // Skip the check if this is the current port for this service.
            if (port != currentPort)
            {
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
            }
            else
            {
                logAction?.Invoke($"Port {port} is the current Apache port", LogType.Info);
            }

            logAction?.Invoke($"Port {port} is available", LogType.Info);
            return true;
        }

        /// <summary>
        /// Validates if a virtual host configuration file exists and contains valid VirtualHost blocks.
        /// </summary>
        /// <param name="vhostConfigFilePath">Path to the virtual host config file</param>
        /// <returns>True if file exists and contains virtual hosts, false otherwise</returns>
        public static bool IsValidVirtualHostConfig(string vhostConfigFilePath)
        {
            if (!File.Exists(vhostConfigFilePath))
                return false;

            try
            {
                // Basic validation - check if file contains VirtualHost directives.
                var content = File.ReadAllText(vhostConfigFilePath);
                return content.Contains("<VirtualHost") && content.Contains("</VirtualHost>");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the virtual host configuration file path for the Apache installation.
        /// </summary>
        /// <param name="apacheBaseDirectory">Base directory of Apache installation</param>
        /// <returns>Path to the virtual host config file</returns>
        public static string GetVirtualHostConfigPath(string apacheBaseDirectory)
        {
            return Path.Combine(apacheBaseDirectory, "conf", "extra", "wampoon-vhosts.conf");
        }
    }
}