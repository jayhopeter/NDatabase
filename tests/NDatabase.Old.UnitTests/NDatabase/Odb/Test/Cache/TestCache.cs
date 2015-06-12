using System.Threading;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Cache
{
    [TestFixture]
    public class TestCache : ODBTest
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            Thread.Sleep(100);
            // Configuration.setUseModifiedClass(true);
            DeleteBase("cache.ndb");
            var odb = Open("cache.ndb");
            for (var i = 0; i < NbObjects; i++)
            {
                odb.Store(new VO.Login.Function("function " + (i + i)));
                odb.Store(new User("olivier " + i, "user@ndatabase.net " + i,
                                   new Profile("profile " + i, new VO.Login.Function("inner function " + i))));
            }
            odb.Close();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteBase("cache.ndb");
        }

        #endregion

        public static int NbObjects = 300;

        [Test]
        public virtual void Test1()
        {
            var odb = Open("cache.ndb");
            var query = odb.Query<VO.Login.Function>();
            query.Descend("name").Constrain((object) "function 10").Equal();
            var l = query.Execute<VO.Login.Function>();
            AssertFalse(l.Count == 0);
            // Cache must have only one object : The function
            
            odb.Close();
        }

        [Test]
        public virtual void Test2()
        {
            var odb = Open("cache.ndb");
            var query = odb.Query<User>();
            query.Descend("name").Constrain((object) "olivier 10").Equal();
            var l = query.Execute<User>();
            AssertFalse(l.Count == 0);
            // Cache must have 3 times the number of Users in list l (check the
            // setup method to understand this)
            
            odb.Close();
        }
    }
}
