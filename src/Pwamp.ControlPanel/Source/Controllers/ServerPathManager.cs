using System;
using System.Collections.Generic;
using System.Linq;
using Frostybee.Pwamp.Models;
using Frostybee.Pwamp.Services;

namespace Frostybee.Pwamp.Controllers
{
    /// <summary>
    /// Facade for server path management using composed services.
    /// </summary>
    public static class ServerPathManager
    {
        private static readonly ServerPathResolver _pathResolver;
        private static readonly ServerFileOperations _fileOperations;
        private static readonly ServerDiagnostics _diagnostics;

        static ServerPathManager()
        {
            var fileOps = new FileOperations();
            _pathResolver = new ServerPathResolver(fileOps);
            _fileOperations = new ServerFileOperations(fileOps, _pathResolver);
            _diagnostics = new ServerDiagnostics(fileOps, _pathResolver);
        }

        public static string ApplicationDirectory => _pathResolver.ApplicationDirectory;
        public static string AppsDirectory => _pathResolver.AppsDirectory;
        public static string ApacheDocumentRoot => _pathResolver.ApacheDocumentRoot;

        public static ServerPathInfo GetServerPath(string serverName) => _pathResolver.GetServerPath(serverName);
        public static Dictionary<string, ServerPathInfo> GetAllServerPaths() => _pathResolver.GetAllServerPaths();
        public static List<ServerPathInfo> GetAvailableServers() => _pathResolver.GetAvailableServers();
        public static string GetExecutablePath(string serverName) => _pathResolver.GetExecutablePath(serverName);
        public static string GetConfigPath(string serverName) => _pathResolver.GetConfigPath(serverName);
        public static string GetSpecialPath(string serverName, string pathType) => _pathResolver.GetSpecialPath(serverName, pathType);
        public static bool IsServerAvailable(string serverName) => _pathResolver.IsServerAvailable(serverName);
        public static string GetServerBinDirectory(string serverName) => _pathResolver.GetServerBinDirectory(serverName);
        public static string GetServerBaseDirectory(string serverName) => _pathResolver.GetServerBaseDirectory(serverName);
        public static List<string> GetServerNames() => _pathResolver.GetServerNames();
        public static List<string> GetAvailableServerNames() => _pathResolver.GetAvailableServerNames();
        public static void RefreshServerAvailability() => _pathResolver.RefreshServerAvailability();

        public static bool OpenConfigFile(string serverName) => _fileOperations.OpenConfigFile(serverName);
        public static bool OpenFileInDefaultEditor(string filePath) => _fileOperations.OpenFileInDefaultEditor(filePath);
        public static bool CanOpenConfigFile(string serverName) => _fileOperations.CanOpenConfigFile(serverName);
        public static string GetConfigFileSize(string serverName) => _fileOperations.GetConfigFileSize(serverName);
        public static DateTime? GetConfigFileLastModified(string serverName) => _fileOperations.GetConfigFileLastModified(serverName);

        public static Dictionary<string, string> GetDiagnosticInfo() => _diagnostics.GetDiagnosticInfo();
        public static void LogApacheDiagnostics() => _diagnostics.LogApacheDiagnostics();
    }    
}
