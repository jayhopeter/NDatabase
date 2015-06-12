using System;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;

namespace Test.NDatabase.Odb.Test.Other
{
    [TestFixture]
    public class Test2Closes : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            DeleteBase("hook.ndb");
            var obase = Open("hook.ndb");
            var query = obase.Query<TestClass>();
            query.Execute<TestClass>();
            obase.Store(new TestClass());
            obase.Close();
            var exception = false;
            try
            {
                obase.Close();
            }
            catch (Exception e)
            {
                exception = true;
                AssertTrue(e.Message.IndexOf("ODB session has already been closed") != -1);
            }
            AssertTrue(exception);
        }
    }
}
