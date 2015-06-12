using NDatabase;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Oid
{
    /// <author>olivier</author>
    [TestFixture]
    public class TestStoreWithOID : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            DeleteBase("withoid");

            var odb = Open("withoid");
            var oid = odb.Store(new VO.Login.Function("f1"));
            odb.Close();
            odb = Open("withoid");
            var f2 = new VO.Login.Function("f2");
            var engine = ((global::NDatabase.Odb)odb).GetStorageEngine();
            engine.Store(oid, f2);
            odb.Close();
            odb = Open("withoid");
            var f = (VO.Login.Function) odb.GetObjectFromId(oid);
            odb.Close();
            AssertEquals("f2", f.GetName());
        }
    }
}
