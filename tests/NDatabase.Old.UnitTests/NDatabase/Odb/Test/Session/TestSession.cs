using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Arraycollectionmap;

namespace Test.NDatabase.Odb.Test.Session
{
    [TestFixture]
    public class TestSession : ODBTest
    {
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1()
        {
            DeleteBase("session.ndb");
            var odb = Open("session.ndb");
            odb.Close();
            var odb2 = Open("session.ndb");
            var query = odb2.Query<PlayerWithList>();
            var l = query.Execute<PlayerWithList>(true);
            AssertEquals(0, l.Count);
            odb2.Close();
            DeleteBase("session.ndb");
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test2()
        {
            DeleteBase("session.ndb");
            var odb = Open("session.ndb");
            var f = new VO.Login.Function("f1");
            odb.Store(f);
            odb.Commit();
            f.SetName("f1 -1");
            odb.Store(f);
            odb.Close();
            odb = Open("session.ndb");
            var query = odb.Query<VO.Login.Function>();
            var os = query.Execute<VO.Login.Function>();
            AssertEquals(1, os.Count);
            var f2 = os.GetFirst();
            odb.Close();
            DeleteBase("session.ndb");
            AssertEquals("f1 -1", f2.GetName());
        }
    }
}
