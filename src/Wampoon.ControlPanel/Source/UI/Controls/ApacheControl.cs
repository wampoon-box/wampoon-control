using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wampoon.ControlPanel.Models;
using Wampoon.ControlPanel.Controllers;
using Wampoon.ControlPanel.Enums;
using Wampoon.ControlPanel.Helpers;
using static Wampoon.ControlPanel.Helpers.ErrorLogHelper;

namespace Wampoon.ControlPanel.Controls
{
    internal partial class ApacheControl : ServerControlBase, IDisposable
    {
        private ApacheServerManager _apacheManager;
        private ToolTip _toolTip;
        // Apache and phpMyAdmin-related paths.
        private string _apacheDirectory;
        private string _customConfigPath;
        //private string _httpdAliasConfigPath;

        public ApacheControl()
        {
            ServiceName = PackageType.Apache.ToServerName();
            DisplayName = "Apache HTTP Server";
            PortNumber = AppConstants.Ports.APACHE_DEFAULT; // Will be updated in InitializeModule()
            lblServerIcon.Text = "🌐";
            btnServerAdmin.Text = "localhost";

            // Add tooltip for the localhost button.
            _toolTip = new ToolTip { InitialDelay = 800, ReshowDelay = 400 };
            _toolTip.SetToolTip(btnServerAdmin, "Open localhost in a browser window");
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
                // Note: UpdateApacheConfig() now calls UpdatePortFromConfig() internally
                UpdateApacheConfig();

                // Initialize log paths using the new temp directory structure.
                var appBaseDirectory = ServerPathManager.AppBaseDirectory;
                if (!string.IsNullOrEmpty(appBaseDirectory))
                {
                    var apacheLogsDirectory = Path.Combine(appBaseDirectory, "apps", "temp", "apache_logs");
                    var phpLogsDirectory = Path.Combine(appBaseDirectory, "apps", "temp", "php_logs");
                    
                    ErrorLogPath = Path.Combine(apacheLogsDirectory, AppConstants.APACHE_ERROR_LOG);
                    AccessLogPath = Path.Combine(apacheLogsDirectory, AppConstants.APACHE_ACCESS_LOG);
                    PhpErrorLogPath = Path.Combine(phpLogsDirectory, AppConstants.PHP_ERROR_LOG);
                    
                    //LogMessage($"Log paths - Apache Error: {ErrorLogPath}, Apache Access: {AccessLogPath}, PHP Error: {PhpErrorLogPath}", LogType.Info);
                }

                EnsureServerManagerInitialized();

                //_apacheManager = new ApacheServerManager(
                //    ServerPathManager.GetExecutablePath(PackageType.Apache.ToServerName()),
                //    ServerPathManager.GetConfigPath(PackageType.Apache.ToServerName()));
                //_apacheManager.ErrorOccurred += HandleServerLogError;
                //_apacheManager.StatusChanged += HandleServerLogMessage;
                //ServerManager = _apacheManager;

                // Set admin URI with the actual port number
                ServerAdminUri = PortNumber == 80 ? AppConstants.Urls.LOCALHOST_HTTP : $"http://localhost:{PortNumber}/";
                UpdateStatus(CurrentStatus);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogMessage(ex.Message, LogType.Error);
            }

            ValidateServerConfig();

        }

        protected override void InitializeServerManager()
        {
            _apacheManager = new ApacheServerManager(
                ServerPathManager.GetExecutablePath(PackageType.Apache.ToServerName()),
                ServerPathManager.GetConfigPath(PackageType.Apache.ToServerName()));
            ServerManager = _apacheManager;
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




        protected override async void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                /*if (!CheckPort(PortNumber, true))
                {
                    return;
                }*/

                if (ServerManager == null)
                {
                    LogMessage("Server manager is not initialized. Attempting to reinitialize...", LogType.Debug);
                    
                    if (!EnsureServerManagerInitialized())
                    {
                        return;
                    }
                }
                              

                await Task.Run(() => base.BtnStart_Click(sender, e));
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
            _customConfigPath = Path.Combine(_apacheDirectory, AppConstants.Directories.APACHE_CONF, AppConstants.Directories.CUSTOM_CONFIG_NAME);
            //_httpdAliasConfigPath = Path.Combine(_apacheDirectory, AppConstants.Directories.APACHE_CONF, "httpd-aliases.conf");

            // Read the existing port from the variables file BEFORE applying configuration
            // This ensures we don't overwrite the configured port with the default
            UpdatePortFromConfig();
            
            ApplyCustomConfiguration();
            //PhpConfigurationHelper.UpdatePhpIniSettings(LogMessage);
        }

        /// <summary>
        /// Creates the custom Apache configuration file with path definitions.
        /// </summary>
        /// <returns>True if the configuration file was created successfully, false otherwise.</returns>
        private bool ApplyCustomConfiguration()
        {
            return ApacheConfigHelper.UpdateVariablesFile(_customConfigPath, ServerPathManager.AppBaseDirectory, PortNumber, LogMessage);
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
        //public bool HttpdAliasConfigExists => File.Exists(_httpdAliasConfigPath);


        /// <summary>
        /// Updates the PORT_NUMBER defined in the httpd-wampoon-variables.conf file.
        /// </summary>
        /// <param name="newPort">The new port number to set.</param>
        public void UpdatePortNumberInConfig(int newPort)
        {
            // Update the PortNumber property first
            PortNumber = newPort;
            
            // Update the port in the variables file using ApacheConfigManager
            ApacheConfigHelper.UpdatePortInVariablesFile(_customConfigPath, newPort, LogMessage);
        }

        #endregion

        /// <summary>
        /// Refreshes the port number by re-reading from Apache configuration file.
        /// </summary>
        public override void RefreshPortFromConfig()
        {
            UpdatePortFromConfig();
            
            // Update the admin URI with the new port
            ServerAdminUri = PortNumber == 80 ? AppConstants.Urls.LOCALHOST_HTTP : $"http://localhost:{PortNumber}/";
            
            // Update the port display in lblServerInfo
            UpdatePortAndPid(CurrentStatus);
            
            // Force UI update
            Invalidate();
        }

        /// <summary>
        /// Updates the port number by reading from httpd-wampoon-variables.conf file.
        /// </summary>
        private void UpdatePortFromConfig()
        {
            try
            {
                var configuredPort = ApacheConfigHelper.GetPortFromVariablesFile(_customConfigPath, LogMessage);
                
                if (configuredPort != PortNumber)
                {
                    LogMessage($"Port updated from variables file: {PortNumber} -> {configuredPort}", LogType.Info);
                    PortNumber = configuredPort;
                    
                    // Store the port in ServerPathManager for other modules to access
                    ServerPathManager.SetServerPort("Apache", configuredPort);
                }
                else
                {
                    LogMessage($"Using configured Apache port from variables file: {PortNumber}", LogType.Info);
                    // Still store the port even if it matches default
                    ServerPathManager.SetServerPort("Apache", PortNumber);
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                LogMessage($"Error reading Apache config, using default port {PortNumber}: {ex.Message}", LogType.Warning);
                // Store default port even on error
                ServerPathManager.SetServerPort("Apache", PortNumber);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_apacheManager != null)
                {
                    _apacheManager.OnLogServerMessage -= HandleServerLog;
                    _apacheManager.Dispose();
                    _apacheManager = null;
                    ServerManager = null; // Clear base class reference too.
                }
                if (_toolTip != null)
                {
                    _toolTip.Dispose();
                    _toolTip = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
