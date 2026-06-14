# Resource Monitor: Real-Time Console Performance Tracker

**Resource Monitor** is a lightweight, terminal-based system monitoring utility written in C# that provides a live, animated dashboard of vital hardware metrics directly within the console. Designed as an efficient pet project to explore Windows system diagnostics, the application bypasses heavy graphical interfaces to deliver real-time performance tracking with minimal overhead. It captures and visualizes processor load, memory consumption, and storage drive activity using dense text layouts and character-based progress bars.

## How It Works

The architecture of the application relies on a continuous polling loop throttled to one-second intervals via thread sleeping, which balances readable data updates with low CPU utilization. Upon execution, the program hooks into the native Windows diagnostics subsystem using performance counters to poll hardware infrastructure. The user interface updates fluidly in place rather than printing a continuous scrolling wall of text; this pseudo-animated rendering is achieved by capturing the console cursor's vertical coordinate and resetting it upward by four lines at the end of every evaluation cycle, forcing the terminal to seamlessly overwrite the previous data block.
