using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pwamp.Admin.Controllers;
using Pwamp.Forms;
using Pwamp.Helpers;
using Pwamp.Models;

namespace Pwamp.Admin
{
    public partial class MainForm : Form
    {
        ApacheManager _apacheManager;
        MySQLManager _mysqlManager;
        private string apacheHttpdPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\bin\httpd.exe"; // CHANGE THIS

        private string configPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\conf\httpd.conf"; // CHANGE THIS

        private string mysqlExecutablePath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\mariadb\bin\mariadbd.exe"; // CHANGE THIS
        private string mysqlConfigPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\mariadb\my.ini"; // CHANGE THIS
        public MainForm()
        {
            InitializeComponent();
            Text = "PWAMP Control Panel";
            InitializeApplication();
        }
        private void InitializeApplication()
        {

            _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
            _apacheManager.ErrorOccurred += LogError;
            _apacheManager.StatusChanged += LogMessage;
            // Initialize MySQL Manager.
            _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
            _mysqlManager.ErrorOccurred += LogError;
            _mysqlManager.StatusChanged += LogMessage;
        }

        private void LogError(object sender, string message)
        {
            if (txtOutputLog.InvokeRequired)
            {
                txtOutputLog.Invoke(new Action<string, string>(LogMessage), message);
                return;
            }
            txtOutputLog.ForeColor = Color.Red;
            txtOutputLog.AppendText(message + Environment.NewLine);
            txtOutputLog.SelectionStart = txtOutputLog.Text.Length;
            txtOutputLog.ScrollToCaret();
        }

        private void LogMessage(object sender, string message)
        {
            if (txtOutputLog.InvokeRequired)
            {
                txtOutputLog.Invoke(new Action<string, string>(LogMessage), message);
                return;
            }
            txtOutputLog.ForeColor = Color.Green;
            txtOutputLog.AppendText(message + Environment.NewLine);
            txtOutputLog.SelectionStart = txtOutputLog.Text.Length;
            txtOutputLog.ScrollToCaret();
        }


        private async void BtnStartApache_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = await _apacheManager.StartAsync();
                if (success)
                {
                    btnStartApache.Enabled = true;
                    btnStartApache.Text = "Apache: Running";
                    //apacheStatusLabel.ForeColor = Color.Green;
                }
                else
                {
                    btnStartApache.Enabled = true;
                    if (_apacheManager != null)
                    {
                        _apacheManager.Dispose();
                        _apacheManager = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStartApache.Enabled = true;
                if (_apacheManager != null)
                {
                    _apacheManager.Dispose();
                    _apacheManager = null;
                }
            }

        }

        private async void BtnStopApache_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = await _apacheManager.StopAsync();
                if (success)
                {
                    btnStartApache.Enabled = true;
                    btnStartApache.Text = "Apache: Running";
                    
                }
                else
                {
                    btnStartApache.Enabled = true;
                    if (_apacheManager != null)
                    {
                        _apacheManager.Dispose();
                        _apacheManager = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStartApache.Enabled = true;
                if (_apacheManager != null)
                {
                    _apacheManager.Dispose();
                    _apacheManager = null;
                }
            }
        }

        private async void BtnStartMySql_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = await _mysqlManager.StartAsync();
                if (success)
                {
                    btnStartMySql.Enabled = true;
                    btnStartMySql.Text = "MariaDB: Running";
                    //apacheStatusLabel.ForeColor = Color.Green;
                }
                else
                {
                    btnStartMySql.Enabled = true;
                    if (_mysqlManager != null)
                    {
                        _mysqlManager.Dispose();
                        _mysqlManager = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting MariaDB: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStartMySql.Enabled = true;
                if (_mysqlManager != null)
                {
                    _mysqlManager.Dispose();
                    _mysqlManager = null;
                }
            }
        }

        private async void BtnStopMysql_Click(object sender, EventArgs e)
        {
            try
            {
                bool success = await _mysqlManager.StopAsync();
                if (success)
                {
                    btnStopApache.Enabled = true;
                    btnStopApache.Text = "Apache: Running";
                    //apacheStatusLabel.ForeColor = Color.Green;
                }
                else
                {
                    btnStopApache.Enabled = true;
                    if (_mysqlManager != null)
                    {
                        _mysqlManager.Dispose();
                        _mysqlManager = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStopApache.Enabled = true;
                if (_mysqlManager != null)
                {
                    _mysqlManager.Dispose();
                    _mysqlManager = null;
                }
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO: Check whether processes are running and ask the user if they want to stop them.
            // Ask the user if they want to stop the processes on exit.
            //var result = MessageBox.Show(
            //    "Do you want to stop the running Apache and MySQL processes started by this application before closing?",
            //    "Confirm Exit",
            //    MessageBoxButtons.YesNoCancel,
            //    MessageBoxIcon.Question);

            //if (result == DialogResult.Cancel)
            //{
            //    e.Cancel = true; // Prevent the form from closing
            //}
            //else if (result == DialogResult.Yes)
            //{
            //    // Attempt to stop processes
            //    //Task.Run(() => _processManager.StopAllProcessesAsync()).Wait(5000);
            //}
        }


    }
}
