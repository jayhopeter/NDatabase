using System.Linq;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.InMemory
{
    public class InMemoryTestClass
    {
        public string Name { get; set; }
        public int Size { get; set; }
    }

    public class When_we_use_ndatabase_as_in_memory_db
    {
        [Test]
        public void It_should_handle_easily_simple_scenario()
        {
            using (var inMemory = OdbFactory.OpenInMemory())
            {
                inMemory.IndexManagerFor<InMemoryTestClass>().AddIndexOn("Index_test", "Name");

                var inMemoryTestClass = new
                    InMemoryTestClass {Name = "Test", Size = 1};

                inMemory.Store(inMemoryTestClass);

                var query = inMemory.Query<InMemoryTestClass>();
                query.Descend("Name").Constrain("Test").Equal();

                var items = query.Execute<InMemoryTestClass>();

                var memoryTestClass = items.FirstOrDefault();
                Assert.That(memoryTestClass, Is.Not.Null);
                Assert.That(memoryTestClass.Name, Is.EqualTo("Test"));
                Assert.That(memoryTestClass.Size, Is.EqualTo(1));
            }
        }
    }
}