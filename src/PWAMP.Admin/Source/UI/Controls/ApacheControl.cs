using Frostybee.Pwamp.Models;
using Frostybee.PwampAdmin.Controllers;
using Frostybee.PwampAdmin.Enums;
using Frostybee.PwampAdmin.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Frostybee.PwampAdmin.MainForm;

namespace Frostybee.PwampAdmin.Controls
{
    internal partial class ApacheControl : ServerControlBase, IDisposable
    {
        private ApacheManager _apacheManager;
        // Apache and phpMyAdmin-related paths.
        private string _currentDirectory;
        private string _apacheDirectory;
        private string _documentRoot;
        private string _customConfigPath;
        private string _phpMyAdminDirectory;
        private string _httpdAliasConfigPath;
        
        public ApacheControl()
        {
            ServiceName = "Apache";
            DisplayName = "Apache HTTP Server";
            PortNumber = 80; // Default HTTP port, change if needed.
            lblServerIcon.Text = "🌐";
        }

        public void InitializeModule()
        {
            try
            {
                lblServerTitle.Text = DisplayName;
                // We need to update the Apache and phpMyAdmin paths to ensure they are properly set up in case
                // the application has been moved to a different directory/drive.
                UpdateApacheConfig();

                _apacheManager = ServerManagerFactory.CreateServerManager<ApacheManager>(ServerDefinitions.Apache.Name);                
                _apacheManager.ErrorOccurred += LogError;
                _apacheManager.StatusChanged += LogMessage;
                ServerManager = _apacheManager;
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message, LogType.Error);
            }

            ValidateServerConfig();

        }

        private void ValidateServerConfig()
        {
            LogMessage($"Initializing server settings... ", LogType.Info);

            //var serverApp = Path.Combine(ConfigManager.BaseDirectory, "apache", "bin", ConfigManager.Config.BinaryNames.Apache);

            //AddLog(LanguageManager._("Checking for module existence..."), LogType.Debug);

            //if (!File.Exists(serverApp))
            //{
            //    StatusPanel.BackColor = ConfigManager.ErrorColor;
            //    AddLog(string.Format(LanguageManager._("Problem detected: {0} Not Found!"), ModuleName), LogType.Error);
            //    AddLog(string.Format(LanguageManager._("Disabling {0} buttons"), ModuleName), LogType.Error);
            //    AddLog(LanguageManager._("Run this program from your XAMPP root directory!"), LogType.Error);

            //    AdminButton.Enabled = false;
            //    ServiceButton.Enabled = false;
            //    StartStopButton.Enabled = false;
            //}

            //if (!ConfigManager.Config.EnableServices.Apache)
            //{
            //    AddLog(string.Format(LanguageManager._("{0} Service is disabled."), ModuleName), LogType.Debug);
            //    ServiceButton.Enabled = false;
            //}

            //AddLog(LanguageManager._("Checking for required tools..."), LogType.Debug);
        }

        private bool CheckPort(int port, bool showDialog = true)
        {
            if (NetworkPortHelper.IsPortInUse(port))
            {
                if (showDialog)
                {
                    MessageBox.Show($"Port {port} is in use. Apache server cannot be started.", "Port In Use",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }
            return true;
        }

        private void LogMessage(object sender, string message)
        {
            //AddLog(string.Format(LanguageManager._("{0} Service is disabled."), ModuleName), LogType.Debug);
            LogMessage(message, LogType.Info);
        }

        private void LogError(object sender, string message)
        {
            LogMessage(message, LogType.Error);
        }

        //public virtual void Dispose()
        //{
        //    if (_apacheManager != null)
        //    {
        //        _apacheManager.Dispose();
        //        _apacheManager = null;
        //    }
        //}

        internal Task Dispose()
        {
            if (_apacheManager != null)
            {
                _apacheManager.Dispose();
                _apacheManager = null;
            }
            return Task.CompletedTask;
        }

        internal bool IsRunning()
        {
            return _apacheManager != null && _apacheManager.IsRunning;
        }

        protected override async void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckPort(PortNumber, false))
                {
                    LogMessage($"Port {PortNumber} is in use. Cannot start {ServiceName}.", LogType.Warning);
                    MessageBox.Show($"Port {PortNumber} is in use. Please close the application using this port and try again.",
                                  "Port In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ServerManager == null)
                {
                    LogMessage("Server manager is not initialized. Attempting to reinitialize...", LogType.Debug);
                    
                    try
                    {
                        _apacheManager = ServerManagerFactory.CreateServerManager<ApacheManager>(ServerDefinitions.Apache.Name);
                        _apacheManager.ErrorOccurred += LogError;
                        _apacheManager.StatusChanged += LogMessage;
                        ServerManager = _apacheManager;
                        LogMessage("Server manager reinitialized successfully.", LogType.Info);
                    }
                    catch (Exception initEx)
                    {
                        LogMessage($"Failed to reinitialize server manager: {initEx.Message}", LogType.Error);
                        MessageBox.Show($"Cannot start {ServiceName}: Server manager initialization failed.\n\n{initEx.Message}", 
                                      "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                              

                base.BtnStart_Click(sender, e);
            }
            catch (Exception ex)
            {
                LogMessage($"Error starting {ServiceName}: {ex.Message}", LogType.Error);
                MessageBox.Show($"Error starting {ServiceName}: {ex.Message}", "Startup Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                UpdateStatus(ServerStatus.Stopped);
            }
        }

        #region Apache Configuration Methods (merged from AppBootstrap)

        /// <summary>
        /// Updates the Apache configuration by creating custom path definitions.
        /// </summary>
        public void UpdateApacheConfig()
        {
            // Get the directory where the application executable is located.
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            if (string.IsNullOrEmpty(assemblyLocation))
            {
                // Fallback to current directory if assembly location is not available.
                _currentDirectory = Directory.GetCurrentDirectory();
            }
            else
            {
                _currentDirectory = Path.GetDirectoryName(assemblyLocation);
            }

            // Validate that we have a valid directory.
            if (string.IsNullOrEmpty(_currentDirectory))
            {
                throw new InvalidOperationException("Unable to determine application directory");
            }

            // Use ServerPathManager to get Apache paths.
            _apacheDirectory = ServerPathManager.GetServerBaseDirectory("Apache");

            // If ServerPathManager doesn't return paths, fall back to manual construction.
            if (string.IsNullOrEmpty(_apacheDirectory))
            {
                _apacheDirectory = Path.Combine(_currentDirectory, "apps", "apache");
            }
            
            _documentRoot = Path.Combine(_currentDirectory, "htdocs");

            // Set up PhpMyAdmin path (assuming it's in apps/phpmyadmin).
            _phpMyAdminDirectory = Path.Combine(_currentDirectory, "apps", "phpmyadmin");

            _customConfigPath = Path.Combine(_apacheDirectory, "conf", "custom_path.conf");
            _httpdAliasConfigPath = Path.Combine(_apacheDirectory, "conf", "httpd-alias.conf");

            CreateCustomConfiguration();
            UpdateHttpdAliasConfiguration();
        }

        /// <summary>
        /// Creates the custom Apache configuration file with path definitions.
        /// </summary>
        /// <returns>True if the configuration file was created successfully, false otherwise.</returns>
        private bool CreateCustomConfiguration()
        {
            try
            {
                // Ensure the conf directory exists.
                var confDirectory = Path.GetDirectoryName(_customConfigPath);
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

                // Create the custom configuration content.
                var configContent = new StringBuilder();
                configContent.AppendLine(string.Format("Define SRVROOT \"{0}\"", _apacheDirectory));
                configContent.AppendLine(string.Format("Define DOCROOT \"{0}\"", _documentRoot));

                // Write the configuration to the file.
                File.WriteAllText(_customConfigPath, configContent.ToString(), Encoding.UTF8);

                // Also update the httpd-alias.conf file.
                if (!UpdateHttpdAliasConfiguration())
                {
                    LogMessage("Failed to update httpd-alias.conf", LogType.Error);
                    // Don't fail the entire operation if alias config update fails.
                }

                return true;
            }
            catch (Exception ex)
            {
                LogMessage(string.Format("Error creating custom Apache configuration: {0}", ex.Message), LogType.Error);
                return false;
            }
        }

        /// <summary>
        /// Updates the httpd-alias.conf file to use the correct PMAROOT path.
        /// </summary>
        /// <returns>True if the file was updated successfully, false otherwise.</returns>
        private bool UpdateHttpdAliasConfiguration()
        {
            try
            {
                if (!File.Exists(_httpdAliasConfigPath))
                {
                    LogMessage(string.Format("httpd-alias.conf file not found: {0}", _httpdAliasConfigPath), LogType.Error);
                    return false;
                }

                // Read all lines from the file
                var lines = File.ReadAllLines(_httpdAliasConfigPath);
                bool updated = false;

                // Update the first line if it contains the PMAROOT Define statement
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i].Trim();
                    if (line.StartsWith("Define PMAROOT", StringComparison.OrdinalIgnoreCase))
                    {
                        // Replace the hardcoded path with the relative path
                        lines[i] = string.Format("Define PMAROOT \"{0}\"", _phpMyAdminDirectory);
                        updated = true;
                        LogMessage(string.Format("Updated PMAROOT path to: {0}", _phpMyAdminDirectory), LogType.Info);
                        break; // Only update the first occurrence
                    }
                }

                if (updated)
                {
                    // Write the updated content back to the file
                    File.WriteAllLines(_httpdAliasConfigPath, lines, Encoding.UTF8);
                    return true;
                }
                else
                {
                    LogMessage("No PMAROOT Define statement found in httpd-alias.conf", LogType.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogMessage(string.Format("Error updating httpd-alias.conf: {0}", ex.Message), LogType.Error);
                return false;
            }
        }

        /// <summary>
        /// Validates that all required directories and files exist for Apache to run.
        /// </summary>
        /// <returns>True if all requirements are met, false otherwise.</returns>
        public bool ValidateApacheEnvironment()
        {
            try
            {
                // Use ServerPathManager to check if Apache is available
                if (!ServerPathManager.IsServerAvailable("Apache"))
                {
                    LogMessage("Apache is not available according to ServerPathManager", LogType.Error);
                    return false;
                }

                // Check if Apache directory exists
                if (!Directory.Exists(_apacheDirectory))
                {
                    LogMessage(string.Format("Apache directory not found: {0}", _apacheDirectory), LogType.Error);
                    return false;
                }

                // Use ServerPathManager to get and validate Apache executable
                var apacheExecutablePath = ServerPathManager.GetExecutablePath("Apache");
                if (string.IsNullOrEmpty(apacheExecutablePath) || !File.Exists(apacheExecutablePath))
                {
                    LogMessage(string.Format("Apache executable not found: {0}", apacheExecutablePath), LogType.Error);
                    return false;
                }

                // Use ServerPathManager to get and validate Apache configuration
                var apacheConfigPath = ServerPathManager.GetConfigPath("Apache");
                if (string.IsNullOrEmpty(apacheConfigPath) || !File.Exists(apacheConfigPath))
                {
                    LogMessage(string.Format("Apache configuration file not found: {0}", apacheConfigPath), LogType.Error);
                    return false;
                }

                // Check if document root directory exists, create if it doesn't
                if (!Directory.Exists(_documentRoot))
                {
                    try
                    {
                        Directory.CreateDirectory(_documentRoot);
                        LogMessage(string.Format("Created document root directory: {0}", _documentRoot), LogType.Info);
                    }
                    catch (IOException ioEx)
                    {
                        // Directory might have been created by another thread, check again
                        if (!Directory.Exists(_documentRoot))
                        {
                            LogMessage(string.Format("Failed to create document root directory: {0}", ioEx.Message), LogType.Error);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMessage(string.Format("Failed to create document root directory: {0}", ex.Message), LogType.Error);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogMessage(string.Format("Error validating Apache environment: {0}", ex.Message), LogType.Error);
                return false;
            }
        }

        /// <summary>
        /// Checks if the custom configuration file exists.
        /// </summary>
        public bool CustomConfigExists => File.Exists(_customConfigPath);

        /// <summary>
        /// Checks if the httpd-alias.conf file exists.
        /// </summary>
        public bool HttpdAliasConfigExists => File.Exists(_httpdAliasConfigPath);

        #endregion


    }
}
