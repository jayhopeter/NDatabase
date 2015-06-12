using NDatabase.Client.UnitTests.Data;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.Misc
{
    public class Add_again_retrieved_object
    {
        private static readonly string DbName = "add_again_retrieved_object.ndb";

        [SetUp]
        public void SetUp()
        {
            OdbFactory.Delete(DbName);
        }

        [Test]
        public void It_should_be_possible_to_add_retrieved_object_after_disconnecting_it()
        {
            using (var odb = OdbFactory.Open(DbName))
            {
                var address = new Address("A", City.Cracow, 1);
                odb.Store(address);
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var address = odb.QueryAndExecute<Address>().GetFirst();
                odb.Store(address);
            }

            Address clonedAddress;
            using (var odb = OdbFactory.Open(DbName))
            {
                var count = odb.QueryAndExecute<Address>().Count;
                Assert.That(count, Is.EqualTo(1));
                clonedAddress = odb.QueryAndExecute<Address>().GetFirst();
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(clonedAddress);
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var count = odb.QueryAndExecute<Address>().Count;
                Assert.That(count, Is.EqualTo(2));
            }
        }
    }
}