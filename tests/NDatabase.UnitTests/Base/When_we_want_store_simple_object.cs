using System.IO;
using System.Linq;
using NDatabase.Api;
using NDatabase.UnitTests.TestData;
using NUnit.Framework;

namespace NDatabase.UnitTests.Base
{
    public class When_we_want_store_simple_object : InstanceSpecification<IOdb>
    {
        private Person _person;
        private string _dbFileName;

        protected override void Establish_context()
        {
            _person = new Person("Julia", 3);
            _dbFileName = "simple.ndb";

            if (File.Exists(_dbFileName))
                File.Delete(_dbFileName);
        }

        protected override IOdb Create_subject_under_test()
        {
            return OdbFactory.Open(_dbFileName);
        }

        protected override void Because()
        {
            SubjectUnderTest.Store(_person);
        }

        [Test]
        public void It_should_store_object_and_return_it_even_after_closing_db()
        {
            SubjectUnderTest.Dispose();

            using (var db = OdbFactory.Open(_dbFileName))
            {
                var query = db.Query<Person>();
                var restoredPerson = query.Execute<Person>().First<Person>();
                Assert.That(restoredPerson.Name, Is.EqualTo(_person.Name));
                Assert.That(restoredPerson.Age, Is.EqualTo(_person.Age));
            }
        }
    }
}