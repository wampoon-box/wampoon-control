using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.Pwamp.Models
{
    /// <summary>
    /// Contains all server definitions with their specific configurations.
    /// </summary>
    public static class ServerDefinitions
    {
        public static readonly ServerDefinition Apache = new ServerDefinition(
            name: "Apache",
            directory: "apache",
            executableName: "httpd.exe",
            configFile: "conf/httpd.conf",
            specialDirectories: new Dictionary<string, string>
            {
            { "DocumentRoot", "htdocs" },
            { "Logs", "logs" }
            }
        );

        public static readonly ServerDefinition MariaDB = new ServerDefinition(
            name: "MariaDB",
            directory: "mariadb",
            executableName: "mysqld.exe",
            configFile: "my.ini",
            specialDirectories: new Dictionary<string, string>
            {
            { "Data", "data" },
            { "Client", "bin/mysql.exe" }  // Special case for client executable.
            }
        );

        // Add more server definitions here as needed
        // public static readonly ServerDefinition PHP = new ServerDefinition(
        //     name: "PHP",
        //     directory: "php",
        //     executableName: "php.exe"
        // );

        /// <summary>
        /// Gets all defined servers.
        /// </summary>
        public static ServerDefinition[] GetAll()
        {
            return new ServerDefinition[] { Apache, MariaDB };
        }

        /// <summary>
        /// Gets a server definition by name.
        /// </summary>
        public static ServerDefinition GetByName(string name)
        {
            return GetAll().FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
