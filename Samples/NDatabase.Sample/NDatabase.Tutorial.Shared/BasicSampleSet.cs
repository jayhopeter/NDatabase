using System;
using System.Diagnostics;
using System.IO;

namespace NDatabase.Tutorial.Shared
{
    public static class BasicSampleSet
    {
        public static void Process(string dbName, string projectName, Action step1, Action step2, Action step3, Action step4, Action step5, Action performanceStep)
        {
            if (File.Exists(dbName))
                File.Delete(dbName);

            Console.WriteLine("NDatabase Tutorial - basic for {0}\n", projectName);
            Console.WriteLine("DB Name: {0}", dbName);
            Console.WriteLine("\n\nSTEP1\n");
            var stopwatch = new Stopwatch();

            Console.WriteLine("Preparing two warriors with empty equipment");
            stopwatch.Start();

            step1();

            stopwatch.Stop();
            Display(stopwatch, "after storing two initial warriors");

            Console.WriteLine("\tFile size: {0} bytes", new FileInfo(dbName).Length);

            Console.WriteLine("\n\nSTEP2\n");
            Console.WriteLine("Retriving warriors, add equipment, update warriors.");

            stopwatch.Start();

            step2();

            stopwatch.Stop();
            Display(stopwatch, "after retriving warriors, assigning equipment and storing back into db");

            Console.WriteLine("\tFile size: {0} bytes", new FileInfo(dbName).Length);

            Console.WriteLine("\n\nSTEP3\n");
            Console.WriteLine("Retrieve and display stored items.");
            stopwatch.Start();

            step3();

            stopwatch.Stop();
            Display(stopwatch, "after displaying and then removing warriors");

            Console.WriteLine("\n\nSTEP4\n");
            Console.WriteLine("Retrieve and display stored warriors, then remove them.");
            stopwatch.Start();

            step4();

            stopwatch.Stop();
            Display(stopwatch, "after displaying and then removing warriors");

            Console.WriteLine("\tFile size: {0} bytes", new FileInfo(dbName).Length);

            Console.WriteLine("\n\nSTEP5\n");
            Console.WriteLine("Get warriors count from db");
            stopwatch.Start();

            step5();

            stopwatch.Stop();
            Display(stopwatch, "after getting empty warrior list");

            Console.WriteLine("\n\nPerformance Test:\n");
            performanceStep();
        }

        private static void Display(Stopwatch stopwatch, string action)
        {
            Console.WriteLine("Action: {0}, \n\tEllapsed: {1} ms.", action, stopwatch.ElapsedMilliseconds);
        }
    }
}