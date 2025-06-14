using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.Pwamp.Models
{
    /// <summary>
    /// Defines the static configuration for a server type.
    /// </summary>
    public class ServerDefinitionInfo
    {
        public string Name { get; set; }
        public string Directory { get; set; }
        public string ExecutableName { get; set; }
        public string ConfigFile { get; set; }
        public Dictionary<string, string> SpecialDirectories { get; set; }

        public ServerDefinitionInfo(string name, string directory, string executableName, string configFile = null, Dictionary<string, string> specialDirectories = null)
        {
            Name = name;
            Directory = directory;
            ExecutableName = executableName;
            ConfigFile = configFile;
            SpecialDirectories = specialDirectories ?? new Dictionary<string, string>();
        }
    }
}
