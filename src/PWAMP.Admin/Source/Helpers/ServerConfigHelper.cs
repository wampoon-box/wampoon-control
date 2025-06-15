using Frostybee.Pwamp.Models;
using Frostybee.PwampAdmin.Controllers;
using System;
using Frostybee.PwampAdmin.Helpers;
using static Frostybee.PwampAdmin.Helpers.ErrorLogHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.PwampAdmin.Helpers
{
    /// <summary>
    /// Generic helper methods for server configuration operations.
    /// </summary>
    public static class ServerConfigHelper
    {
        /// <summary>
        /// Opens a server's configuration file with proper error handling and user feedback.
        /// </summary>
        /// <param name="serverName">Name of the server (e.g., "Apache", "MariaDB")</param>
        /// <param name="showMessages">Whether to show message boxes for errors/warnings</param>
        /// <returns>True if the config file was successfully opened</returns>
        public static bool OpenServerConfigFile(string serverName, bool showMessages = true)
        {
            try
            {
                if (!ServerPathManager.CanOpenConfigFile(serverName))
                {
                    if (showMessages)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("{0} configuration file not found.", serverName), "Warning",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                    return false;
                }

                bool success = ServerPathManager.OpenConfigFile(serverName);
                if (!success && showMessages)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Failed to open {0} configuration file.", serverName), "Error",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                return success;
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                if (showMessages)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Error opening {0} configuration file: {1}", serverName, ex.Message), "Error",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                return false;
            }
        }

        /// <summary>
        /// Opens any file with proper error handling and user feedback.
        /// </summary>
        /// <param name="filePath">Path to the file to open</param>
        /// <param name="fileDescription">Description of the file for error messages</param>
        /// <param name="showMessages">Whether to show message boxes for errors/warnings</param>
        /// <returns>True if the file was successfully opened</returns>
        public static bool OpenFile(string filePath, string fileDescription = "file", bool showMessages = true)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    if (showMessages)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("The {0} was not found.", fileDescription), "Warning",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                    return false;
                }

                bool success = ServerPathManager.OpenFileInDefaultEditor(filePath);
                if (!success && showMessages)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Failed to open the {0}.", fileDescription), "Error",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                return success;
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                if (showMessages)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Error opening {0}: {1}", fileDescription, ex.Message), "Error",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                return false;
            }
        }

        /// <summary>
        /// Gets configuration file information for display purposes.
        /// </summary>
        /// <param name="serverName">Name of the server</param>
        /// <returns>Configuration file info or null if not available</returns>
        public static ServerConfigInfo GetConfigInfo(string serverName)
        {
            var configPath = ServerPathManager.GetConfigPath(serverName);
            if (string.IsNullOrEmpty(configPath))
                return null;

            return new ServerConfigInfo
            {
                ServerName = serverName,
                ConfigPath = configPath,
                Exists = File.Exists(configPath),
                Size = ServerPathManager.GetConfigFileSize(serverName),
                LastModified = ServerPathManager.GetConfigFileLastModified(serverName)
            };
        }
    }
}
