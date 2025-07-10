# WAMPoon Control Panel

A lightweight Windows application for managing Apache and MySQL/MariaDB servers during web development. WAMPoon Control Panel eliminates the hassle of manually starting and stopping servers through command line interfaces.

## What It Does

WAMPoon Control Panel gives you point-and-click control over your local development servers:

- Start and stop Apache HTTP Server with a single click
- Manage MySQL/MariaDB database server operations
- Monitor server status in real-time through integrated logging
- Control all servers at once or individually
- Run quietly in your system tray when minimized

The application handles proper server shutdown procedures and prevents conflicts by ensuring only one instance runs at a time.

## System Requirements

**Operating System:** Windows 10 or later  
**Runtime:** .NET Framework 4.8 (already included in Windows 10 or later)  

**Server Software:**
- Apache HTTP Server 2.4 or newer
- MariaDB 10.3 or newer (or compatible MySQL installation)
- Properly configured httpd.conf and my.ini files


## Getting Started

1. Run `PwampoonControl.exe` to launch the application
2. The control panel will scan for your Apache and MariaDB installations
3. Click individual start/stop buttons to control each server
4. Use "Start All" or "Stop All" for bulk operations
5. Watch the log area for real-time server status updates

Right-click the system tray icon for quick access to common functions when the window is minimized.


## Common Issues

**Servers won't start:** Check if ports 80 (Apache) or 3306 (MySQL) are already in use by other applications.

**"Server Not Found" errors:** Verify the installation paths in your application settings match your actual server locations.

**Configuration problems:** Review your httpd.conf and my.ini files for syntax errors or invalid paths.

**Need more details:** Check the diagnostic logs within the application or look for error files in the `wampoon-logs` directory. Enable debug logging for verbose troubleshooting output.

## Building from Source

1. Clone this repository
2. Open `Pwamp.ControlPanel.sln` in Visual Studio 2022 or later  
3. Build the solution (Ctrl+Shift+B)
4. Run with F5 or Ctrl+F5

## Contributing

Contributions are welcome. Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Make your changes and test thoroughly
4. Commit with clear, descriptive messages
5. Push your branch and create a Pull Request

## Reporting Issues

Found a bug? Please check the [Issues page](https://github.com/orgs/wampoon-box/wampoon-control/issues) first to see if it's already been reported.

**When reporting a new issue, include:**
- Your Windows version and architecture
- Application version (from About dialog)
- Steps to reproduce the problem
- What you expected vs. what actually happened
- Any error messages or relevant log files
- Screenshots if helpful

**Before reporting:**
- Make sure you're using the latest version
- Check the troubleshooting section above
- Look for log files in the `wampoon-logs` directory

## License

MIT License - see [LICENSE](LICENSE) for details.

---

> This tool is designed for local development environments. Production servers require proper administration tools and security hardening. Use at your own risk.
