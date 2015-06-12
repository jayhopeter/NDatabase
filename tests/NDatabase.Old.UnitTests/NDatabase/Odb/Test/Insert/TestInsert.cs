using System;
using System.Collections.Generic;
using NDatabase.Api;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Insert
{
    [TestFixture]
    public class TestInsert : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            DeleteBase("t1.ndb");
            // LogUtil.allOn(true);
            var odb = Open("t1.ndb");
            // LogUtil.objectWriterOn(true);
            var login = new VO.Login.Function("login");
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            var profile1 = new Profile("operator 1", list);
            var user = new User("olivier smadja", "user@ndatabase.net", profile1);
            odb.Store(user);
            odb.Close();
            odb = Open("t1.ndb");
            var query = odb.Query<User>();
            var users = query.Execute<User>(true);
            // assertEquals(nbUsers+2,users.size());
            var user2 = users.GetFirst();
            odb.Close();
            AssertEquals(user.ToString(), user2.ToString());
            DeleteBase("t1.ndb");
        }

        [Test]
        public virtual void Test6()
        {
            DeleteBase("t1u.ndb");
            var odb = Open("t1u.ndb");
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            odb.Store(login);
            odb.Store(logout);
            odb.Close();
            odb = Open("t1u.ndb");
            var login2 = new VO.Login.Function("login2");
            var logout2 = new VO.Login.Function("logout2");
            odb.Store(login2);
            odb.Store(logout2);
            // select without committing
            var query1 = odb.Query<VO.Login.Function>();
            var l = query1.Execute<VO.Login.Function>(true);
            AssertEquals(4, l.Count);
            // println(l);
            odb.Close();
            odb = Open("t1u.ndb");
            var query = odb.Query<VO.Login.Function>();
            l = query.Execute<VO.Login.Function>(true);
            AssertEquals(4, l.Count);
            // println(l);
            odb.Close();
        }

        [Test]
        public virtual void Test7()
        {
            DeleteBase("t1u.ndb");
            var odb = Open("t1u.ndb");
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            odb.Store(login);
            odb.Store(logout);
            odb.Commit();
            var input = new VO.Login.Function("input");
            odb.Store(input);
            odb.Close();
            odb = Open("t1u.ndb");
            var query = odb.Query<VO.Login.Function>();
            var l = query.Execute<VO.Login.Function>(true);
            AssertEquals(3, l.Count);
            // println(l);
            odb.Close();
        }

        /// <summary>
        ///   Test with java util Date and java sql Date
        /// </summary>
        [Test]
        public virtual void Test8()
        {
            var baseName = GetBaseName();
            Println(baseName);
            IOdb odb = null;
            var utilDate = new DateTime();
            var sqlDate = new DateTime(utilDate.Millisecond + 10000);
            var timestamp = new DateTime(utilDate.Millisecond + 20000);
            try
            {
                odb = Open(baseName);
                var o = new ObjectWithDates("object1", utilDate, sqlDate, timestamp);
                odb.Store(o);
                odb.Close();
                odb = Open(baseName);
                var query = odb.Query<ObjectWithDates>();
                var dates = query.Execute<ObjectWithDates>();
                var o2 = dates.GetFirst();
                Println(o2.GetName());
                Println(o2.GetJavaUtilDate());
                Println(o2.GetJavaSqlDte());
                Println(o2.GetTimestamp());
                AssertEquals((string) "object1", (string) o2.GetName());
                AssertEquals(utilDate, o2.GetJavaUtilDate());
                AssertEquals(sqlDate, o2.GetJavaSqlDte());
                AssertEquals(timestamp, o2.GetTimestamp());
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
        }

        [Test]
        public virtual void TestCompositeCollection1()
        {
            DeleteBase("t31.ndb");
            var odb = Open("t31.ndb");
            var login = new VO.Login.Function("login");
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            var profile1 = new Profile("operator 1", list);
            var user = new User("olivier smadja", "user@ndatabase.net", profile1);
            odb.Store(user);
            odb.Close();
            odb = Open("t31.ndb");
            var query = odb.Query<User>();
            var users = query.Execute<User>(true);
            odb.Close();
            // assertEquals(nbUsers+2,users.size());
            var user2 = users.GetFirst();
            AssertEquals(user.ToString(), user2.ToString());
            DeleteBase("t31.ndb");
        }

        [Test]
        public virtual void TestCompositeCollection2()
        {
            DeleteBase("t3.ndb");
            // LogUtil.objectWriterOn(true);
            var odb = Open("t3.ndb");
            var query = odb.Query<User>();
            var nbUsers = query.Execute<User>(true).Count;
            var query1 = odb.Query<Profile>();
            var nbProfiles = query1.Execute<Profile>(true).Count;
            var query2 = odb.Query<VO.Login.Function>();
            var nbFunctions = query2.Execute<VO.Login.Function>(true).Count;
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile1 = new Profile("operator 1", list);
            var profile2 = new Profile("operator 2", list);
            var user = new User("olivier smadja", "user@ndatabase.net", profile1);
            var userB = new User("A√°sa Galv√£o Smadja", "aisa@ndatabase.net", profile2);
            odb.Store(user);
            odb.Store(userB);
            odb.Close();
            odb = Open("t3.ndb");
            var query3 = odb.Query<User>();
            var users = query3.Execute<User>(true);
            var query4 = odb.Query<Profile>();
            var profiles = query4.Execute<Profile>(true);
            var query5 = odb.Query<VO.Login.Function>();
            var functions = query5.Execute<VO.Login.Function>(true);
            // assertEquals(nbUsers+2,users.size());
            var user2 = users.GetFirst();
            AssertEquals(user.ToString(), user2.ToString());
            AssertEquals(nbProfiles + 2, profiles.Count);
            AssertEquals(nbFunctions + 2, functions.Count);
            odb.Close();
            DeleteBase("t3.ndb");
        }

        [Test]
        public virtual void TestCompositeCollection2DifferentObjects()
        {
            DeleteBase("ti1.ndb");
            var odb = Open("ti1.ndb");
            var query5 = odb.Query<User>();
            var nbUsers = query5.Execute<User>(true).Count;
            var query = odb.Query<Profile>();
            var nbProfiles = query.Execute<Profile>(true).Count;
            var query1 = odb.Query<VO.Login.Function>();
            var nbFunctions = query1.Execute<VO.Login.Function>(true).Count;
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            var disconnect = new VO.Login.Function("disconnect");
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            IList<VO.Login.Function> list2 = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile1 = new Profile("operator 1", list);
            var profile2 = new Profile("operator 2", list2);
            var user = new User("olivier smadja", "user@ndatabase.net", profile1);
            var userB = new User("A√°sa Galv√£o Smadja", "aisa@ndatabase.net", profile2);
            odb.Store(user);
            odb.Store(userB);
            odb.Commit();
            var query2 = odb.Query<VO.Login.Function>();
            var functions = query2.Execute<VO.Login.Function>(true);
            var query3 = odb.Query<Profile>();
            var profiles = query3.Execute<Profile>(true);
            var query4 = odb.Query<User>();
            var users = query4.Execute<User>(true);
            odb.Close();
            // assertEquals(nbUsers+2,users.size());
            var user2 = users.GetFirst();
            AssertEquals(user.ToString(), user2.ToString());
            AssertEquals(nbProfiles + 2, profiles.Count);
            AssertEquals(nbFunctions + 2, functions.Count);
            DeleteBase("ti1.ndb");
        }

        [Test]
        public virtual void TestCompositeCollection3()
        {
            DeleteBase("t4.ndb");
            var odb = Open("t4.ndb");
            // Configuration.addLogId("ObjectWriter");
            // Configuration.addLogId("ObjectReader");
            // Configuration.addLogId("FileSystemInterface");
            var query = odb.Query<User>();
            var nbUsers = query.Execute<User>(true).Count;
            var query1 = odb.Query<Profile>();
            var nbProfiles = query1.Execute<Profile>(true).Count;
            var query2 = odb.Query<VO.Login.Function>();
            var nbFunctions = query2.Execute<VO.Login.Function>(true).Count;
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile1 = new Profile("operator 1", list);
            var user = new User("olivier smadja", "user@ndatabase.net", profile1);
            var userB = new User("A√≠sa Galv√£o Smadja", "aisa@ndatabase.net", profile1);
            odb.Store(user);
            odb.Store(userB);
            odb.Close();
            odb = Open("t4.ndb");
            var query3 = odb.Query<User>();
            var users = query3.Execute<User>(true);
            var query4 = odb.Query<Profile>();
            var profiles = query4.Execute<Profile>(true);
            var query5 = odb.Query<VO.Login.Function>();
            var functions = query5.Execute<VO.Login.Function>(true);
            // assertEquals(nbUsers+2,users.size());
            var user2 = users.GetFirst();
            AssertEquals(user.ToString(), user2.ToString());
            AssertEquals(nbProfiles + 1, profiles.Count);
            AssertEquals(nbFunctions + 2, functions.Count);
            odb.Close();
            DeleteBase("t4.ndb");
        }

        [Test]
        public virtual void TestCompositeCollection4()
        {
            DeleteBase("t5.ndb");
            var odb = Open("t5.ndb");
            var query = odb.Query<User>();
            var nbUsers = query.Execute<User>(true).Count;
            var query1 = odb.Query<Profile>();
            var nbProfiles = query1.Execute<Profile>(true).Count;
            var query2 = odb.Query<VO.Login.Function>();
            var nbFunctions = query2.Execute<VO.Login.Function>(true).Count;
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile1 = new Profile("operator 1", list);
            var user = new User("olivier smadja", "user@ndatabase.net", profile1);
            var userB = new User("A√≠sa Galv√£o Smadja", "aisa@ndatabase.net", profile1);
            odb.Store(user);
            odb.Store(userB);
            odb.Commit();
            var query3 = odb.Query<User>();
            var users = query3.Execute<User>(true);
            var query4 = odb.Query<Profile>();
            var profiles = query4.Execute<Profile>(true);
            var query5 = odb.Query<VO.Login.Function>();
            var functions = query5.Execute<VO.Login.Function>(true);
            odb.Close();
            // assertEquals(nbUsers+2,users.size());
            var user2 = users.GetFirst();
            AssertEquals(user.ToString(), user2.ToString());
            AssertEquals(nbProfiles + 1, profiles.Count);
            AssertEquals(nbFunctions + 2, functions.Count);
        }

        // deleteBase("t5.ndb");

        [Test]
        public virtual void TestDatePersistence()
        {
            IOdb odb = null;
            DeleteBase("date.ndb");
            try
            {
                odb = Open("date.ndb");
                var tc1 = new TestClass();
                tc1.SetDate1(new DateTime());
                long t1 = tc1.GetDate1().Millisecond;
                odb.Store(tc1);
                odb.Close();
                odb = Open("date.ndb");
                var query = odb.Query<TestClass>();
                var l = query.Execute<TestClass>();
                AssertEquals(1, l.Count);
                var tc2 = l.GetFirst();
                AssertEquals(t1, (long) tc2.GetDate1().Millisecond);
                AssertEquals(tc1.GetDate1(), tc2.GetDate1());
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
            DeleteBase("date.ndb");
        }

        [Test]
        public virtual void TestSimple()
        {
            DeleteBase("t2.ndb");
            var odb = Open("t2.ndb");
            var query = odb.Query<VO.Login.Function>();
            var nbFunctions = query.Execute<VO.Login.Function>(true).Count;
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            odb.Store(login);
            odb.Store(logout);
            odb.Close();
            odb = Open("t2.ndb");
            var query1 = odb.Query<VO.Login.Function>();
            var functions = query1.Execute<VO.Login.Function>(true);
            var f1 = functions.GetFirst();
            f1.SetName("login1");
            odb.Store(f1);
            odb.Close();
            odb = Open("t2.ndb");
            var query2 = odb.Query<VO.Login.Function>();
            functions = query2.Execute<VO.Login.Function>(true);
            odb.Close();
            AssertEquals(2, functions.Count);
            AssertEquals("login1", (functions.GetFirst()).GetName());
            DeleteBase("t2.ndb");
        }

        [Test]
        public virtual void TestStringPersistence()
        {
            IOdb odb = null;
            DeleteBase("date.ndb");
            try
            {
                odb = Open("date.ndb");
                var tc1 = new TestClass();
                tc1.SetString1(string.Empty);
                odb.Store(tc1);
                odb.Close();
                odb = Open("date.ndb");
                var query = odb.Query<TestClass>();
                var l = query.Execute<TestClass>();
                AssertEquals(1, l.Count);
                var tc2 = l.GetFirst();
                AssertEquals(string.Empty, (string) tc2.GetString1());
                AssertEquals(0m, tc2.GetBigDecimal1());
                AssertEquals(0d, tc2.GetDouble1());
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
        }
    }
}
