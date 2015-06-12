using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Transient_attributes
{
    [TestFixture]
    public class TestTransient : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            var vo = new VoWithTransientAttribute("vo1");
            odb.Store(vo);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<VoWithTransientAttribute>();
            var vos = query.Execute<VoWithTransientAttribute>();
            odb.Close();
            Println(vos.GetFirst().GetName());
            AssertEquals(1, vos.Count);
            AssertEquals("vo1", vos.GetFirst().GetName());
        }
    }
}
