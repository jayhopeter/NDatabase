using NDatabase.Client.UnitTests.Data;
using NDatabase.Exceptions;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.Transactions
{
    public class Transactions_TestCase
    {
        private const string DbName = "transaction_tests.ndb";

        [SetUp]
        public void SetUp()
        {
            OdbFactory.Delete(DbName);
        }

        [Test]
        public void Using_statement_commits_stored_objects()
        {
            var simpleClass = new SimpleClass();
            simpleClass.Name = "abc";
            simpleClass.Value = 3;

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(simpleClass);
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                Assert.That(item.Name, Is.EqualTo("abc"));
                Assert.That(item.Value, Is.EqualTo(3));
            }
        }

        [Test]
        public void Using_statement_doesnt_commit_stored_objects_if_rollback()
        {
            var simpleClass = new SimpleClass();
            simpleClass.Name = "abc";
            simpleClass.Value = 3;

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(simpleClass);
                odb.Rollback();
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                Assert.That(item, Is.Null);
            }
        }

        [Test]
        public void Commit_statement_commits_stored_objects()
        {
            var simpleClass = new SimpleClass();
            simpleClass.Name = "abc";
            simpleClass.Value = 3;

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(simpleClass);
                odb.Commit();

                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                Assert.That(item.Name, Is.EqualTo("abc"));
                Assert.That(item.Value, Is.EqualTo(3));
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                Assert.That(item.Name, Is.EqualTo("abc"));
                Assert.That(item.Value, Is.EqualTo(3));
            }
        }

        [Test]
        public void Commit_statement_commits_stored_objects_even_if_rollback_after()
        {
            var simpleClass = new SimpleClass();
            simpleClass.Name = "abc";
            simpleClass.Value = 3;

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(simpleClass);
                odb.Commit();
                odb.Rollback();
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                Assert.That(item.Name, Is.EqualTo("abc"));
                Assert.That(item.Value, Is.EqualTo(3));
            }
        }

        [Test]
        public void Using_statement_commits_updated_objects()
        {
            var simpleClass = new SimpleClass();
            simpleClass.Name = "abc";
            simpleClass.Value = 3;

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(simpleClass);
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                item.Value = 4;
                odb.Store(item);
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                Assert.That(item.Name, Is.EqualTo("abc"));
                Assert.That(item.Value, Is.EqualTo(4));
            }
        }

        [Test]
        public void Rollback_statement_revert_updated_objects()
        {
            var simpleClass = new SimpleClass();
            simpleClass.Name = "abc";
            simpleClass.Value = 3;

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(simpleClass);
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                item.Value = 4;
                odb.Store(item);
                odb.Rollback();
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                Assert.That(item.Name, Is.EqualTo("abc"));
                Assert.That(item.Value, Is.EqualTo(3));
            }
        }

        [Test]
        public void Store_two_objects_one_commited_one_reverted()
        {
            var a = new SimpleClass();
            a.Name = "abc";
            a.Value = 3;

            var b = new SimpleClass();
            b.Name = "def";
            b.Value = 6;

            using (var odb = OdbFactory.Open(DbName))
            {
                odb.Store(a);
                odb.Commit();
                odb.Store(b);
                odb.Rollback();
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var items = odb.QueryAndExecute<SimpleClass>();
                Assert.That(items.Count, Is.EqualTo(1));

                var item = items.GetFirst();
                Assert.That(item.Name, Is.EqualTo("abc"));
                Assert.That(item.Value, Is.EqualTo(3));
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                item.Name = "ghi";
                odb.Store(item);
                odb.Commit();
                item.Value = 9;
                odb.Store(item);
                odb.Rollback();
            }

            using (var odb = OdbFactory.Open(DbName))
            {
                var item = odb.QueryAndExecute<SimpleClass>().GetFirst();
                Assert.That(item.Name, Is.EqualTo("ghi"));
                Assert.That(item.Value, Is.EqualTo(3));
            }
        }

        [Test]
        public void Using_statement_doesnt_commit_stored_objects_if_rollback_2()
        {
            var simpleClass = new SimpleClass();
            simpleClass.Name = "abc";
            simpleClass.Value = 3;

            Assert.That(() =>
                            {
                                using (var odb = OdbFactory.Open(DbName))
                                {
                                    odb.Store(simpleClass);
                                    odb.Rollback();
                                    odb.Store(simpleClass);
                                }
                            }, Throws.InstanceOf<OdbRuntimeException>());
        }
    }
}