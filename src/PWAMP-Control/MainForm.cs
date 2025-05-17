using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using PwampControl.Forms;
using PwampControl.Helpers;
using PwampControl.Models;

namespace PwampControl
{
    public partial class MainForm : Form
    {
        private Settings _settings;
        private ProcessManager _processManager;

        public MainForm()
        {
            InitializeComponent();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            // Load settings
            _settings = SettingsManager.LoadSettings();

            // Initialize process manager
            _processManager = new ProcessManager(_settings, this);
            _processManager.ProcessStatusChanged += ProcessManager_ProcessStatusChanged;

            // Set form title
            this.Text = "PWAMP Control Panel";

            // Check process statuses
            _processManager.CheckAllProcesses();
        }

        // --- Event Handlers ---

        private void ProcessManager_ProcessStatusChanged(string processName, bool isRunning, string statusText, Color statusColor)
        {
            // Update UI based on process status
            if (processName.Equals("Apache", StringComparison.OrdinalIgnoreCase))
            {
                UpdateUiForStatus(apacheStatusLabel, startApacheButton, stopApacheButton, isRunning, statusText, statusColor);
            }
            else if (processName.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
            {
                UpdateUiForStatus(mysqlStatusLabel, startMysqlButton, stopMysqlButton, isRunning, statusText, statusColor);
            }
        }

        private async void StartApacheButton_Click(object sender, EventArgs e)
        {
            await _processManager.StartApacheAsync();
        }

        private async void StopApacheButton_Click(object sender, EventArgs e)
        {
            await _processManager.StopApacheAsync();
        }

        private async void StartMysqlButton_Click(object sender, EventArgs e)
        {
            await _processManager.StartMySqlAsync();
        }

        private async void StopMysqlButton_Click(object sender, EventArgs e)
        {
            await _processManager.StopMySqlAsync();
        }

        private void PhpMyAdminButton_Click(object sender, EventArgs e)
        {
            BrowserHelper.OpenUrl(_settings.PhpMyAdminUrl);
        }

        private void RefreshStatusButton_Click(object sender, EventArgs e)
        {
            _processManager.CheckAllProcesses();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm(_settings))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // Reload settings
                    _settings = SettingsManager.LoadSettings();
                    
                    // Check process statuses
                    _processManager.CheckAllProcesses();
                }
            }
        }

        // --- UI Helper Methods ---

        private void UpdateUiForStatus(Label statusLabel, Button startButton, Button stopButton, bool isRunning, string statusText, Color statusColor)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.BeginInvoke(new Action(() => 
                    UpdateUiForStatus(statusLabel, startButton, stopButton, isRunning, statusText, statusColor)));
                return;
            }

            statusLabel.Text = statusText;
            statusLabel.ForeColor = statusColor;
            startButton.Enabled = !isRunning;
            stopButton.Enabled = isRunning;
        }

        // --- Form Closing ---
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ask the user if they want to stop the processes on exit
            var result = MessageBox.Show(
                "Do you want to stop the running Apache and MySQL processes started by this application before closing?",
                "Confirm Exit",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                e.Cancel = true; // Prevent the form from closing
            }
            else if (result == DialogResult.Yes)
            {
                // Attempt to stop processes
                Task.Run(() => _processManager.StopAllProcessesAsync()).Wait(5000);
            }
        }
    }
}
