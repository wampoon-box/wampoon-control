using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.PwampAdmin.Enums
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
}
