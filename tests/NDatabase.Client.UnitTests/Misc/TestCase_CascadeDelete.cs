using NDatabase.Api;
using NDatabase.Client.UnitTests.Data;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.Misc
{
    public class TestCase_CascadeDelete
    {
        private const string DbName = "cascade_delete.ndb";

        [Test]
        public void It_is_deleting_only_child_objects_marked_by_CascadeDelete_attribute()
        {
            OdbFactory.Delete(DbName);

            using (var odb = OdbFactory.Open(DbName))
            {
                var shouldBeDeleted = new Address("A", City.Cracow, 1);
                var shouldNotBeDeleted = new Address("B", City.Cracow, 1);
                odb.Store(new TestCascadeDeleteClass(shouldBeDeleted, shouldNotBeDeleted));
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var count = odb.QueryAndExecute<Address>().Count;
                Assert.That(count, Is.EqualTo(2));
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var first = odb.QueryAndExecute<TestCascadeDeleteClass>().GetFirst();
                odb.Delete(first);
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var count = odb.QueryAndExecute<TestCascadeDeleteClass>().Count;
                Assert.That(count, Is.EqualTo(0));

                count = odb.QueryAndExecute<Address>().Count;
                Assert.That(count, Is.EqualTo(1));

                var first = odb.QueryAndExecute<Address>().GetFirst();
                Assert.That(first.Street, Is.EqualTo("B"));
            }
        }
    }

    public class TestCascadeDeleteClass
    {
        [CascadeDelete]
        private readonly Address _address1;

        private readonly Address _address2;

        public TestCascadeDeleteClass(Address address1, Address address2)
        {
            _address1 = address1;
            _address2 = address2;
        }

        public Address Address1
        {
            get { return _address1; }
        }

        public Address Address2
        {
            get { return _address2; }
        }
    }
}