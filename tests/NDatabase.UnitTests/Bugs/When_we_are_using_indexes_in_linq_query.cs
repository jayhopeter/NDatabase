using System.Linq;
using NUnit.Framework;

namespace NDatabase.UnitTests.Bugs
{
    public class SimpleClass
    {
        public string Name { get; set; }
        public int Size { get; set; }
    }

    public class When_we_are_using_indexes_in_linq_query
    {
        [Test]
        public void It_should_handle_easily_simple_scenario()
        {
            OdbFactory.Delete("test.ndb");

            using (var inMemory = OdbFactory.Open("test.ndb"))
            {
                inMemory.IndexManagerFor<SimpleClass>().AddIndexOn("Index_test", "Name");

                var inMemoryTestClass = new
                    SimpleClass { Name = "Test", Size = 1 };

                inMemory.Store(inMemoryTestClass);

                var query = inMemory.Query<SimpleClass>();
                query.Descend("Name").Constrain("Test").Equal();

                var items = query.Execute<SimpleClass>();

                //                var items = (from item in inMemory.AsQueryable<SimpleClass>()
                //                             where item.Name.Equals("Test")
                //                             select item).ToList();

                var memoryTestClass = items.FirstOrDefault();
                Assert.That(memoryTestClass, Is.Not.Null);
                Assert.That(memoryTestClass.Name, Is.EqualTo("Test"));
                Assert.That(memoryTestClass.Size, Is.EqualTo(1));
            }
        }
    }
}