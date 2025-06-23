using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.Pwamp.Models
{
    /// <summary>
    /// Information about a server's configuration file.
    /// </summary>
    public class ServerConfigInfo
    {
        public string ServerName { get; set; }
        public string ConfigPath { get; set; }
        public bool Exists { get; set; }
        public string Size { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
