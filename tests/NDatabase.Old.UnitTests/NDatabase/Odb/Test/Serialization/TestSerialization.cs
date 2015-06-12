using System;
using System.Globalization;
using NDatabase.Odb.Core.Layers.Layer2.Meta;
using NDatabase.Odb.Impl.Core.Layers.Layer2.Meta.Serialization;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Serialization
{
    [TestFixture]
    public class TestSerialization : ODBTest
    {
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestAtomicNativeCollectionBigDecimal()
        {
            var bd = Convert.ToDecimal("123456789.987654321", CultureInfo.InvariantCulture);
            AtomicNativeObjectInfo anoi = null;
            anoi = new AtomicNativeObjectInfo(bd, OdbType.BigDecimalId);
            var s = Serializer.ToString(anoi);
            // println(s);
            var anoi2 = (AtomicNativeObjectInfo) Serializer.FromOneString(s);
            AssertEquals(anoi, anoi2);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestAtomicNativeCollectionDate()
        {
            var date = new DateTime();
            AtomicNativeObjectInfo anoi = null;
            anoi = new AtomicNativeObjectInfo(date, OdbType.DateId);
            var s = Serializer.ToString(anoi);
            // println(s);
            var anoi2 = (AtomicNativeObjectInfo) Serializer.FromOneString(s);
            AssertEquals(anoi, anoi2);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestAtomicNativeCollectionDouble()
        {
            var d = 123456789.789456123;
            AtomicNativeObjectInfo anoi = null;
            anoi = new AtomicNativeObjectInfo(d, OdbType.DoubleId);
            var s = Serializer.ToString(anoi);
            // println(s);
            var anoi2 = (AtomicNativeObjectInfo) Serializer.FromOneString(s);
            Assert.That(anoi.ToString(), Is.EqualTo(anoi2.ToString()));
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestAtomicNativeCollectionInt()
        {
            var i = 123456789;
            AtomicNativeObjectInfo anoi = null;
            anoi = new AtomicNativeObjectInfo(i, OdbType.IntegerId);
            var s = Serializer.ToString(anoi);
            // println(s);
            var anoi2 = (AtomicNativeObjectInfo) Serializer.FromOneString(s);
            AssertEquals(anoi, anoi2);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestAtomicNativeCollectionString()
        {
            var s1 = "ol√° chico";
            AtomicNativeObjectInfo anoi = null;
            anoi = new AtomicNativeObjectInfo(s1, OdbType.StringId);
            var s = Serializer.ToString(anoi);
            // println(s);
            var anoi2 = (AtomicNativeObjectInfo) Serializer.FromOneString(s);
            AssertEquals(anoi, anoi2);
        }
    }
}
