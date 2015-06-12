using NUnit.Framework;

namespace NDatabase.UnitTests.Bugs.Index
{
    public class When_we_use_index_on_invariant_equal_criteria
    {
        [Test]
        public void It_should_return_the_same_number_of_elements_when_using_index_and_when_doesnt_use_index()
        {
            OdbFactory.Delete("IndexIssue.ndb");

            const int size = 50;

            for (var i = 0; i < size; i++)
            {
                using (var odb = OdbFactory.Open("IndexIssue.ndb"))
                {
                    odb.Store(new SampleClass {ID = "ID." + i.ToString(), Value = i});
                }
            }

            long count;
            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<SampleClass>();
                query.Descend("ID").Constrain("id.5%").InvariantLike();
                count = query.Count();
            }

            long count2;
            using (var odb = OdbFactory.OpenLast())
            {
                odb.IndexManagerFor<SampleClass>().AddIndexOn("index", new[] { "ID" });

                var query = odb.Query<SampleClass>();
                query.Descend("ID").Constrain("id.5%").InvariantLike();
                count2 = query.Count();
            }

            Assert.That(count, Is.EqualTo(count2));
        }

        [Test]
        public void It_should_return_the_same_number_of_elements_when_using_unique_index_and_when_doesnt_use_index()
        {
            OdbFactory.Delete("IndexIssue.ndb");

            const int size = 50;

            for (var i = 0; i < size; i++)
            {
                using (var odb = OdbFactory.Open("IndexIssue.ndb"))
                {
                    odb.Store(new SampleClass { ID = "ID." + i.ToString(), Value = i });
                }
            }

            long count;
            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<SampleClass>();
                query.Descend("ID").Constrain("id.5%").InvariantLike();
                count = query.Count();
            }

            long count2;
            using (var odb = OdbFactory.OpenLast())
            {
                odb.IndexManagerFor<SampleClass>().AddUniqueIndexOn("index", new[] { "ID" });

                var query = odb.Query<SampleClass>();
                query.Descend("ID").Constrain("id.5%").InvariantLike();
                count2 = query.Count();
            }

            Assert.That(count, Is.EqualTo(count2));
        }

        #region Nested type: SampleClass

        private class SampleClass
        {
            public string ID { get; set; }
            public int Value { get; set; }
        }

        #endregion
    }
}
