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
        protected string ServiceName { get; set; }
        protected string DisplayName { get; set; }
        protected const string STATUS_STOPPED = "Stopped";
        protected const string STATUS_RUNNING = "Running";
        protected const string STATUS_STOPPING = "Stopping";
        protected const string STATUS_STARTING = "Starting";

        public ServerControlBase()
        {
            InitializeComponent();
            SetupEventHandlers();
        }

        protected virtual void SetupEventHandlers()
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
        }

        protected async virtual void BtnStart_Click(object sender, EventArgs e)
        {
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
