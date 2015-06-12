using System;
using System.Threading;
using NDatabase;
using NDatabase.Api;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Performance
{
    public class PerformanceTest1OnlySelect
    {
        public static int TestSize = 50000;

        public static readonly string OdbFileName = "perf-select.ndb";

        [SetUp]
        public virtual void BuildBase()
        {
            // Deletes the database file
            OdbFactory.Delete(OdbFileName);
            long t1 = 0;
            long t2 = 0;
            IOdb odb;
            // Insert TEST_SIZE objects
            Console.Out.WriteLine("Inserting " + TestSize + " objects");
            t1 = OdbTime.GetCurrentTimeInTicks();
            odb = OdbFactory.Open(OdbFileName);
            for (var i = 0; i < TestSize; i++)
            {
                object o = GetSimpleObjectInstance(i);
                odb.Store(o);
            }
            // System.out.println("Cache="+Dummy.getEngine(odb).getSession().getCache().toString());
            t2 = OdbTime.GetCurrentTimeInTicks();
            // Closes the database
            odb.Close();
        }

        [Test]
        public virtual void TestSelectSimpleObjectODB()
        {
            var t3 = OdbTime.GetCurrentTimeInTicks();
            var inMemory = true;
            Console.Out.WriteLine("Retrieving " + TestSize + " objects");
            // Reopen the database
            var odb = OdbFactory.Open(OdbFileName);
            // Gets the TEST_SIZE objects
            var query = odb.Query<object>();
            var l = query.Execute<object>(inMemory);
            Console.Out.WriteLine(l.GetType().FullName);
            var t4 = OdbTime.GetCurrentTimeInTicks();
            Console.Out.WriteLine("l.size=" + l.Count);
            var i = 0;
            while (l.HasNext())
            {
                object o = l.Next();
                
                i++;
            }
            var t5 = OdbTime.GetCurrentTimeInTicks();
            odb.Close();
            DisplayResult("ODB " + TestSize + " SimpleObject objects ", t3, t4, t5);
        }

        private SimpleObject GetSimpleObjectInstance(int i)
        {
            var so = new SimpleObject();
            so.SetDate(new DateTime());
            so.SetDuration(i);
            so.SetName("Bonjour, comment allez vous?" + i);
            return so;
        }

        private void DisplayResult(string @string, long t1, long t2, long t3)
        {
            var s1 = " total=" + (t3 - t1);
            var s3 = " total select=" + (t3 - t1) + " -- " + "select=" + (t2 - t1) + " get=" + (t3 - t2);
            var s4 = " time/object=" + (float) (t3 - t1) / +TestSize;
            Console.Out.WriteLine(@string + s1 + " | " + s3 + " | " + s4);
        }

        public static void Main2(string[] args)
        {
            var pt = new PerformanceTest1OnlySelect();
            Thread.Sleep(20000);
            pt.TestSelectSimpleObjectODB();
        }
    }
}
