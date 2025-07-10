using System;
using System.Windows.Forms;
using Frostybee.Pwamp.Enums;

namespace Frostybee.Pwamp.Helpers
{
    public static class ExceptionHandlerUtils
    {
        public static bool HandleServerException(Exception ex, string operation, string serviceName, Action<string, LogType> logger)
        {
            ErrorLogHelper.LogExceptionInfo(ex);
            logger?.Invoke(string.Format(AppConstants.Messages.FAILED_TO_OPERATION, operation, ex.Message), LogType.Error);
            return false;
        }

        public static void HandleUIException(Exception ex, string operation, string serviceName, IWin32Window owner = null)
        {
            ErrorLogHelper.LogExceptionInfo(ex);
            MessageBox.Show(
                string.Format(AppConstants.Messages.ERROR_STARTING, serviceName, ex.Message), 
                "Error",
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error);
        }

        public static T HandleException<T>(Func<T> action, T defaultValue = default(T), Action<Exception> onError = null)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                ErrorLogHelper.LogExceptionInfo(ex);
                onError?.Invoke(ex);
                return defaultValue;
            }
        }
    }
}