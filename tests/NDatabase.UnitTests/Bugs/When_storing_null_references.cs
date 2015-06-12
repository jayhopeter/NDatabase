using NUnit.Framework;

namespace NDatabase.UnitTests.Bugs
{
    public class When_storing_null_references
    {
        class A
        {
            public B Value { get; set; }
        }

        class B
        {
            public string Value { get; set; } 
        }


        [Test] 
        public void It_should_store_object_with_null_as_the_object_value()
        {
            OdbFactory.Delete("nullreftest.ndb");

            using (var odb = OdbFactory.Open("nullreftest.ndb"))
            {
                var a = new A {Value = null};

                odb.Store(a);
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<A>();
                var a = query.Execute<A>().GetFirst();

                a.Value = new B();

                odb.Store<A>(a);
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<A>();
                var a = query.Execute<A>().GetFirst();

                a.Value.Value = "Value";

                odb.Store<A>(a);
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<A>();
                var a = query.Execute<A>().GetFirst();

                Assert.That((object) a.Value.Value, Is.EqualTo("Value"));
            }
        }
    }
}