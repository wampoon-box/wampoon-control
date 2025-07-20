using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wampoon.ControlPanel.Controllers;
using Wampoon.ControlPanel.Enums;
using Wampoon.ControlPanel.Helpers;
using static Wampoon.ControlPanel.Helpers.ErrorLogHelper;

namespace Wampoon.ControlPanel.Helpers
{
    /// <summary>
    /// Helper class for managing PHP configuration file settings.
    /// </summary>
    public static class PhpConfigurationHelper
    {
        /// <summary>
        /// Updates PHP ini settings including extension_dir, curl.cainfo, browscap, and session.save_path to use absolute paths.
        /// </summary>
        /// <param name="logAction">Optional action to log messages during the process</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        public static bool UpdatePhpIniSettings(Action<string, LogType> logAction = null)
        {
            try
            {
                logAction?.Invoke("Updating php.ini settings...", LogType.Info);
                
                // Get the php.ini file path.
                var phpBaseDir = ServerPathManager.GetServerBaseDirectory(PackageType.PHP.ToServerName());
                var phpIniPath = Path.Combine(phpBaseDir, "php.ini");
                
                // Check if php.ini exists
                if (!File.Exists(phpIniPath))
                {
                    logAction?.Invoke($"PHP ini file not found: {phpIniPath}", LogType.Warning);
                    return false;
                }
                
                // Get the PHP ext directory path.
                var phpExtDir = Path.Combine(phpBaseDir, "ext").Replace('\\', '/');
                
                // Get the absolute path to curl-ca-bundle.crt in the Apache bin directory.
                var apacheBaseDir = ServerPathManager.GetServerBaseDirectory(PackageType.Apache.ToServerName());
                var curlCaBundlePath = Path.Combine(apacheBaseDir, "bin", "curl-ca-bundle.crt").Replace('\\', '/');
                
                // Get the absolute path to browscap.ini in the PHP extras directory.
                var browscapPath = Path.Combine(phpBaseDir, "extras", "browscap.ini").Replace('\\', '/');
                var browscapExists = File.Exists(browscapPath.Replace('/', '\\'));
                
                // Get the absolute path to sessions directory.
                var sessionsPath = Path.Combine(phpBaseDir, "sessions").Replace('\\', '/');
                
                // Read the current php.ini content.
                var phpIniContent = File.ReadAllText(phpIniPath);
                
                // Update all settings in a single pass.
                var lines = phpIniContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                bool extensionDirUpdated = false;
                bool curlCaInfoUpdated = false;
                bool browscapUpdated = false;
                bool sessionSavePathUpdated = false;
                
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i].Trim();
                    
                    // Check if this line contains extension_dir setting (commented or uncommented).
                    if (!extensionDirUpdated && (line.StartsWith("extension_dir", StringComparison.OrdinalIgnoreCase) || 
                        line.StartsWith(";extension_dir", StringComparison.OrdinalIgnoreCase)))
                    {
                        lines[i] = $"extension_dir = \"{phpExtDir}\"";
                        extensionDirUpdated = true;
                    }
                    // Check if this line contains curl.cainfo setting (commented or uncommented).
                    else if (!curlCaInfoUpdated && (line.StartsWith("curl.cainfo", StringComparison.OrdinalIgnoreCase) || 
                        line.StartsWith(";curl.cainfo", StringComparison.OrdinalIgnoreCase)))
                    {
                        lines[i] = $"curl.cainfo = \"{curlCaBundlePath}\"";
                        curlCaInfoUpdated = true;
                    }
                    // Check if this line contains browscap setting (commented or uncommented).
                    else if (!browscapUpdated && browscapExists && (line.StartsWith("browscap", StringComparison.OrdinalIgnoreCase) || 
                        line.StartsWith(";browscap", StringComparison.OrdinalIgnoreCase)))
                    {
                        lines[i] = $"browscap = \"{browscapPath}\"";
                        browscapUpdated = true;
                    }
                    // Check if this line contains session.save_path setting (commented or uncommented).
                    else if (!sessionSavePathUpdated && (line.StartsWith("session.save_path", StringComparison.OrdinalIgnoreCase) || 
                        line.StartsWith(";session.save_path", StringComparison.OrdinalIgnoreCase)))
                    {
                        lines[i] = $"session.save_path = \"{sessionsPath}\"";
                        sessionSavePathUpdated = true;
                    }
                }
                
                // Add any missing settings at the end.
                var additionalLines = new List<string>();
                if (!extensionDirUpdated)
                {
                    additionalLines.Add($"extension_dir = \"{phpExtDir}\"");
                }
                if (!curlCaInfoUpdated)
                {
                    additionalLines.Add($"curl.cainfo = \"{curlCaBundlePath}\"");
                }
                if (!browscapUpdated && browscapExists)
                {
                    additionalLines.Add($"browscap = \"{browscapPath}\"");
                }
                if (!sessionSavePathUpdated)
                {
                    additionalLines.Add($"session.save_path = \"{sessionsPath}\"");
                }
                
                // Write the updated content.
                var updatedContent = string.Join(Environment.NewLine, lines);
                if (additionalLines.Count > 0)
                {
                    updatedContent += Environment.NewLine + string.Join(Environment.NewLine, additionalLines);
                }
                
                File.WriteAllText(phpIniPath, updatedContent);
                
                logAction?.Invoke($"✓ Updated php.ini extension_dir to: {phpExtDir}", LogType.Info);
                logAction?.Invoke($"✓ Updated php.ini curl.cainfo to: {curlCaBundlePath}", LogType.Info);
                logAction?.Invoke($"✓ Updated php.ini session.save_path to: {sessionsPath}", LogType.Info);
                if (browscapExists)
                {
                    logAction?.Invoke($"✓ Updated php.ini browscap to: {browscapPath}", LogType.Info);
                }
                else
                {
                    logAction?.Invoke($"⚠ Skipped browscap setting - file not found: {browscapPath}", LogType.Warning);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                logAction?.Invoke($"✗ Failed to update php.ini settings: {ex.Message}", LogType.Error);
                return false;
            }
        }
    }
}