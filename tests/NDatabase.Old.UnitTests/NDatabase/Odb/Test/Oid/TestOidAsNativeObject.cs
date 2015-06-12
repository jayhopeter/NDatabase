using NDatabase;
using NDatabase.Oid;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Oid
{
    [TestFixture]
    public class TestOidAsNativeObject : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            var cwo = new ClassWithOid("test", OIDFactory.BuildObjectOID(47));
            DeleteBase("native-oid");
            var odb = Open("native-oid");
            odb.Store(cwo);
            odb.Close();
            odb = Open("native-oid");
            var query = odb.Query<ClassWithOid>();
            var objects = query.Execute<ClassWithOid>();
            AssertEquals(1, objects.Count);
            var cwo2 = objects.GetFirst();
            AssertEquals(47, cwo2.GetOid().ObjectId);
        }
    }
}
