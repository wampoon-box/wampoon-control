using PwampConsole.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwampConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Using the application class
                using (ServerApplication app = new ServerApplication())
                {
                    app.Start();

                    Console.WriteLine("Servers are running. Press Enter to stop and exit.");
                    Console.ReadLine();

                    app.Stop();
                }

                // Alternative usage with factory pattern
                Console.WriteLine("\nAlternative usage with factory pattern:");
                Console.WriteLine("1. Start Apache only");
                Console.WriteLine("2. Start MySQL only");
                Console.WriteLine("3. Start both servers");
                Console.Write("Enter your choice (or any other key to exit): ");

                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.KeyChar)
                {
                    case '1':
                        using (var apache = ServerManagerFactory.CreateManager(ServerManagerFactory.ServerType.Apache))
                        {
                            apache.UpdateConfiguration();
                            apache.StartServer();
                            Console.WriteLine("Press Enter to stop Apache and exit.");
                            Console.ReadLine();
                        }
                        break;

                    case '2':
                        using (var mysql = (MySqlManager)ServerManagerFactory.CreateManager(ServerManagerFactory.ServerType.MySQL))
                        {
                            mysql.UpdateConfiguration();
                            mysql.InitializeDatabase();
                            mysql.StartServer();
                            Console.WriteLine("Press Enter to stop MySQL and exit.");
                            Console.ReadLine();
                        }
                        break;

                    case '3':
                        using (var apache = ServerManagerFactory.CreateManager(ServerManagerFactory.ServerType.Apache))
                        using (var mysql = (MySqlManager)ServerManagerFactory.CreateManager(ServerManagerFactory.ServerType.MySQL))
                        {
                            apache.UpdateConfiguration();
                            mysql.UpdateConfiguration();

                            ((MySqlManager)mysql).InitializeDatabase();

                            apache.StartServer();
                            mysql.StartServer();

                            Console.WriteLine("Press Enter to stop both servers and exit.");
                            Console.ReadLine();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("Application exiting. Press any key to close.");
            Console.ReadKey();
        }
    }
}
}
