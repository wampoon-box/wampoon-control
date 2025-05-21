using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using PwampConsole.Controllers;

namespace PwampConsole.Controllers
{
    public partial class ApacheManagerForm : Form
    {
        private ServerApplication serverApp;
        private ApacheManager apacheManager;
        private MySqlManager mysqlManager;
        private bool isApacheRunning = false;
        private bool isMySqlRunning = false;

        public ApacheManagerForm()
        {
            InitializeComponent();
            UpdateStatusIndicators();
        }

        private void InitializeComponent()
        {
            this.Text = "Server Manager";
            this.Size = new Size(900, 600);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(700, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Main layout panel
            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10),
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 150)); // Increased height for server controls
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            this.Controls.Add(mainPanel);

            // Controls panel
            TableLayoutPanel controlsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(5)
            };
            controlsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            controlsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            mainPanel.Controls.Add(controlsPanel, 0, 0);

            // Apache panel
            Panel apachePanel = CreateServerPanel("Apache", out Label apacheStatusLabel, out Button apacheStartStopButton);
            controlsPanel.Controls.Add(apachePanel, 0, 0);

            // MySQL panel
            Panel mysqlPanel = CreateServerPanel("MySQL", out Label mysqlStatusLabel, out Button mysqlStartStopButton);
            controlsPanel.Controls.Add(mysqlPanel, 0, 1);

            // Output text area
            TextBox outputTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                BackColor = Color.Black,
                ForeColor = Color.LightGreen,
                Font = new Font("Consolas", 10)
            };
            mainPanel.Controls.Add(outputTextBox, 0, 1);

            // Bottom panel
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            mainPanel.Controls.Add(bottomPanel, 0, 2);

            // Clear log button
            Button clearButton = new Button
            {
                Text = "Clear Log",
                Location = new Point(10, 10),
                Width = 100
            };
            clearButton.Click += (sender, e) => outputTextBox.Clear();
            bottomPanel.Controls.Add(clearButton);

            // Initialize both servers button
            Button initBothButton = new Button
            {
                Text = "Initialize Both Servers",
                Location = new Point(120, 10),
                Width = 150
            };
            initBothButton.Click += (sender, e) => InitializeServers();
            bottomPanel.Controls.Add(initBothButton);

            // Start both servers button
            Button startBothButton = new Button
            {
                Text = "Start Both Servers",
                Location = new Point(280, 10),
                Width = 150
            };
            startBothButton.Click += (sender, e) => StartBothServers();
            bottomPanel.Controls.Add(startBothButton);

            // Stop both servers button
            Button stopBothButton = new Button
            {
                Text = "Stop Both Servers",
                Location = new Point(440, 10),
                Width = 150
            };
            stopBothButton.Click += (sender, e) => StopBothServers();
            bottomPanel.Controls.Add(stopBothButton);

            // Set up Apache button event
            apacheStartStopButton.Click += (sender, e) =>
            {
                if (isApacheRunning)
                {
                    StopApache();
                    apacheStartStopButton.Text = "Start Apache";
                }
                else
                {
                    StartApache();
                    apacheStartStopButton.Text = "Stop Apache";
                }
                UpdateStatusIndicators();
            };

            // Set up MySQL button event
            mysqlStartStopButton.Click += (sender, e) =>
            {
                if (isMySqlRunning)
                {
                    StopMySql();
                    mysqlStartStopButton.Text = "Start MySQL";
                }
                else
                {
                    StartMySql();
                    mysqlStartStopButton.Text = "Stop MySQL";
                }
                UpdateStatusIndicators();
            };

            // Save form-level references to controls we'll need to access
            this.outputTextBox = outputTextBox;
            this.apacheStatusLabel = apacheStatusLabel;
            this.apacheStartStopButton = apacheStartStopButton;
            this.mysqlStatusLabel = mysqlStatusLabel;
            this.mysqlStartStopButton = mysqlStartStopButton;
            
            // Handle form closing to stop servers if they're running
            this.FormClosing += (sender, e) =>
            {
                StopBothServers();
            };
        }

        private Panel CreateServerPanel(string serverName, out Label statusLabel, out Button startStopButton)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label titleLabel = new Label
            {
                Text = $"{serverName} Server",
                Font = new Font(this.Font, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            panel.Controls.Add(titleLabel);

            // Status indicator
            Label statusTextLabel = new Label
            {
                Text = "Status:",
                AutoSize = true,
                Location = new Point(10, 40)
            };
            panel.Controls.Add(statusTextLabel);

            statusLabel = new Label
            {
                AutoSize = true,
                Location = new Point(100, 40),
                Text = "Stopped",
                ForeColor = Color.Red,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            panel.Controls.Add(statusLabel);

            // Start/Stop button
            startStopButton = new Button
            {
                Text = $"Start {serverName}",
                Location = new Point(250, 35),
                Width = 120
            };
            panel.Controls.Add(startStopButton);

            return panel;
        }

        private void InitializeServers()
        {
            AppendToLog("Initializing server managers...");
            try
            {
                // Create server managers using the factory
                apacheManager = (ApacheManager)ServerManagerFactory.CreateManager(ServerManagerFactory.ServerType.Apache);
                mysqlManager = (MySqlManager)ServerManagerFactory.CreateManager(ServerManagerFactory.ServerType.MySQL);

                // Initialize MySQL database
                AppendToLog("Initializing MySQL database...");
                bool mysqlInitialized = mysqlManager.InitializeDatabase();
                AppendToLog($"MySQL database initialization: {(mysqlInitialized ? "Success" : "Failed")}");

                UpdateStatusIndicators();
                AppendToLog("Server managers initialized successfully.");
            }
            catch (Exception ex)
            {
                AppendToLog($"Error initializing servers: {ex.Message}", true);
            }
        }

        private void StartBothServers()
        {
            AppendToLog("Starting both servers...");
            
            if (apacheManager == null || mysqlManager == null)
            {
                InitializeServers();
            }

            StartApache();
            StartMySql();
            
            UpdateStatusIndicators();
        }

        private void StopBothServers()
        {
            AppendToLog("Stopping both servers...");
            
            StopApache();
            StopMySql();
            
            UpdateStatusIndicators();
        }

        private void StartApache()
        {
            if (isApacheRunning)
            {
                AppendToLog("Apache is already running.");
                return;
            }

            try
            {
                if (apacheManager == null)
                {
                    apacheManager = (ApacheManager)ServerManagerFactory.CreateManager(ServerManagerFactory.ServerType.Apache);
                }

                // Redirect the output through our custom logger
                RedirectApacheOutput();

                // Start the server
                bool started = apacheManager.StartServer();
                isApacheRunning = started;
                
                AppendToLog($"Apache start command result: {(started ? "Success" : "Failed")}");
                
                // Update UI
                apacheStartStopButton.Text = isApacheRunning ? "Stop Apache" : "Start Apache";
                UpdateStatusIndicators();
            }
            catch (Exception ex)
            {
                AppendToLog($"Error starting Apache: {ex.Message}", true);
            }
        }

        private void StopApache()
        {
            if (!isApacheRunning)
            {
                AppendToLog("Apache is not running.");
                return;
            }

            try
            {
                if (apacheManager == null)
                {
                    AppendToLog("Apache manager not initialized.");
                    return;
                }

                // Stop the server
                bool stopped = apacheManager.StopServer();
                isApacheRunning = !stopped;
                
                AppendToLog($"Apache stop command result: {(stopped ? "Success" : "Failed")}");
                
                // Update UI
                apacheStartStopButton.Text = isApacheRunning ? "Stop Apache" : "Start Apache";
                UpdateStatusIndicators();
            }
            catch (Exception ex)
            {
                AppendToLog($"Error stopping Apache: {ex.Message}", true);
            }
        }

        private void StartMySql()
        {
            if (isMySqlRunning)
            {
                AppendToLog("MySQL is already running.");
                return;
            }

            try
            {
                if (mysqlManager == null)
                {
                    mysqlManager = (MySqlManager)ServerManagerFactory.CreateManager(ServerManagerFactory.ServerType.MySQL);
                    mysqlManager.InitializeDatabase();
                }

                // Redirect the output through our custom logger
                RedirectMySqlOutput();

                // Start the server
                bool started = mysqlManager.StartServer();
                isMySqlRunning = started;
                
                AppendToLog($"MySQL start command result: {(started ? "Success" : "Failed")}");
                
                // Update UI
                mysqlStartStopButton.Text = isMySqlRunning ? "Stop MySQL" : "Start MySQL";
                UpdateStatusIndicators();
            }
            catch (Exception ex)
            {
                AppendToLog($"Error starting MySQL: {ex.Message}", true);
            }
        }

        private void StopMySql()
        {
            if (!isMySqlRunning)
            {
                AppendToLog("MySQL is not running.");
                return;
            }

            try
            {
                if (mysqlManager == null)
                {
                    AppendToLog("MySQL manager not initialized.");
                    return;
                }

                // Stop the server
                bool stopped = mysqlManager.StopServer();
                isMySqlRunning = !stopped;
                
                AppendToLog($"MySQL stop command result: {(stopped ? "Success" : "Failed")}");
                
                // Update UI
                mysqlStartStopButton.Text = isMySqlRunning ? "Stop MySQL" : "Start MySQL";
                UpdateStatusIndicators();
            }
            catch (Exception ex)
            {
                AppendToLog($"Error stopping MySQL: {ex.Message}", true);
            }
        }

        private void RedirectApacheOutput()
        {
            // This is a bit of a hack since we don't have direct access to the Process objects
            // We're using the custom console output redirector to capture console output
            Console.SetOut(new TextBoxOutputRedirector(outputTextBox, "Apache"));
        }

        private void RedirectMySqlOutput()
        {
            // Using the same approach for MySQL output
            Console.SetOut(new TextBoxOutputRedirector(outputTextBox, "MySQL"));
        }

        private void AppendToLog(string text, bool isError = false)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, bool>(AppendToLog), text, isError);
                return;
            }

            outputTextBox.AppendText(DateTime.Now.ToString("[HH:mm:ss] ") + text + Environment.NewLine);
            
            // Auto-scroll to the end
            outputTextBox.SelectionStart = outputTextBox.Text.Length;
            outputTextBox.ScrollToCaret();
            
            if (isError)
            {
                // Could add special handling for errors here if needed
            }
        }

        private void UpdateStatusIndicators()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateStatusIndicators));
                return;
            }

            if (apacheManager != null)
                isApacheRunning = apacheManager.IsRunning;
                
            if (mysqlManager != null)
                isMySqlRunning = mysqlManager.IsRunning;

            apacheStatusLabel.Text = isApacheRunning ? "Running" : "Stopped";
            apacheStatusLabel.ForeColor = isApacheRunning ? Color.Green : Color.Red;
            
            mysqlStatusLabel.Text = isMySqlRunning ? "Running" : "Stopped";
            mysqlStatusLabel.ForeColor = isMySqlRunning ? Color.Green : Color.Red;
            
            apacheStartStopButton.Text = isApacheRunning ? "Stop Apache" : "Start Apache";
            mysqlStartStopButton.Text = isMySqlRunning ? "Stop MySQL" : "Start MySQL";
        }

        // Form-level control references
        private TextBox outputTextBox;
        private Label apacheStatusLabel;
        private Button apacheStartStopButton;
        private Label mysqlStatusLabel;
        private Button mysqlStartStopButton;
    }

    /// <summary>
    /// Custom TextWriter that redirects output to a TextBox
    /// </summary>
    public class TextBoxOutputRedirector : System.IO.TextWriter
    {
        private TextBox textBox;
        private string prefix;

        public TextBoxOutputRedirector(TextBox textBox, string prefix = "")
        {
            this.textBox = textBox;
            this.prefix = string.IsNullOrEmpty(prefix) ? "" : $"[{prefix}] ";
        }

        public override void Write(char value)
        {
            // Character by character is inefficient but we'll redirect line by line
        }

        public override void WriteLine(string value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new Action<string>(s =>
                {
                    textBox.AppendText(DateTime.Now.ToString("[HH:mm:ss] ") + prefix + s + Environment.NewLine);
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.ScrollToCaret();
                }), value);
            }
            else
            {
                textBox.AppendText(DateTime.Now.ToString("[HH:mm:ss] ") + prefix + value + Environment.NewLine);
                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
            }
        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ApacheManagerForm());
        }
    }
}
    
