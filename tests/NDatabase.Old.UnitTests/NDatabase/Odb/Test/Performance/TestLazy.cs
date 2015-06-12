using System.Collections.Generic;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Performance
{
    [TestFixture]
    public class TestLazy : ODBTest
    {
        public const int Size = 4000;

        public static readonly string Filename = "lazy.ndb";

        private object GetInstance()
        {
            var login = new VO.Login.Function("login");
            var logout = new VO.Login.Function("logout");
            var list = new List<VO.Login.Function>();
            list.Add(login);
            list.Add(logout);
            var profile = new Profile("operator", list);
            var user = new User("olivier smadja", "user@ndatabase.net", profile);
            return user;
        }

        /// <summary>
        ///   Test the timeof lazy get
        /// </summary>
        [Test]
        public virtual void Test1()
        {
            DeleteBase(Filename);
            // println("Start inserting " + SIZE + " objects");
            var startinsert = OdbTime.GetCurrentTimeInTicks();
            var odb = Open(Filename);
            for (var i = 0; i < Size; i++)
                odb.Store(GetInstance());
            odb.Close();
            var endinsert = OdbTime.GetCurrentTimeInTicks();
            // println("End inserting " + SIZE + " objects  - " +
            // (endinsert-startinsert) + " ms");
            // println("totalObjects = "+ odb.count(User.class));
            odb = Open(Filename);
            var start1 = OdbTime.GetCurrentTimeInTicks();
            var query = odb.Query<User>();
            var lazyList = query.Execute<User>(false);
            var end1 = OdbTime.GetCurrentTimeInTicks();
            var startget1 = OdbTime.GetCurrentTimeInTicks();
            while (lazyList.HasNext())
            {
                // t1 = OdbTime.getCurrentTimeInMs();
                lazyList.Next();
            }
            // t2 = OdbTime.getCurrentTimeInMs();
            // println(t2-t1);
            var endget1 = OdbTime.GetCurrentTimeInTicks();
            AssertEquals(odb.Query<User>().Count(), lazyList.Count);
            odb.Close();
            var t01 = end1 - start1;
            var tget1 = endget1 - startget1;
            // long t2 = end2-start2;
            // long tget2 = endget2-startget2;
            // println("t1(lazy)="+t1 + " - " +tget1+ "      t2(memory)="+t2 +" - "
            // + tget2);
            // println("t1(lazy)="+t1 + " - " +tget1);
            // assertTrue(t1<t2);
            // println(endinsert-startinsert);
            var c = 501 > tget1;
            Println("Time for " + Size + " lazy gets : " + (tget1));
            if (!c)
                Println("Time for " + Size + " lazy gets : " + (tget1));
            DeleteBase(Filename);
            if (testPerformance && !c)
                Fail("Time for " + Size + " lazy gets : " + (tget1));
        }
    }
}
