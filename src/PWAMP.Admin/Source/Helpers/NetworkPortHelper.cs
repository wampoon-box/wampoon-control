using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Frostybee.Pwamp.Source.Helpers
{
    internal class NetworkPortHelper
    {

        public static bool IsPortInUse(int port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            // Check TCP
            var tcpListeners = ipGlobalProperties.GetActiveTcpListeners();
            var tcpConnections = ipGlobalProperties.GetActiveTcpConnections();

            bool tcpInUse = tcpListeners.Any(listener => listener.Port == port) ||
                            tcpConnections.Any(connection => connection.LocalEndPoint.Port == port);

            // Check UDP
            var udpListeners = ipGlobalProperties.GetActiveUdpListeners();
            bool udpInUse = udpListeners.Any(listener => listener.Port == port);

            return tcpInUse || udpInUse;
        }
    }
}
