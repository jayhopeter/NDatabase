using System;
using NDatabase.Meta.Compare;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Conversion
{
    /// <author>olivier</author>
    [TestFixture]
    public class TestNumberConversion : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            AssertEquals(0, AttributeValueComparator.Compare(10, Convert.ToSingle(10)));
            AssertEquals(0, AttributeValueComparator.Compare(10, Convert.ToInt64(10)));
            AssertEquals(0, AttributeValueComparator.Compare(10, Convert.ToDouble(10)));
            AssertEquals(0, AttributeValueComparator.Compare(10, (byte) 10));
            AssertEquals(0, AttributeValueComparator.Compare(10, 10));
            AssertEquals(0, AttributeValueComparator.Compare(10, (short) 10));
            AssertEquals(1, AttributeValueComparator.Compare(10, (short) 9));
            AssertEquals(10.CompareTo(9), AttributeValueComparator.Compare(10, (short) 9));
        }
    }
}
