using System.Linq;
using NDatabase.Core.Query;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Index
{
    public class TestIndexesWithValueQueries : ODBTest
    {
        [Test]
        public void Test1()
        {
            const int size = 1000;
            var baseName = GetBaseName();

            using (var odb = Open(baseName))
            {
                odb.IndexManagerFor<VO.Login.Function>().AddIndexOn("index1", new[] {"name"});

                for (var i = 0; i < size; i++)
                    odb.Store(new VO.Login.Function("function " + i));
            }

            using (var odb = Open(baseName))
            {
                var vq = odb.ValuesQuery<VO.Login.Function>().Field("name");

                vq.Descend("name").Constrain((object) ("function " + (size - 1))).Equal();

                var values = vq.Execute();
                AssertEquals(1, values.Count());

                Println(((IInternalQuery)vq).GetExecutionPlan().GetDetails());
                AssertEquals(true, ((IInternalQuery)vq).GetExecutionPlan().UseIndex());
            }

            DeleteBase(baseName);
        }
    }
}
