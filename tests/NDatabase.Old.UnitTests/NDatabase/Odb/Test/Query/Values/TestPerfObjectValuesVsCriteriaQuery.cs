using System;
using NDatabase;
using NDatabase.Core.Query;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Query.Values
{
    public class TestPerfObjectValuesVsCriteriaQuery : ODBTest
    {
        private const string DbName = "perfOValuesVsCriteria.ndb";

        [SetUp]
        public virtual void Populate()
        {
            OdbFactory.Delete(DbName);

            using (var odb = Open(DbName))
            {
                const int nbProfiles = 20;
                const int nbUsers = 50;
                var profiles = new Profile[nbProfiles];
                var users = new User2[nbUsers];

                // First creates profiles
                for (var i = 0; i < nbProfiles; i++)
                {
                    profiles[i] = new Profile("profile " + i, new VO.Login.Function("function Profile" + i));
                    odb.Store(profiles[i]);
                }

                // Then creates users
                for (var i = 0; i < nbUsers; i++)
                {
                    users[i] = new User2("user" + i, "user mail" + i, profiles[GetProfileIndex(nbProfiles)], i);
                    odb.Store(users[i]);
                    if (i%10 == 0)
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
                var count = odb.Query<User2>().Count();
                Println(count);

                AssertEquals(Convert.ToDecimal("50"), count);
            }
        }

        [Test]
        public void TestA()
        {
            using (var odb = Open(DbName))
            {
                var q = odb.Query<Profile>();
                var objects = q.Execute<Profile>(false);

                Println(objects.Count);
                Console.WriteLine(((IInternalQuery) q).GetExecutionPlan().GetDetails());

                AssertEquals(20, objects.Count);
            }
        }

        [Test]
        public void Test1()
        {
            using (var odb = Open(DbName))
            {
                var q = odb.ValuesQuery<User2>().Field("name");
                q.Descend("nbLogins").Constrain(10).Equal();
                var v = q.Execute();

                Println(v.Count);

                Console.WriteLine(((IInternalQuery) q).GetExecutionPlan().GetDetails());

                AssertEquals(1, v.Count);
            }
        }
    }
}