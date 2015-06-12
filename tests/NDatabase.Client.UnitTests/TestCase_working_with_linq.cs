using System.Collections.Generic;
using System.Linq;
using NDatabase.Client.UnitTests.Data;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests
{
    public class TestCase_working_with_linq
    {
        private const string DbName = "working_with_linq.ndb";

        [SetUp]
        public void SetUp()
        {
            CleanDb();
            PrepareTheData();
        }

        [Test]
        public void Test_simple_select()
        {
            using (var odb = OdbFactory.Open(DbName))
            {
                var users = from user in odb.AsQueryable<User>()
                            select user;

                Assert.That(users.Count(), Is.EqualTo(10));
            }
        }

        [Test]
        public void Test_simple_select_soda()
        {
            List<User> users;
            using (var odb = OdbFactory.Open(DbName))
            {
                users = odb.QueryAndExecute<User>().ToList();
            }
            Assert.That(users.Count(), Is.EqualTo(10));
        }

        [Test]
        public void Test_simple_where()
        {
            using (var odb = OdbFactory.Open(DbName))
            {
                var users = from user in odb.AsQueryable<User>()
                            where user.Age < 10
                            select user;

                Assert.That(users.Count(), Is.EqualTo(1));
                var julia = users.First();
                Assert.That(julia.Name, Is.EqualTo("Julia"));
                Assert.That(julia.Age, Is.EqualTo(4));
                Assert.That(julia.Address.City, Is.EqualTo(City.Cracow));
                Assert.That(julia.Address.Street, Is.EqualTo("Gwiazdy"));
                Assert.That(julia.Address.Number, Is.EqualTo(874));
            }
        }

        [Test]
        public void Test_where_with_order_by_asc()
        {
            using (var odb = OdbFactory.Open(DbName))
            {
                var users = from user in odb.AsQueryable<User>()
                            where user.Age > 30 && user.Name.Contains("n")
                            orderby user.Name ascending 
                            select user;

                var usersAsList = users.ToList();

                Assert.That(users.Count(), Is.EqualTo(3));

                Assert.That(usersAsList[0].Name, Is.EqualTo("Anna"));
                Assert.That(usersAsList[1].Name, Is.EqualTo("Christian"));
                Assert.That(usersAsList[2].Name, Is.EqualTo("Henry"));
            }
        }

        [Test]
        public void Test_where_with_order_by_desc()
        {
            using (var odb = OdbFactory.Open(DbName))
            {
                var users = from user in odb.AsQueryable<User>()
                            where user.Age > 30 && user.Name.Contains("n")
                            orderby user.Name descending 
                            select user;

                var usersAsList = users.ToList();

                Assert.That(users.Count(), Is.EqualTo(3));

                Assert.That(usersAsList[2].Name, Is.EqualTo("Anna"));
                Assert.That(usersAsList[1].Name, Is.EqualTo("Christian"));
                Assert.That(usersAsList[0].Name, Is.EqualTo("Henry"));
            }
        }

        private static void CleanDb()
        {
            OdbFactory.Delete(DbName);
        }

        private static void PrepareTheData()
        {
            var users = new List<User>
                            {
                                FullUser("Magdalena", 24, "Klarty", City.Cracow, 23),
                                FullUser("John", 26, "Old Street", City.London, 223),
                                FullUser("Henry", 51, "Sweet", City.NewYork, 45),
                                FullUser("Julia", 4, "Gwiazdy", City.Cracow, 874),
                                FullUser("Michal", 17, "Skra", City.Sydney, 1),
                                FullUser("Matthew", 38, "New Orlean", City.Barcelona, 2323),
                                FullUser("Steven", 22, "Under Sky", City.Pekin, 63),
                                FullUser("Sarah", 28, "Butterfly", City.Pekin, 43),
                                FullUser("Christian", 41, "New City", City.London, 2),
                                FullUser("Anna", 34, "Golden Street", City.NewYork, 233)
                            };

            using (var odb = OdbFactory.Open(DbName))
            {
                users.ForEach(user => odb.Store(user));
            }
        }

        private static User FullUser(string name, int age, string street, City city, int number)
        {
            var address = new Address(street, city, number);
            return new User(name, age, address);
        }
    }
}
