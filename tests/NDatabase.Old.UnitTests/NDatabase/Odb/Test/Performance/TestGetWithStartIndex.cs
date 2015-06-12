using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Performance
{
    [TestFixture]
    public class TestGetWithStartIndex : ODBTest
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            DeleteBase("start-index.ndb");
            var odb = Open("start-index.ndb");
            for (var i = 0; i < 10; i++)
                odb.Store(new VO.Login.Function("function " + i));
            odb.Close();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteBase("start-index.ndb");
        }

        #endregion

        [Test]
        public virtual void Test1()
        {
            var odb = Open("start-index.ndb");
            var query = odb.Query<VO.Login.Function>();
            var l = query.Execute<VO.Login.Function>(false, 4, 7);
            AssertEquals(3, l.Count);
            AssertEquals("function 4", l.GetFirst().ToString());
            odb.Close();
        }
    }
}
