using System.Collections;
using NDatabase.Api;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Query.Values
{
    public class TestValuesQueryWithOid : ODBTest
    {
        [Test]
        public void Test1()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);

            OID oid;
            using (var odb = Open(baseName))
            {
                var handler = new Handler();

                for (var i = 0; i < 10; i++)
                    handler.AddParameter(new Parameter("test " + i, "value " + i));

                oid = odb.Store(handler);
            }

            using (var odb = Open(baseName))
            {
                var valuesQuery =
                    odb.ValuesQuery<Handler>(oid).Field("parameters").Sublist("parameters", "sub1", 1, 5, true).Sublist(
                        "parameters", "sub2", 1, 10).Size("parameters", "size");

                var values = valuesQuery.Execute();
                Println(values);

                var ov = values.NextValues();
                var fulllist = (IList) ov.GetByAlias("parameters");
                AssertEquals(10, fulllist.Count);

                var size = (long) ov.GetByAlias("size");
                AssertEquals(10, size);

                var p = (Parameter) fulllist[0];
                AssertEquals("value 0", p.GetValue());

                var p2 = (Parameter) fulllist[9];
                AssertEquals("value 9", p2.GetValue());

                var sublist = (IList) ov.GetByAlias("sub1");
                AssertEquals(5, sublist.Count);

                p = (Parameter) sublist[0];
                AssertEquals("value 1", p.GetValue());

                p2 = (Parameter) sublist[4];
                AssertEquals("value 5", p2.GetValue());

                var sublist2 = (IList) ov.GetByAlias("sub2");
                AssertEquals(9, sublist2.Count);
            }
        }
    }
}
