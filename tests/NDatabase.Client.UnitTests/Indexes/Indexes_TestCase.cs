using NDatabase.Client.UnitTests.Data;
using NDatabase.Exceptions;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.Indexes
{
    public class Indexes_TestCase
    {
        private const string DbName = "indexes.ndb";

        [SetUp]
        public void SetUp()
        {
            OdbFactory.Delete(DbName);
        }

        [Test]
        public void Simple_usage_of_index()
        {
            using (var odb = OdbFactory.Open(DbName))
            {
                odb.IndexManagerFor<Address>().AddIndexOn("MyIndex_PK", "Street");
                odb.IndexManagerFor<Address>().AddUniqueIndexOn("MyUniqueIndex", "Street", "Number");
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(new Address("A", City.Cracow, 1));
                odb.Store(new Address("B", City.Cracow, 1));
            }
            
            using (var odb = OdbFactory.Open(DbName))
            {
                Assert.That(odb.IndexManagerFor<Address>().ExistIndex("MyIndex_PK"), Is.True);
                Assert.That(odb.IndexManagerFor<Address>().ExistIndex("MyUniqueIndex"), Is.True);

                Assert.That(() => odb.Store(new Address("A", City.Cracow, 1)), Throws.InstanceOf<OdbRuntimeException>()); //rollback
            }
        }

        [Test]
        public void Simple_usage_of_index_with_one_value_set_to_null()
        {
            using (var odb = OdbFactory.Open(DbName))
            {
                odb.IndexManagerFor<Address>().AddIndexOn("MyIndex_PK", "Street");
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(new Address("A", City.Cracow, 1));
                odb.Store(new Address(null, City.Cracow, 1));
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                Assert.That(odb.IndexManagerFor<Address>().ExistIndex("MyIndex_PK"), Is.True);

                var count = odb.QueryAndExecute<Address>().Count;
                Assert.That(count, Is.EqualTo(2));
            }
        }


        [Test]
        public void It_requires_comparable_to_create_index()
        {
            using (var odb = OdbFactory.Open(DbName))
            {
                Assert.That(() => odb.IndexManagerFor<User>().AddIndexOn("MyIndex_PK", "Address"), Throws.InstanceOf<OdbRuntimeException>());
            }
        }
    }
}