using NDatabase.Oid;
using NUnit.Framework;

namespace NDatabase.UnitTests.Serialization.Identification
{
    internal class When_we_are_using_db_object_oid : InstanceSpecification<ObjectOID>
    {
        private long _oid;
        private long _returnValue;

        protected override void Establish_context()
        {
            _oid = 12345678L;
        }

        protected override ObjectOID Create_subject_under_test()
        {
            return new ObjectOID(_oid);
        }

        protected override void Because()
        {
            _returnValue = SubjectUnderTest.ObjectId;
        }

        [Test]
        public void It_should_contain_object_id_equal_to_inserted_value()
        {
            Assert.That(_returnValue, Is.EqualTo(_oid));
        }
    }
}
