using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwampConsole.Controllers
{
    /// <summary>
    /// Apache server manager implementation
    /// </summary>
    public class ApacheManager : ServerManagerBase
    {
        public ApacheManager(string baseDirectory = null)
            : base(baseDirectory ?? AppDomain.CurrentDomain.BaseDirectory, "Apache")
        {
        }

        protected override void InitializePaths(string baseDirectory)
        {
            // Path to Apache executable (httpd.exe) relative to the application
            _executablePath = Path.Combine(baseDirectory, "apache", "bin", "httpd.exe");

            // Path to Apache config file
            _configPath = Path.Combine(baseDirectory, "apache", "conf", "httpd.conf");
        }

        protected override string GetStartArguments()
        {
            return $"-k start -f \"{_configPath}\"";
        }

        protected override string GetStopArguments()
        {
            return $"-k stop -f \"{_configPath}\"";
        }

        protected override string GetRestartArguments()
        {
            return $"-k restart -f \"{_configPath}\"";
        }
    }
}
