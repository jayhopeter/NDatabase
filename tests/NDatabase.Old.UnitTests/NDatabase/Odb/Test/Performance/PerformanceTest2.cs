using System;
using System.Collections.Generic;
using NDatabase;
using NDatabase.Api;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Performance
{
    public class PerformanceTest2
    {
        public static int TestSize = 10000;

        public static readonly string OdbFileName = "perf.ndb";

        [Test]
        public virtual void TestInsertSimpleObjectODB()
        {
            var inMemory = true;
            // Deletes the database file
            OdbFactory.Delete(OdbFileName);
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
            IObjectSet<User> l = null;
            User so = null;
            // Insert TEST_SIZE objects
            Console.Out.WriteLine("Inserting " + TestSize + " objects");
            t1 = OdbTime.GetCurrentTimeInTicks();
            odb = OdbFactory.Open(OdbFileName);

            for (var i = 0; i < TestSize; i++)
            {
                var o = GetUserInstance(i);
                odb.Store(o);
            }
            t2 = OdbTime.GetCurrentTimeInTicks();
            // Closes the database
            odb.Close();
            t3 = OdbTime.GetCurrentTimeInTicks();
            Console.Out.WriteLine("Retrieving " + TestSize + " objects");
            // Reopen the database
            odb = OdbFactory.Open(OdbFileName);
            // Gets retrieve the TEST_SIZE objects
            var query = odb.Query<User>();
            l = query.Execute<User>(inMemory);
            t4 = OdbTime.GetCurrentTimeInTicks();
            // Actually get objects
            while (l.HasNext())
            {
                object o = l.Next();
            }
            t5 = OdbTime.GetCurrentTimeInTicks();
            Console.Out.WriteLine("Updating " + TestSize + " objects");
            so = null;
            l.Reset();
            // Actually get objects
            while (l.HasNext())
            {
                so = l.Next();
                // so.setName(so.getName() + " updated");
                // so.setName(so.getName() + " updated-updated-updated-updated");
                so.GetProfile().SetName(so.GetName() + " updated-updated-updated");
                odb.Store(so);
            }
            t6 = OdbTime.GetCurrentTimeInTicks();
            odb.Close();
            t7 = OdbTime.GetCurrentTimeInTicks();
            Console.Out.WriteLine("Deleting " + TestSize + " objects");
            odb = OdbFactory.Open(OdbFileName);
            var query1 = odb.Query<User>();
            l = query1.Execute<User>(inMemory);
            t77 = OdbTime.GetCurrentTimeInTicks();
            // Actually get objects
            while (l.HasNext())
            {
                so = l.Next();
                odb.Delete(so);
            }
            odb.Close();
            t8 = OdbTime.GetCurrentTimeInTicks();
            odb = OdbFactory.Open(OdbFileName);
            odb.Close();
            DisplayResult("ODB " + TestSize + " User objects ", t1, t2, t3, t4, t5, t6, t7, t77, t8);
        }

        private object GetUserInstance(int i)
        {
            var login = new VO.Login.Function("login" + i);
            var logout = new VO.Login.Function("logout" + i);
            var list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile = new Profile("operator" + i, list);
            var user = new User("olivier smadja" + i, "user@ndatabase.net", profile);
            return user;
        }

        private void DisplayResult(string @string, long t1, long t2, long t3, long t4, long t5, long t6, long t7,
                                   long t77, long t8)
        {
            var s1 = " total=" + (t8 - t1);
            var s2 = " total insert=" + (t3 - t1) + " -- " + "insert=" + (t2 - t1) + " commit=" + (t3 - t2);
            var s3 = " total select=" + (t5 - t3) + " -- " + "select=" + (t4 - t3) + " get=" + (t5 - t4);
            var s4 = " total update=" + (t7 - t5) + " -- " + "update=" + (t6 - t5) + " commit=" + (t7 - t6);
            var s5 = " total delete=" + (t8 - t7) + " -- " + "select=" + (t77 - t7) + " - delete=" + (t8 - t77);
            Console.Out.WriteLine(@string + s1 + " | " + s2 + " | " + s3 + " | " + s4 + " | " + s5);
        }
    }
}
