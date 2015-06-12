using System;
using System.Collections.Generic;
using NDatabase;
using NDatabase.Api;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Performance
{
    [TestFixture]
    public class TestFilePersisterHighPerformance2 : ODBTest
    {
        public static int TestSize = 1000;

        public static readonly string OdbFileName = "perf.ndb";

        public static readonly string Db4oFileName = "perf.yap";

        // public static final String ODB_FILE_NAME = "k:/tmp/perf.ndb";

        private void DisplayResult(string @string, long t1, long t2, long t3, long t4, long t5, long t6, long t7,
                                   long t77, long t8)
        {
            var s1 = " total=" + (t8 - t1);
            var s2 = " total insert=" + (t3 - t1) + " -- " + "insert=" + (t2 - t1) + " commit=" + (t3 - t2);
            var s3 = " total select=" + (t5 - t3) + " -- " + "select=" + (t4 - t3) + " get=" + (t5 - t4);
            var s4 = " total update=" + (t7 - t5) + " -- " + "update=" + (t6 - t5) + " commit=" + (t7 - t6);
            var s5 = " total delete=" + (t8 - t7) + " -- " + "select=" + (t77 - t7) + " - delete=" + (t8 - t77);
            Println(@string + s1 + " | " + s2 + " | " + s3 + " | " + s4 + " | " + s5);
        }

        private object GetUserInstance()
        {
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile = new Profile("operator", list);
            var user = new User("olivier smadja", "user@ndatabase.net", profile);
            return user;
        }

        private TestClass GetTestClassInstance()
        {
            var tc = new TestClass();
            tc.SetBigDecimal1(new Decimal(1.123456789));
            tc.SetBoolean1(true);
            tc.SetChar1('d');
            tc.SetDouble1(154.78998989);
            tc.SetInt1(78964);
            tc.SetString1("Ola chico como vc est√° ???");
            tc.SetDate1(new DateTime());
            return tc;
        }

        private SimpleObject GetSimpleObjectInstance(int i)
        {
            var so = new SimpleObject();
            so.SetDate(new DateTime());
            so.SetDuration(i);
            so.SetName("ola chico, como vc esta?" + i);
            return so;
        }

        public static void Main2(string[] args)
        {
            new TestFilePersisterHighPerformance2().TestInsertSimpleObjectODB();
        }

        [Test]
        public virtual void TestInsertSimpleObjectODB()
        {
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
            t1 = OdbTime.GetCurrentTimeInTicks();
            odb = Open(OdbFileName);
            for (var i = 0; i < TestSize; i++)
            {
                object o = GetSimpleObjectInstance(i);
                odb.Store(o);
                if (i % 20000 == 0)
                {
                    Console.Out.Write(".");
                    Println("After insert=" + ((global::NDatabase.Odb)odb).GetStorageEngine().GetSession().GetCache().ToString());
                }
            }
            //
            var engine = ((global::NDatabase.Odb)odb).GetStorageEngine();

            // println("NB WA="+WriteAction.count);
            Println("NB WAs=" + engine.GetSession().GetTransaction().GetNumberOfWriteActions());
            t2 = OdbTime.GetCurrentTimeInTicks();
            odb.Commit();
            Println("after commit : NB WAs=" + engine.GetSession().GetTransaction().GetNumberOfWriteActions());
            // if(true)return;
            // println("After commit="+Dummy.getEngine(odb).getSession().getCache().toString());
            // println("NB WA="+WriteAction.count);
            t3 = OdbTime.GetCurrentTimeInTicks();
            // println("end of insert");
            var query = odb.Query<SimpleObject>();
            l = query.Execute<SimpleObject>(false);
            // println("end of getObjects ");
            t4 = OdbTime.GetCurrentTimeInTicks();
            // println("After getObjects ="+Dummy.getEngine(odb).getSession().getCache().toString());
            // println("NB WA="+WriteAction.count);

            Println("after select : NB WAs=" + engine.GetSession().GetTransaction().GetNumberOfWriteActions());
            var nbObjects = l.Count;
            Println(nbObjects + " objects ");
            var k = 0;
            while (l.HasNext())
            {
                object o = l.Next();
                if (k % 9999 == 0)
                    Println(((SimpleObject) o).GetName());
                k++;
            }
            // println("end of real get ");
            t5 = OdbTime.GetCurrentTimeInTicks();
            Println("select " + (t5 - t3) + " - " + (t5 - t4));
            so = null;
            k = 0;
            l.Reset();
            while (l.HasNext())
            {
                so = l.Next();
                so.SetName(so.GetName() + " updated");
                odb.Store(so);
                if (k % 10000 == 0)
                {
                    Println("update " + k);
                    Println("after update : NB WAs=" +
                            engine.GetSession().GetTransaction().GetNumberOfWriteActions());
                    Println("After update=" + ((global::NDatabase.Odb)odb).GetStorageEngine().GetSession().GetCache().ToString());
                }
                k++;
            }
            Println("after update : NB WAs=" + engine.GetSession().GetTransaction().GetNumberOfWriteActions());
            t6 = OdbTime.GetCurrentTimeInTicks();
            odb.Close();
            t7 = OdbTime.GetCurrentTimeInTicks();
            odb = Open(OdbFileName);
            var query1 = odb.Query<SimpleObject>();
            l = query1.Execute<SimpleObject>(false);
            t77 = OdbTime.GetCurrentTimeInTicks();
            var j = 0;
            while (l.HasNext())
            {
                so = l.Next();
                AssertTrue(so.GetName().EndsWith("updated"));
                odb.Delete(so);
                if (j % 10000 == 0)
                    Println("delete " + j);
                j++;
            }
            odb.Close();
            t8 = OdbTime.GetCurrentTimeInTicks();
            odb = Open(OdbFileName);
            var query2 = odb.Query<SimpleObject>();
            AssertEquals(0, query2.Execute<SimpleObject>().Count);
            odb.Close();
            DisplayResult("ODB " + TestSize + " SimpleObject objects ", t1, t2, t3, t4, t5, t6, t7, t77, t8);
        }

        [Test]
        public virtual void TestInsertUserODB()
        {
            DeleteBase(OdbFileName);
            long t1;
            long t2;
            long t3;
            long t4;
            long t5;
            long t6;
            long t7;
            long t77;
            long t8;
            t1 = OdbTime.GetCurrentTimeInTicks();
            var odb = Open(OdbFileName);
            for (var i = 0; i < TestSize; i++)
            {
                var o = GetUserInstance();
                odb.Store(o);
                if (i % 1000 == 0)
                    Console.Out.Write(".");
            }
            t2 = OdbTime.GetCurrentTimeInTicks();
            // assertEquals(TEST_SIZE,
            // odb.getSession().getCache().getNumberOfObjects ());
            var engine = ((global::NDatabase.Odb)odb).GetStorageEngine();
            Println("NB WAs=" + engine.GetSession().GetTransaction().GetNumberOfWriteActions());
            odb.Commit();
            t3 = OdbTime.GetCurrentTimeInTicks();
            Println("end of insert");
            var query = odb.Query<User>();
            var l = query.Execute<User>(false);
            t4 = OdbTime.GetCurrentTimeInTicks();
            var nbObjects = l.Count;
            Println(nbObjects + " objects ");
            User user = null;
            while (l.HasNext())
            {
                // println(i);
                user = l.Next();
            }
            // assertEquals(TEST_SIZE,
            // odb.getSession().getCache().getNumberOfObjects ());
            Println("end of real get objects");
            t5 = OdbTime.GetCurrentTimeInTicks();
            user = null;
            var j = 0;
            l.Reset();
            while (l.HasNext())
            {
                // println(i);
                user = l.Next();
                user.SetName(user.GetName() + " updated" + j);
                odb.Store(user);
                j++;
            }
            t6 = OdbTime.GetCurrentTimeInTicks();
            Println("end of update");
            odb.Close();
            t7 = OdbTime.GetCurrentTimeInTicks();
            odb = Open(OdbFileName);
            var query1 = odb.Query<User>();
            l = query1.Execute<User>();
            t77 = OdbTime.GetCurrentTimeInTicks();
            j = 0;
            while (l.HasNext())
            {
                user = l.Next();
                Println(j + " " + user.GetName());
                AssertTrue(user.GetName().EndsWith("updated" + j));
                odb.Delete(user);
                j++;
            }
            odb.Close();
            t8 = OdbTime.GetCurrentTimeInTicks();
            odb = Open(OdbFileName);
            var query2 = odb.Query<User>();
            AssertEquals(0, query2.Execute<User>().Count);
            odb.Close();
            DisplayResult("ODB " + TestSize + " User objects ", t1, t2, t3, t4, t5, t6, t7, t77, t8);
            DeleteBase(OdbFileName);
        }

        // new TestFilePersisterHighPerformance2().testInsertUserODB();
        // new TestFilePersisterHighPerformance2().testDeleteSimpleObjectODB();
    }
}
