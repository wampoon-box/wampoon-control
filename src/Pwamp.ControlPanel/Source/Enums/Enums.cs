using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.Pwamp.Enums
{
    public enum ServerStatus
    {
        Stopped,
        Running,
        Stopping,
        Starting,
        Error
    }
    public enum LogType
    {
        Default,
        Info,
        Error,
        Warning,
        Debug,
        DebugDetails
    }
    
    public enum PackageType
    {
        Apache,
        MariaDB,
        MySQL,
        PHP,
        PhpMyAdmin,
        PwampDashboard
    }
    
    public static class PackageTypeExtensions
    {
        public static string  ToServerName(this PackageType packageType)
        {
            // We could use ToString(), however, this method was implemented just in case we want to return a custom
            // server name in the future. 
            return packageType.ToString();
        }
    }

    public class ServerLogEventArgs : EventArgs
    {
        public string Message { get; }
        public LogType LogType { get; }

        public ServerLogEventArgs(string message, LogType logType)
        {
            Message = message;
            LogType = logType;
        }
    }
}
