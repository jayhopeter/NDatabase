using NDatabase.Exceptions;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Other
{
    [TestFixture]
    public class TestAgainstNativeObjects : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            DeleteBase("native.ndb");
            var @base = Open("native.ndb");
            try
            {
                @base.Store("olivier");
            }
            catch (OdbRuntimeException)
            {
                @base.Close();
                DeleteBase("native.ndb");
                return;
            }
            @base.Close();
            Fail("Allow native object direct persistence");
            DeleteBase("native.ndb");
        }

        [Test]
        public virtual void Test2()
        {
            DeleteBase("native.ndb");
            var @base = Open("native.ndb");
            try
            {
                var array = new[] {"olivier", "joao", "peter"};
                @base.Store(array);
            }
            catch (OdbRuntimeException)
            {
                @base.Close();
                DeleteBase("native.ndb");
                return;
            }
            @base.Close();
            Fail("Allow native object direct persistence");
            DeleteBase("native.ndb");
        }
    }
}
