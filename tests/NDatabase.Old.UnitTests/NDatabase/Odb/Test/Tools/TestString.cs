using NDatabase.Exceptions;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Tools
{
    /// <author>olivier</author>
    [TestFixture]
    public class TestString : ODBTest
    {
        [Test]
        public virtual void Test4()
        {
            var s = "ola $1 ola $2";
            s = ExceptionsHelper.ReplaceToken(s, "$", "param", 1);
            AssertEquals("ola param1 ola $2", s);
        }

        [Test]
        public virtual void Test6()
        {
            var s = "ola $1 ola $2 ola $3 ola $4";
            s = ExceptionsHelper.ReplaceToken(s, "$", "param", 2);
            AssertEquals("ola param1 ola param2 ola $3 ola $4", s);
        }

        [Test]
        public virtual void Test8subString()
        {
            var s = "NDatabase ODB - The open source object database";
            for (var i = 0; i < 10; i++)
            {
                var s1 = s.Substring(i, i + 15 - i);
                var s2 = s.Substring(i, i + 15 - i);
                AssertEquals(s1, s2);
            }
        }

        [Test]
        public virtual void Test9subString()
        {
            var s = "NDatabase ODB - The open source object database";
            var s1 = s.Substring(0, s.Length);
            var s2 = s.Substring(0, s.Length);
            AssertEquals(s1, s2);
        }
    }
}
