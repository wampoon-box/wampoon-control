using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Wampoon.ControlPanel.Helpers
{
    internal class NetworkPortHelper
    {

        public static bool IsPortInUse(int port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            // Check TCP listeners - this is what matters for server startup
            var tcpListeners = ipGlobalProperties.GetActiveTcpListeners();
            bool tcpListenerInUse = tcpListeners.Any(listener => listener.Port == port);

            // Only check for established TCP connections, not TIME_WAIT or other transient states
            var tcpConnections = ipGlobalProperties.GetActiveTcpConnections();
            bool tcpEstablishedInUse = tcpConnections.Any(connection => 
                connection.LocalEndPoint.Port == port && 
                connection.State == System.Net.NetworkInformation.TcpState.Established);

            // Check UDP listeners
            var udpListeners = ipGlobalProperties.GetActiveUdpListeners();
            bool udpInUse = udpListeners.Any(listener => listener.Port == port);

            return tcpListenerInUse || tcpEstablishedInUse || udpInUse;
        }

        /// <summary>
        /// Checks if a port is in use, with retry logic for recently force-stopped processes.
        /// </summary>
        public static bool IsPortInUseWithRetry(int port, int retryCount = 3, int delayMs = 1000)
        {
            for (int i = 0; i < retryCount; i++)
            {
                if (!IsPortInUse(port))
                {
                    return false;
                }
                
                if (i < retryCount - 1) // Don't delay on the last iteration.
                {
                    System.Threading.Thread.Sleep(delayMs);
                }
            }
            
            return true;
        }
    }
}
