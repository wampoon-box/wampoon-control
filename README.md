# WAMPoon Control Panel

An application for managing Apache and MySQL/MariaDB servers in a local development environment. This control panel provides an intuitive interface for starting, stopping, and monitoring your web development stack.

## Features

### Server Management
- **Apache HTTP Server Control**: Start, stop, and monitor Apache web server,
- **MySQL/MariaDB Database Control**: Start, stop, and monitor database server,
- **Graceful Shutdown**: Properly terminate servers using appropriate shutdown commands,
- **Bulk Operations**: Start or stop all servers simultaneously.

### User Interface
- **System Tray Integration**: Minimize to system tray for background operation,
- **Single Instance**: Prevents multiple instances from running simultaneously,
- **Context Menus**: Right-click actions for quick access,
- **Status Indicators**: Visual feedback for server states.

## Requirements

### System Requirements
- **Operating System**: Windows 10 or later
- **Framework**: .NET Framework 4.8 
- **Architecture**: Any CPU (x86/x64)
- **Memory**: Minimum 512 MB RAM
- **Disk Space**: ~50 MB for application

### Server Requirements
- **Apache**: Apache HTTP Server 2.4+ (httpd.exe)
- **Database**: MariaDB 10.3+ (mariadbd.exe)
- **Configuration**: Valid httpd.conf and my.ini configuration files


## Usage

### Starting the Application
1. Launch `PwampoonControl.exe`
2. The control panel will initialize and detect your server installations,
3. Use the server control buttons to start/stop individual servers,
4. Monitor server status through the real-time log display.

### Server Operations
- **Start Server**: Click the start button for Apache or MySQL,
- **Stop Server**: Click the stop button to gracefully shutdown servers,
- **Start All**: Use the "Start All" button to launch all servers,
- **Stop All**: Use the "Stop All" button to shutdown all servers.


## Troubleshooting

### Common Issues
1. **Port Already in Use**: Check if other applications are using ports 80 (Apache) or 3306 (MySQL),
2. **Server Not Found**: Verify server installation paths in settings,
4. **Configuration Errors**: Check server configuration files for syntax errors.

### Getting Help
- Check the diagnostic logs in the application
- Review the error log files in the `wampoon-logs` directory
- Enable debug logging for detailed troubleshooting information

## Development

### Building from Source
1. Clone the repository
2. Open `Pwamp.ControlPanel.sln` in Visual Studio
5. Run the application (F5)


## Contributing

We welcome contributions! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Create a Pull Request

## Bug Reports

If you encounter any issues, please report them on our [GitHub Issues page](https://github.com/frostybee/pwamp-control/issues).

### When reporting a bug, please include:
- **Operating System**: Windows version and architecture
- **Application Version**: Version number from the About dialog
- **Steps to Reproduce**: Clear steps to reproduce the issue
- **Expected Behavior**: What you expected to happen
- **Actual Behavior**: What actually happened
- **Error Messages**: Any error messages or logs
- **Screenshots**: If applicable, add screenshots to help explain the problem

### Before submitting a bug report:
1. Check if the issue has already been reported
2. Try the latest version of the application
3. Review the troubleshooting section above
4. Gather relevant log files from the `wampoon-logs` directory

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- **GitHub Repository**: [https://github.com/frostybee/pwamp-control](https://github.com/frostybee/pwamp-control)
- **Issue Tracker**: [https://github.com/frostybee/pwamp-control/issues](https://github.com/frostybee/pwamp-control/issues)

---

**Note**: This application is designed for local development environments. For production use, consider proper server administration tools and security configurations.
> Use at your own risk!
