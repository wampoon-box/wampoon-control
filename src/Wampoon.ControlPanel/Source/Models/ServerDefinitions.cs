using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wampoon.ControlPanel.Models
{
    /// <summary>
    /// Contains all server definitions with their specific configurations.
    /// </summary>
    public static class ServerDefinitions
    {
        public static readonly ServerDefinitionInfo Apache = new ServerDefinitionInfo(
            name: "Apache",
            directory: "apache",
            executableName: "httpd.exe",
            configFile: "conf\\httpd.conf",
            specialDirectories: new Dictionary<string, string>
            {
            { "DocumentRoot", "..\\htdocs" },
            { "Logs", "logs" }
            }
        );

        public static readonly ServerDefinitionInfo MariaDB = new ServerDefinitionInfo(
            name: "MariaDB",
            directory: "mariadb",
            executableName: "mariadbd.exe",
            configFile: "my.ini",
            specialDirectories: new Dictionary<string, string>
            {
            { "Data", "data" },
            { "Client", "bin/mysql.exe" }  // Special case for client executable.
            }
        );


        public static readonly ServerDefinitionInfo php = new ServerDefinitionInfo(
            name: "PHP",
            directory: "php",
            executableName: "php.exe",
            configFile: "php.ini"
        );

        public static readonly ServerDefinitionInfo phpMyAdmin = new ServerDefinitionInfo(
            name: "phpMyAdmin",
            directory: "phpmyadmin",
            executableName: "na",
            configFile: "config.inc.php"            
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
        public static ServerDefinitionInfo[] GetAll()
        {
            return new ServerDefinitionInfo[] { Apache, MariaDB , phpMyAdmin};
        }

        /// <summary>
        /// Gets a server definition by name.
        /// </summary>
        public static ServerDefinitionInfo GetByName(string name)
        {
            return GetAll().FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
