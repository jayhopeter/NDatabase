using System.Text;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Buffer
{
    [TestFixture]
    public class TestBigData : ODBTest
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            DeleteBase("big-data.ndb");
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteBase("big-data.ndb");
        }

        #endregion

        [Test]
        public void Test1()
        {
            var odb = Open("big-data.ndb");
            var buffer = new StringBuilder();
            for (var i = 0; i < 30000; i++)
                buffer.Append('a');
            var function = new VO.Login.Function(buffer.ToString());
            odb.Store(function);
            odb.Close();
            odb = Open("big-data.ndb");
            var query1 = odb.Query<VO.Login.Function>();
            var f2 = query1.Execute<VO.Login.Function>().GetFirst();
            AssertEquals(30000, f2.GetName().Length);
            odb.Close();
            odb = Open("big-data.ndb");
            var query = odb.Query<VO.Login.Function>();
            f2 = query.Execute<VO.Login.Function>().GetFirst();
            f2.SetName(f2.GetName() + "ola chico");
            var newSize = f2.GetName().Length;
            odb.Store(f2);
            odb.Close();
            odb = Open("big-data.ndb");
            var query2 = odb.Query<VO.Login.Function>();
            f2 = query2.Execute<VO.Login.Function>().GetFirst();
            AssertEquals(newSize, f2.GetName().Length);
            AssertEquals(buffer + "ola chico", f2.GetName());
            odb.Close();
        }
    }
}
