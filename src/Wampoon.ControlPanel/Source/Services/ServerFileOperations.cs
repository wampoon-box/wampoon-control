using System;
using System.IO;
using Frostybee.Pwamp.Interfaces;

namespace Frostybee.Pwamp.Services
{
    public class ServerFileOperations
    {
        private readonly IFileOperations _fileOperations;
        private readonly ServerPathResolver _pathResolver;

        public ServerFileOperations(IFileOperations fileOperations, ServerPathResolver pathResolver)
        {
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
            _pathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver));
        }

        public bool OpenConfigFile(string serverName)
        {
            try
            {
                var configPath = _pathResolver.GetConfigPath(serverName);
                if (string.IsNullOrEmpty(configPath) || !_fileOperations.FileExists(configPath))
                {
                    return false;
                }

                return _fileOperations.StartProcess(configPath);
            }
            catch
            {
                return false;
            }
        }

        public bool OpenFileInDefaultEditor(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !_fileOperations.FileExists(filePath))
                {
                    return false;
                }

                return _fileOperations.StartProcess(filePath);
            }
            catch
            {
                return false;
            }
        }

        public bool CanOpenConfigFile(string serverName)
        {
            var configPath = _pathResolver.GetConfigPath(serverName);
            return !string.IsNullOrEmpty(configPath) && _fileOperations.FileExists(configPath);
        }

        public string GetConfigFileSize(string serverName)
        {
            try
            {
                var configPath = _pathResolver.GetConfigPath(serverName);
                if (string.IsNullOrEmpty(configPath) || !_fileOperations.FileExists(configPath))
                {
                    return "N/A";
                }

                var fileInfo = _fileOperations.GetFileInfo(configPath);
                long bytes = fileInfo.Length;

                if (bytes < 1024)
                    return $"{bytes} B";
                else if (bytes < 1024 * 1024)
                    return $"{bytes / 1024.0:F1} KB";
                else
                    return $"{bytes / (1024.0 * 1024.0):F1} MB";
            }
            catch
            {
                return "N/A";
            }
        }

        public DateTime? GetConfigFileLastModified(string serverName)
        {
            try
            {
                var configPath = _pathResolver.GetConfigPath(serverName);
                if (string.IsNullOrEmpty(configPath) || !_fileOperations.FileExists(configPath))
                {
                    return null;
                }

                return _fileOperations.GetLastWriteTime(configPath);
            }
            catch
            {
                return null;
            }
        }
    }
}