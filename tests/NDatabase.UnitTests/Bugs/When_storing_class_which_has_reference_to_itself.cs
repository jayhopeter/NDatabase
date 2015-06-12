using System;
using NDatabase.Exceptions;
using NUnit.Framework;

namespace NDatabase.UnitTests.Bugs
{
    public class MainClass
    {
        public SubClass SubClass { get; set; }
    }

    public class SubClass
    {
        public MainClass MainClass { get; set; }
    }

    public class When_storing_class_which_has_reference_to_itself
    {
        [Test]
        public void It_should_store_it_without_any_problems()
        {
            OdbFactory.Delete("recurence.ndb");

            var item = new MainClass();
            item.SubClass = new SubClass {MainClass = item};

            using (var ndb = OdbFactory.Open("recurence.ndb"))
            {
                ndb.Store(item);
            }

            using (var ndb = OdbFactory.Open("recurence.ndb"))
            {
                var query = ndb.Query<MainClass>();
                var items = query.Execute<MainClass>();
                Assert.That(items, Has.Count.EqualTo(1));
            }
        }

        [Test]
        public void It_should_store_exception_without_any_problems()
        {
            OdbFactory.Delete("recurence_ex.ndb");

            var item = new OdbRuntimeException(NDatabaseError.InternalError);

            using (var ndb = OdbFactory.Open("recurence_ex.ndb"))
            {
                ndb.Store(item);
            }

            using (var ndb = OdbFactory.Open("recurence_ex.ndb"))
            {
                var query = ndb.Query<Exception>();
                var items = query.Execute<Exception>();
                Assert.That(items, Has.Count.EqualTo(1));
            }
        }

        [Test]
        public void It_should_store_null_reference_exception_without_any_problems()
        {
            OdbFactory.Delete("recurence_ex_null.ndb");

            try
            {
                object nullVal = null;
                Console.WriteLine(nullVal.ToString());
            }
            catch (Exception ex)
            {
                using (var ndb = OdbFactory.Open("recurence_ex_null.ndb"))
                {
                    ndb.Store(ex);
                }
            }

            using (var ndb = OdbFactory.Open("recurence_ex.ndb"))
            {
                var query = ndb.Query<Exception>();
                var items = query.Execute<Exception>();
                Assert.That(items, Has.Count.EqualTo(1));
            }
        }
    }
}