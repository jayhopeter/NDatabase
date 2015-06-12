using System;
using System.Diagnostics;
using NDatabase.UnitTests.CodeSnippets.Data;
using NUnit.Framework;

namespace NDatabase.UnitTests.CodeSnippets
{
    public class Documentation_indexes
    {
        [Test]
        public void Add_index_then_query()
        {
            OdbFactory.Delete("index1.ndb");
            using (var odb = OdbFactory.Open("index1.ndb"))
            {
                var fields = new[] { "Name" };
                odb.IndexManagerFor<Player>().AddUniqueIndexOn("nameIndex", fields);
                
                for (var i = 0; i < 50; i++)
                {
                    var player = new Player("Player" + i, DateTime.Now, new Sport("Sport" + i));
                    odb.Store(player);
                }
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<Player>();
                var count = query.Execute<Player>().Count;
                Assert.That(count, Is.EqualTo(50));
            }
        }

        [Test]
        public void Test_perf_of_query_with_index()
        {
            OdbFactory.Delete("index1perf.ndb");
            using (var odb = OdbFactory.Open("index1perf.ndb"))
            {
                var fields = new[] { "Name" };
                odb.IndexManagerFor<Player>().AddUniqueIndexOn("nameIndex", fields);

                for (var i = 0; i < 5000; i++)
                {
                    var player = new Player("Player" + i, DateTime.Now, new Sport("Sport" + i));
                    odb.Store(player);
                }
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<Player>();
                query.Descend("Name").Constrain((object) "Player20").Equal();
                
                var count = query.Execute<Player>().Count;

                Assert.That(count, Is.EqualTo(1));
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<Player>();
                query.Descend("Name").Constrain((object) "Player1234").Equal();
                var count = query.Execute<Player>().Count;
                Assert.That(count, Is.EqualTo(1));
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<Player>();
                query.Descend("Name").Constrain((object) "Player4444").Equal();
                var count = query.Execute<Player>().Count;
                Assert.That(count, Is.EqualTo(1));
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<Player>();
                query.Descend("Name").Constrain((object) "Player3211").Equal();
                var count = query.Execute<Player>().Count;
                Assert.That(count, Is.EqualTo(1));
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed {0} ms", stopwatch.ElapsedMilliseconds);
        }

        [Test]
        public void Test_perf_of_query_without_index()
        {
            OdbFactory.Delete("index1perf.ndb");
            using (var odb = OdbFactory.Open("index1perf.ndb"))
            {
                for (var i = 0; i < 5000; i++)
                {
                    var player = new Player("Player" + i, DateTime.Now, new Sport("Sport" + i));
                    odb.Store(player);
                }
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<Player>();
                query.Descend("Name").Constrain((object) "Player20").Equal();
                var count = query.Execute<Player>().Count;
                Assert.That(count, Is.EqualTo(1));
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<Player>();
                query.Descend("Name").Constrain((object) "Player1234").Equal();
                var count = query.Execute<Player>().Count;
                Assert.That(count, Is.EqualTo(1));
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<Player>();
                query.Descend("Name").Constrain((object) "Player4444").Equal();
                var count = query.Execute<Player>().Count;
                Assert.That(count, Is.EqualTo(1));
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            stopwatch.Start();
            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<Player>();
                query.Descend("Name").Constrain((object) "Player3211").Equal();
                var count = query.Execute<Player>().Count;
                Assert.That(count, Is.EqualTo(1));
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed {0} ms", stopwatch.ElapsedMilliseconds);
        }
    }
}