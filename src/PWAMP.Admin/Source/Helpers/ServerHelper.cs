using Frostybee.Pwamp.Models;
using Frostybee.PwampAdmin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.PwampAdmin.Helpers
{
    /// <summary>
    /// Simplified generic helper for server-specific paths and operations.
    /// </summary>
    public static class ServerHelper
    {
        /// <summary>
        /// Gets the executable path for any server.
        /// </summary>
        public static string GetExecutablePath(string serverName)
        {
            return ServerPathManager.GetExecutablePath(serverName);
        }

        /// <summary>
        /// Gets the configuration path for any server.
        /// </summary>
        public static string GetConfigPath(string serverName)
        {
            return ServerPathManager.GetConfigPath(serverName);
        }

        /// <summary>
        /// Gets a special directory path for any server (DocumentRoot, Data, Logs, etc.).
        /// </summary>
        public static string GetSpecialPath(string serverName, string pathType)
        {
            return ServerPathManager.GetSpecialPath(serverName, pathType);
        }

        /// <summary>
        /// Opens the configuration file for any server.
        /// </summary>
        public static bool OpenConfigFile(string serverName)
        {
            return ServerConfigHelper.OpenServerConfigFile(serverName);
        }

        /// <summary>
        /// Checks if the configuration file can be opened for any server.
        /// </summary>
        public static bool CanOpenConfigFile(string serverName)
        {
            return ServerPathManager.CanOpenConfigFile(serverName);
        }

        // Convenience methods for common operations using server definitions.
        public static string GetApacheDocumentRoot()
        {
            return GetSpecialPath(ServerDefinitions.Apache.Name, "DocumentRoot");
        }

        public static string GetApacheLogsDirectory()
        {
            return GetSpecialPath(ServerDefinitions.Apache.Name, "Logs");
        }

        public static string GetMariaDBDataDirectory()
        {
            return GetSpecialPath(ServerDefinitions.MariaDB.Name, "Data");
        }

        public static string GetMariaDBClientExecutablePath()
        {
            return GetSpecialPath(ServerDefinitions.MariaDB.Name, "Client");
        }

        // Alternative approach: Generic methods that work with any server definition.
        /// <summary>
        /// Gets the executable path for a server using its definition.
        /// </summary>
        public static string GetExecutablePath(ServerDefinition serverDefinition)
        {
            return serverDefinition != null ? GetExecutablePath(serverDefinition.Name) : null;
        }

        /// <summary>
        /// Gets the configuration path for a server using its definition.
        /// </summary>
        public static string GetConfigPath(ServerDefinition serverDefinition)
        {
            return serverDefinition != null ? GetConfigPath(serverDefinition.Name) : null;
        }

        /// <summary>
        /// Gets a special path for a server using its definition.
        /// </summary>
        public static string GetSpecialPath(ServerDefinition serverDefinition, string pathType)
        {
            return serverDefinition != null ? GetSpecialPath(serverDefinition.Name, pathType) : null;
        }

        /// <summary>
        /// Opens the configuration file for a server using its definition.
        /// </summary>
        public static bool OpenConfigFile(ServerDefinition serverDefinition)
        {
            return serverDefinition != null && OpenConfigFile(serverDefinition.Name);
        }

        /// <summary>
        /// Checks if the configuration file can be opened for a server using its definition.
        /// </summary>
        public static bool CanOpenConfigFile(ServerDefinition serverDefinition)
        {
            return serverDefinition != null && CanOpenConfigFile(serverDefinition.Name);
        }
    }
}
