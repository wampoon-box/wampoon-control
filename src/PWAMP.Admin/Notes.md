Starting Apache with //RedirectStandardOutput = true,
                    //RedirectStandardError = true
Will not work. We need to monitor the Apache's log files instead.
- TODO: a workaround needs to be implemented to monitor Apache's output and redirect it to the main window.

## Starting Apache:

- Since Apache is not launched a service, we need to start it as a console application. Hence, capturing the output and error is not possible due to the fact that Apache is awaiting the SIGTERM signal to stop.
- The only way to check for errors is to read Apache's log files.

