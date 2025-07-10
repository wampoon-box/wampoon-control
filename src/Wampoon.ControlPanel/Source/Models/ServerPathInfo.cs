using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wampoon.ControlPanel.Models
{

    /// <summary>
    /// Represents the configuration for a server with computed paths.
    /// </summary>
    public class ServerPathInfo
    {
        public string ServerName { get; set; }
        public string ExecutablePath { get; set; }
        public string ConfigPath { get; set; }
        public string ServerDirectory { get; set; }
        public string ServerBaseDirectory { get; set; }
        public bool IsAvailable { get; set; }
        public Dictionary<string, string> SpecialPaths { get; set; }

        public ServerPathInfo()
        {
            SpecialPaths = new Dictionary<string, string>();
        }
    }
}
