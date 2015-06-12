using System;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;

namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    [TestFixture]
    public class TestCriteriaQuery2 : ODBTest
    {
        public void SetUp(string BaseName)
        {
            base.SetUp();
            DeleteBase(BaseName);
            var odb = Open(BaseName);
            var start = OdbTime.GetCurrentTimeInTicks();
            var size = 50;
            for (var i = 0; i < size; i++)
            {
                var testClass = new TestClass();
                testClass.SetBigDecimal1(new Decimal(i));
                testClass.SetBoolean1(i%3 == 0);
                testClass.SetChar1((char) (i%5));
                testClass.SetDate1(new DateTime(start + i));
                testClass.SetDouble1(((double) (i%10))/size);
                testClass.SetInt1(size - i);
                testClass.SetString1("test class " + i);
                odb.Store(testClass);
            }
            // println(testClass.getDouble1() + " | " + testClass.getString1() +
            // " | " + testClass.getInt1());
            odb.Close();
        }

        [Test]
        public virtual void Test1()
        {
            var BaseName = GetBaseName();
            SetUp(BaseName);
            var odb = Open(BaseName);
            var aq =
                odb.Query<TestClass>();

            aq.Descend("string1").Constrain("test class 1").Equal().Or(
                aq.Descend("string1").Constrain("test class 3").Equal());
            aq.Descend("string1").OrderAscending();
            var l = aq.Execute<TestClass>(true, -1, -1);
            odb.Close();

            AssertEquals(2, l.Count);
            var testClass = l.GetFirst();
            AssertEquals("test class 1", testClass.GetString1());
        }

        [Test]
        public virtual void Test2()
        {
            var BaseName = GetBaseName();
            SetUp(BaseName);
            var odb = Open(BaseName);
            var aq = odb.Query<TestClass>();
            (aq.Descend("string1").Constrain("test class 2").Equal()).Not();
            var l = aq.Execute<TestClass>(true, -1, -1);
            AssertEquals(49, l.Count);
            var testClass = l.GetFirst();
            AssertEquals("test class 0", testClass.GetString1());
            odb.Close();
        }

        [Test]
        public virtual void Test3()
        {
            var BaseName = GetBaseName();
            SetUp(BaseName);
            var odb = Open(BaseName);
            var aq =
                odb.Query<TestClass>();

            (aq.Descend("string1").Constrain("test class 0").Equal()).Or(
                aq.Descend("bigDecimal1").Constrain(new Decimal(5)).Equal()).Not();

            var l = aq.Execute<TestClass>(true, -1, -1);
            AssertEquals(48, l.Count);
            var testClass = l.GetFirst();
            AssertEquals("test class 1", testClass.GetString1());
            odb.Close();
        }

        [Test]
        public virtual void Test4Sort()
        {
            var BaseName = GetBaseName();
            SetUp(BaseName);
            var odb = Open(BaseName);
            var aq =
                odb.Query<TestClass>();

            aq.Descend("string1").Constrain("test class 2").Equal().Or(
                aq.Descend("string1").Constrain("test class 3").Equal()).Not();

            aq.Descend("double1").OrderDescending();
            aq.Descend("int1").OrderDescending();
            var l = aq.Execute<TestClass>(true, -1, -1);

            AssertEquals(48, l.Count);
            var testClass = l.GetFirst();
            AssertEquals("test class 9", testClass.GetString1());
            odb.Close();
        }

        [Test]
        public virtual void Test5Sort()
        {
            var BaseName = GetBaseName();
            SetUp(BaseName);
            var odb = Open(BaseName);
            var aq =
                odb.Query<TestClass>();

            aq.Descend("string1").Constrain("test class 2").Equal().Or(
                aq.Descend("string1").Constrain("test class 3").Equal()).Not();

            aq.Descend("double1").OrderDescending();
            aq.Descend("int1").OrderDescending();
            var l = aq.Execute<TestClass>(true, -1, -1);
            AssertEquals(48, l.Count);
            var testClass = l.GetFirst();
            AssertEquals("test class 9", testClass.GetString1());
            odb.Close();
        }

        [Test]
        public virtual void Test6Sort()
        {
            var BaseName = GetBaseName();
            SetUp(BaseName);
            var odb = Open(BaseName);
            var aq = odb.Query<TestClass>();
            (aq.Descend("string1").Constrain("test class 2").Equal()).Or(
                aq.Descend("string1").Constrain("test class 3").Equal()).Or(
                    aq.Descend("string1").Constrain("test class 4").Equal()).Or(
                        aq.Descend("string1").Constrain("test class 5").Equal());

            aq.Descend("boolean1").OrderDescending();
            aq.Descend("int1").OrderDescending();
            var l = aq.Execute<TestClass>(true, -1, -1);
            AssertEquals(4, l.Count);
            var testClass = l.GetFirst();
            AssertEquals("test class 3", testClass.GetString1());
            odb.Close();
        }
    }
}