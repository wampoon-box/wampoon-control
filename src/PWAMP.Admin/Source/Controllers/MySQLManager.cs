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

        public MySQLManager(string executablePath, string configPath = null) : base(executablePath, configPath)
        {
        }

        protected override ProcessStartInfo GetProcessStartInfo()
        {
            return new ProcessStartInfo(_executablePath)
            {
                UseShellExecute = true, // MySQL typically runs in its own console window.
                CreateNoWindow = false, // We want to see the MySQL console window.                
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = GetStartArguments()
            };
        }

        protected override string GetStartArguments()
        {
            throw new NotImplementedException();
        }

        protected override int GetStartupDelay()
        {
            // Allocate 5 seconds delay needed for MySQL to start up.
            return 5000;
        }

        protected override Task<bool> PerformGracefulShutdown()
        {
            throw new NotImplementedException();
        }
    }
}
