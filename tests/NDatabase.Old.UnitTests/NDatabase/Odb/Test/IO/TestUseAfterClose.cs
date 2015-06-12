using System;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.IO
{
    [TestFixture]
    public class TestUseAfterClose : ODBTest
    {
        [Test]
        public virtual void Test()
        {
            DeleteBase("t111A.ndb");
            var odb = Open("t111A.ndb");
            odb.Close();
            try
            {
                odb.Store(new VO.Login.Function("login"));
            }
            catch (Exception e)
            {
                AssertTrue(e.Message.IndexOf("has already been closed") != -1);
            }
        }

        [Test]
        [Ignore("Long running test - reopening db before throwing exception")]
        public virtual void TestReOpenWithoutClose()
        {
            DeleteBase("t111C.ndb");
            var odb = Open("t111C.ndb");
            try
            {
                odb = Open("t111C.ndb");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Pass();
            }
            odb.Close();
        }

        [Test]
        public virtual void TestTwoCloses()
        {
            DeleteBase("t111B.ndb");
            var odb = Open("t111B.ndb");
            odb.Close();
            try
            {
                odb.Close();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.Pass();
            }
        }
    }
}
