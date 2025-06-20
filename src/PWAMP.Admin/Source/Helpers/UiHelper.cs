using Frostybee.PwampAdmin.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        internal static void DrawBootstrapCardShadow(Graphics graphics, Control control)
        {
            int shadowOffset = 4;
            int shadowSize = 8;
            int radius = 8;
            
            
            // Draw bottom shadow.
            Rectangle bottomShadowRect = new Rectangle(shadowOffset, control.Height - shadowSize, 
                                                     control.Width - shadowOffset * 2, shadowSize);
            
            using (System.Drawing.Drawing2D.LinearGradientBrush bottomShadowBrush = 
                   new System.Drawing.Drawing2D.LinearGradientBrush(
                       new Point(bottomShadowRect.X, bottomShadowRect.Y),
                       new Point(bottomShadowRect.X, bottomShadowRect.Bottom),
                       Color.FromArgb(25, 0, 0, 0),
                       Color.FromArgb(0, 0, 0, 0)))
            {
                using (System.Drawing.Drawing2D.GraphicsPath bottomShadowPath = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    bottomShadowPath.AddArc(bottomShadowRect.X, bottomShadowRect.Y, radius * 2, radius * 2, 180, 90);
                    bottomShadowPath.AddLine(bottomShadowRect.X + radius, bottomShadowRect.Y, bottomShadowRect.Right - radius, bottomShadowRect.Y);
                    bottomShadowPath.AddArc(bottomShadowRect.Right - radius * 2, bottomShadowRect.Y, radius * 2, radius * 2, 270, 90);
                    bottomShadowPath.AddLine(bottomShadowRect.Right, bottomShadowRect.Y + radius, bottomShadowRect.Right, bottomShadowRect.Bottom - radius);
                    bottomShadowPath.AddArc(bottomShadowRect.Right - radius * 2, bottomShadowRect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    bottomShadowPath.AddLine(bottomShadowRect.Right - radius, bottomShadowRect.Bottom, bottomShadowRect.X + radius, bottomShadowRect.Bottom);
                    bottomShadowPath.AddArc(bottomShadowRect.X, bottomShadowRect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    bottomShadowPath.CloseFigure();
                    
                    graphics.FillPath(bottomShadowBrush, bottomShadowPath);
                }
            }
            
            // Draw right shadow.
            Rectangle rightShadowRect = new Rectangle(control.Width - shadowSize, shadowOffset,
                                                    shadowSize, control.Height - shadowOffset * 2);
            
            using (System.Drawing.Drawing2D.LinearGradientBrush rightShadowBrush = 
                   new System.Drawing.Drawing2D.LinearGradientBrush(
                       new Point(rightShadowRect.X, rightShadowRect.Y),
                       new Point(rightShadowRect.Right, rightShadowRect.Y),
                       Color.FromArgb(25, 0, 0, 0),
                       Color.FromArgb(0, 0, 0, 0)))
            {
                using (System.Drawing.Drawing2D.GraphicsPath rightShadowPath = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    rightShadowPath.AddArc(rightShadowRect.X, rightShadowRect.Y, radius * 2, radius * 2, 270, 90);
                    rightShadowPath.AddLine(rightShadowRect.X + radius, rightShadowRect.Y, rightShadowRect.Right, rightShadowRect.Y);
                    rightShadowPath.AddLine(rightShadowRect.Right, rightShadowRect.Y, rightShadowRect.Right, rightShadowRect.Bottom);
                    rightShadowPath.AddLine(rightShadowRect.Right, rightShadowRect.Bottom, rightShadowRect.X + radius, rightShadowRect.Bottom);
                    rightShadowPath.AddArc(rightShadowRect.X, rightShadowRect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    rightShadowPath.AddLine(rightShadowRect.X, rightShadowRect.Bottom - radius, rightShadowRect.X, rightShadowRect.Y + radius);
                    rightShadowPath.CloseFigure();
                    
                    graphics.FillPath(rightShadowBrush, rightShadowPath);
                }
            }
        }
    }
}
