using Pwamp.Admin.Controllers;
using Pwamp.Admin.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Pwamp.Admin.MainForm;

namespace Pwamp.Admin.Controls
{
    public partial class ServerControlBase : UserControl
    {
        protected const string STATUS_STOPPED = "Stopped";
        protected const string STATUS_RUNNING = "Running";
        protected const string STATUS_STOPPING = "Stopping";
        protected const string STATUS_STARTING = "Starting";
        protected string ServiceName { get; set; }
        protected string DisplayName { get; set; }
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
            try
            {
                btnStart.Enabled = false;
                UpdateStatus(STATUS_STOPPING);

                bool success = await ServerManager.StopAsync();
                if (success)
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    UpdateStatus(STATUS_STOPPED);
                }
                else
                {
                    btnStop.Enabled = true;                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStop.Enabled = true;
                UpdateStatus(STATUS_STOPPED);                
            }
        }

        protected async virtual void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;
                UpdateStatus(STATUS_STARTING);
                bool success = await ServerManager.StartAsync();
                if (success)
                {
                    UpdateStatus(STATUS_RUNNING);
                    btnStop.Enabled = true;
                    btnStart.Enabled = false;
                }
                else
                {
                    btnStart.Enabled = true;
                    UpdateStatus(STATUS_STOPPED);                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting Apache: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                UpdateStatus(STATUS_STOPPED);                
            }
        }
        protected void UpdateStatus(string status)
        {
            lblStatus.Text = $"{status}";
            lblStatus.Refresh();
            if (status == STATUS_RUNNING)
            {                
                pcbServerStatus.BackColor = Color.Green;
                lblStatus.ForeColor = Color.DarkBlue;
                lblStatus.BackColor= Color.LightGreen;

            }
            else
            {
                pcbServerStatus.BackColor = Color.Red;
                lblStatus.ForeColor = Color.Red;
                lblStatus.BackColor = Color.White;
            }
        }
        protected virtual void AddLog(string module, string log, LogType logType = LogType.Default)
        {
            MainForm.Instance?.AddLog(module, log, logType);
        }

        protected virtual void AddLog(string log, LogType logType = LogType.Default)
        {
            AddLog(ServiceName, log, logType);
        }
    }
}
