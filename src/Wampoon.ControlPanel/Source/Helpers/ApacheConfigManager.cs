using System;
using System.IO;
using System.Text;
using Wampoon.ControlPanel.Enums;

namespace Wampoon.ControlPanel.Helpers
{
    public static class ApacheConfigManager
    {

        /// <summary>
        /// Reads the PORT_NUMBER define from httpd-wampoon-variables.conf file.
        /// </summary>
        /// <param name="variablesFilePath">Path to the httpd-wampoon-variables.conf file</param>
        /// <param name="logAction">Optional logging action</param>
        /// <returns>Port number from variables file, or default port if not found.</returns>
        public static int GetPortFromVariablesFile(string variablesFilePath, Action<string, LogType> logAction = null)
        {
            try
            {
                if (File.Exists(variablesFilePath))
                {
                    var lines = File.ReadAllLines(variablesFilePath);
                    foreach (var line in lines)
                    {
                        var trimmedLine = line.Trim();
                        if (trimmedLine.StartsWith("Define PORT_NUMBER"))
                        {
                            var parts = trimmedLine.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length >= 3 && int.TryParse(parts[2], out int port))
                            {
                                //logAction?.Invoke($"Read PORT_NUMBER from variables file: {port}", LogType.Info);
                                return port;
                            }
                        }
                    }
                }
                
                logAction?.Invoke($"PORT_NUMBER not found in variables file, using default: {AppConstants.Ports.APACHE_DEFAULT}", LogType.Warning);
                return AppConstants.Ports.APACHE_DEFAULT;
            }
            catch (Exception ex)
            {
                logAction?.Invoke($"Error reading PORT_NUMBER from variables file: {ex.Message}", LogType.Warning);
                return AppConstants.Ports.APACHE_DEFAULT;
            }
        }

        /// <summary>
        /// Updates the Apache configuration variables file with path definitions and port number.
        /// </summary>
        /// <param name="variablesFilePath">Path to the httpd-wampoon-variables.conf file</param>
        /// <param name="wampoonRootDir">Path for WAMPOON_ROOT_DIR define</param>
        /// <param name="portNumber">Port number for PORT_NUMBER define</param>
        /// <param name="logAction">Optional logging action</param>
        /// <returns>True if the configuration file was updated successfully, false otherwise.</returns>
        public static bool UpdateVariablesFile(string variablesFilePath, string wampoonRootDir, int portNumber, Action<string, LogType> logAction = null)
        {
            try
            {
                // Ensure the conf directory exists.
                var confDirectory = Path.GetDirectoryName(variablesFilePath);
                if (!Directory.Exists(confDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(confDirectory);
                    }
                    catch (IOException)
                    {
                        // Directory might have been created by another thread, check again.
                        if (!Directory.Exists(confDirectory))
                        {
                            throw;
                        }
                    }
                }

                // Read existing configuration or create new content.
                string[] lines = File.Exists(variablesFilePath) ? File.ReadAllLines(variablesFilePath) : new string[0];
                var configContent = new StringBuilder();
                
                bool wampoonRootDirUpdated = false;
                bool portNumberUpdated = false;
                
                // Convert Windows path to Unix-style path for Apache configuration
                var unixStylePath = wampoonRootDir.Replace('\\', '/');
                
                // Ensure required temp directories exist
                EnsureTempDirectoriesExist(wampoonRootDir, logAction);
                
                // Process existing lines and update defines if found.
                foreach (string line in lines)
                {
                    if (line.Trim().StartsWith("Define WAMPOON_ROOT_DIR"))
                    {
                        configContent.AppendLine(string.Format("Define WAMPOON_ROOT_DIR \"{0}\"", unixStylePath));
                        wampoonRootDirUpdated = true;
                    }
                    else if (line.Trim().StartsWith("Define PORT_NUMBER"))
                    {
                        configContent.AppendLine(string.Format("Define PORT_NUMBER {0}", portNumber));
                        portNumberUpdated = true;
                    }
                    else
                    {
                        configContent.AppendLine(line);
                    }
                }
                
                // If WAMPOON_ROOT_DIR wasn't found in existing file, add it.
                if (!wampoonRootDirUpdated)
                {
                    configContent.AppendLine(string.Format("Define WAMPOON_ROOT_DIR \"{0}\"", unixStylePath));
                }
                
                // If PORT_NUMBER wasn't found in existing file, add it.
                if (!portNumberUpdated)
                {
                    configContent.AppendLine(string.Format("Define PORT_NUMBER {0}", portNumber));
                    logAction?.Invoke($"Updated variables file with PORT_NUMBER: {portNumber}", LogType.Info);
                }

                // Write the configuration to the file.
                File.WriteAllText(variablesFilePath, configContent.ToString(), Encoding.UTF8);

                
                return true;
            }
            catch (Exception ex)
            {
                logAction?.Invoke($"Error updating Apache variables file: {ex.Message}", LogType.Error);
                return false;
            }
        }

        /// <summary>
        /// Updates only the PORT_NUMBER in the httpd-wampoon-variables.conf file.
        /// </summary>
        /// <param name="variablesFilePath">Path to the httpd-wampoon-variables.conf file</param>
        /// <param name="newPort">New port number to set</param>
        /// <param name="logAction">Optional logging action</param>
        /// <returns>True if update was successful, false otherwise</returns>
        public static bool UpdatePortInVariablesFile(string variablesFilePath, int newPort, Action<string, LogType> logAction = null)
        {
            if (newPort <= 0 || newPort > 65535)
            {
                logAction?.Invoke($"Invalid port number: {newPort}. Must be between 1 and 65535.", LogType.Error);
                return false;
            }

            try
            {
                if (!File.Exists(variablesFilePath))
                {
                    logAction?.Invoke($"Variables file not found: {variablesFilePath}", LogType.Error);
                    return false;
                }

                var lines = File.ReadAllLines(variablesFilePath);
                var modifiedLines = new StringBuilder();
                bool portUpdated = false;

                foreach (var line in lines)
                {
                    if (line.Trim().StartsWith("Define PORT_NUMBER"))
                    {
                        modifiedLines.AppendLine($"Define PORT_NUMBER {newPort}");
                        portUpdated = true;
                        logAction?.Invoke($"Updated PORT_NUMBER: {line.Trim()} → Define PORT_NUMBER {newPort}", LogType.Info);
                    }
                    else
                    {
                        modifiedLines.AppendLine(line);
                    }
                }

                if (!portUpdated)
                {
                    modifiedLines.AppendLine($"Define PORT_NUMBER {newPort}");
                    logAction?.Invoke($"Added new PORT_NUMBER define: {newPort}", LogType.Info);
                }

                File.WriteAllText(variablesFilePath, modifiedLines.ToString(), Encoding.UTF8);
                logAction?.Invoke($"Apache variables file updated successfully with port {newPort}", LogType.Info);
                
                return true;
            }
            catch (Exception ex)
            {
                logAction?.Invoke($"Error updating port in variables file: {ex.Message}", LogType.Error);
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
        /// Ensures that required temporary directories exist under {appBaseDir}/apps/temp.
        /// Creates the temp directory and subdirectories: apache_logs, mysql_logs, php_logs, pma_tmp, uploads, and sessions.
        /// </summary>
        /// <param name="appBaseDir">The application base directory path</param>
        /// <param name="logAction">Optional logging action</param>
        private static void EnsureTempDirectoriesExist(string appBaseDir, Action<string, LogType> logAction = null)
        {
            try
            {
                // Define the temp directory path.
                var tempDir = Path.Combine(appBaseDir, "apps", "temp");
                
                // Required subdirectories
                var requiredSubDirs = new[] { "apache_logs", "mariadb_logs", "php_logs", "pma_tmp", "uploads", "sessions" };

                // Create main temp directory if it doesn't exist.
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                    logAction?.Invoke($"Created temp directory: {tempDir}", LogType.Info);
                }
                
                // Create each required subdirectory.
                foreach (var subDir in requiredSubDirs)
                {
                    var subDirPath = Path.Combine(tempDir, subDir);
                    if (!Directory.Exists(subDirPath))
                    {
                        Directory.CreateDirectory(subDirPath);
                        logAction?.Invoke($"Created temp subdirectory: {subDirPath}", LogType.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                logAction?.Invoke($"Error creating temp directories: {ex.Message}", LogType.Warning);
            }
        }
    }
}