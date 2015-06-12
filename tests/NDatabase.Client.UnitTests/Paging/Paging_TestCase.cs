using System.Linq;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.Paging
{
    public class SimpleItem
    {
        public int Value;
    }

    public class Paging_TestCase
    {
        [Test]
        public void It_should_be_easily_possible_to_get_particular_pages_from_query_result()
        {
            const string dbName = "DbName.ndb";

            OdbFactory.Delete(dbName);

            using (var odb = OdbFactory.Open(dbName))
            {
                for (var i = 0; i < 100; i++)
                {
                    var item = new SimpleItem {Value = i};
                    odb.Store(item);
                }
            }

            using (var odb = OdbFactory.Open(dbName))
            {
                var from0To5Items = odb.Query<SimpleItem>().Execute<SimpleItem>(true, 0, 5).ToList();

                Assert.That(from0To5Items, Has.Count.EqualTo(5));
                for (var i = 0; i < 5; i++)
                    Assert.That(from0To5Items[i].Value, Is.EqualTo(i));
            }

            using (var odb = OdbFactory.Open(dbName))
            {
                var from55To61Items = odb.Query<SimpleItem>().Execute<SimpleItem>(true, 55, 61).ToList();

                Assert.That(from55To61Items, Has.Count.EqualTo(6));
                for (var i = 0; i < 6; i++)
                    Assert.That(from55To61Items[i].Value, Is.EqualTo(i+55));
            }
        }
    }
}