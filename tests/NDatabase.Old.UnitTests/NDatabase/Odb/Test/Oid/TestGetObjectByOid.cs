using System;
using System.Collections.Generic;
using NDatabase;
using NDatabase.Api;
using NDatabase.Oid;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Oid
{
    [TestFixture]
    public class TestGetObjectByOid : ODBTest
    {
        private object GetInstance(int i)
        {
            var login = new VO.Login.Function("login " + i);
            var logout = new VO.Login.Function("logout" + i);
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile = new Profile("operator" + i, list);
            var user = new User("kiko" + i, "user@ndatabase.net" + i, profile);
            return user;
        }

        /// <summary>
        ///   Getting object by id after re opening database
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test1()
        {
            DeleteBase("getid.ndb");
            var function1 = new VO.Login.Function("f1");
            var function2 = new VO.Login.Function("f2");
            var odb = Open("getid.ndb");
            odb.Store(function1);
            odb.Store(function2);
            var id1 = odb.GetObjectId(function1);
            var id2 = odb.GetObjectId(function2);
            odb.Close();
            odb = Open("getid.ndb");
            var function1bis = (VO.Login.Function) odb.GetObjectFromId(id1);
            AssertEquals(function1.GetName(), function1bis.GetName());
            var function2bis = (VO.Login.Function) odb.GetObjectFromId(id2);
            function2bis.SetName("function 2");
            odb.Store(function2bis);
            var id2bis = odb.GetObjectId(function2bis);
            odb.Close();
            odb = Open("getid.ndb");
            var function2ter = (VO.Login.Function) odb.GetObjectFromId(id2);
            AssertEquals("function 2", function2ter.GetName());
            odb.Close();
            DeleteBase("getid.ndb");
        }

        /// <summary>
        ///   Getting object by id during the same transaction
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test2()
        {
            DeleteBase("getid.ndb");
            var function1 = new VO.Login.Function("f1");
            var function2 = new VO.Login.Function("f2");
            var odb = Open("getid.ndb");
            odb.Store(function1);
            odb.Store(function2);
            var id1 = odb.GetObjectId(function1);
            var id2 = odb.GetObjectId(function2);
            var function1bis = (VO.Login.Function) odb.GetObjectFromId(id1);
            odb.Close();
            AssertEquals(function1.GetName(), function1bis.GetName());
            DeleteBase("getid.ndb");
        }

        /// <summary>
        ///   Getting object by id after an update in the same transaction
        ///   than the insert
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test3()
        {
            DeleteBase("getid.ndb");
            var function1 = new VO.Login.Function("f1");
            var function2 = new VO.Login.Function("f2");
            var odb = Open("getid.ndb");
            odb.Store(function1);
            odb.Store(function2);
            function1.SetName("f2");
            odb.Store(function1);
            var id1 = odb.GetObjectId(function1);
            var id2 = odb.GetObjectId(function2);
            var function1bis = (VO.Login.Function) odb.GetObjectFromId(id1);
            odb.Close();
            AssertEquals(function1.GetName(), function1bis.GetName());
            DeleteBase("getid.ndb");
        }

        /// <summary>
        ///   Getting object by id after an update in the same
        ///   transaction than the insert
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test4()
        {
            DeleteBase("getid.ndb");
            var function1 = new VO.Login.Function("f1");
            var function2 = new VO.Login.Function("f2");
            var odb = Open("getid.ndb");
            odb.Store(function1);
            odb.Store(function2);
            function1.SetName("function login and logout");
            odb.Store(function1);
            var id1 = odb.GetObjectId(function1);
            var id2 = odb.GetObjectId(function2);
            var function1bis = (VO.Login.Function) odb.GetObjectFromId(id1);
            odb.Close();
            AssertEquals(function1.GetName(), function1bis.GetName());
            DeleteBase("getid.ndb");
        }

        /// <summary>
        ///   Test performance of retrieving 2 objects by oid
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test5()
        {
            DeleteBase("getid.ndb");
            var function1 = new VO.Login.Function("f1");
            var function2 = new VO.Login.Function("f2");
            var odb = Open("getid.ndb");
            odb.Store(function1);
            odb.Store(function2);
            var id1 = odb.GetObjectId(function1);
            var id2 = odb.GetObjectId(function2);
            odb.Close();
            odb = Open("getid.ndb");
            var t1 = OdbTime.GetCurrentTimeInMs();
            var function1bis = (VO.Login.Function) odb.GetObjectFromId(id1);
            var function2bis = (VO.Login.Function) odb.GetObjectFromId(id2);
            var t2 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            DeleteBase("getid.ndb");
            AssertEquals(function1.GetName(), function1bis.GetName());
            AssertEquals(function2.GetName(), function2bis.GetName());
            var time = t2 - t1;
            Println(time);
            long acceptableTime = 1;
            if (time > acceptableTime)
            {
                // ms
                Fail("Getting two objects by oid lasted more than " + acceptableTime + "ms : " + time);
            }
        }

        /// <summary>
        ///   Test performance of retrieving many simple objects by oid
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test6()
        {
            DeleteBase("getid.ndb");
            var size = 20001;
            var odb = Open("getid.ndb");
            var oids = new OID[size];
            for (var i = 0; i < size; i++)
                oids[i] = odb.Store(new VO.Login.Function("function " + i));
            odb.Close();
            odb = Open("getid.ndb");
            var t1 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var f = (VO.Login.Function) odb.GetObjectFromId(oids[i]);
                AssertEquals("function " + i, f.GetName());
                if (i % 3000 == 0)
                    Println(i + "/" + size);
            }
            var t2 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            DeleteBase("getid.ndb");
            var time = t2 - t1;
            var timeForEachGet = time / (double) size;
            var acceptableTime = 0.022;
            // 0.04294785260736963
            Println("time for each get = " + time + "/" + size + " = " + timeForEachGet);
            if (testPerformance && timeForEachGet > acceptableTime)
            {
                // ms
                Fail("Getting " + size + " simple objects by oid lasted more than " + acceptableTime + "ms : " +
                     timeForEachGet);
            }
        }

        /// <summary>
        ///   Test performance of retrieving many complex objects by oid
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void Test7()
        {
            DeleteBase("getid.ndb");
            var size = 1001;
            var odb = Open("getid.ndb");
            var oids = new OID[size];
            for (var i = 0; i < size; i++)
                oids[i] = odb.Store(GetInstance(i));
            odb.Close();
            odb = Open("getid.ndb");
            var t1 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < size; i++)
            {
                var u = (User) odb.GetObjectFromId(oids[i]);
                AssertEquals("kiko" + i, u.GetName());
            }
            var t2 = OdbTime.GetCurrentTimeInMs();
            odb.Close();
            DeleteBase("getid.ndb");
            var time = t2 - t1;
            var timeForEachGet = time / (double) size;
            var acceptableTime = 0.086;
            // 0.1561843815618438
            Println("time for each get = " + timeForEachGet + " - Total time for " + size + " objects = " + time);
            if (testPerformance && timeForEachGet > acceptableTime)
            {
                // ms
                Println("time for each get = " + timeForEachGet + " - Total time for " + size + " objects = " + time);
                Fail("Getting " + size + " complex objects by oid lasted more than " + acceptableTime + "ms : " +
                     timeForEachGet);
            }
        }

        /// <summary>
        ///   Trying to get an object with OID that does not exist
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestGetOIDThatDoesNotExist()
        {
            DeleteBase("getid.ndb");
            var function2 = new VO.Login.Function("f2");
            var odb = Open("getid.ndb");
            var oid = OIDFactory.BuildObjectOID(49);
            try
            {
                var o = odb.GetObjectFromId(oid);
            }
            catch (Exception e)
            {
                odb.Close();
                AssertFalse(e.Message.IndexOf(" does not exist in the database") == -1);
            }
        }
    }
}
