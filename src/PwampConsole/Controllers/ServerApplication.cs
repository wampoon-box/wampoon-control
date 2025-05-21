using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwampConsole.Controllers
{
    /// <summary>
    /// Example application demonstrating usage of the server managers
    /// </summary>
    public class ServerApplication : IDisposable
    {
        private ApacheManager _apacheManager;
        private MySqlManager _mysqlManager;
        private bool disposedValue;

        public ServerApplication()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Create server managers
            _apacheManager = new ApacheManager(baseDirectory);
            _mysqlManager = new MySqlManager(baseDirectory);
        }

        public void Start()
        {
            Console.WriteLine("Starting server application...");

            // Initialize MySQL if needed
            _mysqlManager.InitializeDatabase();

            // Start the servers
            bool apacheStarted = _apacheManager.StartServer();
            bool mysqlStarted = _mysqlManager.StartServer();

            Console.WriteLine($"Apache running: {apacheStarted}");
            Console.WriteLine($"MySQL running: {mysqlStarted}");
        }

        public void Stop()
        {
            Console.WriteLine("Stopping server application...");

            // Stop the servers
            _apacheManager.StopServer();
            _mysqlManager.StopServer();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _apacheManager?.Dispose();
                    _mysqlManager?.Dispose();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ServerApplication()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
