using Frostybee.PwampAdmin.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.PwampAdmin.Helpers
{
    
    internal class UiHelper
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
                    textColor = Color.Blue;
                    break;
                case LogType.Debug:
                    textColor = Color.Orange;
                    break;
                case LogType.DebugDetails:
                    textColor = Color.DarkGray;
                    break;
                default:
                    textColor = Color.Black;
                    break;
            }
            return textColor;
        }
    }
}
