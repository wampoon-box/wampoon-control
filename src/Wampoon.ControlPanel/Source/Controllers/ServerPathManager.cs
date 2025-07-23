using System;
using System.Collections.Generic;
using System.Linq;
using Wampoon.ControlPanel.Models;
using Wampoon.ControlPanel.Services;
using Wampoon.ControlPanel.Helpers;

namespace Wampoon.ControlPanel.Controllers
{
    /// <summary>
    /// Facade for server path management using composed services.
    /// </summary>
    public static class ServerPathManager
    {
        private static readonly ServerPathResolver _pathResolver;
        private static readonly ServerFileOperations _fileOperations;
        private static readonly ServerDiagnostics _diagnostics;
        
        // Port storage for servers.
        private static readonly Dictionary<string, int> _serverPorts = new Dictionary<string, int>();

        static ServerPathManager()
        {
            var fileOps = new FileOperations();
            _pathResolver = new ServerPathResolver(fileOps);
            _fileOperations = new ServerFileOperations(fileOps, _pathResolver);
            _diagnostics = new ServerDiagnostics(fileOps, _pathResolver);
        }

        public static string AppsDirectory => _pathResolver.AppsDirectory;
        public static string ApacheDocumentRoot => _pathResolver.ApacheDocumentRoot;

        public static ServerPathInfo GetServerPath(string serverName) => _pathResolver.GetServerPath(serverName);
        public static string GetExecutablePath(string serverName) => _pathResolver.GetExecutablePath(serverName);
        public static string GetConfigPath(string serverName) => _pathResolver.GetConfigPath(serverName);
        public static string GetSpecialPath(string serverName, string pathType) => _pathResolver.GetSpecialPath(serverName, pathType);
        public static bool IsServerAvailable(string serverName) => _pathResolver.IsServerAvailable(serverName);
        public static string GetServerBaseDirectory(string serverName) => _pathResolver.GetServerBaseDirectory(serverName);
        public static List<string> GetServerNames() => _pathResolver.GetServerNames();


        public static void LogApacheDiagnostics() => _diagnostics.LogApacheDiagnostics();

        /// <summary>
        /// Gets the port number for a specific server.
        /// </summary>
        /// <param name="serverName">Name of the server (e.g., "Apache", "MariaDB")</param>
        /// <returns>Port number or default port if not set</returns>
        public static int GetServerPort(string serverName)
        {
            if (_serverPorts.TryGetValue(serverName, out int port))
            {
                return port;
            }

            // Return default ports if not set
            switch (serverName.ToLower())
            {
                case "apache":
                    return AppConstants.Ports.APACHE_DEFAULT;
                case "mariadb":
                    return AppConstants.Ports.MYSQL_DEFAULT;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Sets the port number for a specific server.
        /// </summary>
        /// <param name="serverName">Name of the server (e.g., "Apache", "MariaDB")</param>
        /// <param name="port">Port number to set</param>
        public static void SetServerPort(string serverName, int port)
        {
            _serverPorts[serverName] = port;
        }

        /// <summary>
        /// Gets the Apache server port number.
        /// </summary>
        public static int ApachePort => GetServerPort("Apache");

        /// <summary>
        /// Gets the MySQL/MariaDB server port number.
        /// </summary>
        public static int MySqlPort => GetServerPort("MariaDB");
    }    
}
