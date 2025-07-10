using System;
using System.IO;

namespace Wampoon.ControlPanel.Interfaces
{
    public interface IFileOperations
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);
        DateTime GetLastWriteTime(string path);
        FileInfo GetFileInfo(string path);
        string[] GetDirectories(string path);
        string[] GetFiles(string path);
        void WriteAllLines(string path, string[] lines);
        void CreateDirectory(string path);
        bool StartProcess(string fileName);
    }
}