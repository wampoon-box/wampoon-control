using Frostybee.Pwamp.Controllers;
using Frostybee.Pwamp.Enums;
using Frostybee.Pwamp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Frostybee.Pwamp.MainForm;

namespace Frostybee.Pwamp.Controls
{
    public partial class ServerControlBase : UserControl
    {
        protected const string STATUS_STOPPED = "Stopped";
        protected const string STATUS_RUNNING = "Running";
        protected const string STATUS_STOPPING = "Stopping";
        protected const string STATUS_STARTING = "Starting";
        protected string ServiceName { get; set; }
        protected string DisplayName { get; set; }
        protected int PortNumber { get; set; }
        internal ServerManagerBase ServerManager { get; set; }

        public ServerControlBase()
        {
            InitializeComponent();
            SetupEventHandlers();
        }

        protected void SetupEventHandlers()
        {
            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
            btnRestart.Click += BtnRestart_Click;
        }

        protected async virtual void BtnRestart_Click(object sender, EventArgs e)
        {

        }

        protected async virtual void BtnStop_Click(object sender, EventArgs e)
        {
            await StopServer();
        }
        public async Task StopServer()
        {
            try
            {
                btnStart.Enabled = false;
                UpdateStatus(ServerStatus.Stopping);

                bool success = await ServerManager.StopAsync();
                if (success)
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    UpdateStatus(ServerStatus.Stopped);
                }
                else
                {
                    btnStop.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping {ServiceName}: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStop.Enabled = true;
                UpdateStatus(ServerStatus.Stopped);
            }
        }

        protected async virtual void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;
                UpdateStatus(ServerStatus.Starting);
                bool success = await ServerManager.StartAsync();
                if (success)
                {
                    UpdateStatus(ServerStatus.Running);
                    btnStop.Enabled = true;
                    btnStart.Enabled = false;
                }
                else
                {
                    btnStart.Enabled = true;
                    UpdateStatus(ServerStatus.Stopped);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting {ServiceName}: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                UpdateStatus(ServerStatus.Stopped);
            }
        }

        protected void UpdateStatus(ServerStatus status)
        {
            lblStatus.Text = $"{status}";
            lblStatus.Refresh();

            switch (status)
            {
                case ServerStatus.Stopped:
                    pcbServerStatus.BackColor = Color.Red;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.BackColor = Color.White;
                    break;
                case ServerStatus.Running:
                    pcbServerStatus.BackColor = Color.Green;
                    lblStatus.ForeColor = Color.DarkBlue;
                    lblStatus.BackColor = Color.FromArgb(200, 255, 200);
                    break;
                case ServerStatus.Stopping:
                    pcbServerStatus.BackColor = Color.Orange;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.BackColor = Color.Orange;
                    break;
                case ServerStatus.Starting:

                    break;
                case ServerStatus.Error:

                    break;
            }
        }


        protected virtual void LogMessage(string serverModule, string log, LogType logType = LogType.Default)
        {
            MainForm.Instance?.AddLog(serverModule, log, logType);
        }

        protected virtual void LogMessage(string log, LogType logType = LogType.Default)
        {
            LogMessage(ServiceName, log, logType);
        }
    }
}
