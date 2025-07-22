using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wampoon.ControlPanel.Helpers;
using Wampoon.ControlPanel.Enums;
using static Wampoon.ControlPanel.Helpers.ErrorLogHelper;

namespace Wampoon.ControlPanel.UI
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        public static MainForm Instance { get; private set; }
        private IntPtr _iconHandle = IntPtr.Zero;
        private NotifyIcon _notifyIcon;        
        
        private const int MAX_LOG_LINES = 1000;
        private const int TRIMMED_LOG_LINES = 500;

        public MainForm()
        {
            Text = AppConstants.APP_NAME;
            InitializeComponent();

            Instance = this;

            MinimumSize = new Size(766, 761);
            StartPosition = FormStartPosition.Manual;
            CenterToScreen();
            SizeGripStyle = SizeGripStyle.Show;

            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;

            InitializeNotifyIcon();
            InitializeBanner();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetFromIcon();
                // Attempt to initialize the service modules.
                _apacheServerModule.InitializeModule();
                _mySqlServerModule.InitializeModule();

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
            
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "WAMPoon Control Panel";
            _notifyIcon.Visible = true;  // Make it visible immediately for testing

            // Set the icon - try the form's icon first, then fallback to system icon
            try
            {
                _notifyIcon.Icon = this.Icon ?? SystemIcons.Application;
            }
            catch (Exception)
            {
                _notifyIcon.Icon = SystemIcons.Application;
            }

            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            _notifyIcon.ContextMenuStrip = trayContextMenuStrip;
        }

        private void InitializeBanner()
        {
            try
            {
                // Create a professional icon using text since we don't have an image file
                var bitmap = new Bitmap(40, 40);
                using (var g = Graphics.FromImage(bitmap))
                {
                    // Create a gradient background
                    using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                        new Rectangle(0, 0, 40, 40),
                        Color.FromArgb(37, 99, 235),
                        Color.FromArgb(59, 130, 246),
                        45f))
                    {
                        g.FillEllipse(brush, 0, 0, 40, 40);
                    }
                    
                    // Add a subtle border
                    using (var pen = new Pen(Color.FromArgb(29, 78, 216), 2))
                    {
                        g.DrawEllipse(pen, 1, 1, 38, 38);
                    }
                    
                    // Draw the "W" letter
                    using (var font = new Font("Segoe UI", 18, FontStyle.Bold))
                    {
                        g.DrawString("W", font, new SolidBrush(Color.White), 10, 8);
                    }
                }
                bannerIcon.Image = bitmap;
                
                // Update banner title with version if available
                var version = SystemHelper.GetFormattedInstallerVersion();
                if (!string.IsNullOrEmpty(version))
                {
                    titleLabel.Text = $"WAMPoon Control Panel {version}";
                }
            }
            catch
            {
                // If bitmap creation fails, hide the icon
                bannerIcon.Visible = false;
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void RestoreFromTray()
        {
           
            // Bring form to front so user can see any confirmation dialogs
            if (WindowState == FormWindowState.Minimized || !Visible)
            {
                Show();
                _notifyIcon.Visible = false;
                WindowState = FormWindowState.Normal;
                BringToFront();
                TopMost = true;
                TopMost = false;
                Activate();
            }
        }

        private void ExitApplication()
        {
            RestoreFromTray();

            var exitEventArgs = new FormClosingEventArgs(CloseReason.ApplicationExitCall, false);
            MainForm_FormClosing(this, exitEventArgs);

            if (!exitEventArgs.Cancel)
            {
                Application.Exit();
            }
        }

        private void AddLogToControl(RichTextBox control, string module, string message, LogType logType, bool includeTimestamp = true)
        {
            if (control == null) return;
            
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => AddLogToControlInternal(control, module, message, logType, includeTimestamp)));
            }
            else
            {
                AddLogToControlInternal(control, module, message, logType, includeTimestamp);
            }
        }

        private void AddLogToControlInternal(RichTextBox control, string module, string message, LogType logType, bool includeTimestamp)
        {
            if (control == null) return;

            Color messageColor = UiHelper.GetLogColor(logType);
            
            control.SelectionStart = control.TextLength;
            control.SelectionLength = 0;

            if (includeTimestamp)
            {
                // Add timestamp in muted color.
                var timestamp = DateTime.Now.ToString("HH:mm:ss");
                control.SelectionColor = UiHelper.Colors.MutedText;
                control.AppendText($"[{timestamp}] ");
            }

            // Add module in dedicated module color.
            control.SelectionColor = UiHelper.Colors.LogModule;
            control.AppendText($"[{module}] ");

            // Add message in the log type color.
            control.SelectionColor = messageColor;
            control.AppendText($"{message}");

            // Add new line.
            control.AppendText(Environment.NewLine);

            // Reset color to default based on background.
            control.SelectionColor = control.BackColor == UiHelper.Colors.LogBackground ? Color.White : UiHelper.Colors.LogDefault;
            control.ScrollToCaret();

            // Limit log size.
            if (control.Lines.Length > MAX_LOG_LINES)
            {
                var lines = control.Lines;
                var newLines = new string[TRIMMED_LOG_LINES];
                Array.Copy(lines, lines.Length - TRIMMED_LOG_LINES, newLines, 0, TRIMMED_LOG_LINES);
                control.Lines = newLines;
            }
        }

        public void AddLog(string module, string log, LogType logType = LogType.Default)
        {
            AddLogToControl(_rtxtActionsLog, module, log, logType);
        }

        public void AddLog(string log, LogType logType = LogType.Default)
        {
            AddLog("System", log, logType);
        }
        
        public void AddMySqlLog(string log, LogType logType = LogType.Default)
        {
            AddLogToControl(_rtxtErrorLog, "MariaDB", log, logType, false);
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
                    AddLog("Application hidden to system tray", LogType.Info);
                    // Force refresh the tray icon
                    _notifyIcon.Text = "WAMPoon Control Panel - Running";
                    return;
                }
                if (WindowState != FormWindowState.Normal)
                {
                    RestoreFromTray();
                }
                // Clean up managers on application exit.
                bool hasRunningServices =
                                         ((_apacheServerModule != null && _apacheServerModule.IsServerRunning()) ||
                                          (_mySqlServerModule != null && _mySqlServerModule.IsServerRunning()));

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
                /*if (_notifyIcon != null)
                {
                    _notifyIcon.Visible = false;
                    _notifyIcon.Dispose();
                }*/
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
                await _apacheServerModule?.StopServer();
                await _mySqlServerModule?.StopServer();

                // Dispose modules after stopping.
                _apacheServerModule?.Dispose();
                _mySqlServerModule?.Dispose();

                // Exit the application after stopping is complete.
                Application.Exit();
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                System.Diagnostics.Debug.WriteLine($"Error stopping services: {ex.Message}");

                // Still dispose modules even if stopping failed.
                _apacheServerModule?.Dispose();
                _mySqlServerModule?.Dispose();

                // Still try to exit the application.
                Application.Exit();
            }
        }

        private async void BtnStartAllServers_Click(object sender, EventArgs e)
        {
            btnStopAllServers.Enabled = false;
            await _apacheServerModule?.StartServer();
            await _mySqlServerModule?.StartServer();
            btnStopAllServers.Enabled = true;
        }

        private async void BtnStopAllServers_Click(object sender, EventArgs e)
        {            
            if (_apacheServerModule.IsServerRunning() || _mySqlServerModule.IsServerRunning())
            {
                btnStartAllServers.Enabled = false;
                //TODO: Enable the button if both servers are running.
                await _apacheServerModule?.StopServer();
                await _mySqlServerModule?.StopServer();
                btnStartAllServers.Enabled = true;
            }
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

        protected override void WndProc(ref Message m)
        {
            // Process the message sent from the Main method to activate the existing instance.
            // Check for our custom message.
            if (m.Msg == Program.WM_SHOW_RUNNING_INSTANCE)
            {
                ShowToFront();
                return;
            }

            base.WndProc(ref m);
        }
        private void ShowToFront()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }

            // Bring to front and activate.
            BringToFront();
            Activate();
            RestoreFromTray();
            // Alternative approach if the above doesn't work reliably:
            TopMost = true;
            TopMost = false;
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
            base.OnFormClosed(e);
        }

    }
}
