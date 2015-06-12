using System;
using System.Globalization;
using System.Linq;
using NDatabase.UnitTests.CodeSnippets.Data;
using NUnit.Framework;

namespace NDatabase.UnitTests.CodeSnippets
{
    public class Documentation_5_minutes_tutorial
    {
        public const string TutorialDb5MinName = "tutorial.5min.ndb";

        [SetUp]
        public void SetUp()
        {
            OdbFactory.Delete(TutorialDb5MinName);
        }

        [Test]
        public void Snippet_for_tutorial()
        {
            Step1();
            Step2();
            Step3Soda();
            Step3Linq();
            Step4Soda();
            Step4Linq();
            Step5Soda();
            Step5Linq();
            Step6Soda();
            Step6Linq();
            Step7Soda();
            Step7Linq();
            Step8Soda();
            Step8Linq();
            Step9Soda();
            Step9Linq();
            Step10Soda();
            Step10Linq();
        }

        private static void Step1()
        {
            var sport = new Sport("volley-ball");

            using (var odb = OdbFactory.Open(TutorialDb5MinName))
                odb.Store(sport);
        }

        private static void Step2()
        {
            // Create instance
            var volleyball = new Sport("volley-ball");

            // Create 4 players
            var player1 = new Player("julia", DateTime.Now, volleyball);
            var player2 = new Player("magdalena", DateTime.Now, volleyball);
            var player3 = new Player("jacek", DateTime.Now, volleyball);
            var player4 = new Player("michal", DateTime.Now, volleyball);

            // Create two teams
            var team1 = new Team("Krakow");
            var team2 = new Team("Skawina");

            // Set players for team1
            team1.AddPlayer(player1);
            team1.AddPlayer(player2);

            // Set players for team2
            team2.AddPlayer(player3);
            team2.AddPlayer(player4);

            // Then create a volley ball game for the two teams
            var game = new Game(DateTime.Now, volleyball, team1, team2);

            using (var odb = OdbFactory.Open(TutorialDb5MinName))
                odb.Store(game);
        }

        private static void Step3Soda()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var query = odb.Query<Player>();
                query.Descend("Name").Constrain("julia").Equal();

                var players = query.Execute<Player>();

                Console.WriteLine("\nStep 3 (Soda): Players with name julia");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players, Has.Count.EqualTo(1));
            }
        }

        private static void Step3Linq()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var players = from player in odb.AsQueryable<Player>()
                              where player.Name.Equals("julia")
                              select player;

                Console.WriteLine("\nStep 3 (Linq): Players with name julia");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players.Count(), Is.EqualTo(1));
            }
        }


        private static void Step4Soda()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var agassi = new Player("André Agassi", DateTime.Now, new Sport("Tennis"));
                odb.Store(agassi);

                var query = odb.Query<Player>();
                query.Descend("FavoriteSport").Descend("_name")
						.Constrain("volley-ball").Equal();

                var players = query.Execute<Player>();

                Console.WriteLine("\nStep 4 (Soda): Players of Voller-ball");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players, Has.Count.EqualTo(4));
            }
        }

        private static void Step4Linq()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var players = from player in odb.AsQueryable<Player>()
                              where player.FavoriteSport.Name == "volley-ball"
                              select player;

                Console.WriteLine("\nStep 4 (Linq): Players of Voller-ball");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players.Count(), Is.EqualTo(4));
            }
        }

        private static void Step5Soda()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                // retrieve the volley ball sport object
                var query = odb.Query<Sport>();
                query.Descend("_name").Constrain("volley-ball").Equal();
                var volleyBall = query.Execute<Sport>().GetFirst();

                Assert.That(volleyBall.Name, Is.EqualTo("volley-ball"));

                // Now build a query to get all players that play volley ball, using
                // the volley ball object
                query = odb.Query<Player>();
                query.Descend("FavoriteSport").Constrain(volleyBall).Identity();

                var players = query.Execute<Player>();

                Console.WriteLine("\nStep 5 (Soda): Players of Voller-ball");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players, Has.Count.EqualTo(4));
            }
        }

        private static void Step5Linq()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                // retrieve the volley ball sport object
                var volleyBall = (from sport in odb.AsQueryable<Sport>()
                                 where sport.Name == "volley-ball"
                                 select sport).First();

                Assert.That(volleyBall.Name, Is.EqualTo("volley-ball"));

                // Now build a query to get all players that play volley ball, using
                // the volley ball object
                var players = from player in odb.AsQueryable<Player>()
                              where player.FavoriteSport.Equals(volleyBall)
                              select player;

                Console.WriteLine("\nStep 5 (Linq): Players of Voller-ball");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players.Count(), Is.EqualTo(4));
            }
        }

        private static void Step6Soda()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var query =
                    odb.Query<Player>();

                (query.Descend("FavoriteSport._name").Constrain("volley-ball").Equal())
				.Or(query.Descend("FavoriteSport._name").Constrain("%nnis").Like());

                var players = query.Execute<Player>();

                Console.WriteLine("\nStep 6 (Soda): Volley-ball and Tennis Players");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players, Has.Count.EqualTo(5));
            }
        }

        private static void Step6Linq()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var players = from player in odb.AsQueryable<Player>()
                              where
                                  player.FavoriteSport.Name.Equals("volley-ball") ||
                                  player.FavoriteSport.Name.EndsWith("nnis")
                              select player;

                Console.WriteLine("\nStep 6 (Linq): Volley-ball and Tennis Players");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players.Count(), Is.EqualTo(5));
            }
        }

        private static void Step7Soda()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var query = odb.Query<Player>();
                query.Descend("FavoriteSport._name").Constrain("volley-ball")
						.Equal().Not();

                var players = query.Execute<Player>();

                Console.WriteLine("\nStep 7 (Soda): Players that don't play Volley-ball");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players, Has.Count.EqualTo(1));
            }
        }

        private static void Step7Linq()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var players = from player in odb.AsQueryable<Player>()
                              where !player.FavoriteSport.Name.Equals("volley-ball")
                              select player;

                Console.WriteLine("\nStep 7 (Linq): Players that don't play Volley-ball");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players.Count(), Is.EqualTo(1));
            }
        }

        private static void Step8Soda()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var query = odb.Query<Player>();
                query.Descend("FavoriteSport._name").Constrain("VOLLEY").StartsWith(false);

                var players = query.Execute<Player>();

                Console.WriteLine("\nStep 8 (Soda): Players that play Volley-ball");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players, Has.Count.EqualTo(4));
            }
        }

        private static void Step8Linq()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var players = from player in odb.AsQueryable<Player>()
                              where player.FavoriteSport.Name
								.StartsWith("VOLLEY", true, CultureInfo.InvariantCulture)
                              select player;

                Console.WriteLine("\nStep 8 (Linq): Players that play Volley-ball");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players.Count(), Is.EqualTo(4));
            }
        }

//        private static void Step8LinqB()
//        {
//            using (var odb = OdbFactory.Open(TutorialDb5MinName))
//            {
//                var players = from player in odb.AsQueryable<Player>()
//                              where player.FavoriteSport.Name.StartsWith("VOLLEY", StringComparison.OrdinalIgnoreCase)
//                              select player;
//
//                Console.WriteLine("\nStep 8 (Linq): Players that play Volley-ball");
//
//                foreach (var player in players)
//                    Console.WriteLine("\t{0}", player);
//
//                Assert.That(players.Count(), Is.EqualTo(4));
//            }
//        }

        private static void Step9Soda()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var query = odb.Query<Player>();
                query.Descend("Name").Constrain("gdalen").Contains();
                var players = query.Execute<Player>();

                var magdalena = players.GetFirst();

                // builds a query to get all teams where mihn plays
                query = odb.Query<Team>();
                query.Descend("Players").Constrain(magdalena).Contains();

                var teams = query.Execute<Team>();

                Console.WriteLine("\nStep 9 (Soda): Team where magdalena plays");

                foreach (var team in teams)
                    Console.WriteLine("\t{0}", team);

                Assert.That(teams, Has.Count.EqualTo(1));
            }
        }

        private static void Step9Linq()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var players = from player in odb.AsQueryable<Player>()
                              where player.Name.Contains("gdalen")
                              select player;

                var magdalena = players.First();

                var teams = from team in odb.AsQueryable<Team>()
                            where team.Players.Contains(magdalena)
                            select team;

                Console.WriteLine("\nStep 9 (Linq): Team where magdalena plays");

                foreach (var team in teams)
                    Console.WriteLine("\t{0}", team);

                Assert.That(teams.Count(), Is.EqualTo(1));
            }
        }

        private static void Step10Soda()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var query = odb.Query<Player>();
                query.Descend("Name").OrderAscending();

                var players = query.Execute<Player>();

                Console.WriteLine("\nStep 10 (Soda): Players ordered by name asc");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players, Has.Count.EqualTo(5));

                query.Descend("Name").OrderDescending();
                players = query.Execute<Player>();

                Console.WriteLine("\nStep 10 (Soda): Players ordered by name desc");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players, Has.Count.EqualTo(5));
            }
        }

        private static void Step10Linq()
        {
            using (var odb = OdbFactory.Open(TutorialDb5MinName))
            {
                var players = from player in odb.AsQueryable<Player>()
                              orderby player.Name ascending
                              select player;

                Console.WriteLine("\nStep 10 (Linq): Players ordered by name asc");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players.Count(), Is.EqualTo(5));

                players = from player in odb.AsQueryable<Player>()
                          orderby player.Name descending
                          select player;

                Console.WriteLine("\nStep 10 (Linq): Players ordered by name desc");

                foreach (var player in players)
                    Console.WriteLine("\t{0}", player);

                Assert.That(players.Count(), Is.EqualTo(5));
            }
        }
    }
}