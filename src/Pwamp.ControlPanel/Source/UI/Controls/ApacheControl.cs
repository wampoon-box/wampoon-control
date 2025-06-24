using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frostybee.Pwamp.Models;
using Frostybee.Pwamp.Controllers;
using Frostybee.Pwamp.Enums;
using Frostybee.Pwamp.Helpers;
using static Frostybee.Pwamp.Helpers.ErrorLogHelper;

namespace Frostybee.Pwamp.Controls
{
    internal partial class ApacheControl : ServerControlBase, IDisposable
    {
        private ApacheServerManager _apacheManager;
        // Apache and phpMyAdmin-related paths.
        private string _apacheDirectory;
        private string _customConfigPath;
        private string _httpdAliasConfigPath;
        
        public ApacheControl()
        {
            ServiceName = PackageType.Apache.ToServerName();
            DisplayName = "Apache HTTP Server";
            PortNumber = 80; // Default HTTP port, change if needed.
            lblServerIcon.Text = "🌐";            
            btnServerAdmin.Text = "localhost";
        }

        public void InitializeModule()
        {
            try
            {
                LogMessage($"Initializing server settings... ", LogType.Info);
                lblServerTitle.Text = DisplayName;
                
                // Log diagnostic information for debugging configuration issues
                ServerPathManager.LogApacheDiagnostics();
                
                // We need to update the Apache and phpMyAdmin paths to ensure they are properly set up in case
                // the application has been moved to a different directory/drive.
                UpdateApacheConfig();

                // Initialize log paths using ServerPathManager.
                var logsDirectory = ServerPathManager.GetSpecialPath(PackageType.Apache.ToServerName(), "Logs");
                if (!string.IsNullOrEmpty(logsDirectory))
                {
                    ErrorLogPath = Path.Combine(logsDirectory, "error.log");
                    AccessLogPath = Path.Combine(logsDirectory, "access.log");
                }

                _apacheManager = ServerManagerFactory.CreateServerManager<ApacheServerManager>(ServerDefinitions.Apache.Name);                
                _apacheManager.ErrorOccurred += LogError;
                _apacheManager.StatusChanged += LogMessage;
                ServerManager = _apacheManager;
                //TODO: Default admin URI for Apache. Might need to add the port number. 
                //ServerAdminUri = $"http://localhost:{PortNumber}/"; 
                ServerAdminUri = $"http://localhost";
                UpdateStatus(CurrentStatus);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogMessage(ex.Message, LogType.Error);
            }

            ValidateServerConfig();

        }

        private void ValidateServerConfig()
        {
            //LogMessage($"Initializing server settings... ", LogType.Info);

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

        private void LogMessage(object sender, ServerLogEventArgs e)
        {
            //AddLog(string.Format(LanguageManager._("{0} Service is disabled."), ModuleName), LogType.Debug);
            LogMessage(e.Message, e.LogType);
        }

        private void LogError(object sender, ServerLogEventArgs e)
        {
            LogMessage(e.Message, e.LogType);
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
                        _apacheManager = ServerManagerFactory.CreateServerManager<ApacheServerManager>(ServerDefinitions.Apache.Name);
                        _apacheManager.ErrorOccurred += LogError;
                        _apacheManager.StatusChanged += LogMessage;
                        ServerManager = _apacheManager;
                        LogMessage("Server manager reinitialized successfully.", LogType.Info);
                    }
                    catch (Exception initEx)
                    {
                        ErrorLogHelper.LogExceptionInfo(initEx);
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
                ErrorLogHelper.LogExceptionInfo(ex);
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
            _apacheDirectory = ServerPathManager.GetServerBaseDirectory(PackageType.Apache.ToServerName());
            if (string.IsNullOrEmpty(_apacheDirectory))
            {
                throw new InvalidOperationException("Unable to determine Apache directory. Ensure that apache is installed...");
            }            
            _customConfigPath = Path.Combine(_apacheDirectory, "conf", "pwamp-custom-path.conf");

            ApplyCustomConfiguration();
        }

        /// <summary>
        /// Creates the custom Apache configuration file with path definitions.
        /// </summary>
        /// <returns>True if the configuration file was created successfully, false otherwise.</returns>
        private bool ApplyCustomConfiguration()
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
                configContent.AppendLine(string.Format("Define DOCROOT \"{0}\"", ServerPathManager.ApacheDocumentRoot));
                configContent.AppendLine(string.Format("Define PWAMP_APPS_DIR \"{0}\"", ServerPathManager.AppsDirectory));

                // Write the configuration to the file.
                File.WriteAllText(_customConfigPath, configContent.ToString(), Encoding.UTF8);

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogMessage(string.Format("Error creating custom Apache configuration: {0}", ex.Message), LogType.Error);
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
                // Use ServerPathManager to check if Apache is available.
                if (!ServerPathManager.IsServerAvailable(PackageType.Apache.ToServerName()))
                {
                    LogMessage("Apache is not available according to ServerPathManager", LogType.Error);
                    return false;
                }

                // Check if Apache directory exists.
                if (!Directory.Exists(_apacheDirectory))
                {
                    LogMessage(string.Format("Apache directory not found: {0}", _apacheDirectory), LogType.Error);
                    return false;
                }

                // Use ServerPathManager to get and validate Apache executable.
                var apacheExecutablePath = ServerPathManager.GetExecutablePath(PackageType.Apache.ToServerName());
                if (string.IsNullOrEmpty(apacheExecutablePath) || !File.Exists(apacheExecutablePath))
                {
                    LogMessage(string.Format("Apache executable not found: {0}", apacheExecutablePath), LogType.Error);
                    return false;
                }

                // Use ServerPathManager to get and validate Apache configuration.
                var apacheConfigPath = ServerPathManager.GetConfigPath(PackageType.Apache.ToServerName());
                if (string.IsNullOrEmpty(apacheConfigPath) || !File.Exists(apacheConfigPath))
                {
                    LogMessage(string.Format("Apache configuration file not found: {0}", apacheConfigPath), LogType.Error);
                    return false;
                }

                // Check if document root directory exists, create if it doesn't.
                if (!Directory.Exists(ServerPathManager.ApacheDocumentRoot))
                {
                    try
                    {
                        Directory.CreateDirectory(ServerPathManager.ApacheDocumentRoot);
                        LogMessage(string.Format("Created document root directory: {0}", ServerPathManager.ApacheDocumentRoot), LogType.Info);
                    }
                    catch (IOException ioEx)
                    {
                        // Directory might have been created by another thread, check again.
                        if (!Directory.Exists(ServerPathManager.ApacheDocumentRoot))
                        {
                            LogMessage(string.Format("Failed to create document root directory: {0}", ioEx.Message), LogType.Error);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLogHelper.LogExceptionInfo(ex);
                        LogMessage(string.Format("Failed to create document root directory: {0}", ex.Message), LogType.Error);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_apacheManager != null)
                {
                    _apacheManager.ErrorOccurred -= LogError;
                    _apacheManager.StatusChanged -= LogMessage;
                    _apacheManager.Dispose();
                    _apacheManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
