using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pwamp.Admin.Controllers
{
    internal class MySQLManager : ServerManagerBase
    {
        public override string ServerName { get; set; } = "MySQL/MariaDB";
        protected override bool CanMonitorOutput { get; set; } = true;

        public MySQLManager(string executablePath, string configPath = null) : base(executablePath, configPath)
        {
        }

        protected override ProcessStartInfo GetProcessStartInfo()
        {
            return new ProcessStartInfo()
            {
                FileName = _executablePath,
                UseShellExecute = false, 
                CreateNoWindow = true, 
                RedirectStandardError = true, 
                RedirectStandardOutput = true, 
                WindowStyle = ProcessWindowStyle.Normal,         
            };
        }

        protected override string GetStartArguments()
        {
            // Empty for now.
            return string.Empty;
        }

        protected override int GetStartupDelay()
        {
            // Allocate 3 seconds delay needed for MySQL to start up.
            return 3000;
        }

        protected override Task<bool> PerformGracefulShutdown()
        {
            throw new NotImplementedException();
        }
    }
}
