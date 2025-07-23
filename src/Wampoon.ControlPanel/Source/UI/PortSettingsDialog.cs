using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Wampoon.ControlPanel.Controllers;
using Wampoon.ControlPanel.Enums;
using Wampoon.ControlPanel.Helpers;
using Wampoon.ControlPanel.Models;

namespace Wampoon.ControlPanel.UI
{
    public partial class PortSettingsDialog : Form
    {
        private NumericUpDown nudApachePort;
        private NumericUpDown nudMySqlPort;
        private Label lblApachePort;
        private Label lblMySqlPort;
        private Label lblApacheStatus;
        private Label lblMySqlStatus;
        private Button btnOK;
        private Button btnCancel;
        private Button btnValidate;
        private RichTextBox rtxtLog;
        private CheckBox chkRestartServers;

        private int _originalApachePort;
        private int _originalMySqlPort;

        public PortSettingsDialog()
        {
            InitializeComponent();
            LoadCurrentPorts();
        }

        private void InitializeComponent()
        {
            this.nudApachePort = new NumericUpDown();
            this.nudMySqlPort = new NumericUpDown();
            this.lblApachePort = new Label();
            this.lblMySqlPort = new Label();
            this.lblApacheStatus = new Label();
            this.lblMySqlStatus = new Label();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnValidate = new Button();
            this.rtxtLog = new RichTextBox();
            this.chkRestartServers = new CheckBox();
            this.SuspendLayout();

            // 
            // PortSettingsDialog
            // 
            this.Text = "Port Configuration";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;

            // 
            // lblApachePort
            // 
            this.lblApachePort.Text = "Apache HTTP Port:";
            this.lblApachePort.Location = new Point(20, 20);
            this.lblApachePort.Size = new Size(120, 23);
            this.lblApachePort.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // nudApachePort
            // 
            this.nudApachePort.Location = new Point(150, 20);
            this.nudApachePort.Size = new Size(80, 23);
            this.nudApachePort.Minimum = 1;
            this.nudApachePort.Maximum = 65535;
            this.nudApachePort.Value = 80;
            this.nudApachePort.ValueChanged += NudApachePort_ValueChanged;

            // 
            // lblApacheStatus
            // 
            this.lblApacheStatus.Location = new Point(240, 20);
            this.lblApacheStatus.Size = new Size(200, 23);
            this.lblApacheStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.lblApacheStatus.ForeColor = Color.Gray;

            // 
            // lblMySqlPort
            // 
            this.lblMySqlPort.Text = "MySQL/MariaDB Port:";
            this.lblMySqlPort.Location = new Point(20, 60);
            this.lblMySqlPort.Size = new Size(120, 23);
            this.lblMySqlPort.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // nudMySqlPort
            // 
            this.nudMySqlPort.Location = new Point(150, 60);
            this.nudMySqlPort.Size = new Size(80, 23);
            this.nudMySqlPort.Minimum = 1;
            this.nudMySqlPort.Maximum = 65535;
            this.nudMySqlPort.Value = 3306;
            this.nudMySqlPort.ValueChanged += NudMySqlPort_ValueChanged;

            // 
            // lblMySqlStatus
            // 
            this.lblMySqlStatus.Location = new Point(240, 60);
            this.lblMySqlStatus.Size = new Size(200, 23);
            this.lblMySqlStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.lblMySqlStatus.ForeColor = Color.Gray;

            // 
            // btnValidate
            // 
            this.btnValidate.Text = "Validate Ports";
            this.btnValidate.Location = new Point(20, 100);
            this.btnValidate.Size = new Size(100, 30);
            this.btnValidate.Click += BtnValidate_Click;

            // 
            // chkRestartServers
            // 
            this.chkRestartServers.Text = "Restart servers automatically after applying changes";
            this.chkRestartServers.Location = new Point(130, 105);
            this.chkRestartServers.Size = new Size(340, 20);
            this.chkRestartServers.Checked = true;

            // 
            // rtxtLog
            // 
            this.rtxtLog.Location = new Point(20, 140);
            this.rtxtLog.Size = new Size(440, 360);
            this.rtxtLog.ReadOnly = true;
            this.rtxtLog.BackColor = Color.Black;
            this.rtxtLog.ForeColor = Color.White;
            this.rtxtLog.Font = new Font("Consolas", 9F);

            // 
            // btnOK
            // 
            this.btnOK.Text = "Apply Changes";
            this.btnOK.Location = new Point(280, 520);
            this.btnOK.Size = new Size(100, 30);
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Click += BtnOK_Click;

            // 
            // btnCancel
            // 
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new Point(390, 520);
            this.btnCancel.Size = new Size(70, 30);
            this.btnCancel.DialogResult = DialogResult.Cancel;

            // Add controls to form
            this.Controls.Add(this.lblApachePort);
            this.Controls.Add(this.nudApachePort);
            this.Controls.Add(this.lblApacheStatus);
            this.Controls.Add(this.lblMySqlPort);
            this.Controls.Add(this.nudMySqlPort);
            this.Controls.Add(this.lblMySqlStatus);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.chkRestartServers);
            this.Controls.Add(this.rtxtLog);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);

            this.ResumeLayout(false);
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
            if (ApacheConfigManager.ValidatePort(port, null))
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
            if (MySqlConfigManager.ValidatePort(port, null))
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
            ApacheConfigManager.ValidatePort(apachePort, LogMessage);

            LogMessage($"Validating MySQL port {mysqlPort}:", LogType.Info);
            MySqlConfigManager.ValidatePort(mysqlPort, LogMessage);

            LogMessage("=== Validation Complete ===", LogType.Info);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var apachePort = (int)nudApachePort.Value;
            var mysqlPort = (int)nudMySqlPort.Value;

            // Validate both ports before applying
            LogMessage("=== Applying Port Changes ===", LogType.Info);
            
            bool apacheValid = ApacheConfigManager.ValidatePort(apachePort, LogMessage);
            bool mysqlValid = MySqlConfigManager.ValidatePort(mysqlPort, LogMessage);

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
                    
                    if (chkRestartServers.Checked)
                    {
                        LogMessage("Note: Server restart may be required for changes to take effect", LogType.Warning);
                    }

                    MessageBox.Show("Port configuration updated successfully!\n\nNote: You may need to restart the servers for changes to take effect.", 
                        "Configuration Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
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