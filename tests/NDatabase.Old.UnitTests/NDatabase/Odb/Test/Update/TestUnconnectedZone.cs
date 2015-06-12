using NDatabase;
using NDatabase.Meta;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Update
{
    [TestFixture]
    public class TestUnconnectedZone : ODBTest
    {
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1()
        {
            DeleteBase("unconnected");
            var odb = Open("unconnected");
            var oid = odb.Store(new VO.Login.Function("f1"));
            odb.Close();
            Println("Oid=" + oid);
            odb = Open("unconnected");
            var f2 = (VO.Login.Function) odb.GetObjectFromId(oid);
            f2.SetName("New Function");
            odb.Store(f2);
            var storageEngine = ((global::NDatabase.Odb)odb).GetStorageEngine();
            // retrieve the class info to check connected and unconnected zone
            var fullClassName = OdbClassNameResolver.GetFullName(typeof (VO.Login.Function));
            var classInfo = storageEngine.GetSession().GetMetaModel().GetClassInfo(fullClassName, true);
            odb.Close();
            AssertEquals(1, classInfo.CommitedZoneInfo.GetNumberbOfObjects());
            AssertNotNull(classInfo.CommitedZoneInfo.First);
            AssertNotNull(classInfo.CommitedZoneInfo.Last);
            AssertEquals(0, classInfo.UncommittedZoneInfo.GetNumberbOfObjects());
            AssertNull(classInfo.UncommittedZoneInfo.First);
            AssertNull(classInfo.UncommittedZoneInfo.Last);
            odb = Open("unconnected");
            var query = odb.Query<VO.Login.Function>();
            AssertEquals(1, query.Execute<VO.Login.Function>().Count);
            odb.Close();
        }
    }
}
