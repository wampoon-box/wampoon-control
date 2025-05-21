using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwampConsole.Controllers
{
    /// <summary>
    /// Server Manager Factory to create appropriate server manager instances
    /// </summary>
    public static class ServerManagerFactory
    {
        public enum ServerType
        {
            Apache,
            MySQL
        }

        public static ServerManagerBase CreateManager(ServerType type, string baseDirectory = null)
        {
            baseDirectory = baseDirectory ?? AppDomain.CurrentDomain.BaseDirectory;

            switch (type)
            {
                case ServerType.Apache:
                    return new ApacheManager(baseDirectory);
                case ServerType.MySQL:
                    return new MySqlManager(baseDirectory);
                default:
                    throw new ArgumentException($"Unsupported server type: {type}");
            }
        }
    }
}
