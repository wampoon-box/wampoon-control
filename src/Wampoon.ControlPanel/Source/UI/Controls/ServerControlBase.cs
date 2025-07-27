using Wampoon.ControlPanel.Controllers;
using Wampoon.ControlPanel.Enums;
using Wampoon.ControlPanel.Helpers;
using Wampoon.ControlPanel.UI;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Wampoon.ControlPanel.Helpers.ErrorLogHelper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Wampoon.ControlPanel.Controls
{
    public partial class ServerControlBase : UserControl
    {
        protected string ServiceName { get; set; }
        protected string DisplayName { get; set; }
        protected string ServerAdminUri { get; set; }
        protected string ConfigFilePath { get; set; }
        protected string ErrorLogPath { get; set; }
        protected string AccessLogPath { get; set; }
        protected string PhpErrorLogPath { get; set; }
        protected string MariaDbErrorLogPath { get; set; }
        protected int PortNumber { get; set; }
        protected int ProcessId { get; set; }
        protected ServerStatus CurrentStatus { get; set; } = ServerStatus.Stopped;
        internal ServerManagerBase ServerManager { get; set; }

        public ServerControlBase()
        {
            InitializeComponent();
            SetupEventHandlers();
            SetupToolsMenu();
            pnlControls.Paint += PnlControls_Paint;
        }

        protected void SetupEventHandlers()
        {
            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
            btnServerAdmin.Click += BtnOpenAdmin_Click;
            btnTools.Click += BtnTools_Click;
        }

        protected virtual void SetupToolsMenu()
        {
            contextMenuTools.Items.Clear();
            
            var configItem = contextMenuTools.Items.Add("📄 View Config File (Read-Only)");
            configItem.Click += (s, e) => OpenConfigFile();
            
            var configLocationItem = contextMenuTools.Items.Add("📁 Open Config File Location");
            configLocationItem.Click += (s, e) => OpenConfigFileLocation();
            
            contextMenuTools.Items.Add("-"); // Separator.
            var portConfigItem = contextMenuTools.Items.Add("⚙️ Port Configuration");
            portConfigItem.Click += (s, e) => OpenPortConfiguration();
            
            contextMenuTools.Items.Add("-"); // Separator.
            var errorLogItem = contextMenuTools.Items.Add("📋 View Apache Error Logs");
            errorLogItem.Click += (s, e) => OpenErrorLogs();

            var accessLogItem = contextMenuTools.Items.Add("📊 View Apache Access Logs");
            accessLogItem.Click += (s, e) => OpenAccessLogs();

            var phpErrorLogItem = contextMenuTools.Items.Add("🐘 View PHP Error Logs");
            phpErrorLogItem.Click += (s, e) => OpenPhpErrorLogs();
        }

        protected virtual void BtnTools_Click(object sender, EventArgs e)
        {
            contextMenuTools.Show(btnTools, 0, btnTools.Height);
        }

        protected async virtual void BtnOpenAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(()=> SystemHelper.OpenUrl(ServerAdminUri));
            }
            catch (Exception ex)
            {
                LogExceptionInfo(ex);
                LogMessage($"Failed to open admin interface for {ServiceName}. Please check the URL: {ServerAdminUri}.", LogType.Error);
            }
        }

        protected async virtual void BtnStop_Click(object sender, EventArgs e)
        {
            await StopServer();
        }
        public async Task StopServer()
        {
            try
            {
                if (!ServerManager.IsRunning)
                {
                    return;
                }
                btnStart.Enabled = false;
                UpdateStatus(ServerStatus.Stopping);

                bool success = await ServerManager.StopAsync();
                if (success)
                {
                    ProcessId = 0; // Clear PID when stopped.
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    UpdateStatus(ServerStatus.Stopped);
                }
                else
                {
                    btnStop.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                MessageBox.Show($"Error stopping {ServiceName}: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProcessId = 0; // Clear PID on error
                btnStop.Enabled = true;
                UpdateStatus(ServerStatus.Stopped);
            }
        }

        protected async virtual void BtnStart_Click(object sender, EventArgs e)
        {
            await StartServer();
        }

        public async Task StartServer()
        {

            try
            {
                btnStart.Enabled = false;
                UpdateStatus(ServerStatus.Starting);
                bool success = await ServerManager.StartAsync();
                if (success)
                {
                    // Update PID when started.
                    ProcessId = ServerManager.ProcessId.HasValue ? (int)ServerManager.ProcessId.Value : 0;
                    UpdateStatus(ServerStatus.Running);
                    btnStop.Enabled = true;
                    btnStart.Enabled = false;
                }
                else
                {
                    btnStart.Enabled = true;
                    UpdateStatus(ServerStatus.Stopped);
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                MessageBox.Show($"Error starting {ServiceName}: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProcessId = 0; // Clear PID on error.
                btnStart.Enabled = true;
                UpdateStatus(ServerStatus.Stopped);
            }
        }

        protected void UpdateStatus(ServerStatus serverStatus)
        {
            lblStatus.Text = serverStatus.ToString().ToUpper();
            lblStatus.Refresh();
            CurrentStatus = serverStatus;
            UpdatePortAndPid(serverStatus);

            switch (serverStatus)
            {
                case ServerStatus.Stopped:
                    ApplyControlStyle(Color.Red, Color.DarkRed, Color.FromArgb(232, 162, 162));
                    break;
                case ServerStatus.Running:
                    ApplyControlStyle(Color.Green, Color.DarkBlue, Color.FromArgb(200, 255, 200));
                    break;
                case ServerStatus.Stopping:
                    ApplyControlStyle(Color.Orange, Color.Blue, Color.FromArgb(243, 156, 18));
                    break;
                case ServerStatus.Starting:
                    ApplyControlStyle(Color.Orange, Color.Blue, Color.FromArgb(243, 156, 18));
                    break;
                case ServerStatus.Error:

                    break;
            }

            // Redraw the control to apply the new styles to the control's left border.
            pnlControls.Invalidate();
        }

        protected void UpdatePortAndPid(ServerStatus serverStatus)
        {
            // Update detailed info.
            string portInfo = PortNumber > 0 ? $"Port: {PortNumber}" : "Port: Not Set";
            string pidInfo;

            if (serverStatus == ServerStatus.Running && ProcessId > 0)
            {
                pidInfo = $"PID: {ProcessId}";
            }
            else if (serverStatus == ServerStatus.Starting || serverStatus == ServerStatus.Stopping)
            {
                pidInfo = "PID: ...";
            }
            else
            {
                pidInfo = "PID: Not Running";
            }

            lblServerInfo.Text = $"{portInfo} | {pidInfo}";
        }

        private void ApplyControlStyle(Color statusColor, Color lblForeColor, Color lblBackColor)
        {
            pcbServerStatus.BackColor = statusColor;
            lblStatus.ForeColor = lblForeColor;
            lblStatus.BackColor = lblBackColor;
            
            // Apply left borders to buttons instead of background colors
            //UiHelper.ApplyLeftBorderToButton(btnStart, statusColor);
            //UiHelper.ApplyLeftBorderToButton(btnStop, statusColor);
            //UiHelper.ApplyLeftBorderToButton(btnServerAdmin, statusColor);
            //UiHelper.ApplyLeftBorderToButton(btnTools, statusColor);
        }


        protected virtual void LogMessage(string serverModule, string log, LogType logType = LogType.Default)
        {
            MainForm.Instance?.AddLog(serverModule, log, logType);
        }

        protected virtual void LogMessage(string log, LogType logType = LogType.Default)
        {
            LogMessage(ServiceName, log, logType);
        }

        protected virtual void OpenConfigFile()
        {
            var configPath = ServerPathManager.GetConfigPath(ServiceName);
            OpenServerFile(configPath, "configuration file", true); // Open config files in read-only mode
        }

        protected virtual void OpenConfigFileLocation()
        {
            try
            {
                var configPath = ServerPathManager.GetConfigPath(ServiceName);
                if (string.IsNullOrEmpty(configPath))
                {
                    LogMessage("Configuration file path not configured.", LogType.Warning);
                    MessageBox.Show($"Configuration file path not configured for {ServiceName}.", "Configuration", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (System.IO.File.Exists(configPath))
                {
                    // Open Explorer and select the config file
                    SystemHelper.ExecuteFile("explorer.exe", $"/select,\"{configPath}\"", System.Diagnostics.ProcessWindowStyle.Normal);
                    LogMessage($"Opened configuration file location: {System.IO.Path.GetDirectoryName(configPath)}", LogType.Info);
                }
                else
                {
                    LogMessage($"Configuration file not found: {configPath}", LogType.Warning);
                    MessageBox.Show($"Configuration file not found: {configPath}", "File Not Found", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogExceptionInfo(ex);
                LogMessage($"Error opening configuration file location: {ex.Message}", LogType.Error);
                MessageBox.Show($"Error opening configuration file location: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void OpenServerFile(string filePath, string fileTypeDisplayName)
        {
            OpenServerFile(filePath, fileTypeDisplayName, false);
        }

        protected virtual void OpenServerFile(string filePath, string fileTypeDisplayName, bool readOnly)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    LogMessage($"{fileTypeDisplayName} path not configured.", LogType.Warning);
                    MessageBox.Show($"{fileTypeDisplayName} path not configured for {ServiceName}.", "Configuration", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (System.IO.File.Exists(filePath))
                {
                    if (readOnly)
                    {
                        OpenFileReadOnly(filePath, fileTypeDisplayName);
                    }
                    else
                    {
                        SystemHelper.ExecuteFile("notepad.exe", filePath, System.Diagnostics.ProcessWindowStyle.Normal);
                        LogMessage($"Opened {fileTypeDisplayName.ToLower()}: {filePath}", LogType.Info);
                    }
                }
                else
                {
                    LogMessage($"{fileTypeDisplayName} file not found: {filePath}", LogType.Warning);
                    MessageBox.Show($"{fileTypeDisplayName} file not found: {filePath}", "File Not Found", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogExceptionInfo(ex);
                LogMessage($"Error opening {fileTypeDisplayName.ToLower()}: {ex.Message}", LogType.Error);
                MessageBox.Show($"Error opening {fileTypeDisplayName.ToLower()}: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenFileReadOnly(string filePath, string fileTypeDisplayName)
        {
            string tempPath = null;
            try
            {
                // Create a temporary directory and preserve the original filename
                string tempDir = System.IO.Path.GetTempPath();
                string originalFileName = System.IO.Path.GetFileName(filePath);
                string tempFileName = $"{System.IO.Path.GetFileNameWithoutExtension(originalFileName)}_readonly{System.IO.Path.GetExtension(originalFileName)}";
                tempPath = System.IO.Path.Combine(tempDir, tempFileName);
                
                // If temp file exists, remove it first (it might be read-only)
                if (System.IO.File.Exists(tempPath))
                {
                    System.IO.File.SetAttributes(tempPath, System.IO.FileAttributes.Normal);
                    System.IO.File.Delete(tempPath);
                }
                
                System.IO.File.Copy(filePath, tempPath, true);
                
                // Set the temporary file as read-only
                System.IO.File.SetAttributes(tempPath, System.IO.File.GetAttributes(tempPath) | System.IO.FileAttributes.ReadOnly);
                
                // Open the read-only copy
                SystemHelper.ExecuteFile("notepad.exe", tempPath, System.Diagnostics.ProcessWindowStyle.Normal);
                LogMessage($"Opened {fileTypeDisplayName.ToLower()} (read-only): {originalFileName}", LogType.Info);
            }
            catch (Exception ex)
            {
                // Clean up temp file if something went wrong
                if (!string.IsNullOrEmpty(tempPath) && System.IO.File.Exists(tempPath))
                {
                    try
                    {
                        System.IO.File.SetAttributes(tempPath, System.IO.FileAttributes.Normal);
                        System.IO.File.Delete(tempPath);
                    }
                    catch { }
                }
                
                LogExceptionInfo(ex);
                LogMessage($"Error opening {fileTypeDisplayName.ToLower()} in read-only mode: {ex.Message}", LogType.Error);
                MessageBox.Show($"Error opening {fileTypeDisplayName.ToLower()} in read-only mode: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void OpenErrorLogs()
        {
            OpenServerFile(ErrorLogPath, "Error log");
        }

        protected virtual void OpenAccessLogs()
        {
            OpenServerFile(AccessLogPath, "Access log");
        }

        protected virtual void OpenPhpErrorLogs()
        {
            OpenServerFile(PhpErrorLogPath, "PHP error log");
        }

        protected virtual void OpenMariaDbErrorLogs()
        {
            OpenServerFile(MariaDbErrorLogPath, "MariaDB error log");
        }


        protected virtual void OpenPortConfiguration()
        {
            try
            {
                // Get reference to main form through FindForm()
                var mainForm = this.FindForm();
                if (mainForm is MainForm form)
                {
                    // Call the existing port configuration method
                    form.OpenPortConfigurationDialog();
                }
                else
                {
                    LogMessage("Could not access main form for port configuration", LogType.Error);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error opening port configuration: {ex.Message}", LogType.Error);
                ErrorLogHelper.ShowErrorReport(ex, "Error occurred while opening port configuration", this);
            }
        }

        private void PnlControls_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                UiHelper.DrawBootstrapCardShadow(e.Graphics, pnlControls);

                using (Brush borderBrush = new SolidBrush(GetLeftBorderColor()))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int borderWidth = 4;
                        int radius = 8;

                        path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                        path.AddLine(radius, 0, borderWidth, 0);
                        path.AddLine(borderWidth, 0, borderWidth, pnlControls.Height - 5);
                        path.AddLine(borderWidth, pnlControls.Height - 5, radius, pnlControls.Height - 5);
                        path.AddArc(0, pnlControls.Height - 5 - radius * 2, radius * 2, radius * 2, 90, 90);
                        path.CloseFigure();

                        e.Graphics.FillPath(borderBrush, path);
                    }
                }                
            }
            catch (Exception ex)
            {
                LogExceptionInfo(ex);
            }
        }

        protected bool CheckPort(int port, bool showDialog = true)
        {
            // Use retry logic for port checking, especially after force stops
            if (NetworkPortHelper.IsPortInUseWithRetry(port))
            {
                if (showDialog)
                {
                    MessageBox.Show(string.Format(AppConstants.Messages.PORT_IN_USE, port, ServiceName), "Port In Use",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }
            return true;
        }

        protected void HandleServerLog(object sender, ServerLogEventArgs e)
        {
            LogMessage(e.Message, e.LogType);
        }

        protected virtual void InitializeServerManager()
        {
            // Override in derived classes to initialize specific server managers
        }

        protected bool EnsureServerManagerInitialized()
        {
            if (ServerManager != null) return true;            
            //LogMessage(AppConstants.Messages.SERVER_MANAGER_NOT_INITIALIZED, LogType.Debug);
            
            try
            {
                InitializeServerManager();
                if (ServerManager != null)
                {
                    ServerManager.OnLogServerMessage += HandleServerLog;
                    //LogMessage(AppConstants.Messages.SERVER_MANAGER_REINITIALIZED, LogType.Info);
                    return true;
                }
                else
                {
                    LogMessage("Server manager initialization returned null", LogType.Error);
                    return false;
                }
            }
            catch (Exception initEx)
            {
                ErrorLogHelper.LogExceptionInfo(initEx);
                LogMessage(string.Format(AppConstants.Messages.FAILED_TO_OPERATION, "reinitialize server manager", initEx.Message), LogType.Error);
                MessageBox.Show($"Cannot start {ServiceName}: Server manager initialization failed.\n\n{initEx.Message}", 
                              "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        internal bool IsServerRunning()
        {
            return ServerManager != null && ServerManager.IsRunning;
        }

        /// <summary>
        /// Refreshes the port number from the current configuration and updates the display.
        /// </summary>
        public virtual void RefreshPortFromConfig()
        {
            try
            {
                // Get the current port from ServerPathManager based on service name
                if (ServiceName == "Apache")
                {
                    PortNumber = ServerPathManager.ApachePort;
                }
                else if (ServiceName == "MariaDB")
                {
                    PortNumber = ServerPathManager.MySqlPort;
                }

                // Update the display with the new port number
                UpdatePortAndPid(CurrentStatus);
                
                LogMessage($"Port refreshed from config: {PortNumber}", LogType.Info);
            }
            catch (Exception ex)
            {
                LogExceptionInfo(ex);
                LogMessage($"Error refreshing port from config: {ex.Message}", LogType.Error);
            }
        }

        private Color GetLeftBorderColor()
        {
            if (CurrentStatus == ServerStatus.Running)
            {
                return Color.Green;
            }
                
            if(CurrentStatus == ServerStatus.Stopping || CurrentStatus == ServerStatus.Starting)
            {
                return Color.Orange;
            }
            else
            {
                if (!string.IsNullOrEmpty(ServiceName) && ServiceName.Contains(PackageType.Apache.ToServerName()))
                {
                    return Color.Blue;

                }
                if (!string.IsNullOrEmpty(ServiceName) && ServiceName.Contains(PackageType.MariaDB.ToServerName()))
                {
                    return Color.OrangeRed;

                }
            }
            return Color.Red;
        }
    }
}
