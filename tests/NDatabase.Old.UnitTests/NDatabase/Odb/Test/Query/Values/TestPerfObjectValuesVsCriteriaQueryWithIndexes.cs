using System;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Query.Values
{
    public class TestPerfObjectValuesVsCriteriaQueryWithIndexes : ODBTest
    {
        private const string DbName = "perfOValuesVsCriteriaIndex1.ndb";

        [SetUp]
        public void Populate()
        {
            DeleteBase(DbName);

            using (var odb = Open(DbName))
            {
                var atts = new[] {"name"};
                odb.IndexManagerFor<User2>().AddUniqueIndexOn("Index", atts);

                const int nbProfiles = 200;
                const int nbUsers = 500;
                var profiles = new Profile[nbProfiles];
                var users = new User2[nbUsers];
                const int userStart = 1500;
                const int profileStart = 600;

                // First creates profiles
                for (var i = 0; i < nbProfiles; i++)
                {
                    profiles[i] = new Profile("profile " + (i + profileStart),
                                              new VO.Login.Function("function Profile" + i));
                    odb.Store(profiles[i]);
                }

                // Then creates users
                for (var i = 0; i < nbUsers; i++)
                {
                    users[i] = new User2("user" + (i + userStart), "user mail" + i,
                                         profiles[GetProfileIndex(nbProfiles)], i);
                    odb.Store(users[i]);
                    if (i%100 == 0)
                        Println(i);
                }
            }
        }

        private static int GetProfileIndex(int nbProfiles)
        {
            return Math.Abs(OdbRandom.GetRandomInteger()*nbProfiles)%nbProfiles;
        }

        [Test]
        public void Test0()
        {
            using (var odb = Open(DbName))
            {
                var q = odb.Query<User2>();
                var count = q.Count();
                Println(count);
                AssertEquals(Convert.ToDecimal("500"), count);
            }
        }

        [Test]
        public void Test1()
        {
            using (var odb = Open(DbName))
            {
                var q = odb.Query<User2>();
                q.Descend("name").Constrain("user1599").Equal();
                var objects = q.Execute<User2>(false);

                Println(objects.Count);
                AssertEquals(1, objects.Count);

                objects = q.Execute<User2>(false);
                
                Println(objects.Count);
                AssertEquals(1, objects.Count);
            }
        }

        [Test]
        public void TestA()
        {
            using (var odb = Open(DbName))
            {
                var q = odb.ValuesQuery<User2>().Field("name");
                q.Descend("name").Constrain("user1599").Equal();
                var v = q.Execute();

                Println(v.Count);
                AssertEquals(1, v.Count);
            }
        }
    }
}