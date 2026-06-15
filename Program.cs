using System;
using System.Diagnostics;
using System.Threading;
using static System.Console;

namespace Application
{
    class Program
    {

        static int getRAMSize()
        {
            // 1. Get total bytes
            ulong memBytes = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;

            // 2. Convert to raw MB as a decimal
            double rawMb = (double) memBytes / (1024 * 1024);

            // 3. Divide by 1024 to find the "fractional GBs", round UP, then multiply back by 1024
            int exactMb = (int) Math.Ceiling(rawMb / 1024.0) * 1024;

            return exactMb;
        }

        static void Main(string[] args)
        {
            // Nullable variables to store system metrics. 
            // Making them nullable allows us to later verify they successfully received data (HasValue) before displaying them.
            double? CPU_inUse = null, RAM_inUse = null, disk_activity = null;

            // Initialize Windows performance counters for CPU, RAM, and Disk.
            // We call NextValue() once immediately because the first read of these counters typically returns 0.
            PerformanceCounter cpuUsage = new("Processor Information", "% Processor Utility", "_Total");
            cpuUsage.NextValue();

            PerformanceCounter availableRAM = new("Memory", "Available MBytes");
            availableRAM.NextValue();

            PerformanceCounter diskIdleTime = new("PhysicalDisk", "% Idle Time", "_Total");
            diskIdleTime.NextValue();

            // Store how many megabytes of RAM the computer has in the RAMsize variable
            int RAMsize = getRAMSize();

            try
            {
                // Infinite loop to continuously monitor metrics and update the console display.
                while (true)
                {
                    // Pause execution for 1 second (1000ms) to prevent overwhelming the CPU and to provide readable UI updates.
                    Thread.Sleep(1000);

                    // Fetch the latest values. 
                    // Disk activity is calculated inversely from idle time. Clamp prevents values outside the 0-100 range.
                    CPU_inUse = cpuUsage.NextValue();
                    RAM_inUse = 100 * ((RAMsize - availableRAM.NextValue()) / RAMsize);
                    disk_activity = Math.Clamp((100 - diskIdleTime.NextValue()), 0, 100);

                    // Construct a 20-character visual progress bar for the CPU. 
                    // Each block represents 5%. It concatenates 'busy' (filled) and 'free' (empty) characters.
                    int numberCPU = CPU_inUse >= 99.9 ? 20 : (int) CPU_inUse / 5;
                    string CPU_busy = new('■', numberCPU);
                    string CPU_free = new('□', 20 - numberCPU);
                    string CPU = string.Concat(CPU_busy, CPU_free);

                    // Construct the 20-character progress bar for RAM.
                    int numberRAM = RAM_inUse >= 99.9 ? 20 : (int) RAM_inUse / 5;
                    string RAM_busy = new('■', numberRAM);
                    string RAM_free = new('□', 20 - numberRAM);
                    string RAM = string.Concat(RAM_busy, RAM_free);

                    // Construct the 20-character progress bar for Disk Activity.
                    int numberdisk = disk_activity >= 99.9 ? 20 : (int) disk_activity / 5;
                    string disk_busy = new('■', numberdisk);
                    string disk_free = new('□', 20 - numberdisk);
                    string disk = string.Concat(disk_busy, disk_free);

                    // Verify all nullable variables have valid data before attempting to print to the console.
                    if (CPU_inUse.HasValue && RAM_inUse.HasValue && disk_activity.HasValue)
                    {
                        // Print the current time as the dashboard header.
                        WriteLine(DateTime.Now.ToString("T"));

                        // Output CPU metric. If it hits 100%, display (MAXED) instead of the percentage to maintain clean formatting.
                        if (Math.Round(CPU_inUse ?? 0.0, 1) <= 99.9)
                        {
                            WriteLine($"CPU Usage:\t{CPU}\t\t({CPU_inUse:00.0}%)");
                        }
                        else
                        {
                            WriteLine($"CPU Usage:\t{CPU}\t\t(MAXED)");
                        }

                        // Output RAM metric, handling the maxed-out edge case.
                        if (Math.Round(RAM_inUse ?? 0.0, 1) <= 99.9)
                        {
                            WriteLine($"RAM Usage:\t{RAM}\t\t({RAM_inUse:00.0}%)");
                        }
                        else
                        {
                            WriteLine($"RAM Usage:\t{RAM}\t\t(MAXED)");
                        }

                        // Output Disk metric, handling the maxed-out edge case.
                        if (Math.Round(disk_activity ?? 0.0, 1) <= 99.9)
                        {
                            WriteLine($"Disk Activity:\t{disk}\t\t({disk_activity:00.0}%)");
                        }
                        else
                        {
                            WriteLine($"Disk Activity:\t{disk}\t\t(MAXED)");
                        }

                        // Move the console cursor up 4 lines. 
                        // This ensures the next loop iteration overwrites the current text, creating an in-place animation.
                        SetCursorPosition(0, CursorTop - 4);
                    }
                }
            }
            catch (Exception e)
            {
                // Catch any runtime exceptions (e.g., performance counters not existing on the machine) 
                // and print them safely below the dashboard area.
                WriteLine($"\n\n\n\n\n{e}");
            }
        }

    }
}
