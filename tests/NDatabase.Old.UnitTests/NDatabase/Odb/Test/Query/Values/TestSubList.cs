using System;
using System.Collections;
using System.Collections.Generic;
using NDatabase;
using NDatabase.Api;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Query.Values
{
    public class TestSubList : ODBTest
    {
        private const string DbName = "valuesSubList.ndb";
        private const string DbName2 = "valuesSubList2.ndb";
        private const string DbName3 = "valuesSubList3.ndb";
        private const string SublistDbName = "sublist4.ndb";

        private static Parameter AsParameter(object nonNativeObjectInfo)
        {
            return (Parameter) nonNativeObjectInfo;
        }

        [Test]
        public void Test1()
        {
            DeleteBase(DbName);

            using (var odb = Open(DbName))
            {
                var handler = new Handler();
                
                for (var i = 0; i < 10; i++)
                    handler.AddParameter(new Parameter("test " + i, "value " + i));
                
                odb.Store(handler);
            }

            using (var odb = Open(DbName))
            {
                var valuesQuery =
                    odb.ValuesQuery<Handler>().Field("parameters").Sublist("parameters", "sub1", 1, 5, true).Sublist(
                        "parameters", "sub2", 1, 10).Size("parameters", "size");

                var values = valuesQuery.Execute();
                Println(values);

                var ov = values.NextValues();
                var fulllist = (IList) ov.GetByAlias("parameters");
                AssertEquals(10, fulllist.Count);

                var size = (long) ov.GetByAlias("size");
                AssertEquals(10, size);

                var p = AsParameter(fulllist[0]);
                AssertEquals("value 0", p.GetValue());

                var p2 = AsParameter(fulllist[9]);
                AssertEquals("value 9", p2.GetValue());

                var sublist = (IList) ov.GetByAlias("sub1");
                AssertEquals(5, sublist.Count);

                p = AsParameter(sublist[0]);
                AssertEquals("value 1", p.GetValue());

                p2 = AsParameter(sublist[4]);
                AssertEquals("value 5", p2.GetValue());

                var sublist2 = (IList) ov.GetByAlias("sub2");
                AssertEquals(9, sublist2.Count);
            }
        }

        [Test]
        public void Test11()
        {
            using (var odb = OdbFactory.Open(DbName))
            {
                var handler = new Handler();
                for (var i = 0; i < 10; i++)
                    handler.AddParameter(new Parameter("test " + i, "value " + i));
                odb.Store(handler);
            }

            using (var odb = Open(DbName))
            {
                var valuesQuery =
                    odb.ValuesQuery<Handler>().Field("parameters").Sublist("parameters", "sub1", 1, 5, true).Sublist(
                        "parameters", "sub2", 1, 10).Size("parameters", "size");

                var values = valuesQuery.Execute();
                var ov = values.NextValues();

                // Retrieve Result values
                var fulllist = (IList) ov.GetByAlias("parameters");

                Assert.That(fulllist, Has.Count.EqualTo(10));

                var size = (long) ov.GetByAlias("size");
                Assert.That(size, Is.EqualTo(10));

                var sublist = (IList) ov.GetByAlias("sub1");
                Assert.That(sublist, Has.Count.EqualTo(5));
            }
        }

        [Test]
        public void Test2()
        {
            DeleteBase(DbName2);

            OID oid;
            using (var odb = Open(DbName2))
            {
                var handler = new Handler();

                for (var i = 0; i < 500; i++)
                    handler.AddParameter(new Parameter("test " + i, "value " + i));

                oid = odb.Store(handler);
            }

            using (var odb = Open(DbName2))
            {
                var h = (Handler) odb.GetObjectFromId(oid);
                Println("size of list = " + h.GetListOfParameters().Count);

                var start = OdbTime.GetCurrentTimeInMs();
                var valuesQuery = odb.ValuesQuery<Handler>().Sublist("parameters", "sub", 490, 5, true).Size("parameters", "size");
                var values = valuesQuery.Execute();
                var end = OdbTime.GetCurrentTimeInMs();
                Println("time to load sublist of 5 itens from 40000 : " + (end - start));
                Println(values);

                var ov = values.NextValues();
                var sublist = (IList) ov.GetByAlias("sub");
                AssertEquals(5, sublist.Count);

                var size = (long) ov.GetByAlias("size");
                AssertEquals(500, size);

                var p = AsParameter(sublist[0]);
                AssertEquals("value 490", p.GetValue());

                var p2 = AsParameter(sublist[4]);
                AssertEquals("value 494", p2.GetValue());
            }
        }

        [Test]
        public void Test3_Using_Object_representation_instead_of_real_object()
        {
            const int sublistSize = 10000;
            DeleteBase(DbName3);

            using (var odb = Open(DbName3))
            {
                var handler = new Handler();

                for (var i = 0; i < sublistSize; i++)
                    handler.AddParameter(new Parameter("test " + i, "value " + i));

                odb.Store(handler);
            }

            using (var odb = Open(DbName3))
            {
                var start = OdbTime.GetCurrentTimeInMs();
                var q = odb.ValuesQuery<Handler>().Sublist("parameters", "sub", 9990, 5, true);
                q.SetReturnInstance(false);
                var values = q.Execute();
                var end = OdbTime.GetCurrentTimeInMs();
                Println("time to load sublist of 5 itens from 40000 : " + (end - start));
                Println(values);

                var ov = values.NextValues();
                var sublist = (IList) ov.GetByAlias("sub");
                AssertEquals(5, sublist.Count);

                var parameter = (Parameter) sublist[0];
                AssertEquals("value 9990", parameter.GetValue());

                var parameter1 = (Parameter)sublist[4];
                AssertEquals("value 9994", parameter1.GetValue());
            }
        }

        [Test]
        public void Test4()
        {
            DeleteBase(SublistDbName);

            int i;
            using (var odb = Open(SublistDbName))
            {
                IList<VO.Login.Function> functions1 = new List<VO.Login.Function>();

                for (i = 0; i < 30; i++)
                    functions1.Add(new VO.Login.Function("f1-" + i));

                IList<VO.Login.Function> functions2 = new List<VO.Login.Function>();

                for (i = 0; i < 60; i++)
                    functions2.Add(new VO.Login.Function("f2-" + i));

                IList<VO.Login.Function> functions3 = new List<VO.Login.Function>();

                for (i = 0; i < 90; i++)
                    functions3.Add(new VO.Login.Function("f3-" + i));

                var user1 = new User("User1", "user1@neodtis.org", new Profile("profile1", functions1));
                var user2 = new User("User2", "user1@neodtis.org", new Profile("profile2", functions2));
                var user3 = new User("User3", "user1@neodtis.org", new Profile("profile3", functions3));

                odb.Store(user1);
                odb.Store(user2);
                odb.Store(user3);
            }

            using (var odb = Open(SublistDbName))
            {
                var query = odb.Query<User>();
                var u = query.Execute<User>().GetFirst();
                Console.WriteLine(u);

                var q =
                    odb.ValuesQuery<Profile>().Field("name").Sublist("functions", 1, 2, false).Size(
                        "functions", "fsize");

                var v = q.Execute();

                i = 0;
                while (v.HasNext())
                {
                    var ov = v.NextValues();
                    var profileName = (string) ov.GetByAlias("name");
                    Println(profileName);

                    AssertEquals("profile" + (i + 1), profileName);
                    AssertEquals(Convert.ToInt64(30 * (i + 1)), ov.GetByAlias("fsize"));

                    var l = (IList) ov.GetByAlias("functions");
                    Println(l);
                    AssertEquals(2, l.Count);
                    i++;
                }
            }
        }

        [Test]
        public void Test5_Using_Object_representation_instead_of_real_object()
        {
            const int sublistSize = 400;

            var baseName = GetBaseName();
            DeleteBase(baseName);

            using (var odb = Open(baseName))
            {
                var handler = new Handler();
                for (var i = 0; i < sublistSize; i++)
                    handler.AddParameter(new Parameter("test " + i, "value " + i));
                odb.Store(handler);
            }

            using (var odb = Open(DbName3))
            {
                var start = OdbTime.GetCurrentTimeInMs();
                var q = odb.ValuesQuery<Handler>().Sublist("parameters", "sub", 0, 2, true);
                var values = q.Execute();
                var end = OdbTime.GetCurrentTimeInMs();
                Println("time to load sublist of 5 itens for " + sublistSize + " : " + (end - start));
                Println(values);
                var ov = values.NextValues();
                var sublist = (IList) ov.GetByAlias("sub");
                AssertEquals(2, sublist.Count);

                var parameter = AsParameter(sublist[1]);
                AssertEquals("value 1", parameter.GetValue());
                var oid = odb.GetObjectId(parameter);
                Println(oid);
            }
        }

        [Test]
        public void Test6_Check_if_objects_of_list_are_known_by_ODB()
        {
            const int sublistSize = 400;

            var baseName = GetBaseName();
            DeleteBase(baseName);
            
            using (var odb = Open(baseName))
            {
                var handler = new Handler();

                for (var i = 0; i < sublistSize; i++)
                    handler.AddParameter(new Parameter("test " + i, "value " + i));

                odb.Store(handler);
            }

            using (var odb = Open(DbName3))
            {
                var start = OdbTime.GetCurrentTimeInMs();
                var q = odb.Query<Handler>();
                var objects = q.Execute<Handler>();
                var end = OdbTime.GetCurrentTimeInMs();

                Console.WriteLine("Query time: {0} ms", end - start);

                var h = objects.GetFirst();
                var parameter = (Parameter) h.GetListOfParameters()[0];
                AssertEquals("value 0", parameter.GetValue());

                var oid = odb.GetObjectId(parameter);
                AssertNotNull(oid);
            }
        }
    }
}
