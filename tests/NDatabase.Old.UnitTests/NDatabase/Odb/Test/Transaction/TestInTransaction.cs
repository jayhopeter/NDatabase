using System;
using NDatabase.Api;
using NDatabase.Api.Query;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;
using Test.NDatabase.Odb.Test.VO.Sport;

namespace Test.NDatabase.Odb.Test.Transaction
{
    [TestFixture]
    public class TestInTransaction : ODBTest
    {
        public readonly string BaseName = "transaction";

        /// <summary>
        ///   Test select objects that are not yet commited
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestSelectUnCommitedObject()
        {
            IOdb odb = null;
            try
            {
                DeleteBase(BaseName);
                odb = Open(BaseName);
                for (var i = 0; i < 4; i++)
                    odb.Store(new VO.Login.Function("function " + i));
                odb.Close();
                // reopen the database
                odb = Open(BaseName);
                // stores a new function
                odb.Store(new VO.Login.Function("function uncommited"));
                var query = odb.Query<VO.Login.Function>();
                var functions = query.Execute<VO.Login.Function>();
                AssertEquals(5, functions.Count);
            }
            finally
            {
                if (odb != null)
                {
                    odb.Close();
                    DeleteBase(BaseName);
                }
            }
        }

        /// <summary>
        ///   Test select objects that are not yet commited
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestSelectUnCommitedObject2()
        {
            IOdb odb = null;
            try
            {
                DeleteBase(BaseName);
                odb = Open(BaseName);
                for (var i = 0; i < 4; i++)
                    odb.Store(new User("user" + i, "email" + i,
                                       new Profile("profile" + i, new VO.Login.Function("function" + i))));
                odb.Close();
                // reopen the database
                odb = Open(BaseName);
                // stores a new function
                odb.Store(new User("uncommited user", "uncommied email",
                                   new Profile("uncommiedt profile", new VO.Login.Function("uncommited function"))));
                var query = odb.Query<User>();
                var users = query.Execute<User>();
                AssertEquals(5, users.Count);
                var query1 = odb.Query<VO.Login.Function>();
                var functions = query1.Execute<VO.Login.Function>();
                AssertEquals(5, functions.Count);
                var query2 = odb.Query<Profile>();
                var profiles = query2.Execute<Profile>();
                AssertEquals(5, profiles.Count);
            }
            finally
            {
                if (odb != null)
                {
                    odb.Close();
                    DeleteBase(BaseName);
                }
            }
        }

        /// <summary>
        ///   Test select objects that are not yet commited.
        /// </summary>
        /// <remarks>
        ///   Test select objects that are not yet commited. It also test the meta
        ///   model class reference for in transaction class creation
        /// </remarks>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestSelectUnCommitedObject3()
        {
            DeleteBase(BaseName);
            // Create instance
            var sport = new Sport("volley-ball");
            IOdb odb = null;
            try
            {
                // Open the database
                odb = Open(BaseName);
                // Store the object
                odb.Store(sport);
            }
            finally
            {
                if (odb != null)
                {
                    // Close the database
                    odb.Close();
                }
            }
            try
            {
                // Open the database
                odb = Open(BaseName);
                // Let's insert a tennis player
                var agassi = new Player("Andr√© Agassi", new DateTime(), new Sport("Tennis"));
                odb.Store(agassi);
                IQuery query = odb.Query<Player>();
                query.Descend("favoriteSport.name").Constrain((object) "volley-ball").Equal();
                var players = query.Execute<Player>();
                Println("\nStep 4 : Players of Voller-ball");
                var i = 1;
                // display each object
                while (players.HasNext())
                    Println((i++) + "\t: " + players.Next());
            }
            finally
            {
                if (odb != null)
                {
                    // Close the database
                    odb.Close();
                }
            }
            DeleteBase(BaseName);
        }

        /// <summary>
        ///   Test select objects that are not yet commited
        /// </summary>
        /// <exception cref="System.Exception">System.Exception</exception>
        [Test]
        public virtual void TestSelectUnCommitedObject4()
        {
            DeleteBase(BaseName);
            // Create instance
            var sport = new Sport("volley-ball");
            IOdb odb = null;
            try
            {
                // Open the database
                odb = Open(BaseName);
                // Store the object
                odb.Store(sport);
            }
            finally
            {
                if (odb != null)
                {
                    // Close the database
                    odb.Close();
                }
            }
            // Create instance
            var volleyball = new Sport("volley-ball");
            // Create 4 players
            var player1 = new Player("olivier", new DateTime(), volleyball);
            var player2 = new Player("pierre", new DateTime(), volleyball);
            var player3 = new Player("elohim", new DateTime(), volleyball);
            var player4 = new Player("minh", new DateTime(), volleyball);
            // Create two teams
            var team1 = new Team("Paris");
            var team2 = new Team("Montpellier");
            // Set players for team1
            team1.AddPlayer(player1);
            team1.AddPlayer(player2);
            // Set players for team2
            team2.AddPlayer(player3);
            team2.AddPlayer(player4);
            // Then create a volley ball game for the two teams
            var game = new Game(new DateTime(), volleyball, team1, team2);
            odb = null;
            try
            {
                // Open the database
                odb = Open(BaseName);
                // Store the object
                odb.Store(game);
            }
            finally
            {
                if (odb != null)
                {
                    // Close the database
                    odb.Close();
                }
            }
            try
            {
                // Open the database
                odb = Open(BaseName);
                IQuery query = odb.Query<Player>();
                query.Descend("name").Constrain((object) "olivier").Equal();
                var players = query.Execute<Player>();
                Println("\nStep 3 : Players with name olivier");
                var i = 1;
                // display each object
                while (players.HasNext())
                    Println((i++) + "\t: " + players.Next());
            }
            finally
            {
                if (odb != null)
                {
                    // Close the database
                    odb.Close();
                }
            }
            try
            {
                // Open the database
                odb = Open(BaseName);
                // Let's insert a tennis player
                var agassi = new Player("Andr√© Agassi", new DateTime(), new Sport("Tennis"));
                var oid = odb.Store(agassi);
                IQuery query = odb.Query<Player>();
                query.Descend("favoriteSport.name").Constrain((object) "volley-ball").Equal();
                var players = query.Execute<Player>();
                Println("\nStep 4 : Players of Voller-ball");
                var i = 1;
                // display each object
                while (players.HasNext())
                    Println((i++) + "\t: " + players.Next());
            }
            finally
            {
                if (odb != null)
                {
                    // Close the database
                    odb.Close();
                }
            }
            DeleteBase(BaseName);
        }
    }
}
