using NDatabase.Api.Query;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    [TestFixture]
    public class TestRelation : ODBTest
    {
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestNullRelation()
        {
            DeleteBase("null-rel.ndb");
            var odb = Open("null-rel.ndb");
            odb.Store(new Class2());
            odb.Close();
            odb = Open("null-rel.ndb");
            IQuery q = odb.Query<Class2>();
            q.Descend("class1.name").Constrain(null).Equal();
            var os = q.Execute<Class2>();
            odb.Close();
            AssertEquals(1, os.Count);
            var c2 = os.GetFirst();
            AssertEquals(null, c2.GetClass1());
        }
    }
}
