using System;
using System.Collections.Generic;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Update
{
    [TestFixture]
    public class TestSimpleUpdateObject : ODBTest
    {
        /// <exception cref="System.Exception"></exception>
        public override void TearDown()
        {
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1()
        {
            DeleteBase("t1u.ndb");
            var odb = Open("t1u.ndb");
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            odb.Store(login);
            Println("--------");
            odb.Store(login);
            odb.Store(logout);
            // odb.commit();
            odb.Close();
            odb = Open("t1u.ndb");
            var query = odb.Query<VO.Login.Function>();
            var l = query.Execute<VO.Login.Function>(true);
            var f2 = l.GetFirst();
            f2.SetName("login function");
            odb.Store(f2);
            odb.Close();
            var odb2 = Open("t1u.ndb");
            var query1 = odb2.Query<VO.Login.Function>();
            var f = query1.Execute<VO.Login.Function>().GetFirst();
            AssertEquals("login function", f.GetName());
            odb2.Close();
            DeleteBase("t1u.ndb");
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test2()
        {
            DeleteBase("t2.ndb");
            var odb = Open("t2.ndb");
            var query8 = odb.Query<User>();
            var nbUsers = query8.Execute<User>().Count;
            var query = odb.Query<Profile>();
            var nbProfiles = query.Execute<Profile>(true).Count;
            var query1 = odb.Query<VO.Login.Function>();
            var nbFunctions = query1.Execute<VO.Login.Function>(true).Count;
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            IList<VO.Login.Function> list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile = new Profile("operator", list);
            var olivier = new User("olivier smadja", "user@ndatabase.net", profile);
            var aisa = new User("A√≠sa Galv√£o Smadja", "aisa@neodRMuatis.com", profile);
            odb.Store(olivier);
            odb.Store(aisa);
            odb.Commit();
            var query2 = odb.Query<User>();
            var users = query2.Execute<User>(true);
            var query3 = odb.Query<Profile>();
            var profiles = query3.Execute<Profile>(true);
            var query4 = odb.Query<VO.Login.Function>();
            var functions = query4.Execute<VO.Login.Function>(true);
            odb.Close();
            // println("Users:"+users);
            Println("Profiles:" + profiles);
            Println("Functions:" + functions);
            odb = Open("t2.ndb");
            var query5 = odb.Query<User>();
            var l = query5.Execute<User>(true);
            odb.Close();
            AssertEquals(nbUsers + 2, users.Count);
            var user2 = users.GetFirst();
            AssertEquals(olivier.ToString(), user2.ToString());
            AssertEquals(nbProfiles + 1, profiles.Count);
            AssertEquals(nbFunctions + 2, functions.Count);
            var odb2 = Open("t2.ndb");
            var query6 = odb2.Query<VO.Login.Function>();
            var l2 = query6.Execute<VO.Login.Function>(true);
            var function = l2.GetFirst();
            function.SetName("login function");
            odb2.Store(function);
            odb2.Close();
            var odb3 = Open("t2.ndb");
            var query7 = odb3.Query<User>();
            var l3 = query7.Execute<User>(true);
            var i = 0;
            while (l3.HasNext() && i < Math.Min(2, l3.Count))
            {
                var user = l3.Next();
                AssertEquals("login function", string.Empty + user.GetProfile().GetFunctions()[0]);
                i++;
            }
            odb3.Close();
            DeleteBase("t2.ndb");
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test3()
        {
            DeleteBase("t1u2.ndb");
            var odb = Open("t1u2.ndb");
            var login = new VO.Login.Function(null);
            odb.Store(login);
            odb.Close();
            odb = Open("t1u2.ndb");
            var query = odb.Query<VO.Login.Function>();
            query.Descend("name").Constrain(null).Equal();
            login = query.Execute<VO.Login.Function>().GetFirst();
            AssertTrue(login.GetName() == null);
            login.SetName("login");
            odb.Store(login);
            odb.Close();
            odb = Open("t1u2.ndb");
            var query1 = odb.Query<VO.Login.Function>();
            login = query1.Execute<VO.Login.Function>().GetFirst();
            AssertTrue(login.GetName().Equals("login"));
            odb.Close();
            DeleteBase("t1u2.ndb");
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test5()
        {
            DeleteBase("t5.ndb");
            var odb = Open("t5.ndb");
            var nbFunctions = odb.Query<VO.Login.Function>().Count();
            var nbProfiles = odb.Query<Profile>().Count();
            var nbUsers = odb.Query<User>().Count();
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            var list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile = new Profile("operator", list);
            var olivier = new User("olivier smadja", "user@ndatabase.net", profile);
            var aisa = new User("A√≠sa Galv√£o Smadja", "aisa@ndatabase.net", profile);
            odb.Store(olivier);
            odb.Store(profile);
            odb.Commit();
            odb.Close();
            odb = Open("t5.ndb");
            var query = odb.Query<User>();
            var users = query.Execute<User>(true);
            var query1 = odb.Query<Profile>();
            var profiles = query1.Execute<Profile>(true);
            var query2 = odb.Query<VO.Login.Function>();
            var functions = query2.Execute<VO.Login.Function>(true);
            odb.Close();
            AssertEquals(nbUsers + 1, users.Count);
            AssertEquals(nbProfiles + 1, profiles.Count);
            AssertEquals(nbFunctions + 2, functions.Count);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test6()
        {
            // LogUtil.objectWriterOn(true);
            DeleteBase("t6.ndb");
            var odb = Open("t6.ndb");
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            var list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile = new Profile("operator", list);
            var olivier = new User("olivier smadja", "user@ndatabase.net", profile);
            odb.Store(olivier);
            odb.Close();
            Println("----------");
            odb = Open("t6.ndb");
            var query = odb.Query<User>();
            var users = query.Execute<User>(true);
            var u1 = users.GetFirst();
            u1.GetProfile().SetName("operator 234567891011121314");
            odb.Store(u1);
            odb.Close();
            odb = Open("t6.ndb");
            var query1 = odb.Query<Profile>();
            var profiles = query1.Execute<Profile>(true);
            AssertEquals(1, profiles.Count);
            var p1 = profiles.GetFirst();
            AssertEquals(u1.GetProfile().GetName(), p1.GetName());
        }
    }
}
