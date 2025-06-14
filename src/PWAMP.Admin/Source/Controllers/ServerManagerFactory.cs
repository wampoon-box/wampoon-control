using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.PwampAdmin.Controllers
{
    /// <summary>
    /// Extension methods for ServerManagerBase to work with the path manager.
    /// </summary>
    public static class ServerManagerPathExtensions
    {
        /// <summary>
        /// Creates a server manager instance using paths from the path manager.
        /// </summary>
        public static T CreateFromPaths<T>(string serverName) where T : class
        {
            var pathInfo = ServerPathManager.GetServerPath(serverName) ?? throw new ArgumentException(string.Format("Server configuration not found: {0}", serverName));
            if (!pathInfo.IsAvailable)
            {
                throw new InvalidOperationException(string.Format("Server executable not found: {0}", pathInfo.ExecutablePath));
            }

            // Use reflection to create the server manager instance.
            return (T)Activator.CreateInstance(typeof(T), pathInfo.ExecutablePath, pathInfo.ConfigPath);
        }

        /// <summary>
        /// Validates that a server is available before creating a manager.
        /// </summary>
        public static bool CanCreateServerManager(string serverName)
        {
            return ServerPathManager.IsServerAvailable(serverName);
        }
    }

    /// <summary>
    /// Utility class for creating server managers with error handling.
    /// </summary>
    public static class ServerManagerFactory
    {
        /// <summary>
        /// Safely creates a server manager with proper error handling.
        /// </summary>
        public static T CreateServerManager<T>(string serverName) where T : class
        {
            try
            {
                if (!ServerPathManager.IsServerAvailable(serverName))
                {
                    throw new InvalidOperationException(string.Format("Server '{0}' is not available. Check if the executable exists.", serverName));
                }

                return ServerManagerPathExtensions.CreateFromPaths<T>(serverName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Failed to create server manager for '{0}': {1}", serverName, ex.Message), ex);
            }
        }

        /// <summary>
        /// Gets a list of servers that can be successfully created.
        /// </summary>
        public static List<string> GetCreatableServers()
        {
            return ServerPathManager.GetServerNames()
                .Where(ServerPathManager.IsServerAvailable)
                .ToList();
        }
    }
}
