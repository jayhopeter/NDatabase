using System.Linq;
using NDatabase.Api;
using NDatabase.Oid;
using NUnit.Framework;

namespace NDatabase.UnitTests.Serialization.Identification
{
    public class When_we_are_using_database_id : InstanceSpecification<IDatabaseId>
    {
        private long[] _ids;
        private long _firstId;
        private long _secondId;

        protected override void Establish_context()
        {
            _firstId = 1234L;
            _secondId = 445566L;
        }

        protected override IDatabaseId Create_subject_under_test()
        {
            return new DatabaseId(new []{_firstId, _secondId});
        }

        protected override void Because()
        {
            _ids = SubjectUnderTest.GetIds();
        }

        [Test]
        public void It_should_contain_all_inserted_ids_and_only_them()
        {
            Assert.That(_ids.Contains(_firstId), Is.True);
            Assert.That(_ids.Contains(_secondId), Is.True);
            Assert.That(_ids.Contains(3333L), Is.False);
        }

        [Test]
        public void It_returns_correct_string_representation_of_state()
        {
            var stringRepresentation = string.Format("{0}-{1}", _firstId, _secondId);
            Assert.That(SubjectUnderTest.ToString(), Is.EqualTo(stringRepresentation));
        }

        [Test]
        public void It_returns_string_which_could_be_used_to_create_equal_second_instance()
        {
            var stringRepresentation = SubjectUnderTest.ToString();
            var secondInstance = FromString(stringRepresentation);

            Assert.That(secondInstance, Is.EqualTo(SubjectUnderTest));
            Assert.That(secondInstance.GetHashCode(), Is.EqualTo(SubjectUnderTest.GetHashCode()));
        }
        
        internal static IDatabaseId FromString(string sid)
        {
            var tokens = sid.Split('-');

            var ids = new long[tokens.Length];

            for (var i = 0; i < ids.Length; i++)
                ids[i] = long.Parse(tokens[i]);

            return new DatabaseId(ids);
        }

        [Test]
        public void It_should_be_different_than_new_instance_with_different_ids()
        {
            var secondInstance = new DatabaseId(new[] { 11L, 22L, 33L });

            Assert.That(secondInstance, Is.Not.EqualTo(SubjectUnderTest));
            Assert.That(secondInstance.GetHashCode(), Is.Not.EqualTo(SubjectUnderTest.GetHashCode()));
        }
    }
}