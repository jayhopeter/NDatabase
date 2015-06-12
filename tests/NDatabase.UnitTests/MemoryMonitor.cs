using System;
using System.Diagnostics;
using System.Text;

namespace NDatabase.UnitTests
{
    public static class MemoryMonitor
    {
        private static readonly PerformanceCounter Memory = new PerformanceCounter("Memory", "Available MBytes");

        public static void DisplayCurrentMemory(string label, bool all)
        {
            var buffer = new StringBuilder();

            buffer.Append(label).Append(":Free=").Append(Memory.NextValue()).Append("k / Total=").Append("?").Append("k");

            Console.Out.WriteLine(buffer.ToString());
        }
    }
}
