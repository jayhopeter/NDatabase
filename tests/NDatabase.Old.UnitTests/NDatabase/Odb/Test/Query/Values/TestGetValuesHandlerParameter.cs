using System;
using System.Collections;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Query.Values
{
    public class TestGetValuesHandlerParameter : ODBTest
    {
        private const string DbName = "valuesA1.ndb";

        [Test]
        public void Test1()
        {
            DeleteBase(DbName);
            using (var odb = Open(DbName))
            {
                var handler = new Handler();

                for (var i = 0; i < 10; i++)
                    handler.AddParameter(new Parameter("test " + i, "value" + i));

                odb.Store(handler);
            }

            using (var odb = Open(DbName))
            {
                var values = odb.ValuesQuery<Handler>().Field("parameters").Execute();
                Println(values);
                var ov = values.NextValues();
                var l = (IList) ov.GetByAlias("parameters");
                AssertEquals(10, l.Count);
            }
        }

        [Test]
        public void Test2()
        {
            DeleteBase(DbName);

            using (var odb = Open(DbName))
            {
                var handler = new Handler();

                for (var i = 0; i < 10; i++)
                    handler.AddParameter(new Parameter("test " + i, "value" + i));

                odb.Store(handler);
            }

            using (var odb = Open(DbName))
            {
                try
                {
                    odb.ValuesQuery<Handler>().Field("parameters").Execute<Handler>();
                    Fail("Should throw exception");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Assert.Pass();
                }
            }
        }
    }
}
