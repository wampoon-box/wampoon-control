using Wampoon.ControlPanel.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wampoon.ControlPanel.Helpers
{
    
    internal class UiHelper
    {
        // Enhanced color palette for better readability.
        public static class Colors
        {
            // Main background colors - slightly darker than default.
            public static readonly Color MainBackground = Color.FromArgb(240, 242, 245);
            public static readonly Color PanelBackground = Color.FromArgb(230, 234, 238);
            public static readonly Color CardBackground = Color.FromArgb(255, 255, 255);
            
            // Text colors.
            public static readonly Color PrimaryText = Color.FromArgb(31, 41, 55);
            public static readonly Color SecondaryText = Color.FromArgb(75, 85, 99);
            public static readonly Color MutedText = Color.FromArgb(107, 114, 128);

            // Log colors with better contrast.
            public static readonly Color LogError = Color.FromArgb(220, 38, 38);
            public static readonly Color LogSuccess = Color.FromArgb(12, 229, 44);
            public static readonly Color LogInfo = Color.FromArgb(0, 255, 255);
            public static readonly Color LogWarning = Color.FromArgb(245, 158, 11);
            public static readonly Color LogDebug = Color.FromArgb(107, 114, 128);
            public static readonly Color LogDefault = Color.FromArgb(31, 41, 55);
            
            // Module name color (different from LogInfo).
            //public static readonly Color LogModule = Color.FromArgb(30, 250, 115);
            public static readonly Color LogModule = Color.GreenYellow;

            // Log background colors.
            public static readonly Color LogBackground = Color.FromArgb(17, 24, 39);
            public static readonly Color LogBackgroundLight = Color.FromArgb(249, 250, 251);
        }
        internal static Color GetLogColor(LogType logType)
        {
            Color textColor;
            switch (logType)
            {
                case LogType.Error:
                    textColor = Colors.LogError;
                    break;
                case LogType.Info:
                    textColor = Colors.LogInfo;
                    break;
                case LogType.Success:
                    textColor = Colors.LogSuccess;
                    break;
                case LogType.Warning:
                    textColor = Colors.LogWarning;
                    break;
                case LogType.DebugDetails:
                    textColor = Colors.LogDebug;
                    break;
                default:
                    textColor = Colors.LogDefault;
                    break;
            }
            return textColor;
        }

        internal static void DrawBootstrapCardShadow(Graphics graphics, Control control)
        {
            int shadowOffset = AppConstants.UI.SHADOW_OFFSET;
            int shadowSize = AppConstants.UI.SHADOW_SIZE;
            int radius = AppConstants.UI.BORDER_RADIUS;
            
            // Draw bottom shadow (inside control bounds but at bottom edge).
            Rectangle bottomShadowRect = new Rectangle(shadowOffset, control.Height - shadowSize, 
                                                     control.Width - shadowOffset * 2, shadowSize);
            
            using (System.Drawing.Drawing2D.LinearGradientBrush bottomShadowBrush = 
                   new System.Drawing.Drawing2D.LinearGradientBrush(
                       new Point(bottomShadowRect.X, bottomShadowRect.Y),
                       new Point(bottomShadowRect.X, bottomShadowRect.Bottom),
                       Color.FromArgb(30, 0, 0, 0),
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
            
            // Draw right shadow (inside control bounds but at right edge).
            Rectangle rightShadowRect = new Rectangle(control.Width - shadowSize, shadowOffset,
                                                    shadowSize, control.Height - shadowOffset * 2);
            
            using (System.Drawing.Drawing2D.LinearGradientBrush rightShadowBrush = 
                   new System.Drawing.Drawing2D.LinearGradientBrush(
                       new Point(rightShadowRect.X, rightShadowRect.Y),
                       new Point(rightShadowRect.Right, rightShadowRect.Y),
                       Color.FromArgb(30, 0, 0, 0),
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

        internal static void ApplyLeftBorderToButton(Button button, Color borderColor)
        {
            // Keep the button's existing flat appearance but clear its border.
            button.FlatAppearance.BorderSize = 0;

            // Store the border color in the button's Tag property.
            button.Tag = borderColor;

            // Remove any existing paint handler and add the new one.
            button.Paint -= Button_Paint;
            button.Paint += Button_Paint;

            // Force the button to repaint.
            button.Invalidate();
        }

        private static void Button_Paint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null || button.Tag == null) return;

            Color borderColor = (Color)button.Tag;
            
            using (Brush borderBrush = new SolidBrush(borderColor))
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int borderWidth = AppConstants.UI.BORDER_WIDTH;
                    int radius = AppConstants.UI.BUTTON_BORDER_RADIUS;
                    int height = button.Height;

                    // Create the rounded left border path - draw over the existing button.
                    if (height > radius * 2)
                    {
                        path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                        path.AddLine(radius, 0, borderWidth, 0);
                        path.AddLine(borderWidth, 0, borderWidth, height - radius);
                        path.AddLine(borderWidth, height - radius, radius, height - radius);
                        path.AddArc(0, height - radius * 2, radius * 2, radius * 2, 90, 90);
                        path.CloseFigure();

                        e.Graphics.FillPath(borderBrush, path);
                    }
                    else
                    {
                        // Fallback for very small buttons - just draw a simple left border.
                        Rectangle borderRect = new Rectangle(0, 0, borderWidth, height);
                        e.Graphics.FillRectangle(borderBrush, borderRect);
                    }
                }
            }
        }
    }
}
