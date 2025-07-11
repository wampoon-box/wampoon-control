using System;
using System.IO;
using Wampoon.ControlPanel.Interfaces;

namespace Wampoon.ControlPanel.Services
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
            return OpenConfigFile(serverName, true); // Default to read-only for config files.
        }

        public bool OpenConfigFile(string serverName, bool readOnly)
        {
            try
            {
                var configPath = _pathResolver.GetConfigPath(serverName);
                if (string.IsNullOrEmpty(configPath) || !_fileOperations.FileExists(configPath))
                {
                    return false;
                }

                if (readOnly)
                {
                    return OpenFileReadOnly(configPath);
                }
                else
                {
                    return _fileOperations.StartProcess(configPath);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Opens a config file in read-only mode.
        /// We want to prevent users from unintentionally tempering with the content of a server config file.
        /// </summary>
        /// <param name="filePath">The path of the server config file to be opened.</param>
        /// <returns></returns>
        private bool OpenFileReadOnly(string filePath)
        {
            string tempPath = null;
            try
            {
                // Create a temporary directory and preserve the original filename.
                string tempDir = System.IO.Path.GetTempPath();
                string originalFileName = System.IO.Path.GetFileName(filePath);
                string tempFileName = $"{System.IO.Path.GetFileNameWithoutExtension(originalFileName)}_readonly{System.IO.Path.GetExtension(originalFileName)}";
                tempPath = System.IO.Path.Combine(tempDir, tempFileName);

                // If temp file exists, remove it first (it might be read-only).
                if (System.IO.File.Exists(tempPath))
                {
                    System.IO.File.SetAttributes(tempPath, System.IO.FileAttributes.Normal);
                    System.IO.File.Delete(tempPath);
                }
                
                System.IO.File.Copy(filePath, tempPath, true);

                // Set the temporary file as read-only.
                System.IO.File.SetAttributes(tempPath, System.IO.File.GetAttributes(tempPath) | System.IO.FileAttributes.ReadOnly);

                // Open the read-only copy.
                return _fileOperations.StartProcess(tempPath);
            }
            catch
            {
                // Clean up temp file if something went wrong.
                if (!string.IsNullOrEmpty(tempPath) && System.IO.File.Exists(tempPath))
                {
                    try
                    {
                        System.IO.File.SetAttributes(tempPath, System.IO.FileAttributes.Normal);
                        System.IO.File.Delete(tempPath);
                    }
                    catch { }
                }
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

        public bool OpenConfigFileLocation(string serverName)
        {
            try
            {
                var configPath = _pathResolver.GetConfigPath(serverName);
                if (string.IsNullOrEmpty(configPath) || !_fileOperations.FileExists(configPath))
                {
                    return false;
                }

                // Open the directory containing the config file.
                var directoryPath = System.IO.Path.GetDirectoryName(configPath);
                return _fileOperations.StartProcess(directoryPath);
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