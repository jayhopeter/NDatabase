using System;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Performance
{
    [TestFixture]
    public class TestBatchInsert : ODBTest
    {
//        public static int TestSize = 2000000;

        #region Setup/Teardown

        [SetUp]
        public virtual void T1est1()
        {
            //OdbConfiguration.setUseCache(false);
            DeleteBase(OdbFileName);
            //OdbConfiguration.set
            var odb = Open(OdbFileName);
            for (var i = 0; i < TestSize; i++)
            {
                odb.Store(GetSimpleObjectInstance(i));
                if (i % 10000 == 0)
                {
                    odb.Close();
                    odb = Open(OdbFileName);
                }
            }
            odb.Close();
        }

        #endregion

        public static int TestSize = 200;

        public static readonly string OdbFileName = "perf-batch.ndb";

        private SimpleObject GetSimpleObjectInstance(int i)
        {
            var so = new SimpleObject();
            so.SetDate(new DateTime());
            so.SetDuration(i);
            so.SetName("Bonjour, comment allez vous?" + i);
            return so;
        }

        [Test]
        public virtual void TestSelect()
        {
            var odb = Open(OdbFileName);
            var query = odb.Query<SimpleObject>();
            query.Descend("name").Constrain((object) "Bonjour, comment allez vous?100").Equal();
            var functions =
                query.Execute<SimpleObject>();
            odb.Close();
            AssertEquals(1, functions.Count);
        }
    }
}
