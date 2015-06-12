using System.IO;
using NDatabase.UnitTests.CodeSnippets.Data;
using NUnit.Framework;

namespace NDatabase.UnitTests.CodeSnippets
{
    public class Home_persist_and_retrieve_the_object
    {
        private const string DbName = "test.ndb";

        [SetUp]
        public void SetUp()
        {
            const string testDb = DbName;

            if (File.Exists(testDb))
                File.Delete(testDb);
        }

        [Test]
        public void TheSnippet()
        {
            //=================================

            // Create the instance be stored
            var sport = new Sport("volley-ball") {Value = "gold"};

            // Open the database
            using (var odb = OdbFactory.Open(DbName))
            {
                // Store the object
                odb.Store(sport);
            }

            //=================================

            // Open the database
            using (var odb = OdbFactory.Open(DbName))
            {
                var sports = odb.QueryAndExecute<Sport>();
                
                Assert.That(sports, Has.Count.EqualTo(1));
            }

            //=================================

            // Open the database
            using (var odb = OdbFactory.Open(DbName))
            {
                var firstSport = odb.QueryAndExecute<Sport>().GetFirst();

                firstSport.Value = "silver";
                odb.Store(firstSport);
            }

            //=================================

            //Open the database
            using (var odb = OdbFactory.Open(DbName))
            {
                var first = odb.QueryAndExecute<Sport>().GetFirst();
                odb.Delete(first);
            }

            // Open the database
            using (var odb = OdbFactory.Open(DbName))
            {
                var sports = odb.QueryAndExecute<Sport>();
                
                Assert.That(sports, Has.Count.EqualTo(0));
            }

            //=================================
        }
    }
}
