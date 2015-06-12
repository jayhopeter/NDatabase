using System;
using System.Diagnostics;
using System.Linq;
using NDatabase.Performance.Tester.Domain;

namespace NDatabase.Performance.Tester
{
    class Program
    {
        private const string DbName = "perf.ndb";

        static void Main(string[] args)
        {
            if (args[0] == "q")
            {
                CheckQuery();
                return;
            }

            var count = Convert.ToInt32(args[0]);

            OdbFactory.Delete(DbName);

            for (var i = 1; i <= count; i++)
            {
                Console.WriteLine("Start processing: " + i + ".");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                using (var odb = OdbFactory.Open(DbName))
                {
                    odb.Store(PrepareNewItem(i));
                }
                stopwatch.Stop();

                Console.WriteLine("Processed " + i * (100 * 100 * 100 + 1) + " items in " + stopwatch.ElapsedMilliseconds + " ms.");

                //                Console.WriteLine("Start generating count for " + i + " iteration.");
                //                stopwatch.Reset();
                //                stopwatch.Start();
                //                long count1;
                //                using (var odb = OdbFactory.Open(DbName))
                //                {
                //                    count1 = odb.QueryAndExecute<object>().Count;
                //                }
                //                stopwatch.Stop();
                //                Console.WriteLine("Items in db: " + count1 + ". Time: " + stopwatch.ElapsedMilliseconds + " ms.");
            }
        }

        private static void CheckQuery()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var odb = OdbFactory.Open(DbName))
            {
                var storedItem = (from item in odb.QueryAndExecute<HolderB>().AsQueryable()
                                 where item.Id.Equals(10)
                                 select item).First();

                Console.WriteLine(storedItem.Name);
            }

            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds + "ms.");
        }

        private static object PrepareNewItem(int i)
        {
            var holderA = new HolderA { Id = i, Name = "HolderA" + i };

            for (var j = 0; j < 100; j++)
            {
                var holderB = new HolderB { Id = j, Name = "HolderB" + j };
                holderA.ListOfHolderBItems.Add(holderB);

                for (var k = 0; k < 100; k++)
                {
                    var holderC = new HolderC { Id = k, Name = "HolderC" + k };
                    holderB.ListOfHolderCItems.Add(holderC);

                    for (var l = 0; l < 100; l++)
                    {
                        var item = new FinalItem { Id = l, Name = "FinalItem" + l };
                        holderC.FinalItems.Add(item);
                    }
                }
            }

            return holderA;
        }
    }
}
