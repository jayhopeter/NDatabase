using NDatabase.Exceptions;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Error
{
    [TestFixture]
    public class TestError : ODBTest
    {
        [Test]
        public virtual void Test2()
        {
            var e = new NDatabaseError(0, "x @1 @2 @3 @5 y");
            e.AddParameter("param1");
            e.AddParameter("param2");
            e.AddParameter("param3");
            e.AddParameter("param4");
            AssertEquals("0:x param1 param2 param3 @5 y", e.ToString());
        }

        [Test]
        public virtual void Test3()
        {
            var e = new NDatabaseError(0, "x y");
            e.AddParameter("param1");
            e.AddParameter("param2");
            e.AddParameter("param3");
            e.AddParameter("param4");
            AssertEquals("0:x y", e.ToString());
        }

        [Test]
        public virtual void Test4()
        {
            var e = new NDatabaseError(12, "x @1 @2 @3 @5 y");
            AssertEquals("12:x @1 @2 @3 @5 y", e.ToString());
        }

        /// <summary>
        ///   Submitted by Tom Davies (tgdavies) Source forge Feature request 1900092
        /// </summary>
        [Test]
        public virtual void TestDollarInParam()
        {
            var e = new NDatabaseError(0, "x @1 y");
            e.AddParameter("foo$bar");
            AssertEquals("0:x foo$bar y", e.ToString());
        }
    }
}
