using Pwamp.Admin.Controllers;
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
        MySQLManager _mysqlManager;
        public static MainForm Instance { get; private set; }
        private string apacheHttpdPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\bin\httpd.exe"; // CHANGE THIS

        private string configPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\apache\conf\httpd.conf"; // CHANGE THIS

        private string mysqlExecutablePath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\mariadb\bin\mariadbd.exe"; // CHANGE THIS
        private string mysqlConfigPath = @"D:\Dev\my-repos\pwamp\pwamp-bundle\apps\mariadb\my.ini"; // CHANGE THIS
        public MainForm()
        {
            Instance = this;
            InitializeComponent();
            Text = "PWAMP Control Panel";
            InitializeApplication();
        }
        private void InitializeApplication()
        {
            // Initialize MySQL Manager.
            _mysqlManager = new MySQLManager(mysqlExecutablePath, mysqlConfigPath);
            _mysqlManager.ErrorOccurred += LogError;
            _mysqlManager.StatusChanged += LogMessage;
            
            //--
            _apacheModule.InitializeModule();
            AddLog("Application initialized successfully", LogType.Info);
        }

        private void LogError(object sender, string message)
        {
            try
            {
                if (_logTextBox.InvokeRequired)
                {
                    _logTextBox.Invoke(new Action<object, string>(LogError), sender, message);
                    return;
                }
                _logTextBox.ForeColor = Color.Red;
                _logTextBox.AppendText(message + Environment.NewLine);
                _logTextBox.SelectionStart = _logTextBox.Text.Length;
                _logTextBox.ScrollToCaret();
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
            if (_logTextBox.InvokeRequired)
            {
                _logTextBox.Invoke(new Action<object, string>(LogMessage), sender, message);
                return;
            }
            _logTextBox.ForeColor = Color.Green;
            _logTextBox.AppendText(message + Environment.NewLine);
            _logTextBox.SelectionStart = _logTextBox.Text.Length;
            _logTextBox.ScrollToCaret();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("An error has occurred: " + ex.Message, "Error",
            //                  MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
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
        public enum LogType
        {
            Default,
            Info,
            Error,
            Debug,
            DebugDetails
        }
        private void AddLogInternal(string logEntry, LogType logType)
        {
            if (_logTextBox == null) return;

            Color textColor = GetLogColor(logType);

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

        private Color GetLogColor(LogType logType)
        {
            Color textColor;
            switch (logType)
            {
                case LogType.Error:
                    textColor = Color.Red;
                    break;
                case LogType.Info:
                    textColor = Color.LightBlue;
                    break;
                case LogType.Debug:
                    textColor = Color.Gray;
                    break;
                case LogType.DebugDetails:
                    textColor = Color.DarkGray;
                    break;
                default:
                    textColor = Color.White;
                    break;
            }
            return textColor;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up managers on application exit
            try
            {
                _apacheModule?.Dispose();
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
