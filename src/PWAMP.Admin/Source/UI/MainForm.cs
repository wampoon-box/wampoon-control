using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frostybee.PwampAdmin.Helpers;
using Frostybee.PwampAdmin.Enums;
using static Frostybee.PwampAdmin.Helpers.ErrorLogHelper;

namespace Frostybee.PwampAdmin.UI
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);
        
        public static MainForm Instance { get; private set; }
        private IntPtr _iconHandle = IntPtr.Zero;
        private NotifyIcon _notifyIcon;

        public MainForm()
        {
            Text = "PWAMP Control Panel";
            InitializeComponent();
            
            Instance = this;
            
            MinimumSize = new Size(850, 790);
            StartPosition = FormStartPosition.Manual;
            CenterToScreen();
            SizeGripStyle = SizeGripStyle.Show;

            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            
            InitializeNotifyIcon();            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetFromIcon();
                // Attempt to initialize the service modules.
                _apacheModule.InitializeModule();
                _mySqlModule.InitializeModule();

                AddLog("Application initialized successfully", LogType.Info);
            }
            catch (Exception ex)
            {
                AddLog($"Error during application initialization: {ex.Message}", LogType.Error);
                ErrorLogHelper.ShowErrorReport(ex, "Error occurred during application initialization", this);
            }
        }
                
        private void SetFromIcon()
        {
            try
            {
                byte[] pwamp_icon = Properties.Resources.pwamp_icon;
                using (MemoryStream ms = new MemoryStream(pwamp_icon))
                using (Bitmap bitmap = new Bitmap(ms))
                {
                    _iconHandle = bitmap.GetHicon();
                    this.Icon = Icon.FromHandle(_iconHandle);
                }
            }
            catch (Exception)
            {
            }
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "PWAMP Control Panel";
            _notifyIcon.Visible = false;
            
            try
            {
                byte[] pwamp_icon = Properties.Resources.pwamp_icon;
                using (MemoryStream ms = new MemoryStream(pwamp_icon))
                using (Bitmap bitmap = new Bitmap(ms))
                {
                    _notifyIcon.Icon = Icon.FromHandle(bitmap.GetHicon());
                }
            }
            catch (Exception)
            {
                _notifyIcon.Icon = SystemIcons.Application;
            }
            
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Restore", null, (s, e) => RestoreFromTray());
            contextMenu.Items.Add("Exit", null, (s, e) => ExitApplication());
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void RestoreFromTray()
        {
            Show();
            WindowState = FormWindowState.Normal;
            _notifyIcon.Visible = false;
            BringToFront();
        }

        private void ExitApplication()
        {
            // Bring form to front so user can see any confirmation dialogs
            if (WindowState == FormWindowState.Minimized || !Visible)
            {
                Show();
                WindowState = FormWindowState.Normal;
                BringToFront();
                Activate();
            }
            
            var exitEventArgs = new FormClosingEventArgs(CloseReason.ApplicationExitCall, false);
            MainForm_FormClosing(this, exitEventArgs);
            
            if (!exitEventArgs.Cancel)
            {
                Application.Exit();
            }
        }

        public void AddLog(string module, string log, LogType logType = LogType.Default)
        {
            if (_logTextBox == null) return;

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            var logEntry = $"[{timestamp}] [{module}] {log}";

            if (_logTextBox.InvokeRequired)
            {
                _logTextBox.Invoke(new Action(() => AddLogInternal(logEntry, logType)));
            }
            else
            {
                AddLogInternal(logEntry, logType);
            }
        }

        public void AddLog(string log, LogType logType = LogType.Default)
        {
            AddLog("System", log, logType);
        }

        public void AddErrorLog(string module, string log)
        {
            if (_errorLogTextBox == null) return;

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            var logEntry = $"[{timestamp}] [{module}] {log}";

            if (_errorLogTextBox.InvokeRequired)
            {
                _errorLogTextBox.Invoke(new Action(() => AddErrorLogInternal(logEntry)));
            }
            else
            {
                AddErrorLogInternal(logEntry);
            }
        }

        private void AddErrorLogInternal(string logEntry)
        {
            if (_errorLogTextBox == null) return;

            _errorLogTextBox.SelectionStart = _errorLogTextBox.TextLength;
            _errorLogTextBox.SelectionLength = 0;
            _errorLogTextBox.SelectionColor = Color.Red;
            _errorLogTextBox.AppendText(logEntry + Environment.NewLine);
            _errorLogTextBox.SelectionColor = _errorLogTextBox.ForeColor;
            _errorLogTextBox.ScrollToCaret();

            // Limit log size
            if (_errorLogTextBox.Lines.Length > 1000)
            {
                var lines = _errorLogTextBox.Lines;
                var newLines = new string[500];
                Array.Copy(lines, lines.Length - 500, newLines, 0, 500);
                _errorLogTextBox.Lines = newLines;
            }
        }

        private void AddLogInternal(string logEntry, LogType logType)
        {
            if (_logTextBox == null) return;

            Color textColor = UiHelper.GetLogColor(logType);

            _logTextBox.SelectionStart = _logTextBox.TextLength;
            _logTextBox.SelectionLength = 0;
            _logTextBox.SelectionColor = textColor;
            _logTextBox.AppendText(logEntry + Environment.NewLine);
            _logTextBox.SelectionColor = _logTextBox.ForeColor;
            _logTextBox.ScrollToCaret();

            // Limit log size
            if (_logTextBox.Lines.Length > 1000)
            {
                var lines = _logTextBox.Lines;
                var newLines = new string[500];
                Array.Copy(lines, lines.Length - 500, newLines, 0, 500);
                _logTextBox.Lines = newLines;
            }
        }

        private void BtnOpenExplorer_Click(object sender, EventArgs e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                SystemHelper.ExecuteFile("explorer.exe", path, ProcessWindowStyle.Normal);
            }
            catch (Exception ex)
            {
                AddLog($"Error opening file explorer: {ex.Message}", LogType.Error);
                ErrorLogHelper.ShowErrorReport(ex, "Error occurred while opening file explorer", this);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    Hide();
                    _notifyIcon.Visible = true;
                    //_notifyIcon.ShowBalloonTip(300, "PWAMP Control Panel", "Application minimized to system tray", ToolTipIcon.Info);
                    return;
                }
                if (WindowState != FormWindowState.Normal)
                {
                    RestoreFromTray();
                }
                // Clean up managers on application exit.
                bool hasRunningServices = 
                                         ((_apacheModule != null && _apacheModule.IsRunning()) ||
                                          (_mySqlModule != null && _mySqlModule.IsRunning()));

                if (hasRunningServices)
                {
                    var result = MessageBox.Show(
                        "Some services are still running. Do you want to stop them before closing?",
                        "Confirm Close",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Cancel || result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        // Cancel the close to allow async stopping.
                        e.Cancel = true;                     
                        StopRunningServicesAsync();
                    }
                }

                // Clean up icon handle.
                if (_iconHandle != IntPtr.Zero)
                {
                    DestroyIcon(_iconHandle);
                    _iconHandle = IntPtr.Zero;
                }

                // Clean up notify icon.
                if (_notifyIcon != null)
                {
                    _notifyIcon.Visible = false;
                    _notifyIcon.Dispose();
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                // Log error but don't prevent closing.
                System.Diagnostics.Debug.WriteLine($"Error during cleanup: {ex.Message}");
            }            
        }
        
        private async void StopRunningServicesAsync()
        {
            try
            {
                // Stop servers asynchronously
                await _apacheModule?.StopServer();
                await _mySqlModule?.StopServer();

                // Dispose modules after stopping.
                _apacheModule?.Dispose();
                _mySqlModule?.Dispose();

                // Exit the application after stopping is complete.
                Application.Exit();
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                System.Diagnostics.Debug.WriteLine($"Error stopping services: {ex.Message}");
                
                // Still dispose modules even if stopping failed.
                _apacheModule?.Dispose();
                _mySqlModule?.Dispose();

                // Still try to exit the application.
                Application.Exit();
            }
        }

        private async void BtnStartAllServers_Click(object sender, EventArgs e)
        {
            btnStopAllServers.Enabled = false;
            await _apacheModule?.StartServer();
            await _mySqlModule?.StartServer();
            btnStopAllServers.Enabled = true;
        }

        private async void BtnStopAllServers_Click(object sender, EventArgs e)
        {
            btnStartAllServers.Enabled = false;
            //TODO: Enable the button if both servers are running.
            await _apacheModule?.StopServer();
            await _mySqlModule?.StopServer();
            btnStartAllServers.Enabled = true;
        }

        private void BtnExportLogs_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                //saveDialog.FileName = $"{currentLogTab}_logs_{DateTime.Now:yyyy-MM-dd}.txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        //File.WriteAllText(saveDialog.FileName, logs[currentLogTab].ToString());
                        AddLog("output", $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Logs exported: {Path.GetFileName(saveDialog.FileName)}");
                        MessageBox.Show("Logs exported successfully!", "Export Complete",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        AddLog($"Error exporting logs: {ex.Message}", LogType.Error);
                        ErrorLogHelper.ShowErrorReport(ex, "Error occurred while exporting logs", this);
                    }
                }
            }

        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            try
            {
                using (var aboutForm = new AboutForm())
                {
                    aboutForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                AddLog($"Error opening About dialog: {ex.Message}", LogType.Error);
                ErrorLogHelper.ShowErrorReport(ex, "Error occurred while opening About dialog", this);
            }
        }

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            var exitEventArgs = new FormClosingEventArgs(CloseReason.ApplicationExitCall, false);
            MainForm_FormClosing(this, exitEventArgs);

            if (!exitEventArgs.Cancel)
            {
                Application.Exit();
            }

        }
    }
}
