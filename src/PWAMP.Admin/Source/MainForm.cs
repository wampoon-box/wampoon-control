using Pwamp.Admin.Controllers;
using Pwamp.Admin.Helpers;
using Pwamp.Admin.Source.Helpers;
using Pwamp.Forms;
using Pwamp.Helpers;
using Pwamp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pwamp.Admin
{
    public partial class MainForm : Form
    {
        public static MainForm Instance { get; private set; }

        public MainForm()
        {
            Instance = this;
            InitializeComponent();
            Text = "PWAMP Control Panel";
            InitializeApplication();
        }
        private void InitializeApplication()
        {
            _apacheModule.InitializeModule();
            _mySqlModule.InitializeModule();

            SetFromIcon();
            FormClosing += MainForm_FormClosing;
            AddLog("Application initialized successfully", LogType.Info);

            if (NetworkPortHelper.IsPortInUse(80))
            {
                MessageBox.Show($"Port {80} is in use.", "Port Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    IntPtr hIcon = bitmap.GetHicon();
                    this.Icon = Icon.FromHandle(hIcon);

                    // Optional: Clean up the handle when form closes
                    // DestroyIcon(hIcon); // Requires [DllImport("user32.dll")]
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

            Color textColor = LogMessageHelper.GetLogColor(logType);

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

                    if (result != DialogResult.Yes)
                    {
                        e.Cancel = true;

                        
                    }
                    else 
                    {
                        e.Cancel = true;
                        StopRunningService();
                    }
                    // If result == DialogResult.No, let the form close normally.
                }
            }
            catch (Exception ex)
            {
                // Log error but don't prevent closing.
                System.Diagnostics.Debug.WriteLine($"Error during cleanup: {ex.Message}");
            }            
        }
        
        private void StopRunningService()
        {
            Task.Run(async () =>
            {
                try
                {
                    await _apacheModule?.StopServer();
                    await _mySqlModule?.StopServer();

                    // Fix: Check if form is still valid before invoking
                    if (!this.IsDisposed && this.IsHandleCreated)
                    {
                        this.Invoke(new Action(() =>
                        {
                            if (!this.IsDisposed)
                                this.Close();
                        }));
                    }
                }
                catch (Exception ex)
                {
                    // Handle errors in the async operation.
                    System.Diagnostics.Debug.WriteLine($"Error stopping services: {ex.Message}");

                    // Still try to close the form if possible.
                    if (!this.IsDisposed && this.IsHandleCreated)
                    {
                        this.Invoke(new Action(() =>
                        {
                            if (!this.IsDisposed)
                                this.Close();
                        }));
                    }
                }
            });
        }
    }
}
