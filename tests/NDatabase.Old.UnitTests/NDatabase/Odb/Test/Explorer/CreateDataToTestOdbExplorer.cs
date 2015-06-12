using System;
using System.Linq;
using NDatabase;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;

namespace Test.NDatabase.Odb.Test.Explorer
{
    public class CreateDataToTestOdbExplorer
    {
        /// <summary>
        ///   bug found by Julio Jimenez Borreguero When there exist an index on a
        ///   numeric field, the criteria query is constructed with a value of type String instead of numeric
        /// </summary>
        [Test]
        public virtual void Test1()
        {
            OdbFactory.Delete("base1.ndb");
            using (var odb = OdbFactory.Open("base1.ndb"))
            {
                var fields = new[] {"int1"};
                odb.IndexManagerFor<TestClass>().AddUniqueIndexOn("index1", fields);
                var start = OdbTime.GetCurrentTimeInMs();
                var size = 50;
                for (var i = 0; i < size; i++)
                {
                    var testClass = new TestClass();
                    testClass.SetBigDecimal1(new Decimal(i));
                    testClass.SetBoolean1(i % 3 == 0);
                    testClass.SetChar1((char) (i % 5));
                    testClass.SetDate1(new DateTime(start + i));
                    testClass.SetDouble1(((double) (i % 10)) / size);
                    testClass.SetInt1(size - i);
                    testClass.SetString1("test class " + i);
                    odb.Store(testClass);
                }
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<TestClass>();
                var count = query.Execute<TestClass>().Count<TestClass>();
                Assert.That(count, Is.EqualTo(50));
            }
        }
    }
}
