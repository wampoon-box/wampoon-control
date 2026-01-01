using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wampoon.ControlPanel.Controllers;
using Wampoon.ControlPanel.Enums;
using Wampoon.ControlPanel.Helpers;
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

        private ToolTip _toolTip;

        public MainForm()
        {
            Text = AppConstants.APP_NAME;
            InitializeComponent();

            Instance = this;

            MinimumSize = new Size(766, 761);
            StartPosition = FormStartPosition.Manual;
            CenterToScreen();
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            SizeGripStyle = SizeGripStyle.Show;

            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;

            InitializeNotifyIcon();
            InitializeBanner();
            InitializeTooltips();
        }

        private void InitializeTooltips()
        {
            _toolTip = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 800,
                ReshowDelay = 400,
                ShowAlways = true
            };

            _toolTip.SetToolTip(btnOpenDocRoot, "Open the Apache document root folder (htdocs)");
            _toolTip.SetToolTip(btnStartAllServers, "Start both Apache and MariaDB servers");
            _toolTip.SetToolTip(btnStopAllServers, "Stop all running servers");
            _toolTip.SetToolTip(btnAbout, "View application information");
            _toolTip.SetToolTip(btnQuit, "Stop all servers and exit the application");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetFromIcon();
                Text = AppConstants.APP_NAME;
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
            _notifyIcon.Text = AppConstants.APP_NAME;
            _notifyIcon.Visible = true;  // Make it visible immediately for testing.

            // Set the icon - try the form's icon first, then fallback to system icon.
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
                // Create a professional icon with shadow and refined styling.
                int iconSize = 52;
                var bitmap = new Bitmap(iconSize + 4, iconSize + 4); // Extra space for shadow
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                    // Draw drop shadow.
                    using (var shadowBrush = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
                    {
                        g.FillEllipse(shadowBrush, 4, 5, iconSize - 2, iconSize - 2);
                    }

                    // Create main gradient background with a modern blue palette.
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path.AddEllipse(2, 2, iconSize - 2, iconSize - 2);

                        using (var gradientBrush = new System.Drawing.Drawing2D.PathGradientBrush(path))
                        {
                            gradientBrush.CenterColor = Color.FromArgb(96, 165, 250);  // Lighter blue center
                            gradientBrush.SurroundColors = new[] { Color.FromArgb(37, 99, 235) }; // Darker blue edge
                            gradientBrush.CenterPoint = new PointF(iconSize * 0.35f, iconSize * 0.35f);
                            g.FillPath(gradientBrush, path);
                        }
                    }

                    // Add inner highlight for depth (top-left).
                    using (var highlightBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                        new Rectangle(4, 4, iconSize / 2, iconSize / 2),
                        Color.FromArgb(60, 255, 255, 255),
                        Color.FromArgb(0, 255, 255, 255),
                        System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal))
                    {
                        g.FillEllipse(highlightBrush, 6, 4, iconSize / 2 - 4, iconSize / 3);
                    }

                    // Add refined border.
                    using (var borderPen = new Pen(Color.FromArgb(29, 78, 216), 2))
                    {
                        g.DrawEllipse(borderPen, 3, 3, iconSize - 4, iconSize - 4);
                    }

                    // Draw the "W" letter with subtle shadow.
                    using (var font = new Font("Segoe UI", 22, FontStyle.Bold))
                    {
                        var format = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        var textRect = new RectangleF(2, 2, iconSize - 2, iconSize - 2);

                        // Text shadow.
                        using (var shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
                        {
                            var shadowRect = textRect;
                            shadowRect.Offset(1, 1);
                            g.DrawString("W", font, shadowBrush, shadowRect, format);
                        }

                        // Main text.
                        using (var textBrush = new SolidBrush(Color.White))
                        {
                            g.DrawString("W", font, textBrush, textRect, format);
                        }
                    }
                }
                bannerIcon.Image = bitmap;
                bannerIcon.Size = new Size(iconSize + 4, iconSize + 4);
                bannerIcon.Location = new Point(16, 8);

                // Improve layout - position title closer to icon.
                titleLabel.Location = new Point(72, 12);
                subtitleLabel.Location = new Point(74, 42);
                subtitleLabel.ForeColor = Color.FromArgb(200, 210, 220);

                // Update banner title with version if available.
                var version = SystemHelper.GetFormattedInstallerVersion();
                if (!string.IsNullOrEmpty(version))
                {
                    titleLabel.Text = $"ampoon Control Panel {version}";
                }

                // Add paint handler for gradient background and accent line.
                headerPanel.Paint += HeaderPanel_Paint;
            }
            catch
            {
                // If bitmap creation fails, hide the icon.
                bannerIcon.Visible = false;
            }
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // Draw gradient background (darker at top, slightly lighter at bottom).
            using (var gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                new Rectangle(0, 0, headerPanel.Width, headerPanel.Height),
                Color.FromArgb(25, 55, 95),    // Darker blue at top
                Color.FromArgb(35, 75, 125),   // Slightly lighter at bottom
                System.Drawing.Drawing2D.LinearGradientMode.Vertical))
            {
                g.FillRectangle(gradientBrush, 0, 0, headerPanel.Width, headerPanel.Height);
            }

            // Draw subtle diagonal pattern overlay for texture.
            using (var patternPen = new Pen(Color.FromArgb(8, 255, 255, 255), 1))
            {
                for (int i = -headerPanel.Height; i < headerPanel.Width + headerPanel.Height; i += 20)
                {
                    g.DrawLine(patternPen, i, headerPanel.Height, i + headerPanel.Height, 0);
                }
            }

            // Draw accent line at the bottom with gradient.
            using (var accentBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                new Rectangle(0, headerPanel.Height - 3, headerPanel.Width, 3),
                Color.FromArgb(59, 130, 246),
                Color.FromArgb(139, 92, 246),  // Purple accent
                System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
            {
                g.FillRectangle(accentBrush, 0, headerPanel.Height - 3, headerPanel.Width, 3);
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void RestoreFromTray()
        {

            // Bring form to front so user can see any confirmation dialogs.
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
            control.AppendText($"\t{message}");

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
                string htdocsPath = ServerPathManager.ApacheDocumentRoot;
                
                if (Directory.Exists(htdocsPath))
                {
                    SystemHelper.ExecuteFile("explorer.exe", htdocsPath, ProcessWindowStyle.Normal);
                    //AddLog($"Opened htdocs folder: {htdocsPath}", LogType.Info);
                }
                else
                {
                    AddLog($"htdocs folder not found at: {htdocsPath}", LogType.Warning);
                    MessageBox.Show($"The htdocs folder was not found at:\n{htdocsPath}\n\nPlease ensure the folder exists.",
                        "Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                AddLog($"Error opening htdocs folder: {ex.Message}", LogType.Error);
                ErrorLogHelper.ShowErrorReport(ex, "Error occurred while opening htdocs folder", this);
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
                    _notifyIcon.Text = $"{AppConstants.APP_NAME} - Running";
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

        private void BtnPortConfig_Click(object sender, EventArgs e)
        {
            OpenPortConfigurationDialog();
        }

        public void OpenPortConfigurationDialog()
        {
            try
            {
                using (var portDialog = new PortSettingsDialog())
                {
                    if (portDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        AddLog("Port configuration dialog completed successfully", LogType.Info);
                        
                        // Refresh both server modules to show updated port numbers
                        try
                        {
                            _apacheServerModule.RefreshPortFromConfig();
                            _mySqlServerModule.RefreshPortFromConfig();
                            AddLog("Server modules refreshed with new port configurations", LogType.Info);
                        }
                        catch (Exception refreshEx)
                        {
                            AddLog($"Warning: Could not refresh server modules: {refreshEx.Message}", LogType.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"Error opening Port Configuration dialog: {ex.Message}", LogType.Error);
                ErrorLogHelper.ShowErrorReport(ex, "Error occurred while opening Port Configuration dialog", this);
            }
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
