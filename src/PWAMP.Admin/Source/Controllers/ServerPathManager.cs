using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Frostybee.Pwamp.Models;
using Frostybee.PwampAdmin.Helpers;

namespace Frostybee.PwampAdmin.Controllers
{
    /// <summary>
    /// Manages server paths based on the application's directory structure.
    /// </summary>
    public static class ServerPathManager
    {
        private static readonly string _applicationDirectory;
        private static readonly string _appsDirectory;
        private static readonly Dictionary<string, ServerPathInfo> _serverPaths;

        static ServerPathManager()
        {
            // Get the directory where myApp.exe is located
            _applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _appsDirectory = Path.Combine(_applicationDirectory, "apps");

            _serverPaths = new Dictionary<string, ServerPathInfo>();
            InitializeServerPaths();
        }

        /// <summary>
        /// Initializes the server paths based on the known directory structure
        /// </summary>
        private static void InitializeServerPaths()
        {
            var serverDefinitions = ServerDefinitions.GetAll();

            foreach (var definition in serverDefinitions)
            {
                var serverBaseDir = Path.Combine(_appsDirectory, definition.Directory);
                var serverBinDir = Path.Combine(serverBaseDir, "bin");
                var executablePath = Path.Combine(serverBinDir, definition.ExecutableName);

                string configPath = null;
                if (!string.IsNullOrEmpty(definition.ConfigFile))
                {
                    configPath = Path.Combine(serverBaseDir, definition.ConfigFile);
                    if (!File.Exists(configPath))
                    {
                        configPath = null;
                    }
                }

                var pathInfo = new ServerPathInfo
                {
                    ServerName = definition.Name,
                    ServerDirectory = serverBinDir,
                    ServerBaseDirectory = serverBaseDir,
                    ExecutablePath = executablePath,
                    ConfigPath = configPath,
                    IsAvailable = File.Exists(executablePath)
                };

                // Compute special paths
                foreach (var specialDir in definition.SpecialDirectories)
                {
                    var specialPath = specialDir.Value.Contains(".exe")
                        ? Path.Combine(serverBaseDir, specialDir.Value)  // For executable files
                        : Path.Combine(serverBaseDir, specialDir.Value); // For directories

                    pathInfo.SpecialPaths[specialDir.Key] = specialPath;
                }

                _serverPaths[definition.Name] = pathInfo;
            }
        }

        /// <summary>
        /// Gets the path information for a specific server
        /// </summary>
        public static ServerPathInfo GetServerPath(string serverName)
        {
            if (string.IsNullOrWhiteSpace(serverName))
                return null;

            ServerPathInfo pathInfo;
            return _serverPaths.TryGetValue(serverName, out pathInfo) ? pathInfo : null;
        }

        /// <summary>
        /// Gets all available server path information
        /// </summary>
        public static Dictionary<string, ServerPathInfo> GetAllServerPaths()
        {
            return new Dictionary<string, ServerPathInfo>(_serverPaths);
        }

        /// <summary>
        /// Gets only the servers that are available (executable exists)
        /// </summary>
        public static List<ServerPathInfo> GetAvailableServers()
        {
            return _serverPaths.Values.Where(s => s.IsAvailable).ToList();
        }

        /// <summary>
        /// Gets the executable path for a server
        /// </summary>
        public static string GetExecutablePath(string serverName)
        {
            var serverPath = GetServerPath(serverName);
            return serverPath != null ? serverPath.ExecutablePath : null;
        }

        /// <summary>
        /// Gets the configuration file path for a server
        /// </summary>
        public static string GetConfigPath(string serverName)
        {
            var serverPath = GetServerPath(serverName);
            return serverPath != null ? serverPath.ConfigPath : null;
        }

        /// <summary>
        /// Gets a special path for a server (e.g., DocumentRoot, Data directory, etc.)
        /// </summary>
        public static string GetSpecialPath(string serverName, string pathType)
        {
            var pathInfo = GetServerPath(serverName);
            if (pathInfo == null)
                return null;

            string path;
            return pathInfo.SpecialPaths.TryGetValue(pathType, out path) ? path : null;
        }

        /// <summary>
        /// Checks if a server is available (executable exists)
        /// </summary>
        public static bool IsServerAvailable(string serverName)
        {
            var pathInfo = GetServerPath(serverName);
            return pathInfo != null && pathInfo.IsAvailable;
        }

        /// <summary>
        /// Gets the server directory (bin folder)
        /// </summary>
        public static string GetServerDirectory(string serverName)
        {
            var serverPath = GetServerPath(serverName);
            return serverPath != null ? serverPath.ServerDirectory : null;
        }

        /// <summary>
        /// Gets the server base directory (parent of bin folder)
        /// </summary>
        public static string GetServerBaseDirectory(string serverName)
        {
            var serverPath = GetServerPath(serverName);
            return serverPath != null ? serverPath.ServerBaseDirectory : null;
        }

        /// <summary>
        /// Gets a list of all configured server names
        /// </summary>
        public static List<string> GetServerNames()
        {
            return _serverPaths.Keys.ToList();
        }

        /// <summary>
        /// Gets a list of available server names
        /// </summary>
        public static List<string> GetAvailableServerNames()
        {
            return _serverPaths.Values.Where(s => s.IsAvailable).Select(s => s.ServerName).ToList();
        }

        /// <summary>
        /// Refreshes the availability status of all servers (re-checks if executables exist)
        /// </summary>
        public static void RefreshServerAvailability()
        {
            foreach (var pathInfo in _serverPaths.Values)
            {
                pathInfo.IsAvailable = File.Exists(pathInfo.ExecutablePath);

                // Also refresh config path availability
                if (!string.IsNullOrEmpty(pathInfo.ConfigPath))
                {
                    if (!File.Exists(pathInfo.ConfigPath))
                    {
                        pathInfo.ConfigPath = null;
                    }
                }
            }
        }

        /// <summary>
        /// Opens the configuration file for a server in the default system editor
        /// </summary>
        public static bool OpenConfigFile(string serverName)
        {
            try
            {
                var configPath = GetConfigPath(serverName);
                if (string.IsNullOrEmpty(configPath))
                {
                    return false; // No config file defined or found
                }

                if (!File.Exists(configPath))
                {
                    return false; // Config file doesn't exist
                }

                return OpenFileInDefaultEditor(configPath);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Opens any file in the default system editor
        /// </summary>
        public static bool OpenFileInDefaultEditor(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    return false;
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true,
                    Verb = "open"
                };

                Process.Start(startInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a server has a configuration file that can be opened
        /// </summary>
        public static bool CanOpenConfigFile(string serverName)
        {
            var configPath = GetConfigPath(serverName);
            return !string.IsNullOrEmpty(configPath) && File.Exists(configPath);
        }

        /// <summary>
        /// Gets the file size of a configuration file in a human-readable format
        /// </summary>
        public static string GetConfigFileSize(string serverName)
        {
            try
            {
                var configPath = GetConfigPath(serverName);
                if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
                {
                    return "N/A";
                }

                var fileInfo = new FileInfo(configPath);
                long bytes = fileInfo.Length;

                if (bytes < 1024)
                    return string.Format("{0} B", bytes);
                else if (bytes < 1024 * 1024)
                    return string.Format("{0:F1} KB", bytes / 1024.0);
                else
                    return string.Format("{0:F1} MB", bytes / (1024.0 * 1024.0));
            }
            catch (Exception)
            {
                return "N/A";
            }
        }

        /// <summary>
        /// Gets the last modified date of a configuration file
        /// </summary>
        public static DateTime? GetConfigFileLastModified(string serverName)
        {
            try
            {
                var configPath = GetConfigPath(serverName);
                if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
                {
                    return null;
                }

                return File.GetLastWriteTime(configPath);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets diagnostic information for troubleshooting
        /// </summary>
        public static Dictionary<string, string> GetDiagnosticInfo()
        {
            var diagnostics = new Dictionary<string, string>();
            diagnostics["Application Directory"] = _applicationDirectory;
            diagnostics["Apps Directory"] = _appsDirectory;
            diagnostics["Apps Directory Exists"] = Directory.Exists(_appsDirectory).ToString();

            foreach (var kvp in _serverPaths)
            {
                var pathInfo = kvp.Value;
                diagnostics[string.Format("{0} - Executable Path", kvp.Key)] = pathInfo.ExecutablePath;
                diagnostics[string.Format("{0} - Executable Exists", kvp.Key)] = File.Exists(pathInfo.ExecutablePath).ToString();
                diagnostics[string.Format("{0} - Config Path", kvp.Key)] = pathInfo.ConfigPath ?? "Not configured";
                diagnostics[string.Format("{0} - Config Exists", kvp.Key)] = (pathInfo.ConfigPath != null && File.Exists(pathInfo.ConfigPath)).ToString();
            }

            return diagnostics;
        }
    }

    

    /// <summary>
    /// Extension methods for ServerManagerBase to work with the path manager
    /// </summary>
    public static class ServerManagerPathExtensions
    {
        /// <summary>
        /// Creates a server manager instance using paths from the path manager
        /// </summary>
        public static T CreateFromPaths<T>(string serverName) where T : class
        {
            var pathInfo = ServerPathManager.GetServerPath(serverName);
            if (pathInfo == null)
                throw new ArgumentException(string.Format("Server configuration not found: {0}", serverName));

            if (!pathInfo.IsAvailable)
                throw new InvalidOperationException(string.Format("Server executable not found: {0}", pathInfo.ExecutablePath));

            // Use reflection to create the server manager instance
            return (T)Activator.CreateInstance(typeof(T), pathInfo.ExecutablePath, pathInfo.ConfigPath);
        }

        /// <summary>
        /// Validates that a server is available before creating a manager
        /// </summary>
        public static bool CanCreateServerManager(string serverName)
        {
            return ServerPathManager.IsServerAvailable(serverName);
        }
    }

    /// <summary>
    /// Utility class for creating server managers with error handling
    /// </summary>
    public static class ServerManagerFactory
    {
        /// <summary>
        /// Safely creates a server manager with proper error handling
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
        /// Gets a list of servers that can be successfully created
        /// </summary>
        public static List<string> GetCreatableServers()
        {
            return ServerPathManager.GetServerNames()
                .Where(ServerPathManager.IsServerAvailable)
                .ToList();
        }
    }
}
