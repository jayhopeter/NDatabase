using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Enum
{
    [TestFixture]
    public class TestEnum : ODBTest
    {
        [Test]
        public virtual void TestEnum1()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            var e = new ClassWithEnum("enum1", ObjectType.Medium);
            odb.Store(e);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<ClassWithEnum>();
            var objects = query.Execute<ClassWithEnum>();
            odb.Close();
            AssertEquals(1, objects.Count);
        }

        [Test]
        public virtual void TestEnumUpdate()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            var e = new ClassWithEnum("enum1", ObjectType.Medium);
            odb.Store(e);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<ClassWithEnum>();
            var objects = query.Execute<ClassWithEnum>();
            var cwe = objects.GetFirst();
            cwe.SetObjectType(ObjectType.Small);
            odb.Store(cwe);
            odb.Close();
            odb = Open(baseName);
            var query1 = odb.Query<ClassWithEnum>();
            objects = query1.Execute<ClassWithEnum>();
            AssertEquals(1, objects.Count);

            cwe = objects.GetFirst();
            odb.Close();
            AssertEquals(ObjectType.Small, cwe.GetObjectType());
        }
    }
}
