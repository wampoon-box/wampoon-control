using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wampoon.ControlPanel.Enums;
using Wampoon.ControlPanel.Helpers;
using Wampoon.ControlPanel.Interfaces;
using Wampoon.ControlPanel.Models;

namespace Wampoon.ControlPanel.Services
{
    public class ServerDiagnostics
    {
        private readonly IFileOperations _fileOperations;
        private readonly ServerPathResolver _pathResolver;

        public ServerDiagnostics(IFileOperations fileOperations, ServerPathResolver pathResolver)
        {
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
            _pathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver));
        }

        public Dictionary<string, string> GetDiagnosticInfo()
        {
            var diagnostics = new Dictionary<string, string>
            {
                ["Application Directory"] = _pathResolver.ApplicationDirectory,
                ["Apps Directory"] = _pathResolver.AppsDirectory,
                ["Apps Directory Exists"] = _fileOperations.DirectoryExists(_pathResolver.AppsDirectory).ToString()
            };

            var serverPaths = _pathResolver.GetAllServerPaths();
            foreach (var kvp in serverPaths)
            {
                var pathInfo = kvp.Value;
                var serverKey = kvp.Key;
                
                diagnostics[$"{serverKey} - Executable Path"] = pathInfo.ExecutablePath;
                diagnostics[$"{serverKey} - Executable Exists"] = _fileOperations.FileExists(pathInfo.ExecutablePath).ToString();
                diagnostics[$"{serverKey} - Config Path"] = pathInfo.ConfigPath ?? "Not configured";
                diagnostics[$"{serverKey} - Config Exists"] = (pathInfo.ConfigPath != null && _fileOperations.FileExists(pathInfo.ConfigPath)).ToString();
            }

            return diagnostics;
        }

        public void LogApacheDiagnostics()
        {
            try
            {
                var apacheDefinition = ServerDefinitions.GetByName(PackageType.Apache.ToServerName());
                var diagnosticLines = CreateApacheDiagnosticLines(apacheDefinition);
                
                var logDir = Path.Combine(_pathResolver.ApplicationDirectory, "wampoon-logs");
                var logFile = Path.Combine(logDir, "apache-diagnostics.log");
                
                if (!_fileOperations.DirectoryExists(logDir))
                    _fileOperations.CreateDirectory(logDir);
                    
                _fileOperations.WriteAllLines(logFile, diagnosticLines.ToArray());
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
            }
        }

        private List<string> CreateApacheDiagnosticLines(ServerDefinitionInfo apacheDefinition)
        {
            var diagnosticLines = new List<string>
            {
                "=== APACHE CONFIGURATION DIAGNOSTICS ===",
                $"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                $"Application Directory: {_pathResolver.ApplicationDirectory}",
                $"Apps Directory: {_pathResolver.AppsDirectory}",
                $"Apps Directory Exists: {_fileOperations.DirectoryExists(_pathResolver.AppsDirectory)}",
                "",
                "Apache Definition Info:",
                $"  Name: {apacheDefinition?.Name ?? "NULL"}",
                $"  Directory: {apacheDefinition?.Directory ?? "NULL"}",
                $"  ExecutableName: {apacheDefinition?.ExecutableName ?? "NULL"}",
                $"  ConfigFile: {apacheDefinition?.ConfigFile ?? "NULL"}",
                ""
            };

            if (apacheDefinition != null)
            {
                AddApachePathDiagnostics(diagnosticLines, apacheDefinition);
                AddApachePathInfoDiagnostics(diagnosticLines);
                AddApacheDirectoryContentsDiagnostics(diagnosticLines, apacheDefinition);
            }

            diagnosticLines.Add("=== END DIAGNOSTICS ===");
            return diagnosticLines;
        }

        private void AddApachePathDiagnostics(List<string> diagnosticLines, ServerDefinitionInfo apacheDefinition)
        {
            var serverBaseDir = Path.Combine(_pathResolver.AppsDirectory, apacheDefinition.Directory);
            var configPath = Path.Combine(serverBaseDir, apacheDefinition.ConfigFile);
            
            diagnosticLines.AddRange(new[]
            {
                "Computed Paths:",
                $"  Server Base Dir: {serverBaseDir}",
                $"  Server Base Dir Exists: {_fileOperations.DirectoryExists(serverBaseDir)}",
                $"  Config Path: {configPath}",
                $"  Config File Exists: {_fileOperations.FileExists(configPath)}",
                "",
                "ServerPathManager Results:",
                $"  GetServerPath('Apache') != null: {_pathResolver.GetServerPath(PackageType.Apache.ToServerName()) != null}",
                $"  GetConfigPath('Apache'): {_pathResolver.GetConfigPath(PackageType.Apache.ToServerName()) ?? "NULL"}",
                ""
            });
        }

        private void AddApachePathInfoDiagnostics(List<string> diagnosticLines)
        {
            var pathInfo = _pathResolver.GetServerPath(PackageType.Apache.ToServerName());
            if (pathInfo != null)
            {
                diagnosticLines.AddRange(new[]
                {
                    "Apache PathInfo Details:",
                    $"  ServerName: {pathInfo.ServerName ?? "NULL"}",
                    $"  ExecutablePath: {pathInfo.ExecutablePath ?? "NULL"}",
                    $"  ConfigPath: {pathInfo.ConfigPath ?? "NULL"}",
                    $"  ServerDirectory: {pathInfo.ServerDirectory ?? "NULL"}",
                    $"  ServerBaseDirectory: {pathInfo.ServerBaseDirectory ?? "NULL"}",
                    $"  IsAvailable: {pathInfo.IsAvailable}",
                    ""
                });
            }
            else
            {
                diagnosticLines.Add("Apache PathInfo: NULL");
            }
        }

        private void AddApacheDirectoryContentsDiagnostics(List<string> diagnosticLines, ServerDefinitionInfo apacheDefinition)
        {
            var serverBaseDir = Path.Combine(_pathResolver.AppsDirectory, apacheDefinition.Directory);
            
            if (_fileOperations.DirectoryExists(serverBaseDir))
            {
                try
                {
                    var subdirs = _fileOperations.GetDirectories(serverBaseDir);
                    var files = _fileOperations.GetFiles(serverBaseDir);
                    
                    diagnosticLines.AddRange(new[]
                    {
                        $"Contents of {serverBaseDir}:",
                        $"  Subdirectories: {string.Join(", ", subdirs.Select(Path.GetFileName))}",
                        $"  Files: {string.Join(", ", files.Select(Path.GetFileName))}",
                        ""
                    });

                    var confDir = Path.Combine(serverBaseDir, "conf");
                    if (_fileOperations.DirectoryExists(confDir))
                    {
                        var confFiles = _fileOperations.GetFiles(confDir);
                        diagnosticLines.AddRange(new[]
                        {
                            $"Contents of {confDir}:",
                            $"  Config Files: {string.Join(", ", confFiles.Select(Path.GetFileName))}",
                            ""
                        });
                    }
                    else
                    {
                        diagnosticLines.Add($"Config directory does not exist: {confDir}");
                    }
                }
                catch (Exception ex)
                {
                    diagnosticLines.Add($"Error listing directory contents: {ex.Message}");
                }
            }
        }
    }
}