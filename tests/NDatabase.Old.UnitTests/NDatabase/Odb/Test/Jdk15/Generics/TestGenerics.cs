using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Jdk15.Generics
{
    /// <author>olivier</author>
    [TestFixture]
    public class TestGenerics : ODBTest
    {
        [Test]
        public virtual void TestGetObects()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            odb.Store(new VO.Login.Function("Test"));
            var query = odb.Query<VO.Login.Function>();
            var functions = query.Execute<VO.Login.Function>();
            var f = functions.GetFirst();
            odb.Close();
            AssertEquals(1, functions.Count);
        }

        [Test]
        public virtual void TestGetObects2()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            odb.Store(new VO.Login.Function("Test"));
            var query = odb.Query<VO.Login.Function>();
            var functions = query.Execute<VO.Login.Function>();
            var f = functions.Next();
            odb.Close();
            AssertEquals(1, functions.Count);
        }

        [Test]
        public virtual void TestGetObects3()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            odb.Store(new VO.Login.Function("Test"));
            var query = odb.Query<VO.Login.Function>();
            var functions = query.Execute<VO.Login.Function>();
            var iterator = functions.GetEnumerator();
            var f = iterator.Current;
            odb.Close();
            AssertEquals(1, functions.Count);
        }
    }
}
