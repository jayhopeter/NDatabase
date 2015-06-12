using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Intropector
{
    public class IntrospectorWithNestedClasses : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            DeleteBase("test-nested");
            var odb = Open("test-nested");
            var c1 = new Class1("name1", "name2", "name3");
            var oid = odb.Store(c1);
            odb.Close();
            odb = Open("test-nested");
            var c11 = (Class1) odb.GetObjectFromId(oid);
            AssertEquals(c1.GetName1(), c11.GetName1());
            AssertEquals(c1.GetClass2().GetClass3().GetName3(), c11.GetClass2().GetClass3().GetName3());
        }
    }
}
