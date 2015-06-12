using System;
using System.Threading;
using System.Threading.Tasks;

namespace NDatabase.Multithreading.Tester
{
    class Point
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Layer { get; set; }
    }

    class Program
    {
        private const string DbName = "concurrent_access.ndb";
        private static readonly Random Random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Starting.");

            var task1 = Task.Factory.StartNew(Write1000Items);
            var task2 = Task.Factory.StartNew(ReadAllItems);
            var task3 = Task.Factory.StartNew(Write1000Items);
            var task4 = Task.Factory.StartNew(ReadAllItems);

            Task.WaitAll(task1, task2, task3, task4);
            Console.WriteLine("Done.");
        }

        private static void Write1000Items()
        {
            var managedThreadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("#" + managedThreadId + ": Starting to write 1000 elements.");

            using (var odb = OdbFactory.Open(DbName))
            {
                for (var i = 0; i < 1000; i++)
                {
                    var point = new Point
                                    {
                                        Layer = i,
                                        Name = "Name #" + i,
                                        X = i*Random.NextDouble(),
                                        Y = i*Random.NextDouble()
                                    };
                    odb.Store(point);
                }
            }

            Console.WriteLine("#" + managedThreadId + ": Writing done.");
        }

        private static void ReadAllItems()
        {
            var managedThreadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("#" + managedThreadId + ": Starting to read all items.");

            using (var odb = OdbFactory.Open(DbName))
            {
                var items = odb.QueryAndExecute<Point>();
                Console.WriteLine("#" + managedThreadId + ": Count: " + items.Count);
            }

            Console.WriteLine("#" + managedThreadId + ": All items are read.");
        }
    }
}
