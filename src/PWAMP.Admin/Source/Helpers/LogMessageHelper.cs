using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pwamp.Admin.Helpers
{
    public enum LogType
    {
        Default,
        Info,
        Error,
        Debug,
        DebugDetails
    }

    internal class LogMessageHelper
    {
        internal static Color GetLogColor(LogType logType)
        {
            Color textColor;
            switch (logType)
            {
                case LogType.Error:
                    textColor = Color.Red;
                    break;
                case LogType.Info:
                    textColor = Color.LightGreen;
                    break;
                case LogType.Debug:
                    textColor = Color.Gray;
                    break;
                case LogType.DebugDetails:
                    textColor = Color.DarkGray;
                    break;
                default:
                    textColor = Color.White;
                    break;
            }
            return textColor;
        }
    }
}
