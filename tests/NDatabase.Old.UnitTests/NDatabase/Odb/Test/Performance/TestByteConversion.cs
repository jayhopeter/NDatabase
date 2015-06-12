using System;
using NDatabase.Core.Engine;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Performance
{
    [TestFixture]
    public class TestByteConversion : ODBTest
    {
        public const int Size = 1000;

        public const int Size0 = 1000;

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal1()
        {
            var bd1 = new Decimal(10);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal10()
        {
            var bd1 = new Decimal(123456789123456789123456789.123456789123456789);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal11()
        {
            var bd1 = new Decimal(-0.00000);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal2()
        {
            var bd1 = new Decimal(10.123456789123456789);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal3()
        {
            var bd1 = new Decimal(0);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal4()
        {
            var bd1 = new Decimal(10);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal5()
        {
            var bd1 = new Decimal(0.000);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal6()
        {
            var bd1 = new Decimal(0.000000000000000123456789);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal7()
        {
            var bd1 = new Decimal(-1);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal8()
        {
            var bd1 = new Decimal(-123456789);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBigDecimal9()
        {
            var bd1 = new Decimal(-0.000000000000000000000000000000123456789);
            var b2 = ByteArrayConverter.DecimalToByteArray(bd1);
            var bd2 = ByteArrayConverter.ByteArrayToDecimal(b2);
            AssertEquals(bd1, bd2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestBoolean()
        {
            var b1 = true;
            var b2 = ByteArrayConverter.BooleanToByteArray(b1);
            var b3 = ByteArrayConverter.ByteArrayToBoolean(b2, 0);
            AssertEquals(b1, b3);
            b1 = false;
            b2 = ByteArrayConverter.BooleanToByteArray(b1);
            b3 = ByteArrayConverter.ByteArrayToBoolean(b2, 0);
            AssertEquals(b1, b3);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestChar()
        {
            var c = '\u00E1';
            var b2 = ByteArrayConverter.CharToByteArray(c);
            var c1 = ByteArrayConverter.ByteArrayToChar(b2);
            AssertEquals(c, c1);
        }

        [Test]
        public virtual void TestDouble()
        {
            var l1 = 785412.4875;
            var b2 = ByteArrayConverter.DoubleToByteArray(l1);
            var l2 = ByteArrayConverter.ByteArrayToDouble(b2);
            AssertEquals(l1, l2, 0);
        }

        [Test]
        public virtual void TestFloat()
        {
            var l1 = (float) 785412.4875;
            var b2 = ByteArrayConverter.FloatToByteArray(l1);
            var l2 = ByteArrayConverter.ByteArrayToFloat(b2);
            AssertEquals(l1, l2, 0);
        }

        [Test]
        public virtual void TestInt()
        {
            var l1 = 785412;
            var b = ByteArrayConverter.IntToByteArray(l1);
            var l2 = ByteArrayConverter.ByteArrayToInt(b);
            AssertEquals(l1, l2);
        }

        [Test]
        public virtual void TestLong()
        {
            long l1 = 785412;
            var b = ByteArrayConverter.LongToByteArray(l1);
            var l2 = ByteArrayConverter.ByteArrayToLong(b);
            AssertEquals(l1, l2);
            l1 = long.MaxValue;
            b = ByteArrayConverter.LongToByteArray(l1);
            l2 = ByteArrayConverter.ByteArrayToLong(b);
            AssertEquals(l1, l2);
            l1 = long.MinValue;
            b = ByteArrayConverter.LongToByteArray(l1);
            l2 = ByteArrayConverter.ByteArrayToLong(b);
            AssertEquals(l1, l2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestShort()
        {
            short s = 4598;
            var b2 = ByteArrayConverter.ShortToByteArray(s);
            var s2 = ByteArrayConverter.ByteArrayToShort(b2);
            // assertEquals(s,s2);
            s = 10000;
            b2 = ByteArrayConverter.ShortToByteArray(s);
            s2 = ByteArrayConverter.ByteArrayToShort(b2);
            AssertEquals(s, s2);
            s = short.MaxValue;
            b2 = ByteArrayConverter.ShortToByteArray(s);
            s2 = ByteArrayConverter.ByteArrayToShort(b2);
            AssertEquals(s, s2);
            s = short.MinValue;
            b2 = ByteArrayConverter.ShortToByteArray(s);
            s2 = ByteArrayConverter.ByteArrayToShort(b2);
            AssertEquals(s, s2);
        }

        /// <exception cref="System.IO.IOException"></exception>
        [Test]
        public virtual void TestString()
        {
            var s = "test1";
            var b2 = ByteArrayConverter.StringToByteArray(s, -1);
            var s2 = ByteArrayConverter.ByteArrayToString(b2);
            AssertEquals(s, s2);
        }
    }
}
