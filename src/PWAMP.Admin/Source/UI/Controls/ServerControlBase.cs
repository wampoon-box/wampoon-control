using Frostybee.PwampAdmin.Controllers;
using Frostybee.PwampAdmin.Enums;
using Frostybee.PwampAdmin.Helpers;
using static Frostybee.PwampAdmin.Helpers.ErrorLogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Frostybee.PwampAdmin.MainForm;

namespace Frostybee.PwampAdmin.Controls
{
    public partial class ServerControlBase : UserControl
    {        
        protected string ServiceName { get; set; }
        protected string DisplayName { get; set; }
        protected int PortNumber { get; set; } 
        protected int ProcessId { get; set; } 
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
                if (!ServerManager.IsRunning)
                {
                    return;
                }
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
                ErrorLogHelper.LogExceptionInfo(ex);
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
                ErrorLogHelper.LogExceptionInfo(ex);
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
                    ApplyControlStyle(Color.Red, Color.DarkRed, Color.WhiteSmoke);
                    break;
                case ServerStatus.Running:
                    ApplyControlStyle(Color.Green, Color.DarkBlue, Color.FromArgb(200, 255, 200)); 
                    break;
                case ServerStatus.Stopping:
                    ApplyControlStyle(Color.Orange, Color.Blue, Color.FromArgb(243, 156, 18));
                    break;
                case ServerStatus.Starting:
                    ApplyControlStyle(Color.Orange, Color.Blue, Color.FromArgb(243, 156, 18));
                    break;
                case ServerStatus.Error:

                    break;
            }
        }
       
        private void ApplyControlStyle(Color statusColor, Color lblForeColor, Color lblBackColor)
        {
            pcbServerStatus.BackColor = statusColor;
            lblStatus.ForeColor = lblForeColor;
            lblStatus.BackColor = lblBackColor;
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
