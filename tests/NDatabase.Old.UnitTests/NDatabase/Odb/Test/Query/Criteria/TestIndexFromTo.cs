using NDatabase.Api.Query;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    [TestFixture]
    public class TestIndexFromTo : ODBTest
    {
        [Test]
        public virtual void TestGetLimitedResult()
        {
            var baseName = GetBaseName();
            var size = 1000;
            var odb = Open(baseName);
            for (var i = 0; i < size; i++)
                odb.Store(new VO.Login.Function("function " + i));
            odb.Close();
            odb = Open(baseName);
            IQuery q = odb.Query<VO.Login.Function>();
            var os = q.Execute<VO.Login.Function>(true, 0, 10);
            AssertEquals(10, os.Count);
            for (var i = 0; i < 10; i++)
            {
                var f = os.Next();
                AssertEquals("function " + i, f.GetName());
            }
            odb.Close();
            DeleteBase(baseName);
        }

        [Test]
        public virtual void TestGetLimitedResult1()
        {
            var baseName = GetBaseName();
            var size = 1000;
            var odb = Open(baseName);
            for (var i = 0; i < size; i++)
                odb.Store(new VO.Login.Function("function " + i));
            odb.Close();
            odb = Open(baseName);
            IQuery q = odb.Query<VO.Login.Function>();
            var os = q.Execute<VO.Login.Function>(true, 0, 1);
            AssertEquals(1, os.Count);
            for (var i = 0; i < os.Count; i++)
            {
                var f = os.Next();
                AssertEquals("function " + i, f.GetName());
            }
            odb.Close();
            DeleteBase(baseName);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestGetLimitedResult2()
        {
            var baseName = GetBaseName();
            var size = 1000;
            var odb = Open(baseName);
            for (var i = 0; i < size; i++)
                odb.Store(new VO.Login.Function("function " + i));
            odb.Close();
            odb = Open(baseName);
            IQuery q = odb.Query<VO.Login.Function>();
            var os = q.Execute<VO.Login.Function>(true, 10, 20);
            AssertEquals(10, os.Count);
            for (var i = 10; i < 20; i++)
            {
                var f = os.Next();
                AssertEquals("function " + i, f.GetName());
            }
            odb.Close();
            DeleteBase(baseName);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestGetLimitedResult3()
        {
            var baseName = GetBaseName();
            var size = 1000;
            var odb = Open(baseName);
            for (var i = 0; i < size; i++)
            {
                if (i < size / 2)
                    odb.Store(new VO.Login.Function("function " + i));
                else
                    odb.Store(new VO.Login.Function("FUNCTION " + i));
            }
            odb.Close();
            odb = Open(baseName);
            IQuery q = odb.Query<VO.Login.Function>();
            q.Descend("name").Constrain("FUNCTION%").Like();
            var os = q.Execute<VO.Login.Function>(true, 0, 10);
            AssertEquals(10, os.Count);
            for (var i = size / 2; i < size / 2 + 10; i++)
            {
                var f = os.Next();
                AssertEquals("FUNCTION " + i, f.GetName());
            }
            odb.Close();
            DeleteBase(baseName);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestGetLimitedResult4()
        {
            var baseName = GetBaseName();
            var size = 1000;
            var odb = Open(baseName);
            for (var i = 0; i < size; i++)
            {
                if (i < size / 2)
                    odb.Store(new VO.Login.Function("function " + i));
                else
                    odb.Store(new VO.Login.Function("FUNCTION " + i));
            }
            odb.Close();
            odb = Open(baseName);
            IQuery q = odb.Query<VO.Login.Function>();
            q.Descend("name").Constrain("FUNCTION%").Like();
            var os = q.Execute<VO.Login.Function>(true, 10, 20);
            AssertEquals(10, os.Count);
            for (var i = size / 2 + 10; i < size / 2 + 20; i++)
            {
                var f = os.Next();
                AssertEquals("FUNCTION " + i, f.GetName());
            }
            odb.Close();
            DeleteBase(baseName);
        }
    }
}
