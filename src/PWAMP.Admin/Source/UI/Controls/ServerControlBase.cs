using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frostybee.PwampAdmin.Controllers;
using Frostybee.PwampAdmin.Enums;
using Frostybee.PwampAdmin.Helpers;
using Frostybee.PwampAdmin.UI;
using static Frostybee.PwampAdmin.Helpers.ErrorLogHelper;

namespace Frostybee.PwampAdmin.Controls
{
    public partial class ServerControlBase : UserControl
    {
        protected string ServiceName { get; set; }
        protected string DisplayName { get; set; }
        protected string ServerAdminUri { get; set; }
        protected string ConfigFilePath { get; set; }
        protected string ErrorLogPath { get; set; }
        protected string AccessLogPath { get; set; }
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

        protected void SetupToolsMenu()
        {
            contextMenuTools.Items.Clear();
            
            var configItem = contextMenuTools.Items.Add("📄 Open Config File");
            configItem.Click += (s, e) => OpenConfigFile();
            
            var errorLogItem = contextMenuTools.Items.Add("📋 View Error Logs");
            errorLogItem.Click += (s, e) => OpenErrorLogs();
            
            var accessLogItem = contextMenuTools.Items.Add("📊 View Access Logs");
            accessLogItem.Click += (s, e) => OpenAccessLogs();
            
            contextMenuTools.Items.Add("-"); // Separator.
            
            var refreshItem = contextMenuTools.Items.Add("🔄 Refresh Status");
            refreshItem.Click += (s, e) => RefreshServerStatus();
        }

        protected virtual void BtnTools_Click(object sender, EventArgs e)
        {
            contextMenuTools.Show(btnTools, 0, btnTools.Height);
        }

        protected async virtual void BtnOpenAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                SystemHelper.OpenUrl(ServerAdminUri);
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
                    ProcessId = (int)ServerManager.ProcessId; // Update PID when started.
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

        protected void UpdateStatus(ServerStatus status)
        {
            lblStatus.Text = status.ToString().ToUpper();
            lblStatus.Refresh();
            CurrentStatus = status;

            // Update detailed info.
            string portInfo = PortNumber > 0 ? $"Port: {PortNumber}" : "Port: Not Set";
            string pidInfo;
            
            if (status == ServerStatus.Running && ProcessId > 0)
            {
                pidInfo = $"PID: {ProcessId}";
            }
            else if (status == ServerStatus.Starting || status == ServerStatus.Stopping)
            {
                pidInfo = "PID: ...";
            }
            else
            {
                pidInfo = "PID: Not Running";
            }
            
            lblServerInfo.Text = $"{portInfo} | {pidInfo}";

            switch (status)
            {
                case ServerStatus.Stopped:
                    ApplyControlStyle(Color.Red, Color.DarkRed, Color.WhiteSmoke);
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

        private void ApplyControlStyle(Color statusColor, Color lblForeColor, Color lblBackColor)
        {
            pcbServerStatus.BackColor = statusColor;
            lblStatus.ForeColor = lblForeColor;
            lblStatus.BackColor = lblBackColor;
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
            try
            {
                if (string.IsNullOrEmpty(ConfigFilePath))
                {
                    LogMessage("Config file path not configured.", LogType.Warning);
                    MessageBox.Show($"Config file path not configured for {ServiceName}.", "Configuration", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (System.IO.File.Exists(ConfigFilePath))
                {
                    SystemHelper.ExecuteFile("notepad.exe", ConfigFilePath, System.Diagnostics.ProcessWindowStyle.Normal);
                    LogMessage($"Opened config file: {ConfigFilePath}", LogType.Info);
                }
                else
                {
                    LogMessage($"Config file not found: {ConfigFilePath}", LogType.Warning);
                    MessageBox.Show($"Config file not found: {ConfigFilePath}", "File Not Found", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogExceptionInfo(ex);
                LogMessage($"Error opening config file: {ex.Message}", LogType.Error);
                MessageBox.Show($"Error opening config file: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void OpenErrorLogs()
        {
            try
            {
                if (string.IsNullOrEmpty(ErrorLogPath))
                {
                    LogMessage("Error log path not configured.", LogType.Warning);
                    MessageBox.Show($"Error log path not configured for {ServiceName}.", "Configuration", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (System.IO.File.Exists(ErrorLogPath))
                {
                    SystemHelper.ExecuteFile("notepad.exe", ErrorLogPath, System.Diagnostics.ProcessWindowStyle.Normal);
                    LogMessage($"Opened error log: {ErrorLogPath}", LogType.Info);
                }
                else
                {
                    LogMessage($"Error log file not found: {ErrorLogPath}", LogType.Warning);
                    MessageBox.Show($"Error log file not found: {ErrorLogPath}", "File Not Found", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogExceptionInfo(ex);
                LogMessage($"Error opening error log: {ex.Message}", LogType.Error);
                MessageBox.Show($"Error opening error log: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void OpenAccessLogs()
        {
            try
            {
                if (string.IsNullOrEmpty(AccessLogPath))
                {
                    LogMessage("Access log path not configured.", LogType.Warning);
                    MessageBox.Show($"Access log path not configured for {ServiceName}.", "Configuration", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (System.IO.File.Exists(AccessLogPath))
                {
                    SystemHelper.ExecuteFile("notepad.exe", AccessLogPath, System.Diagnostics.ProcessWindowStyle.Normal);
                    LogMessage($"Opened access log: {AccessLogPath}", LogType.Info);
                }
                else
                {
                    LogMessage($"Access log file not found: {AccessLogPath}", LogType.Warning);
                    MessageBox.Show($"Access log file not found: {AccessLogPath}", "File Not Found", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogExceptionInfo(ex);
                LogMessage($"Error opening access log: {ex.Message}", LogType.Error);
                MessageBox.Show($"Error opening access log: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void RefreshServerStatus()
        {
            try
            {
                if (ServerManager != null)
                {
                    bool isRunning = ServerManager.IsRunning;
                    ProcessId = (int)ServerManager.ProcessId;
                    
                    UpdateStatus(isRunning ? ServerStatus.Running : ServerStatus.Stopped);
                    
                    btnStart.Enabled = !isRunning;
                    btnStop.Enabled = isRunning;
                    
                    LogMessage("Server status refreshed.", LogType.Info);
                }
            }
            catch (Exception ex)
            {
                LogExceptionInfo(ex);
                LogMessage($"Error refreshing server status: {ex.Message}", LogType.Error);
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

        private Color GetLeftBorderColor()
        {
            if (ServerManager != null && ServerManager.IsRunning)
            {
                return Color.Green;
            }
                
            if(CurrentStatus == ServerStatus.Stopping || CurrentStatus == ServerStatus.Starting)
            {
                return Color.Orange;
            }
            else
            {
                if (!string.IsNullOrEmpty(ServiceName) && ServiceName.Contains("Apache"))
                {
                    return Color.Blue;

                }
                if (!string.IsNullOrEmpty(ServiceName) && ServiceName.Contains("MariaDB"))
                {
                    return Color.OrangeRed;

                }
            }
            return Color.Red;
        }
    }
}
