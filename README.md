# Resource Monitor: Real-Time Console Performance Tracker

**Resource Monitor** is a lightweight, terminal-based system monitoring utility written in C# that provides a live, animated dashboard of vital hardware metrics directly within the console. Designed as an efficient pet project to explore Windows system diagnostics, the application bypasses heavy graphical interfaces to deliver real-time performance tracking with minimal overhead. It captures and visualizes processor load, memory consumption, and storage drive activity using dense text layouts and character-based progress bars.

## How It Works

The architecture of the application relies on a continuous polling loop throttled to one-second intervals via thread sleeping, which balances readable data updates with low CPU utilization. Upon execution, the program hooks into the native Windows diagnostics subsystem using performance counters to poll hardware infrastructure. The user interface updates fluidly in place rather than printing a continuous scrolling wall of text; this pseudo-animated rendering is achieved by capturing the console cursor's vertical coordinate and resetting it upward by four lines at the end of every evaluation cycle, forcing the terminal to seamlessly overwrite the previous data block.

## Troubleshooting Character Encoding Issues

If the application displays broken characters, misaligned symbols, or incorrect text in the terminal, follow the steps below to enable UTF-8 support on Windows.

> **Note:** This change is only necessary if you encounter character encoding problems while running the application.

### Step 1 — Open the Region Settings Window

Press **Win + R** to open the **Run** dialog.

Type:

```text
intl.cpl
```

and press **Enter**.

<p align="center">
  <img width="482" height="283" alt="Screenshot 2026-06-15 164231" src="https://github.com/user-attachments/assets/3dd245c9-e30f-43d1-b5d5-84834ad296d3" />
</p>


---

### Step 2 — Enable UTF-8 Support

In the **Region** window:

1. Open the **Administrative** tab.
2. Click **Change system locale...**
3. Check **Beta: Use Unicode UTF-8 for worldwide language support**.
4. Click **OK**.

<p align="center">
  <img width="628" height="782" alt="Screenshot 2026-06-15 164503" src="https://github.com/user-attachments/assets/4d83ccee-8850-45fe-a158-b29074608da5" />
</p>


---

### Step 3 — Restart Windows

Restart your computer to apply the change.

After the restart, launch the application again. The terminal should now display Unicode characters correctly.
