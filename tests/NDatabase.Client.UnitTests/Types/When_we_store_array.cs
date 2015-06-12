using System;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.Types
{
    public class When_we_store_array
    {
        [Test]
        public void Storing_multi_dimension_arrays()
        {
            var someClass = new SomeClass();

            using (var odb = OdbFactory.OpenInMemory())
            {
                odb.Store(someClass);

                var storedClass = odb.QueryAndExecute<SomeClass>().GetFirst();

                Assert.That(storedClass.DaysOfWeekMattrix[0][0], Is.EqualTo(DayOfWeek.Friday));
            }
        }

        public class SomeClass
        {
            public SomeClass()
            {
                DaysOfWeekMattrix = new DayOfWeek[2][];
                DaysOfWeekMattrix[0] = new[] { DayOfWeek.Friday };
                DaysOfWeekMattrix[1] = new[] { DayOfWeek.Sunday, DayOfWeek.Saturday };
            }

            public DayOfWeek[][] DaysOfWeekMattrix { get; set; }
        } 
    }
}