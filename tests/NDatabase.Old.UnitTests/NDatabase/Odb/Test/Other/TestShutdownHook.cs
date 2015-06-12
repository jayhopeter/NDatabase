using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;

namespace Test.NDatabase.Odb.Test.Other
{
    [TestFixture]
    public class TestShutdownHook : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            DeleteBase("hook.ndb");
            var obase = Open("hook.ndb");
            var query = obase.Query<TestClass>();
            query.Execute<TestClass>();
            obase.Store(new TestClass());
            obase.Close();
            DeleteBase("hook.ndb");
        }
    }
}
