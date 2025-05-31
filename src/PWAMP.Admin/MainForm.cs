using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pwamp.Forms;
using Pwamp.Helpers;
using Pwamp.Models;

namespace Pwamp.Admin
{
    public partial class MainForm : Form
    {
        
        public MainForm()
        {
            InitializeComponent();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            // Set form title
            this.Text = "PWAMP Control Panel";
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
