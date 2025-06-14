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

namespace Frostybee.PwampAdmin
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);
        
        public static MainForm Instance { get; private set; }
        private IntPtr _iconHandle = IntPtr.Zero;

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
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetFromIcon();
            // Attempt to initialize the service modules.
            _apacheModule.InitializeModule();
            _mySqlModule.InitializeModule();

            AddLog("Application initialized successfully", LogType.Info);            
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
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Clean up managers on application exit.
                bool hasRunningServices = (_apacheModule != null && _apacheModule.IsRunning()) ||
                                         (_mySqlModule != null && _mySqlModule.IsRunning());

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
            }
            catch (Exception ex)
            {
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

                // Close the form after stopping is complete.
                this.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error stopping services: {ex.Message}");
                
                // Still dispose modules even if stopping failed.
                _apacheModule?.Dispose();
                _mySqlModule?.Dispose();

                // Still try to close the form.
                this.Close();
            }
        }

        private async void BtnStopAllServers_Click(object sender, EventArgs e)
        {
            //TODO: Enable the button if both servers are running.
            await _apacheModule?.StopServer();
            await _mySqlModule?.StopServer();
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
                        MessageBox.Show($"Error exporting logs: {ex.Message}", "Export Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
    }
}
