using System;
using System.Globalization;
using System.Linq;
using NDatabase.Api;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Query.Values
{
    public class TestGetValuesGroupBy : ODBTest
    {
        private const string DbName = "values2.test1.ndb";
        private const string DbName2 = "values2.test2.ndb";
        private const string DbName3 = "values2.test3.ndb";

        [Test]
        public void Test1()
        {
            DeleteBase(DbName);

            using (var odb = Open(DbName))
            {
                var tc1 = new TestClass();
                tc1.SetInt1(45);
                odb.Store(tc1);

                var tc2 = new TestClass();
                tc2.SetInt1(45);
                odb.Store(tc2);

                var tc3 = new TestClass();
                tc3.SetInt1(46);
                odb.Store(tc3);
            }

            IValues values;
            using (var odb = Open(DbName))
            {
                var vq = odb.ValuesQuery<TestClass>().Sum("int1", "sum of int1").GroupBy("int1");
                vq.Descend("int1").OrderAscending();
                values = vq.Execute();
                AssertEquals(2, values.Count());

                Println(values);
                var ov = values.NextValues();
                AssertEquals(new Decimal(90), ov.GetByAlias("sum of int1"));

                ov = values.NextValues();
                AssertEquals(new Decimal(46), ov.GetByAlias("sum of int1"));
            }
            AssertEquals(2, values.Count());
        }

        [Test]
        public void Test2()
        {
            DeleteBase(DbName2);
            using (var odb = Open(DbName2))
            {
                var tc1 = new TestClass();
                tc1.SetInt1(45);
                odb.Store(tc1);

                var tc2 = new TestClass();
                tc2.SetInt1(45);
                odb.Store(tc2);

                var tc3 = new TestClass();
                tc3.SetInt1(46);
                odb.Store(tc3);
            }

            IValues values;
            using (var odb = Open(DbName2))
            {
                var vq =
                    odb.ValuesQuery<TestClass>().Sum("int1", "sum of int1").Count("count").GroupBy("int1");
                vq.Descend("int1").OrderAscending();
                values = vq.Execute();
                Println(values);

                var ov = values.NextValues();
                AssertEquals(new Decimal(90), ov.GetByAlias("sum of int1"));
                AssertEquals(new Decimal(2), ov.GetByAlias("count"));

                ov = values.NextValues();
                AssertEquals(new Decimal(46), ov.GetByAlias("sum of int1"));
                AssertEquals(new Decimal(1), ov.GetByAlias("count"));
            }

            AssertEquals(2, values.Count());
        }

        /// <summary>
        ///   Retrieving the name of the profile, the number of user for that profile
        ///   and their average login number grouped by the name of the profile
        /// </summary>
        [Test]
        public void Test3()
        {
            DeleteBase(DbName3);
            using (var odb = Open(DbName3))
            {
                var p1 = new Profile("profile1", new VO.Login.Function("f1"));
                var p2 = new Profile("profile2", new VO.Login.Function("f2"));

                User u1 = new User2("user1", "user@ndatabase.net", p1, 1);
                User u2 = new User2("user2", "user@ndatabase.net", p1, 2);
                User u3 = new User2("user3", "user@ndatabase.net", p1, 3);
                User u4 = new User2("user4", "user@ndatabase.net", p2, 4);
                User u5 = new User2("user5", "user@ndatabase.net", p2, 5);

                odb.Store(u1);
                odb.Store(u2);
                odb.Store(u3);
                odb.Store(u4);
                odb.Store(u5);
            }

            IValues values;
            using (var odb = Open(DbName3))
            {
                var q =
                    odb.ValuesQuery<User2>().Field("profile.name").Count("count").Avg("nbLogins", "avg").
                        GroupBy("profile.name");

                q.Descend("name").OrderAscending();
                values = q.Execute();
                Println(values);

                var ov = values.NextValues();
                AssertEquals(2, values.Count());
                AssertEquals("profile1", ov.GetByAlias("profile.name"));
                AssertEquals(Convert.ToDecimal("3"), ov.GetByAlias("count"));
                AssertEquals(Convert.ToDecimal("2.00", CultureInfo.InvariantCulture), ov.GetByAlias("avg"));
            }

            AssertEquals(2, values.Count());
        }
    }
}
