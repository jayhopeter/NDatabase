using System;
using NDatabase.Api;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Attribute;
using Test.NDatabase.Odb.Test.VO.Login;
using Test.NDatabase.Tool;

namespace Test.NDatabase.Odb.Test.Query.Values
{
    public class TestGetValues : ODBTest
    {
        [Test]
        public void Test1()
        {
            DeleteBase("valuesA");

            using (var odb = Open("valuesA"))
            {
                odb.Store(new VO.Login.Function("f1"));
            }

            IObjectValues ov;
            using (var odb = Open("valuesA"))
            {
                var valuesQuery = odb.ValuesQuery<VO.Login.Function>();
                valuesQuery.Field("name");
                var values = valuesQuery.Execute();
                Println(values);
                ov = values.NextValues();
            }

            AssertEquals("f1", ov.GetByAlias("name"));
            AssertEquals("f1", ov.GetByIndex(0));
        }

        [Test]
        public void Test16()
        {
            DeleteBase("valuesA3");
            
            var t = new StopWatch();
            const int size = 100;
            for (var j = 0; j < 10; j++)
            {
                t.Start();
                using (var odb = Open("valuesA3"))
                {
                    for (var i = 0; i < size; i++)
                    {
                        var tc1 = new TestClass();
                        tc1.SetInt1(45);
                        odb.Store(tc1);
                    }
                }
                t.End();
                Println(" time for insert = " + t.GetDurationInMiliseconds());
            }

            IValues values;
            IObjectValues ov;
            using (var odb = Open("valuesA3"))
            {
                t.Start();
                var valuesQuery = odb.ValuesQuery<TestClass>().Count("nb objects");
                values = valuesQuery.Execute();
                t.End();
                Println(values);
                Println(" time for count = " + t.GetDurationInMiliseconds());
                ov = values.NextValues();
            }

            AssertEquals(size * 10, ov.GetByAlias("nb objects"));
            AssertEquals(1, values.Count);
        }

        [Test]
        public void Test17()
        {
            DeleteBase("valuesA4");

            using (var odb = Open("valuesA4"))
            {
                odb.Store(new User("user1", "email1", new Profile("profile name", new VO.Login.Function("f111"))));
            }

            IObjectValues ov;
            using (var odb = Open("valuesA4"))
            {
                var valuesQuery = odb.ValuesQuery<User>().Field("name").Field("profile");
                var values = valuesQuery.Execute();
                Println(values);
                ov = values.NextValues();
            }
            AssertEquals("user1", ov.GetByAlias("name"));
            AssertEquals("user1", ov.GetByIndex(0));
            var p2 = (Profile) ov.GetByAlias("profile");
            AssertEquals("profile name", p2.GetName());
        }

        [Test]
        public void Test2()
        {
            DeleteBase("valuesA21");
            
            using (var odb = Open("valuesA21"))
            {
                odb.Store(new VO.Login.Function("f1"));
            }

            IObjectValues ov;
            using (var odb = Open("valuesA21"))
            {
                var valuesQuery = odb.ValuesQuery<VO.Login.Function>().Field("name", "Alias of the field");
                var values =
                    valuesQuery.Execute();
                Println(values);
                ov = values.NextValues();
            }

            AssertEquals("f1", ov.GetByAlias("Alias of the field"));
            AssertEquals("f1", ov.GetByIndex(0));
        }

        [Test]
        public void Test3()
        {
            DeleteBase("valuesA3");
            
            using (var odb = Open("valuesA3"))
            {
                odb.Store(new User("user1", "email1", new Profile("profile name", new VO.Login.Function("f111"))));
            }

            IObjectValues ov;
            using (var odb = Open("valuesA3"))
            {
                var valuesQuery = odb.ValuesQuery<User>().Field("name").Field("profile.name");
                var values = valuesQuery.Execute();
                Println(values);
                ov = values.NextValues();
            }

            AssertEquals("user1", ov.GetByAlias("name"));
            AssertEquals("user1", ov.GetByIndex(0));
            AssertEquals("profile name", ov.GetByAlias("profile.name"));
            AssertEquals("profile name", ov.GetByIndex(1));
        }

        [Test]
        public void Test4()
        {
            DeleteBase("valuesA4");

            using (var odb = Open("valuesA4"))
            {
                var tc1 = new TestClass();
                tc1.SetInt1(45);
                odb.Store(tc1);

                var tc2 = new TestClass();
                tc2.SetInt1(5);
                odb.Store(tc2);
            }

            IValues values;
            IObjectValues ov;
            using (var odb = Open("valuesA4"))
            {
                var valuesQuery = odb.ValuesQuery<TestClass>().Sum("int1", "sum of int1").Count("nb objects");
                values = valuesQuery.Execute();
                Println(values);
                ov = values.NextValues();
            }

            AssertEquals(50, ov.GetByAlias("sum of int1"));
            AssertEquals(2, ov.GetByAlias("nb objects"));
            AssertEquals(1, values.Count);
        }

        [Test]
        public void Test5()
        {
            DeleteBase("valuesA5");
            const int size = 1000;

            using (var odb = Open("valuesA5"))
            {
                for (var i = 0; i < size; i++)
                {
                    var tc1 = new TestClass();
                    tc1.SetInt1(45);
                    odb.Store(tc1);
                }
            }

            IValues values;
            IObjectValues ov;
            using (var odb = Open("valuesA5"))
            {
                var valuesQuery = odb.ValuesQuery<TestClass>().Count("nb objects");
                values = valuesQuery.Execute();
                Println(values);
                ov = values.NextValues();
            }
            AssertEquals(size, ov.GetByAlias("nb objects"));
            AssertEquals(1, values.Count);
        }

        [Test]
        public void Test6()
        {
            DeleteBase("valuesA6");

            using (var odb = Open("valuesA6"))
            {
                const int size = 1000;
                for (var i = 0; i < size; i++)
                {
                    var tc1 = new TestClass();
                    tc1.SetInt1(i);
                    odb.Store(tc1);
                }
            }

            IValues values;
            IObjectValues ov;
            using (var odb = Open("valuesA6"))
            {
                var valuesCriteriaQuery = odb.ValuesQuery<TestClass>();
                valuesCriteriaQuery.Descend("int1").Constrain(2).Equal();

                var valuesQuery = valuesCriteriaQuery.Count("nb objects");
                values = valuesQuery.Execute();
                Println(values);
                ov = values.NextValues();
            }

            AssertEquals(1, ov.GetByAlias("nb objects"));
            AssertEquals(1, values.Count);
        }

        [Test]
        public void Test7()
        {
            DeleteBase("valuesA7");

            using (var odb = Open("valuesA7"))
            {
                const int size = 1000;
                for (var i = 0; i < size; i++)
                {
                    var tc1 = new TestClass();
                    tc1.SetInt1(i);
                    odb.Store(tc1);
                }
            }

            decimal nb;
            using (var odb = Open("valuesA7"))
            {
                var query = odb.Query<TestClass>();
                query.Descend("int1").Constrain(2).Equal();
                nb = query.Count();
                Println(nb);
            }

            AssertEquals(1, nb);
        }

        [Test]
        public void Test8_max_and_average()
        {
            DeleteBase("valuesA8");

            const int size = 1000;
            long sum;
            using (var odb = Open("valuesA8"))
            {
                sum = 0;
                for (var i = 0; i < size; i++)
                {
                    var tc1 = new TestClass();
                    tc1.SetInt1(i);
                    odb.Store(tc1);
                    sum += i;
                }
            }

            decimal max;
            decimal avg;
            decimal bsum;
            using (var odb = Open("valuesA8"))
            {
                var valuesQuery =
                    odb.ValuesQuery<TestClass>().Max("int1", "max of int1").Avg("int1", "avg of int1").Sum("int1",
                                                                                                           "sum of int1");
                var values = valuesQuery.Execute();
                var ov = values.NextValues();
                max = (Decimal) ov.GetByAlias("max of int1");
                avg = (Decimal) ov.GetByAlias("avg of int1");
                bsum = (Decimal) ov.GetByAlias("sum of int1");

                Println(max);
                Println(avg);
                Println(bsum);
            }
            AssertEquals(new Decimal(sum), bsum);
            AssertEquals(new Decimal(size - 1), max);
            AssertEquals(bsum / size, avg);
        }

        [Test]
        public void Test9_Min()
        {
            DeleteBase("valuesA9");
            long sum;
            using (var odb = Open("valuesA9"))
            {
                const int size = 1000;
                sum = 0;

                for (var i = 0; i < size; i++)
                {
                    var tc1 = new TestClass();
                    tc1.SetInt1(i);
                    odb.Store(tc1);
                    sum += i;
                }
            }

            decimal min;
            decimal avg;
            decimal bsum;
            using (var odb = Open("valuesA9"))
            {
                var valuesQuery =
                    odb.ValuesQuery<TestClass>().Min("int1", "min of int1").Avg("int1", "avg of int1").Sum("int1",
                                                                                                           "sum of int1");
                var values = valuesQuery.Execute();
                var ov = values.NextValues();
                min = (Decimal) ov.GetByAlias("min of int1");
                avg = (Decimal) ov.GetByAlias("avg of int1");
                bsum = (Decimal) ov.GetByAlias("sum of int1");
                Println(min);
                Println(avg);
                Println(bsum);
            }

            AssertEquals(new Decimal(sum), bsum);
            AssertEquals(new Decimal(0), min);
            AssertEquals(bsum / 1000, avg);
        }
    }
}
