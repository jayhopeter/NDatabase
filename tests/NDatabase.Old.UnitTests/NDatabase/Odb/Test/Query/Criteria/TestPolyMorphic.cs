using System;
using NDatabase.Api;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Human;

namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    public class TestPolyMorphic : ODBTest
    {
        private const string DbName = "TestPolyMorphic.ndb";

        [Test]
        public void Test1()
        {
            DeleteBase(DbName);
            using (var odb = Open(DbName))
            {
                odb.Store(new Animal("dog", "M", "my dog"));
                odb.Store(new Animal("cat", "F", "my cat"));
                odb.Store(new Man("Joe"));
                odb.Store(new Woman("Karine"));
            }

            IObjectSet<object> os;
            using (var odb = Open(DbName))
            {
                var q = odb.Query<object>();

                os = q.Execute<object>();
                Println(os);
            }

            AssertEquals(4, os.Count);
            DeleteBase(DbName);
        }

        [Test]
        public void Test2()
        {
            DeleteBase(DbName);
            using (var odb = Open(DbName))
            {
                odb.Store(new Animal("dog", "M", "my dog"));
                odb.Store(new Animal("cat", "F", "my cat"));
                odb.Store(new Man("Joe"));
                odb.Store(new Woman("Karine"));
            }

            IObjectSet<Human> os;
            using (var odb = Open(DbName))
            {
                var q = odb.Query<Human>();

                os = q.Execute<Human>();
                Println(os);
            }

            AssertEquals(2, os.Count);
            DeleteBase(DbName);
        }

        [Test]
        public void Test3()
        {
            DeleteBase(DbName);
            using (var odb = Open(DbName))
            {
                odb.Store(new Animal("dog", "M", "my dog"));
                odb.Store(new Animal("cat", "F", "my cat"));
                odb.Store(new Man("Joe"));
                odb.Store(new Woman("Karine"));
            }

            IValues os;
            using (var odb = Open(DbName))
            {
                var q = odb.ValuesQuery<object>().Field("specie");

                os = q.Execute();
                Println(os);
            }

            AssertEquals(4, os.Count);
            DeleteBase(DbName);
        }

        [Test]
        public void Test4()
        {
            DeleteBase(DbName);
            using (var odb = Open(DbName))
            {
                odb.Store(new Animal("dog", "M", "my dog"));
                odb.Store(new Animal("cat", "F", "my cat"));
                odb.Store(new Man("Joe"));
                odb.Store(new Woman("Karine"));
            }

            IValues os;
            using (var odb = Open(DbName))
            {
                var q = odb.ValuesQuery<Human>().Field("specie");

                os = q.Execute();
                Println(os);
            }

            AssertEquals(2, os.Count);
            DeleteBase(DbName);
        }

        [Test]
        public void Test5()
        {
            DeleteBase(DbName);
            using (var odb = Open(DbName))
            {
                odb.Store(new Animal("dog", "M", "my dog"));
                odb.Store(new Animal("cat", "F", "my cat"));
                odb.Store(new Man("Joe"));
                odb.Store(new Woman("Karine"));
            }

            IValues os;
            using (var odb = Open(DbName))
            {
                var q = odb.ValuesQuery<Man>().Field("specie");

                os = q.Execute();
                Println(os);
            }

            AssertEquals(1, os.Count);
            DeleteBase(DbName);
        }

        [Test]
        public void Test6()
        {
            DeleteBase(DbName);
            using (var odb = Open(DbName))
            {
                odb.Store(new Animal("dog", "M", "my dog"));
                odb.Store(new Animal("cat", "F", "my cat"));
                odb.Store(new Man("Joe"));
                odb.Store(new Woman("Karine"));
            }

            Decimal nb;
            using (var odb = Open(DbName))
            {
                var q = odb.Query<object>();

                nb = q.Count();
                Println(nb);
            }

            AssertEquals(new Decimal(4), nb);
            DeleteBase(DbName);
        }

        [Test]
        public void Test7()
        {
            const int size = 3000;
            var baseName = GetBaseName();

            using (var odb = Open(baseName))
            {
                for (var i = 0; i < size; i++)
                {
                    odb.Store(new Animal("dog", "M", "my dog"));
                    odb.Store(new Animal("cat", "F", "my cat"));
                    odb.Store(new Man("Joe" + i));
                    odb.Store(new Woman("Karine" + i));
                }
            }

            Decimal nb;
            using (var odb = Open(baseName))
            {
                var q = odb.Query<object>();

                nb = q.Count();
                Println(nb);
            }

            AssertEquals(new Decimal(4 * size), nb);
            DeleteBase(baseName);
        }

        [Test]
        public void Test8()
        {
            const int size = 3000;
            var baseName = GetBaseName();
            using (var odb = Open(baseName))
            {
                for (var i = 0; i < size; i++)
                {
                    odb.Store(new Animal("dog" + i, "M", "my dog" + i));
                    odb.Store(new Animal("cat" + i, "F", "my cat" + i));
                    odb.Store(new Man("Joe" + i));
                    odb.Store(new Woman("Karine" + i));
                }
            }

            Decimal nb;
            using (var odb = Open(baseName))
            {
                var q = odb.Query<object>();
                q.Descend("specie").Constrain("man").Equal();

                nb = q.Count();
                Println(nb);
            }

            AssertEquals(new Decimal(1 * size), nb);
            DeleteBase(baseName);
        }
    }
}
