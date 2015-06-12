using System;
using System.Collections.Generic;
using NDatabase;
using NDatabase.Api;
using NDatabase.Exceptions;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Delete
{
    [TestFixture]
    public class TestDelete : ODBTest
    {
        private static readonly long start = OdbTime.GetCurrentTimeInMs();

        public static string FileName1 = "test-delete.ndb";

        public static string FileName2 = "test-delete-defrag.ndb";

        public override void TearDown()
        {
            // deleteBase("t-delete12.ndb");
            DeleteBase("t-delete1.ndb");
        }

        [Test]
        public virtual void Test1()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            decimal n = odb.Query<VO.Login.Function>().Count();
            var function1 = new VO.Login.Function("function1");
            var function2 = new VO.Login.Function("function2");
            var function3 = new VO.Login.Function("function3");
            odb.Store(function1);
            odb.Store(function2);
            odb.Store(function3);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            query.Descend("name").Constrain((object) "function2").Equal();
            var l = query.Execute<VO.Login.Function>();
            var function = l.GetFirst();
            odb.Delete(function);
            odb.Close();
            odb = Open(baseName);
            var query1 = odb.Query<VO.Login.Function>();
            var l2 = query1.Execute<VO.Login.Function>(true);
            AssertEquals(n + 2, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            DeleteBase(baseName);
        }

        /// <summary>
        ///   Test : delete the unique object
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test10()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            long size = query.Execute<VO.Login.Function>().Count;
            var f1 = new VO.Login.Function("function1");
            odb.Store(f1);
            odb.Close();
            odb = Open(baseName);
            var query1 = odb.Query<VO.Login.Function>();
            var f1bis = query1.Execute<VO.Login.Function>().GetFirst();
            odb.Delete(f1bis);
            odb.Close();
            odb = Open(baseName);
            var query3 = odb.Query<VO.Login.Function>();
            AssertEquals(size, query3.Execute<VO.Login.Function>().Count);
            odb.Store(new VO.Login.Function("last function"));
            odb.Close();
            odb = Open(baseName);
            var query2 = odb.Query<VO.Login.Function>();
            var l = query2.Execute<VO.Login.Function>();
            odb.Close();
            AssertEquals(size + 1, l.Count);
        }

        /// <summary>
        ///   Test : delete the unique object
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test11()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var size = odb.Query<VO.Login.Function>().Count();
            var f1 = new VO.Login.Function("function1");
            odb.Store(f1);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            var f1bis = query.Execute<VO.Login.Function>().GetFirst();
            odb.Delete(f1bis);
            odb.Store(new VO.Login.Function("last function"));
            odb.Close();
            odb = Open(baseName);
            var query1 = odb.Query<VO.Login.Function>();
            AssertEquals(size + 1, query1.Execute<VO.Login.Function>().Count);
            odb.Close();
        }

        /// <summary>
        ///   Bug detected by Olivier using the ODBMainExplorer, deleting many objects
        ///   without commiting,and commiting at the end
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test12()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            var f3 = new VO.Login.Function("function3");
            odb.Store(f1);
            odb.Store(f2);
            odb.Store(f3);
            var idf1 = odb.GetObjectId(f1);
            var idf2 = odb.GetObjectId(f2);
            var idf3 = odb.GetObjectId(f3);
            odb.Close();
            try
            {
                odb = Open(baseName);
                odb.DeleteObjectWithId(idf3);
                odb.DeleteObjectWithId(idf2);
                odb.Close();
                odb = Open(baseName);
                var query = odb.Query<VO.Login.Function>();
                var l = query.Execute<VO.Login.Function>();
                odb.Close();
                AssertEquals(1, l.Count);
            }
            catch (OdbRuntimeException)
            {
                DeleteBase(baseName);
                throw;
            }
        }

        /// <summary>
        ///   Bug detected by Olivier using the ODBMainExplorer, deleting many objects
        ///   without commiting,and commiting at the end
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test13()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            DeleteBase(baseName);
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            var f3 = new VO.Login.Function("function3");
            odb.Store(f1);
            odb.Store(f2);
            odb.Store(f3);
            var idf1 = odb.GetObjectId(f1);
            var idf2 = odb.GetObjectId(f2);
            var idf3 = odb.GetObjectId(f3);
            var storageEngine = ((global::NDatabase.Odb) odb).GetStorageEngine();

            var p1 = storageEngine.GetObjectReader().GetObjectPositionFromItsOid(idf1, true, false);
            var p2 = storageEngine.GetObjectReader().GetObjectPositionFromItsOid(idf2, true, false);
            var p3 = storageEngine.GetObjectReader().GetObjectPositionFromItsOid(idf3, true, false);
            odb.Close();
            try
            {
                odb = Open(baseName);
                f1 = (VO.Login.Function) odb.GetObjectFromId(idf1);
                f2 = (VO.Login.Function) odb.GetObjectFromId(idf2);
                f3 = (VO.Login.Function) odb.GetObjectFromId(idf3);
                odb.Delete(f3);
                odb.Delete(f2);
                odb.Close();
                odb = Open(baseName);
                var query = odb.Query<VO.Login.Function>();
                var l = query.Execute<VO.Login.Function>();
                odb.Close();
                AssertEquals(1, l.Count);
            }
            catch (OdbRuntimeException)
            {
                DeleteBase(baseName);
                throw;
            }
            DeleteBase(baseName);
        }

        /// <summary>
        ///   creates 5 objects,commit.
        /// </summary>
        /// <remarks>
        ///   creates 5 objects,commit. Then create 2 new objects and delete 4 existing
        ///   objects without committing,and committing at the end
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test14()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            var f3 = new VO.Login.Function("function3");
            var f4 = new VO.Login.Function("function4");
            var f5 = new VO.Login.Function("function5");
            odb.Store(f1);
            odb.Store(f2);
            odb.Store(f3);
            odb.Store(f4);
            odb.Store(f5);
            AssertEquals(5, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            try
            {
                odb = Open(baseName);
                var f6 = new VO.Login.Function("function6");
                var f7 = new VO.Login.Function("function7");
                odb.Store(f6);
                odb.Store(f7);
                AssertEquals(7, odb.Query<VO.Login.Function>().Count());
                var query = odb.Query<VO.Login.Function>();
                var objects = query.Execute<VO.Login.Function>();
                var i = 0;
                while (objects.HasNext() && i < 4)
                {
                    odb.Delete<VO.Login.Function>(objects.Next());
                    i++;
                }
                AssertEquals(3, odb.Query<VO.Login.Function>().Count());
                odb.Close();
                odb = Open(baseName);
                AssertEquals(3, odb.Query<VO.Login.Function>().Count());
                var query1 = odb.Query<VO.Login.Function>();
                objects = query1.Execute<VO.Login.Function>();
                // println(objects);
                AssertEquals((string) "function5", (string) (objects.Next()).GetName());
                AssertEquals((string) "function6", (string) (objects.Next()).GetName());
                AssertEquals((string) "function7", (string) (objects.Next()).GetName());
                odb.Close();
            }
            catch (OdbRuntimeException)
            {
                DeleteBase(baseName);
                throw;
            }
            DeleteBase(baseName);
        }

        /// <summary>
        ///   creates 2 objects.
        /// </summary>
        /// <remarks>
        ///   creates 2 objects. Delete them. And create 2 new objects
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test15()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            odb.Store(f1);
            odb.Store(f2);
            AssertEquals(2, odb.Query<VO.Login.Function>().Count());
            odb.Delete(f1);
            odb.Delete(f2);
            AssertEquals(0, odb.Query<VO.Login.Function>().Count());
            odb.Store(f1);
            odb.Store(f2);
            AssertEquals(2, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            AssertEquals(2, query.Execute<VO.Login.Function>().Count);
            odb.Close();
            DeleteBase(baseName);
        }

        /// <summary>
        ///   creates 2 objects.
        /// </summary>
        /// <remarks>
        ///   creates 2 objects. Delete them by oid. And create 2 new objects
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test15_by_oid()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            var oid1 = odb.Store(f1);
            var oid2 = odb.Store(f2);
            AssertEquals(2, odb.Query<VO.Login.Function>().Count());
            odb.DeleteObjectWithId(oid1);
            odb.DeleteObjectWithId(oid2);
            AssertEquals(0, odb.Query<VO.Login.Function>().Count());
            odb.Store(f1);
            odb.Store(f2);
            AssertEquals(2, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            AssertEquals(2, query.Execute<VO.Login.Function>().Count);
            odb.Close();
            DeleteBase(baseName);
        }

        /// <summary>
        ///   creates 2 objects.
        /// </summary>
        /// <remarks>
        ///   creates 2 objects. Delete them by oid. And create 2 new objects
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test15_by_oid_2()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            DeleteBase(baseName);
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            var oid1 = odb.Store(f1);
            var oid2 = odb.Store(f2);
            AssertEquals(2, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            odb = Open(baseName);
            odb.DeleteObjectWithId(oid1);
            odb.DeleteObjectWithId(oid2);
            AssertEquals(0, odb.Query<VO.Login.Function>().Count());
            odb.Store(f1);
            odb.Store(f2);
            AssertEquals(2, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            AssertEquals(2, query.Execute<VO.Login.Function>().Count);
            odb.Close();
            DeleteBase(baseName);
        }

        /// <summary>
        ///   creates x objects.
        /// </summary>
        /// <remarks>
        ///   creates x objects. Delete them. And create x new objects
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test16()
        {
            var baseName = GetBaseName();
            var size = 10000;
            IOdb odb = null;
            DeleteBase(baseName);
            odb = Open(baseName);
            var oids = new OID[size];
            for (var i = 0; i < size; i++)
                oids[i] = odb.Store(new VO.Login.Function("function" + i));
            AssertEquals(size, odb.Query<VO.Login.Function>().Count());
            for (var i = 0; i < size; i++)
                odb.DeleteObjectWithId(oids[i]);
            AssertEquals(0, odb.Query<VO.Login.Function>().Count());
            for (var i = 0; i < size; i++)
                oids[i] = odb.Store(new VO.Login.Function("function" + i));
            AssertEquals(size, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            AssertEquals(size, query.Execute<VO.Login.Function>().Count);
            odb.Close();
            DeleteBase(baseName);
        }

        /// <summary>
        ///   creates 3 objects.
        /// </summary>
        /// <remarks>
        ///   creates 3 objects. Delete the 2th. And create 3 new objects
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test17()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            DeleteBase(baseName);
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            var f3 = new VO.Login.Function("function2");
            odb.Store(f1);
            odb.Store(f2);
            odb.Store(f3);
            AssertEquals(3, odb.Query<VO.Login.Function>().Count());
            odb.Delete(f2);
            AssertEquals(2, odb.Query<VO.Login.Function>().Count());
            // odb.store(f1);
            odb.Store(f2);
            // odb.store(f3);
            AssertEquals(3, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            AssertEquals(3, query.Execute<VO.Login.Function>().Count);
            odb.Close();
            DeleteBase(baseName);
        }

        /// <summary>
        ///   creates 3 objects.
        /// </summary>
        /// <remarks>
        ///   creates 3 objects. commit. Creates 3 new . Delete the 2th commited. And
        ///   create 3 new objects
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test18()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            DeleteBase(baseName);
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            var f3 = new VO.Login.Function("function2");
            var oid1 = odb.Store(f1);
            var oid2 = odb.Store(f2);
            var oid3 = odb.Store(f3);
            AssertEquals(3, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            odb = Open(baseName);
            odb.DeleteObjectWithId(oid2);
            AssertEquals(2, odb.Query<VO.Login.Function>().Count());
            // odb.store(f1);
            odb.Store(new VO.Login.Function("f11"));
            odb.Store(new VO.Login.Function("f12"));
            odb.Store(new VO.Login.Function("f13"));
            // odb.store(f3);
            AssertEquals(5, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            AssertEquals(5, query.Execute<VO.Login.Function>().Count);
            odb.Close();
            DeleteBase(baseName);
        }

        /// <summary>
        ///   Stores an object, closes the base.
        /// </summary>
        /// <remarks>
        ///   Stores an object, closes the base. Loads the object, gets its oid and
        ///   delete by oid.
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test19()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            odb.Store(f1);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            var objects = query.Execute<VO.Login.Function>();
            AssertEquals(1, objects.Count);
            var f2 = objects.GetFirst();
            var oid = odb.GetObjectId(f2);
            odb.DeleteObjectWithId(oid);
            var query1 = odb.Query<VO.Login.Function>();
            AssertEquals(0, query1.Execute<VO.Login.Function>().Count);
            odb.Close();
            odb = Open(baseName);
            var query2 = odb.Query<VO.Login.Function>();
            objects = query2.Execute<VO.Login.Function>();
            AssertEquals(0, objects.Count);
        }

        [Test]
        public virtual void Test2()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);

            long nbFunctions;
            using (var odb = Open(baseName))
            {
                nbFunctions = odb.Query<VO.Login.Function>().Count();
                decimal nbProfiles = odb.Query<Profile>().Count();

                var function1 = new VO.Login.Function("function1");
                var function2 = new VO.Login.Function("function2");
                var function3 = new VO.Login.Function("function3");
                var functions = new List<VO.Login.Function>();

                functions.Add(function1);
                functions.Add(function2);
                functions.Add(function3);

                var profile1 = new Profile("profile1", functions);
                var profile2 = new Profile("profile2", function1);

                odb.Store(profile1);
                odb.Store(profile2);
            }

            using (var odb = Open(baseName))
            {
                var query1 = odb.Query<VO.Login.Function>();
                var lfunctions = query1.Execute<VO.Login.Function>(true);
                AssertEquals(nbFunctions + 3, lfunctions.Count);

                var query = odb.Query<VO.Login.Function>();
                query.Descend("name").Constrain("function2").Equal();
                var l = query.Execute<VO.Login.Function>();
                var function = l.GetFirst();

                odb.Delete(function);
            }

            using (var odb = Open(baseName))
            {
                AssertEquals(nbFunctions + 2, odb.Query<VO.Login.Function>().Count());

                var query3 = odb.Query<VO.Login.Function>();
                var l2 = query3.Execute<VO.Login.Function>(true);

                // check Profile 1
                var query2 = odb.Query<Profile>();
                query2.Descend("name").Constrain("profile1").Equal();

                var lprofile = query2.Execute<Profile>();
                var p1 = lprofile.GetFirst();
                AssertEquals(3, p1.GetFunctions().Count);
                Assert.That(p1.GetFunctions(), Contains.Item(null));
            }
            
        }

        /// <summary>
        ///   Stores on object and close database then Stores another object, commits
        ///   without closing.
        /// </summary>
        /// <remarks>
        ///   Stores on object and close database then Stores another object, commits
        ///   without closing. Loads the object, gets its oid and delete by oid. In the
        ///   case the commit has no write actions. And there was a bug : when there is
        ///   no write actions, the commit process is much more simple! but in this the
        ///   cache was not calling the transaction.clear and this was a reason for
        ///   some connected/unconnected zone problem! (step14 of the turotial.)
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        [Ignore("it contains commiting bug, it is well know and will be fixed in the future")]
        public virtual void Test20()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var f0 = new VO.Login.Function("1function0");
            odb.Store(f0);
            odb.Close();
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            odb.Store(f1);
            odb.Commit();
            var query = odb.Query<VO.Login.Function>();
            query.Descend("name").Constrain("func%").Like();
            var objects = query.Execute<VO.Login.Function>();
            AssertEquals(1, objects.Count);
            var f2 = objects.GetFirst();
            var oid = odb.GetObjectId(f2);
            odb.DeleteObjectWithId(oid);
            var query1 = odb.Query<VO.Login.Function>();
            AssertEquals(1, query1.Execute<VO.Login.Function>().Count);
            odb.Close();
            odb = Open(baseName);
            var query2 = odb.Query<VO.Login.Function>();
            objects = query2.Execute<VO.Login.Function>();
            AssertEquals(1, objects.Count);
        }

        /// <summary>
        ///   Bug when deleting the first object of unconnected zone when commited zone
        ///   already have at least one object.
        /// </summary>
        /// <remarks>
        ///   Bug when deleting the first object of unconnected zone when commited zone
        ///   already have at least one object.
        ///   Detected running the polePosiiton Bahrain circuit.
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test21()
        {
            IOdb odb = null;
            var baseName = GetBaseName();
            odb = Open(baseName);
            var f0 = new VO.Login.Function("function0");
            odb.Store(f0);
            odb.Close();
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            odb.Store(f1);
            var f2 = new VO.Login.Function("function2");
            odb.Store(f2);
            odb.Delete(f1);
            odb.Close();
            odb = Open(baseName);
            var objects = odb.Query<VO.Login.Function>().Execute<VO.Login.Function>();
            AssertEquals(2, objects.Count);
            odb.Close();
        }

        [Test]
        public virtual void Test22Last_toCheckDuration()
        {
            var duration = OdbTime.GetCurrentTimeInMs() - start;
            long d = 2200;

            Println("duration=" + duration);
            if (testPerformance && duration > d)
                Fail("Duration is higher than " + d + " : " + duration);
        }

        [Test]
        public virtual void Test3()
        {
            var baseName = GetBaseName();
            var baseName2 = "2" + baseName;
            var odb = Open(baseName);
            var size = 1000;
            for (var i = 0; i < size; i++)
                odb.Store(new VO.Login.Function("function " + i));
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            var objects = query.Execute<VO.Login.Function>(false);
            var j = 0;
            while (objects.HasNext() && j < objects.Count - 1)
            {
                odb.Delete(objects.Next());
                j++;
            }
            odb.Close();
            odb = Open(baseName);
            AssertEquals(1, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            odb = Open(baseName);
            odb.DefragmentTo(baseName2);
            odb.Close();
            odb = Open(baseName2);
            AssertEquals(1, odb.Query<VO.Login.Function>().Count());
            odb.Close();
            DeleteBase(baseName);
            DeleteBase(baseName2);
        }

        [Test]
        public virtual void Test30()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            var oid1 = odb.Store(new VO.Login.Function("function 1"));
            var oid2 = odb.Store(new VO.Login.Function("function 2"));
            odb.Close();
            Println(oid1);
            Println(oid2);
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            odb.Delete(query.Execute<VO.Login.Function>().GetFirst());
            odb.Close();
            odb = Open(baseName);
            var query1 = odb.Query<VO.Login.Function>();
            var f = query1.Execute<VO.Login.Function>().GetFirst();
            odb.Close();
            DeleteBase(baseName);
            AssertEquals("function 2", f.GetName());
        }

        [Test]
        public virtual void Test4()
        {
            var baseName = GetBaseName();
            var n = 100;
            var odb = Open(baseName);
            var size = odb.Query<VO.Login.Function>().Count();
            for (var i = 0; i < n; i++)
            {
                var login = new VO.Login.Function("login - " + (i + 1));
                odb.Store(login);
                AssertEquals(size + i + 1, odb.Query<VO.Login.Function>().Count());
                Console.WriteLine(i);
            }
            // IStorageEngine engine = Dummy.getEngine(odb);
            odb.Commit();
            var query = odb.Query<VO.Login.Function>();
            var l = query.Execute<VO.Login.Function>(true);
            var j = 0;
            while (l.HasNext())
            {
                Console.WriteLine(" i=" + j);
                var f = l.Next();
                odb.Delete(f);
                var query1 = odb.Query<VO.Login.Function>();
                var l2 = query1.Execute<VO.Login.Function>();
                AssertEquals(size + n - (j + 1), l2.Count);
                j++;
            }
            odb.Commit();
            odb.Close();
            DeleteBase(baseName);
        }

        [Test]
        public virtual void Test5()
        {
            IOdb odb = null;
            var baseName = GetBaseName();
            odb = Open(baseName);
            var f = new VO.Login.Function("function1");
            odb.Store(f);
            var id = odb.GetObjectId(f);
            try
            {
                odb.Delete(f);
                var id2 = odb.GetObjectId(f);
                Fail("The object has been deleted, the id should have been marked as deleted");
            }
            catch (OdbRuntimeException)
            {
                odb.Close();
                DeleteBase(baseName);
            }
        }

        [Test]
        public virtual void Test5_byOid()
        {
            IOdb odb = null;
            var baseName = GetBaseName();
            odb = Open(baseName);
            var f = new VO.Login.Function("function1");
            odb.Store(f);
            var oid = odb.GetObjectId(f);
            try
            {
                odb.DeleteObjectWithId(oid);
                var id2 = odb.GetObjectId(f);
                Fail("The object has been deleted, the id should have been marked as deleted");
            }
            catch (OdbRuntimeException)
            {
                odb.Close();
                DeleteBase(baseName);
            }
        }

        [Test]
        public virtual void Test6()
        {
            IOdb odb = null;
            var baseName = GetBaseName();
            odb = Open(baseName);
            var f = new VO.Login.Function("function1");
            odb.Store(f);
            var id = odb.GetObjectId(f);
            odb.Commit();
            try
            {
                odb.Delete(f);
                odb.GetObjectFromId(id);
                Fail("The object has been deleted, the id should have been marked as deleted");
            }
            catch (OdbRuntimeException)
            {
                odb.Close();
                DeleteBase("t-delete1.ndb");
            }
        }

        [Test]
        public virtual void Test7()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            var f3 = new VO.Login.Function("function3");
            odb.Store(f1);
            odb.Store(f2);
            odb.Store(f3);
            var id = odb.GetObjectId(f3);
            odb.Close();
            try
            {
                odb = Open(baseName);
                var f3bis = (VO.Login.Function) odb.GetObjectFromId(id);
                odb.Delete(f3bis);
                odb.Close();
                odb = Open(baseName);
                var query = odb.Query<VO.Login.Function>();
                var l = query.Execute<VO.Login.Function>();
                odb.Close();
                AssertEquals(2, l.Count);
            }
            catch (OdbRuntimeException)
            {
                odb.Close();
                DeleteBase(baseName);
            }
        }

        /// <summary>
        ///   Test : delete the last object and insert a new one in the same
        ///   transaction - detected by Alessandra
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test8()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            var f3 = new VO.Login.Function("function3");
            odb.Store(f1);
            odb.Store(f2);
            odb.Store(f3);
            var id = odb.GetObjectId(f3);
            odb.Close();
            odb = Open(baseName);
            var f3bis = (VO.Login.Function) odb.GetObjectFromId(id);
            odb.Delete(f3bis);
            odb.Store(new VO.Login.Function("last function"));
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            var l = query.Execute<VO.Login.Function>();
            odb.Close();
            AssertEquals(3, l.Count);
        }

        /// <summary>
        ///   Test : delete the last object and insert a new one in another transaction
        ///   - detected by Alessandra
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test9()
        {
            var baseName = GetBaseName();
            IOdb odb = null;
            odb = Open(baseName);
            var f1 = new VO.Login.Function("function1");
            var f2 = new VO.Login.Function("function2");
            var f3 = new VO.Login.Function("function3");
            odb.Store(f1);
            odb.Store(f2);
            odb.Store(f3);
            var id = odb.GetObjectId(f3);
            odb.Close();
            odb = Open(baseName);
            var f3bis = (VO.Login.Function) odb.GetObjectFromId(id);
            odb.Delete(f3bis);
            odb.Close();
            odb = Open(baseName);
            odb.Store(new VO.Login.Function("last function"));
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VO.Login.Function>();
            var l = query.Execute<VO.Login.Function>();
            odb.Close();
            AssertEquals(3, l.Count);
        }
    }
}
