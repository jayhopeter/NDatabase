using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;

namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    [TestFixture]
    public class TestCriteriaQuery5 : ODBTest
    {
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestCriteriaWithDate()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            for (var i = 0; i < 10; i++)
            {
                var tc = new TestClass();
                tc.SetInt1(i);
                odb.Store(tc);
            }
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<TestClass>();
            query.Descend("int1").Constrain(0).Greater().Equal();
            var os = query.Execute<TestClass>();
            AssertEquals(10, os.Count);
            var j = 0;
            while (os.HasNext())
            {
                var tc = os.Next();
                AssertEquals(j, tc.GetInt1());
                j++;
            }
            odb.Close();
        }

        [Test]
        public virtual void TestIntLongCriteriaQuery()
        {
            var baseName = GetBaseName();

            var odb = Open(baseName);
            var cwi = new ClassWithInt(1, "test");
            odb.Store(cwi);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<ClassWithInt>();
            query.Descend("i").Constrain((object) ((long) 1)).Equal();
            var os = query.Execute<ClassWithInt>();
            AssertEquals(1, os.Count);
            odb.Close();
        }

        [Test]
        public virtual void TestLongIntCriteriaQuery()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            var cwl = new ClassWithLong(1L, "test");
            odb.Store(cwl);
            odb.Close();
            odb = Open(baseName);
            var query = odb.Query<ClassWithLong>();
            query.Descend("i").Constrain((object) 1L).Equal();
            var os = query.Execute<ClassWithLong>();
            AssertEquals(1, os.Count);
            odb.Close();
        }

        [Test]
        public virtual void TestLongIntCriteriaQueryGt()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            var cwl = new ClassWithLong(1L, "test");
            odb.Store(cwl);
            odb.Close();
            odb = Open(baseName);
            var criteriaQuery = odb.Query<ClassWithLong>();
            criteriaQuery.Descend("i").Constrain(1L).Greater().Equal();
            var os = criteriaQuery.Execute<ClassWithLong>();
            AssertEquals(1, os.Count);
            var query = odb.Query<ClassWithLong>();
            query.Descend("i").Constrain(1L).Greater();
            os = query.Execute<ClassWithLong>();
            AssertEquals(0, os.Count);
            odb.Close();
        }
    }
}
