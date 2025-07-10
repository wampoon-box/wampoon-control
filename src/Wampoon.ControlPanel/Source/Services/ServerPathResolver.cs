using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Frostybee.Pwamp.Interfaces;
using Frostybee.Pwamp.Models;

namespace Frostybee.Pwamp.Services
{
    public class ServerPathResolver
    {
        private readonly IFileOperations _fileOperations;
        private readonly string _applicationDirectory;
        private readonly string _appsDirectory;
        private readonly Dictionary<string, ServerPathInfo> _serverPaths;

        public string ApplicationDirectory => _applicationDirectory;
        public string AppsDirectory => _appsDirectory;
        public string ApacheDocumentRoot => Path.Combine(_applicationDirectory, "htdocs");

        public ServerPathResolver(IFileOperations fileOperations)
        {
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
            _applicationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _appsDirectory = Path.Combine(_applicationDirectory, "apps");
            _serverPaths = new Dictionary<string, ServerPathInfo>();
            InitializeServerPaths();
        }

        private void InitializeServerPaths()
        {
            var serverDefinitions = ServerDefinitions.GetAll();

            foreach (var definition in serverDefinitions)
            {
                var pathInfo = CreateServerPathInfo(definition);
                _serverPaths[definition.Name] = pathInfo;
            }
        }

        private ServerPathInfo CreateServerPathInfo(ServerDefinitionInfo definition)
        {
            var serverBaseDir = Path.Combine(_appsDirectory, definition.Directory);
            var serverBinDir = Path.Combine(serverBaseDir, "bin");
            var executablePath = Path.Combine(serverBinDir, definition.ExecutableName);

            string configPath = null;
            if (!string.IsNullOrEmpty(definition.ConfigFile))
            {
                configPath = Path.Combine(serverBaseDir, definition.ConfigFile);
            }

            var pathInfo = new ServerPathInfo
            {
                ServerName = definition.Name,
                ServerDirectory = serverBinDir,
                ServerBaseDirectory = serverBaseDir,
                ExecutablePath = executablePath,
                ConfigPath = configPath,
                IsAvailable = _fileOperations.FileExists(executablePath)
            };

            // Compute special paths
            foreach (var specialDir in definition.SpecialDirectories)
            {
                var specialPath = Path.Combine(serverBaseDir, specialDir.Value);
                pathInfo.SpecialPaths[specialDir.Key] = specialPath;
            }

            return pathInfo;
        }

        public ServerPathInfo GetServerPath(string serverName)
        {
            if (string.IsNullOrWhiteSpace(serverName))
                return null;

            return _serverPaths.TryGetValue(serverName, out var pathInfo) ? pathInfo : null;
        }

        public Dictionary<string, ServerPathInfo> GetAllServerPaths()
        {
            return new Dictionary<string, ServerPathInfo>(_serverPaths);
        }

        public List<ServerPathInfo> GetAvailableServers()
        {
            return _serverPaths.Values.Where(s => s.IsAvailable).ToList();
        }

        public string GetExecutablePath(string serverName)
        {
            var serverPath = GetServerPath(serverName);
            return serverPath?.ExecutablePath;
        }

        public string GetConfigPath(string serverName)
        {
            var serverPath = GetServerPath(serverName);
            return serverPath?.ConfigPath;
        }

        public string GetSpecialPath(string serverName, string pathType)
        {
            var pathInfo = GetServerPath(serverName);
            if (pathInfo == null)
                return null;

            return pathInfo.SpecialPaths.TryGetValue(pathType, out var path) ? path : null;
        }

        public bool IsServerAvailable(string serverName)
        {
            var pathInfo = GetServerPath(serverName);
            return pathInfo != null && pathInfo.IsAvailable;
        }

        public string GetServerBinDirectory(string serverName)
        {
            var serverPath = GetServerPath(serverName);
            return serverPath?.ServerDirectory;
        }

        public string GetServerBaseDirectory(string serverName)
        {
            var serverPath = GetServerPath(serverName);
            return serverPath?.ServerBaseDirectory;
        }

        public List<string> GetServerNames()
        {
            return _serverPaths.Keys.ToList();
        }

        public List<string> GetAvailableServerNames()
        {
            return _serverPaths.Values.Where(s => s.IsAvailable).Select(s => s.ServerName).ToList();
        }

        public void RefreshServerAvailability()
        {
            foreach (var pathInfo in _serverPaths.Values)
            {
                pathInfo.IsAvailable = _fileOperations.FileExists(pathInfo.ExecutablePath);
            }
        }
    }
}