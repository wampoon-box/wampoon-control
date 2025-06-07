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

namespace Pwamp.Admin.UI
{
    public partial class ServerBaseControl : UserControl
    {
        protected string ServiceName { get; set; }
        protected string DisplayName { get; set; }

        public ServerBaseControl()
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
