using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Wampoon.ControlPanel.Enums;

namespace Wampoon.ControlPanel.Helpers
{
    public static class ApacheConfigParser
    {
        private static readonly Regex ListenDirectiveRegex = new Regex(
            @"^\s*Listen\s+(?:(?<ip>\d+\.\d+\.\d+\.\d+):)?(?<port>\d+)(?:\s+ssl)?\s*(?:#.*)?$",
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
                // Log the exception but don't throw - return empty list as fallback
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
    }
}