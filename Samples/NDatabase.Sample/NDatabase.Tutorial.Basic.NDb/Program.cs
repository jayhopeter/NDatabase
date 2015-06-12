using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NDatabase.Tutorial.Shared;
using NDatabase.Tutorial.Shared.Heroes;
using NDatabase.Tutorial.Shared.Items;

namespace NDatabase.Tutorial.Basic.NDb
{
    class Program
    {
        private const string DBName = "game.ndb";
        private const string ProjectName = "NDatabase";

        static void Main()
        {
            BasicSampleSet.Process(DBName, ProjectName, Step1, Step2, Step3, Step4, Step5, PerformanceStep);
        }

        private static void PerformanceStep()
        {
            const int count = 10000;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var odb = OdbFactory.OpenLast())
            {
                for (var i = 0; i < count; i++)
                    odb.Store(new Warrior(string.Format("Warrior {0}", i + 1)));
            }

            stopwatch.Stop();
            Display(stopwatch, string.Format("insert of {0} warriors", count));

            stopwatch.Start();

            using (var odb = OdbFactory.OpenLast())
            {
                var warriors = odb.QueryAndExecute<Warrior>().ToList();

                for (var i = 0; i < count; i++)
                {
                    warriors[i].LeftHand = new Field(i+3);
                    warriors[i].RightHand = new Sword(i+2);
                    odb.Store(warriors[i]);
                }
            }

            stopwatch.Stop();
            Display(stopwatch, string.Format("query and update of {0} warriors and {1} items", count, count * 2));

            stopwatch.Start();

            using (var odb = OdbFactory.OpenLast())
            {
                var warriors = odb.QueryAndExecute<Warrior>().ToList();

                foreach (var warrior in warriors)
                    odb.Delete(warrior);
            }

            stopwatch.Stop();
            Display(stopwatch, string.Format("query and remove of {0} warriors", count));

            stopwatch.Start();

            using (var odb = OdbFactory.OpenLast())
            {
                var finalCount = odb.QueryAndExecute<Warrior>().Count;
                Console.WriteLine("Final count: {0}", finalCount);
            }

            stopwatch.Stop();
            Display(stopwatch, string.Format("query empty list"));
        }

        private static void Display(Stopwatch stopwatch, string action)
        {
            Console.WriteLine("Action: {0}, \n\tEllapsed: {1} ms.", action, stopwatch.ElapsedMilliseconds);
            Console.WriteLine("\tFile size: {0} bytes", new FileInfo(DBName).Length);
        }

        private static void Step1()
        {
            var warrior1 = new Warrior("Warrior 1");
            var warrior2 = new Warrior("Warrior 2");

            using (var odb2 = OdbFactory.Open(DBName))
            {
                odb2.Store(warrior1);
                odb2.Store(warrior2);
            }
        }

        private static void Step2()
        {
            var sword1 = new Sword(5);
            var sword2 = new Sword(3);

            var field1 = new Field(3);
            var field2 = new Field(5);


            using (var odb = OdbFactory.OpenLast())
            {
                IList<Warrior> warriors = odb.QueryAndExecute<Warrior>().ToList();

                warriors[0].RightHand = sword1;
                warriors[0].LeftHand = field1;

                warriors[1].RightHand = sword2;
                warriors[1].LeftHand = field2;

                odb.Store(warriors[0]);
                odb.Store(warriors[1]);
            }
        }

        private static void Step3()
        {
            using (var odb1 = OdbFactory.OpenLast())
            {
                IList<IItem> items = odb1.QueryAndExecute<IItem>().ToList();

                foreach (var item in items)
                    Console.WriteLine(item);
            }
        }

        private static void Step4()
        {
            using (var odb1 = OdbFactory.OpenLast())
            {
                IList<Warrior> warriors = odb1.QueryAndExecute<Warrior>().ToList();

                foreach (var warrior in warriors)
                    Console.WriteLine(warrior);

                Console.WriteLine("Remove warriors");

                odb1.Delete(warriors[0]);
                odb1.Delete(warriors[1]);
            }
        }

        private static void Step5()
        {
            int count;
            using (var odb = OdbFactory.OpenLast())
                count = odb.QueryAndExecute<Warrior>().Count;

            Console.WriteLine("Warriors count: {0}", count);
        }
    }
}
