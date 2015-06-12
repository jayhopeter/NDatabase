using System;
using NDatabase.Api;
using NDatabase.Api.Query;
using NDatabase.Client.UnitTests.Data.Heroes;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.Queries.Soda
{
    public class Building_soda_queries_doc
    {
        private const string DbName = "building_soda.ndb";

        [SetUp]
        public void SetUp()
        {
            OdbFactory.Delete(DbName);

            var warrior1 = new Warrior("Warrior 1");
            var warrior2 = new Warrior("Warrior 2");

            using (var odb2 = OdbFactory.Open(DbName))
            {
                odb2.Store(warrior1);
                odb2.Store(warrior2);
            }
        }

        [Test]
        public void GetAllWarriors()
        {
            IOdb odb = OdbFactory.Open(DbName);
            try
            {
                IQuery query = odb.Query<Warrior>();
                IObjectSet<Warrior> result = query.Execute<Warrior>();
                Assert.That(result.Count, Is.EqualTo(2));
                PrintResult(result);
            }
            finally
            {
                odb.Close();
            }
        }

        [Test]
        public void GetAllWarriors2()
        {
            IOdb odb = OdbFactory.Open(DbName);
            try
            {
                var result = odb.QueryAndExecute<Warrior>();
                Assert.That(result.Count, Is.EqualTo(2));
                PrintResult(result);
            }
            finally
            {
                odb.Close();
            }
        }

        [Test]
        public void GetWarriorByName()
        {
            IOdb odb = OdbFactory.Open(DbName);
            try
            {
                IQuery query = odb.Query<Warrior>();
                query.Descend("_name").Constrain("Warrior 1").Equal();
                IObjectSet<Warrior> result = query.Execute<Warrior>();
                Assert.That(result.Count, Is.EqualTo(1));
                PrintResult(result);
            }
            finally
            {
                odb.Close();
            }
        }

        [Test]
        public void GetWarriorByExactAttackValue()
        {
            IOdb odb = OdbFactory.Open(DbName);
            try
            {
                IQuery query = odb.Query<Warrior>();
                query.Descend("_attack").Constrain(3).Equal();
                IObjectSet<Warrior> result = query.Execute<Warrior>();
                Assert.That(result.Count, Is.EqualTo(2));
                PrintResult(result);
            }
            finally
            {
                odb.Close();
            }
        }

        [Test]
        public void GetWarriorByNegation()
        {
            IOdb odb = OdbFactory.Open(DbName);
            try
            {
                IQuery query = odb.Query<Warrior>();
                query.Descend("_name").Constrain("Warrior 1").Equal().Not();
                IObjectSet<Warrior> result = query.Execute<Warrior>();
                Assert.That(result.Count, Is.EqualTo(1));
                PrintResult(result);
            }
            finally
            {
                odb.Close();
            }
        }

        [Test]
        public void GetWarriorByConjunction()
        {
            IOdb odb = OdbFactory.Open(DbName);
            try
            {
                IQuery query = odb.Query<Warrior>();
                var nameConstraint = query.Descend("_name").Constrain("Warrior 1").Equal();
                query.Descend("_attack").Constrain(3).And(nameConstraint);
                IObjectSet<Warrior> result = query.Execute<Warrior>();
                Assert.That(result.Count, Is.EqualTo(1));
                PrintResult(result);
            }
            finally
            {
                odb.Close();
            }
        }

        [Test]
        public void GetWarriorByDisjunction()
        {
            IOdb odb = OdbFactory.Open(DbName);
            try
            {
                IQuery query = odb.Query<Warrior>();
                var nameConstraint = query.Descend("_name").Constrain("Warrior 1").Equal();
                query.Descend("_attack").Constrain(3).Or(nameConstraint);
                IObjectSet<Warrior> result = query.Execute<Warrior>();
                Assert.That(result.Count, Is.EqualTo(2));
                PrintResult(result);
            }
            finally
            {
                odb.Close();
            }
        }

        [Test]
        public static void GetWarriorsByComparison()
        {
            IOdb odb = OdbFactory.Open(DbName);
            try
            {
                IQuery query = odb.Query<Warrior>();
                query.Descend("_attack").Constrain(2).Greater();
                IObjectSet<Warrior> result = query.Execute<Warrior>();
                Assert.That(result.Count, Is.EqualTo(2));
                PrintResult(result);
            }
            finally
            {
                odb.Close();
            }
        }

        [Test]
        public void GetWarriorsByLikeOrContains()
        {
            IOdb odb = OdbFactory.Open(DbName);
            try
            {
                IQuery query2 = odb.Query<Warrior>();
                
                query2.Descend("_name").Constrain("rior").Contains();
                IObjectSet<Warrior> result1 = query2.Execute<Warrior>();
                Assert.That(result1.Count, Is.EqualTo(2));
                PrintResult(result1);


                IQuery query3 = odb.Query<Warrior>();
                
                query3.Descend("_name").Constrain("rior").Like();
                IObjectSet<Warrior> result2 = query3.Execute<Warrior>();
                Assert.That(result2.Count, Is.EqualTo(2));
                PrintResult(result2);
            }
            finally
            {
                odb.Close();
            }
        }

        [Test]
        public void GetWarriorsByStartsOrEndsWith()
        {
            IOdb odb = OdbFactory.Open(DbName);
            try
            {
                IQuery query2 = odb.Query<Warrior>();
                query2.Descend("_name").Constrain("Warr").StartsWith(false);
                IObjectSet<Warrior> result1 = query2.Execute<Warrior>();
                Assert.That(result1.Count, Is.EqualTo(2));
                PrintResult(result1);

                IQuery query3 = odb.Query<Warrior>();
                query3.Descend("_name").Constrain("rior 1").EndsWith(false);
                IObjectSet<Warrior> result2 = query3.Execute<Warrior>();
                Assert.That(result2.Count, Is.EqualTo(1));
                PrintResult(result2);
            }
            finally
            {
                odb.Close();
            }
        }

        private static void PrintResult(IObjectSet<Warrior> result)
        {
            foreach (var warrior in result)
                Console.WriteLine(warrior);
        }
    }
}