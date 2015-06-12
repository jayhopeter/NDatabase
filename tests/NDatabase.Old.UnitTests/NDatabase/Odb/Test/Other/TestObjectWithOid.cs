using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Other
{
    /// <summary>
    ///   Reported bug by Moises &gt; on 1.5.6
    /// </summary>
    /// <author>osmadja</author>
    [TestFixture]
    public class TestObjectWithOid : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            DeleteBase("test-object-with-oid");
            var odb = Open("test-object-with-oid");
            var o = new ObjectWithOid("15", "test");
            var oid = odb.Store(o);
            odb.Close();
            odb = Open("test-object-with-oid");
            var o2 = (ObjectWithOid) odb.GetObjectFromId(oid);
            odb.Close();
            AssertEquals(o.GetOid(), o2.GetOid());
            AssertEquals(o.GetName(), o2.GetName());
        }
    }
}
