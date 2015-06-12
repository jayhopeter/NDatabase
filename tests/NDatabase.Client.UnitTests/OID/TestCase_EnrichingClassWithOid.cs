using NDatabase.Api;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.OID
{
    public class TestCase_EnrichingClassWithOid
    {
        private const string DbName = "enrich_oid.ndb";

        [Test]
        public void It_should_be_possible_to_enrich_long_field_of_class_with_OID()
        {
            OdbFactory.Delete(DbName);
            const string value = "value";

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(new TestOidClassLong(value));
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var first = odb.QueryAndExecute<TestOidClassLong>().GetFirst();

                Assert.That(first.OID, Is.GreaterThan(0L));
                Assert.That(first.Value, Is.EqualTo(value));
            }
        }

        [Test]
        public void It_should_be_possible_to_enrich_OID_field_of_class_with_OID()
        {
            OdbFactory.Delete(DbName);
            const string value = "value";

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(new TestOidClassOID(value));
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var first = odb.QueryAndExecute<TestOidClassOID>().GetFirst();

                Assert.That(first.OID, Is.Not.Null);
                Assert.That(first.OID.ObjectId, Is.GreaterThan(0));
                Assert.That(first.Value, Is.EqualTo(value));
            }
        }

        [Test]
        public void It_should_not_be_possible_to_enrich_non_long_nor_OID_field_of_class_with_OID()
        {
            OdbFactory.Delete(DbName);
            const string value = "value";

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(new TestOidClassString(value));
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var first = odb.QueryAndExecute<TestOidClassString>().GetFirst();

                Assert.That(first.OID, Is.Null);
                Assert.That(first.Value, Is.EqualTo(value));
            }
        }
    }

    public class TestOidClassLong
    {
        [OID]
        private readonly long _oid;

        public TestOidClassLong(string value)
        {
            Value = value;
            _oid = -1;
        }

        public string Value { get; set; }

        public long OID
        {
            get { return _oid; }
        }
    }

    public class TestOidClassOID
    {
        [OID]
        private readonly Api.OID _oid;

        public TestOidClassOID(string value)
        {
            Value = value;
            _oid = null;
        }

        public string Value { get; set; }

        public Api.OID OID
        {
            get { return _oid; }
        }
    }

    public class TestOidClassString
    {
        [OID]
        private readonly string _oid;

        public TestOidClassString(string value)
        {
            Value = value;
            _oid = null;
        }

        public string Value { get; set; }

        public string OID
        {
            get { return _oid; }
        }
    }                                         
}