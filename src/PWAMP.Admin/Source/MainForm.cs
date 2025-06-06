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
using System.Linq;

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
            try
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
            catch (Exception ex) 
            {
                MessageBox.Show("An error has occurred: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogMessage(object sender, string message)
        {
            //try
            //{
                if (txtOutputLog.InvokeRequired)
                {
                    txtOutputLog.Invoke(new Action<string, string>(LogMessage), message);
                    return;
                }
                txtOutputLog.ForeColor = Color.Green;
                txtOutputLog.AppendText(message + Environment.NewLine);
                txtOutputLog.SelectionStart = txtOutputLog.Text.Length;
                txtOutputLog.ScrollToCaret();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("An error has occurred: " + ex.Message, "Error",
            //                  MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }


        private async void BtnStartApache_Click(object sender, EventArgs e)
        {
            try
            {
                btnStartApache.Enabled = false;
                btnStartApache.Text = "Starting...";
                
                bool success = await _apacheManager.StartAsync();
                if (success)
                {
                    btnStartApache.Text = "Apache: Running";
                    btnStartApache.Enabled = true;
                }
                else
                {
                    btnStartApache.Text = "Start Apache";
                    btnStartApache.Enabled = true;
                    // Only dispose on failure - manager is reusable
                    if (_apacheManager != null)
                    {
                        _apacheManager.Dispose();
                        _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                        _apacheManager.ErrorOccurred += LogError;
                        _apacheManager.StatusChanged += LogMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStartApache.Text = "Start Apache";
                btnStartApache.Enabled = true;
                // Only dispose on unrecoverable error
                if (_apacheManager != null)
                {
                    _apacheManager.Dispose();
                    _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                    _apacheManager.ErrorOccurred += LogError;
                    _apacheManager.StatusChanged += LogMessage;
                }
            }
        }

        private async void BtnStopApache_Click(object sender, EventArgs e)
        {
            try
            {
                btnStopApache.Enabled = false;
                btnStopApache.Text = "Stopping...";
                
                bool success = await _apacheManager.StopAsync();
                if (success)
                {
                    btnStartApache.Text = "Start Apache";
                    btnStopApache.Text = "Stop Apache";
                    btnStartApache.Enabled = true;
                    btnStopApache.Enabled = true;
                    // Don't dispose manager - keep it for future use
                }
                else
                {
                    btnStopApache.Text = "Stop Apache";
                    btnStopApache.Enabled = true;
                    // Only dispose on failure
                    if (_apacheManager != null)
                    {
                        _apacheManager.Dispose();
                        _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                        _apacheManager.ErrorOccurred += LogError;
                        _apacheManager.StatusChanged += LogMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStopApache.Text = "Stop Apache";
                btnStopApache.Enabled = true;
                // Only dispose on unrecoverable error
                if (_apacheManager != null)
                {
                    _apacheManager.Dispose();
                    _apacheManager = new ApacheManager(apacheHttpdPath, configPath);
                    _apacheManager.ErrorOccurred += LogError;
                    _apacheManager.StatusChanged += LogMessage;
                }
            }
        }

        private async void BtnStartMySql_Click(object sender, EventArgs e)
        {
            try
            {
                btnStartMySql.Enabled = false;
                btnStartMySql.Text = "Starting...";
                
                bool success = await _mysqlManager.StartAsync();
                if (success)
                {
                    btnStartMySql.Text = "MariaDB: Running";
                    btnStartMySql.Enabled = true;
                }
                else
                {
                    btnStartMySql.Text = "Start MariaDB";
                    btnStartMySql.Enabled = true;
                    // Only dispose on failure - manager is reusable
                    if (_mysqlManager != null)
                    {
                        _mysqlManager.Dispose();
                        _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
                        _mysqlManager.ErrorOccurred += LogError;
                        _mysqlManager.StatusChanged += LogMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting MariaDB: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStartMySql.Text = "Start MariaDB";
                btnStartMySql.Enabled = true;
                // Only dispose on unrecoverable error
                if (_mysqlManager != null)
                {
                    _mysqlManager.Dispose();
                    _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
                    _mysqlManager.ErrorOccurred += LogError;
                    _mysqlManager.StatusChanged += LogMessage;
                }
            }
        }

        private async void BtnStopMysql_Click(object sender, EventArgs e)
        {
            try
            {
                btnStopMysql.Enabled = false;
                btnStopMysql.Text = "Stopping...";
                
                bool success = await _mysqlManager.StopAsync();
                if (success)
                {
                    btnStartMySql.Text = "Start MariaDB";
                    btnStopMysql.Text = "Stop MariaDB";
                    btnStartMySql.Enabled = true;
                    btnStopMysql.Enabled = true;
                    // Don't dispose manager - keep it for future use
                }
                else
                {
                    btnStopMysql.Text = "Stop MariaDB";
                    btnStopMysql.Enabled = true;
                    // Only dispose on failure
                    if (_mysqlManager != null)
                    {
                        _mysqlManager.Dispose();
                        _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
                        _mysqlManager.ErrorOccurred += LogError;
                        _mysqlManager.StatusChanged += LogMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping MariaDB: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStopMysql.Text = "Stop MariaDB";
                btnStopMysql.Enabled = true;
                // Only dispose on unrecoverable error
                if (_mysqlManager != null)
                {
                    _mysqlManager.Dispose();
                    _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
                    _mysqlManager.ErrorOccurred += LogError;
                    _mysqlManager.StatusChanged += LogMessage;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up managers on application exit
            try
            {
                if (_apacheManager != null)
                {
                    _apacheManager.Dispose();
                    _apacheManager = null;
                }
                if (_mysqlManager != null)
                {
                    _mysqlManager.Dispose();
                    _mysqlManager = null;
                }
            }
            catch (Exception ex)
            {
                // Log error but don't prevent closing
                System.Diagnostics.Debug.WriteLine($"Error during cleanup: {ex.Message}");
            }
        }


    }
}
