using System;
using System.Diagnostics;
using System.IO;
using Frostybee.Pwamp.Interfaces;

namespace Frostybee.Pwamp.Services
{
    public class FileOperations : IFileOperations
    {
        public bool FileExists(string path) => File.Exists(path);
        
        public bool DirectoryExists(string path) => Directory.Exists(path);
        
        public DateTime GetLastWriteTime(string path) => File.GetLastWriteTime(path);
        
        public FileInfo GetFileInfo(string path) => new FileInfo(path);
        
        public string[] GetDirectories(string path) => Directory.GetDirectories(path);
        
        public string[] GetFiles(string path) => Directory.GetFiles(path);
        
        public void WriteAllLines(string path, string[] lines) => File.WriteAllLines(path, lines);
        
        public void CreateDirectory(string path) => Directory.CreateDirectory(path);
        
        public bool StartProcess(string fileName)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(startInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}