using NDatabase.Exceptions;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Rollback
{
    [TestFixture]
    public class TestRollBack : ODBTest
    {
        /// <exception cref="System.Exception"></exception>
        public override void TearDown()
        {
            DeleteBase("rollback.ndb");
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1()
        {
            DeleteBase("rollback.ndb");
            var odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("f1"));
            odb.Store(new VO.Login.Function("f2"));
            odb.Store(new VO.Login.Function("f3"));
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("f3"));
            odb.Rollback();
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query = odb.Query<VO.Login.Function>();
            AssertEquals(3, query.Execute<VO.Login.Function>().Count);
            odb.Close();
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test2()
        {
            DeleteBase("rollback.ndb");
            var odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("f1"));
            odb.Store(new VO.Login.Function("f2"));
            odb.Store(new VO.Login.Function("f3"));
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("f3"));
            odb.Rollback();
            // odb.close();
            try
            {
                var query = odb.Query<VO.Login.Function>();
                AssertEquals(3, query.Execute<VO.Login.Function>().Count);
            }
            catch (OdbRuntimeException e)
            {
                var s = e.ToString();
                AssertFalse(s.IndexOf("ODB session has been rollbacked") == -1);
            }
            odb.Close();
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test3RollbackOneStore()
        {
            DeleteBase("rollback.ndb");
            var odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("f1"));
            odb.Store(new VO.Login.Function("f2"));
            odb.Store(new VO.Login.Function("f3"));
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("f3"));
            odb.Rollback();
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query = odb.Query<VO.Login.Function>();
            AssertEquals(3, query.Execute<VO.Login.Function>().Count);
            odb.Close();
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test4RollbackXXXStores()
        {
            DeleteBase("rollback.ndb");
            var odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("f1"));
            odb.Store(new VO.Login.Function("f2"));
            odb.Store(new VO.Login.Function("f3"));
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            for (var i = 0; i < 500; i++)
                odb.Store(new VO.Login.Function("f3 - " + i));
            odb.Rollback();
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query = odb.Query<VO.Login.Function>();
            AssertEquals(3, query.Execute<VO.Login.Function>().Count);
            odb.Close();
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test5RollbackDelete()
        {
            DeleteBase("rollback.ndb");
            var odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("f1"));
            odb.Store(new VO.Login.Function("f2"));
            odb.Store(new VO.Login.Function("f3"));
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query = odb.Query<VO.Login.Function>();
            var objects = query.Execute<VO.Login.Function>();
            while (objects.HasNext())
                odb.Delete(objects.Next());
            odb.Rollback();
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query1 = odb.Query<VO.Login.Function>();
            AssertEquals(3, query1.Execute<VO.Login.Function>().Count);
            odb.Close();
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test6RollbackDeleteAndStore()
        {
            DeleteBase("rollback.ndb");
            var odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("f1"));
            odb.Store(new VO.Login.Function("f2"));
            odb.Store(new VO.Login.Function("f3"));
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query = odb.Query<VO.Login.Function>();
            var objects = query.Execute<VO.Login.Function>();
            while (objects.HasNext())
                odb.Delete(objects.Next());
            for (var i = 0; i < 500; i++)
                odb.Store(new VO.Login.Function("f3 - " + i));
            odb.Rollback();
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query1 = odb.Query<VO.Login.Function>();
            AssertEquals(3, query1.Execute<VO.Login.Function>().Count);
            odb.Close();
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test7Update()
        {
            DeleteBase("rollback.ndb");
            var odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("1function"));
            odb.Store(new VO.Login.Function("2function"));
            odb.Store(new VO.Login.Function("3function"));
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query = odb.Query<VO.Login.Function>();
            var objects = query.Execute<VO.Login.Function>();
            while (objects.HasNext())
            {
                var f = objects.Next();
                f.SetName(f.GetName().Substring(1));
                odb.Store(f);
            }
            odb.Rollback();
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query1 = odb.Query<VO.Login.Function>();
            AssertEquals(3, query1.Execute<VO.Login.Function>().Count);
            odb.Close();
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test8RollbackDeleteAndStore()
        {
            DeleteBase("rollback.ndb");
            var odb = Open("rollback.ndb", "u1", "p1");
            odb.Store(new VO.Login.Function("f1"));
            odb.Store(new VO.Login.Function("f2"));
            odb.Store(new VO.Login.Function("f3"));
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query = odb.Query<VO.Login.Function>();
            var objects = query.Execute<VO.Login.Function>();
            while (objects.HasNext())
            {
                var f = objects.Next();
                f.SetName(f.GetName().Substring(1));
                odb.Store(f);
            }
            objects.Reset();
            while (objects.HasNext())
                odb.Delete(objects.Next());
            for (var i = 0; i < 500; i++)
                odb.Store(new VO.Login.Function("f3 - " + i));
            odb.Rollback();
            odb.Close();
            odb = Open("rollback.ndb", "u1", "p1");
            var query1 = odb.Query<VO.Login.Function>();
            AssertEquals(3, query1.Execute<VO.Login.Function>().Count);
            odb.Close();
        }
    }
}
