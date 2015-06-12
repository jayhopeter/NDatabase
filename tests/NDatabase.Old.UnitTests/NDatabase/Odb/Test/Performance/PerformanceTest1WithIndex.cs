using System;
using NDatabase.Api;
using NDatabase.Api.Query;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Performance
{
    public class PerformanceTest1WithIndex : ODBTest
    {
        public static int TestSize = 110;

        public static string OdbFileName = "perf.ndb";

        [Test]
        public virtual void Test2()
        {
            T1estInsertSimpleObjectODB(200);
        }

        public virtual void T1estInsertSimpleObjectODB(int size)
        {
            OdbFileName = "perf-" + size + ".ndb";
            TestSize = size;
            var doUpdate = true;
            var doDelete = true;

            var inMemory = true;

            // Deletes the database file
            DeleteBase(OdbFileName);
            long t1 = 0;
            long t2 = 0;
            long t3 = 0;
            long t4 = 0;
            long t5 = 0;
            long t6 = 0;
            long t7 = 0;
            long t77 = 0;
            long t8 = 0;
            IOdb odb = null;
            IObjectSet<SimpleObject> l = null;
            SimpleObject so = null;
            // Insert TEST_SIZE objects
            Println("Inserting " + TestSize + " objects");
            t1 = OdbTime.GetCurrentTimeInMs();
            odb = Open(OdbFileName);
            var fields = new[] {"name"};
            odb.IndexManagerFor<SimpleObject>().AddUniqueIndexOn("index1", fields);
            for (var i = 0; i < TestSize; i++)
            {
                object o = GetSimpleObjectInstance(i);
                odb.Store(o);
            }
            // println("Cache="+Dummy.getEngine(odb).getSession().getCache().toString());
            t2 = OdbTime.GetCurrentTimeInMs();
            // Closes the database
            odb.Close();
            // if(true)return;
            t3 = OdbTime.GetCurrentTimeInMs();
            Println("Retrieving " + TestSize + " objects");
            // Reopen the database
            odb = Open(OdbFileName);
            // Gets the TEST_SIZE objects
            t4 = OdbTime.GetCurrentTimeInMs();
            IQuery q = null;
            for (var j = 0; j < TestSize; j++)
            {
                // println("Bonjour, comment allez vous?" + j);
                q = odb.Query<SimpleObject>();
                q.Descend("name").Constrain((object) ("Bonjour, comment allez vous?" + j)).Equal();
                var objects = q.Execute<SimpleObject>();
                so = objects.GetFirst();
                if (!so.GetName().Equals("Bonjour, comment allez vous?" + j))
                {
                    throw new Exception("error while getting object : expected = " + "Bonjour, comment allez vous?" + j +
                                        " / actual = " + so.GetName());
                }
                if (j % 1000 == 0)
                    Println("got " + j + " objects");
            }
            t5 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            odb = Open(OdbFileName);
            if (doUpdate)
            {
                Println("Updating " + TestSize + " objects");
                so = null;
                var query = odb.Query<SimpleObject>();
                l = query.Execute<SimpleObject>(inMemory);
                while (l.HasNext())
                {
                    so = l.Next();
                    so.SetName(so.GetName().ToUpper());
                    odb.Store(so);
                }
            }
            t6 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            // if(true)return;
            t7 = OdbTime.GetCurrentTimeInMs();
            if (doDelete)
            {
                Println("Deleting " + TestSize + " objects");
                odb = Open(OdbFileName);
                Println("After open - before delete");
                var query = odb.Query<SimpleObject>();
                l = query.Execute<SimpleObject>(inMemory);
                t77 = OdbTime.GetCurrentTimeInMs();
                Println("After getting objects - before delete");
                var i = 0;
                while (l.HasNext())
                {
                    so = l.Next();
                    if (!so.GetName().StartsWith("BONJOUR"))
                        throw new Exception("Update  not ok for " + so.GetName());
                    odb.Delete(so);
                    if (i % 10000 == 0)
                        Println("s=" + i);
                    // println("Cache="+Dummy.getEngine(odb).getSession().getCache().toString());
                    i++;
                }
                odb.Close();
            }
            t8 = OdbTime.GetCurrentTimeInMs();
            // t4 2 times
            DisplayResult("ODB " + TestSize + " SimpleObject objects ", t1, t2, t4, t4, t5, t6, t7, t77, t8);
        }

        private SimpleObject GetSimpleObjectInstance(int i)
        {
            var so = new SimpleObject();
            so.SetDate(new DateTime());
            so.SetDuration(i);
            so.SetName("Bonjour, comment allez vous?" + i);
            return so;
        }

        private void DisplayResult(string @string, long t1, long t2, long t3, long t4, long t5, long t6, long t7,
                                   long t77, long t8)
        {
            var s1 = " total=" + (t8 - t1);
            var s2 = " total insert=" + (t3 - t1) + " -- " + "insert=" + (t2 - t1) + " commit=" + (t3 - t2) + " o/s=" +
                     TestSize / (float) ((t3 - t1)) * 1000;
            var s3 = " total select=" + (t5 - t3) + " -- " + "select=" + (t4 - t3) + " get=" + (t5 - t4) + " o/s=" +
                     TestSize / (float) ((t5 - t3)) * 1000;
            var s4 = " total update=" + (t7 - t5) + " -- " + "update=" + (t6 - t5) + " commit=" + (t7 - t6) + " o/s=" +
                     TestSize / (float) ((t7 - t5)) * 1000;
            var s5 = " total delete=" + (t8 - t7) + " -- " + "select=" + (t77 - t7) + " - delete=" + (t8 - t77) +
                     " o/s=" + TestSize / (float) ((t8 - t7)) * 1000;
            Println(@string + s1 + " | " + s2 + " | " + s3 + " | " + s4 + " | " + s5);
            var tinsert = t3 - t1;
            var tselect = t5 - t3;
            var tupdate = t7 - t5;
            var tdelete = t8 - t7;

            Assert.That(tinsert, Is.LessThan(7500));
            Assert.That(tselect, Is.LessThan(4000));
            Assert.That(tupdate, Is.LessThan(9000));
            Assert.That(tdelete, Is.LessThan(6000));
        }

        public static void Main2(string[] args)
        {
            var pt = new PerformanceTest1WithIndex();
            // Thread.sleep(20000);
            // LogUtil.allOn(true);
            pt.T1estInsertSimpleObjectODB(10000);
        }
    }
}
