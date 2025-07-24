using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Wampoon.ControlPanel.Controllers;
using Wampoon.ControlPanel.Enums;
using Wampoon.ControlPanel.Helpers;


namespace Wampoon.ControlPanel.UI
{
    public partial class PortSettingsDialog : Form
    {
        private int _originalApachePort;
        private int _originalMySqlPort;

        public PortSettingsDialog()
        {
            InitializeComponent();
            LoadCurrentPorts();
        }


        private void LoadCurrentPorts()
        {
            // Get current ports from ServerPathManager
            _originalApachePort = ServerPathManager.ApachePort;
            _originalMySqlPort = ServerPathManager.MySqlPort;

            nudApachePort.Value = _originalApachePort;
            nudMySqlPort.Value = _originalMySqlPort;

            LogMessage($"Loaded current ports - Apache: {_originalApachePort}, MySQL: {_originalMySqlPort}", LogType.Info);
            
            // Initial validation
            ValidateApachePort();
            ValidateMySqlPort();
        }

        private void NudApachePort_ValueChanged(object sender, EventArgs e)
        {
            ValidateApachePort();
        }

        private void NudMySqlPort_ValueChanged(object sender, EventArgs e)
        {
            ValidateMySqlPort();
        }

        private void ValidateApachePort()
        {
            var port = (int)nudApachePort.Value;
            if (ApacheConfigManager.ValidatePort(port, null, _originalApachePort))
            {
                lblApacheStatus.Text = "✓ Available";
                lblApacheStatus.ForeColor = Color.Green;
            }
            else
            {
                lblApacheStatus.Text = "✗ In use / Invalid";
                lblApacheStatus.ForeColor = Color.Red;
            }
        }

        private void ValidateMySqlPort()
        {
            var port = (int)nudMySqlPort.Value;
            if (MySqlConfigManager.ValidatePort(port, null, _originalMySqlPort))
            {
                lblMySqlStatus.Text = "✓ Available";
                lblMySqlStatus.ForeColor = Color.Green;
            }
            else
            {
                lblMySqlStatus.Text = "✗ In use / Invalid";
                lblMySqlStatus.ForeColor = Color.Red;
            }
        }

        private void BtnValidate_Click(object sender, EventArgs e)
        {
            LogMessage("=== Port Validation Report ===", LogType.Info);
            
            var apachePort = (int)nudApachePort.Value;
            var mysqlPort = (int)nudMySqlPort.Value;

            LogMessage($"Validating Apache port {apachePort}:", LogType.Info);
            ApacheConfigManager.ValidatePort(apachePort, LogMessage, _originalApachePort);

            LogMessage($"Validating MySQL port {mysqlPort}:", LogType.Info);
            MySqlConfigManager.ValidatePort(mysqlPort, LogMessage, _originalMySqlPort);

            LogMessage("=== Validation Complete ===", LogType.Info);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var apachePort = (int)nudApachePort.Value;
            var mysqlPort = (int)nudMySqlPort.Value;

            // Validate both ports before applying
            LogMessage("=== Applying Port Changes ===", LogType.Info);
            
            bool apacheValid = ApacheConfigManager.ValidatePort(apachePort, LogMessage, _originalApachePort);
            bool mysqlValid = MySqlConfigManager.ValidatePort(mysqlPort, LogMessage, _originalMySqlPort);

            if (!apacheValid || !mysqlValid)
            {
                LogMessage("Cannot apply changes - one or more ports are invalid", LogType.Error);
                MessageBox.Show("Cannot apply changes. Please check the validation log and fix any issues.", 
                    "Port Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool success = true;

                // Update Apache port if changed.
                if (apachePort != _originalApachePort)
                {
                    var apacheConfigPath = ServerPathManager.GetConfigPath(PackageType.Apache.ToServerName());

                    // Get virtual host config path.
                    var apacheBaseDir = ServerPathManager.GetServerBaseDirectory(PackageType.Apache.ToServerName());
                    var vhostConfigPath = ApacheConfigManager.GetVirtualHostConfigPath(apacheBaseDir);

                    // Log virtual host config status.
                    if (ApacheConfigManager.IsValidVirtualHostConfig(vhostConfigPath))
                    {
                        LogMessage($"Found virtual host config: {Path.GetFileName(vhostConfigPath)}", LogType.Info);
                    }
                    else
                    {
                        LogMessage($"Virtual host config not found or invalid: {vhostConfigPath}", LogType.Warning);
                        LogMessage("Port update will proceed with main config only", LogType.Warning);
                    }
                    
                    // Use atomic update for both main config and virtual hosts
                    if (ApacheConfigManager.UpdatePortWithVirtualHosts(apacheConfigPath, vhostConfigPath, apachePort, LogMessage))
                    {
                        ServerPathManager.SetServerPort("Apache", apachePort);
                        LogMessage($"Apache port updated successfully to {apachePort} (including virtual hosts)", LogType.Info);
                    }
                    else
                    {
                        success = false;
                        LogMessage($"Failed to update Apache port and virtual hosts", LogType.Error);
                    }
                }

                // Update MySQL port if changed
                if (mysqlPort != _originalMySqlPort)
                {
                    var mysqlConfigPath = ServerPathManager.GetConfigPath(PackageType.MariaDB.ToServerName());
                    if (MySqlConfigManager.UpdatePort(mysqlConfigPath, mysqlPort, LogMessage))
                    {
                        ServerPathManager.SetServerPort("MariaDB", mysqlPort);
                        LogMessage($"MySQL port updated successfully to {mysqlPort}", LogType.Info);
                    }
                    else
                    {
                        success = false;
                        LogMessage($"Failed to update MySQL port", LogType.Error);
                    }
                }

                if (success)
                {
                    LogMessage("=== All Changes Applied Successfully ===", LogType.Info);
                    
                    string message = "Port configuration updated successfully!";
                    if (chkRestartServers.Checked)
                    {
                        LogMessage("Important: Server restart required for changes to take effect", LogType.Warning);
                        message += "\n\nIMPORTANT: You must restart the affected servers for the port changes to take effect.\nUse the Start/Stop buttons in the main window to restart the servers.";
                    }

                    MessageBox.Show(message, "Configuration Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    LogMessage("=== Some Changes Failed ===", LogType.Error);
                    MessageBox.Show("Some port changes failed to apply. Please check the log for details.", 
                        "Partial Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Unexpected error: {ex.Message}", LogType.Error);
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogMessage(string message, LogType logType)
        {
            if (rtxtLog.InvokeRequired)
            {
                rtxtLog.Invoke(new Action(() => LogMessageInternal(message, logType)));
            }
            else
            {
                LogMessageInternal(message, logType);
            }
        }

        private void LogMessageInternal(string message, LogType logType)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            
            // Set color based on log type
            Color messageColor;
            switch (logType)
            {
                case LogType.Error:
                    messageColor = Color.Red;
                    break;
                case LogType.Warning:
                    messageColor = Color.Orange;
                    break;
                case LogType.Info:
                    messageColor = Color.LightBlue;
                    break;
                default:
                    messageColor = Color.White;
                    break;
            }

            rtxtLog.SelectionStart = rtxtLog.TextLength;
            rtxtLog.SelectionLength = 0;

            // Add timestamp
            rtxtLog.SelectionColor = Color.Gray;
            rtxtLog.AppendText($"[{timestamp}] ");

            // Add message
            rtxtLog.SelectionColor = messageColor;
            rtxtLog.AppendText($"{message}\n");

            // Reset color and scroll to bottom
            rtxtLog.SelectionColor = Color.White;
            rtxtLog.ScrollToCaret();
        }
    }
}