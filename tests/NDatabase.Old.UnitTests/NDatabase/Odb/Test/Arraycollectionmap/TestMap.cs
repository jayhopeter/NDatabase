using NDatabase.Api.Query;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Arraycollectionmap;

namespace Test.NDatabase.Odb.Test.Arraycollectionmap
{
    [TestFixture]
    public class TestMap : ODBTest
    {
        private const string DbName = "map.ndb";

        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            DeleteBase(DbName);
            var odb = Open(DbName);
            var dictionnary1 = new Dictionnary("test1");
            dictionnary1.AddEntry("olivier", "Smadja");
            dictionnary1.AddEntry("kiko", "vidal");
            dictionnary1.AddEntry("karine", "galvao");

            var dictionnary2 = new Dictionnary("test2");
            dictionnary2.AddEntry("f1", new VO.Login.Function("function1"));
            dictionnary2.AddEntry("f2", new VO.Login.Function("function2"));
            dictionnary2.AddEntry("f3", new VO.Login.Function("function3"));
            dictionnary2.AddEntry(dictionnary1, new VO.Login.Function("function4"));
            dictionnary2.AddEntry("f4", null);

            odb.Store(dictionnary1);
            odb.Store(dictionnary2);

            odb.Store(new VO.Login.Function("login"));
            odb.Close();
        }

        #endregion

        public override void TearDown()
        {
            DeleteBase(DbName);
        }

        [Test]
        public virtual void Test1()
        {
            var odb = Open(DbName);
            var query = odb.Query<Dictionnary>();
            var l = query.Execute<Dictionnary>(true);
            // assertEquals(2,l.size());
            var dictionnary = l.GetFirst();
            AssertEquals("Smadja", dictionnary.Get("olivier"));
            odb.Close();
        }

        [Test]
        public virtual void Test2()
        {
            var odb = Open(DbName);
            var query = odb.Query<Dictionnary>();
            var l = query.Execute<Dictionnary>();
            var aq = odb.Query<Dictionnary>();
            aq.Descend("name").Constrain((object) "test2").Equal();
            l = aq.Execute<Dictionnary>();
            var dictionnary = l.GetFirst();
            AssertEquals("function2", ((VO.Login.Function) dictionnary.Get("f2")).GetName());
            odb.Close();
        }

        [Test]
        public virtual void Test3()
        {
            var odb = Open(DbName);
            var size = odb.Query<Dictionnary>().Count();
            var dictionnary1 = new Dictionnary("test1");
            dictionnary1.SetMap(null);
            odb.Store(dictionnary1);
            odb.Close();
            odb = Open(DbName);
            var query = odb.Query<Dictionnary>();
            AssertEquals(size + 1, query.Execute<Dictionnary>().Count);
            AssertEquals(size + 1, odb.Query<Dictionnary>().Count());
            odb.Close();
        }

        [Test]
        public virtual void Test4()
        {
            var odb = Open(DbName);
            var n = odb.Query<Dictionnary>().Count();
            IQuery query = odb.Query<Dictionnary>();
            query.Descend("name").Constrain((object) "test2").Equal();
            var l = query.Execute<Dictionnary>();
            var dictionnary = l.GetFirst();
            dictionnary.SetMap(null);
            odb.Store(dictionnary);
            odb.Close();

            odb = Open(DbName);
            query = odb.Query<Dictionnary>();
            query.Descend("name").Constrain((object) "test2").Equal();
            AssertEquals(n, odb.Query<Dictionnary>().Count());
            var dic = query.Execute<Dictionnary>().GetFirst();
            AssertEquals(null, dic.GetMap());
            odb.Close();
        }

        [Test]
        public virtual void Test5updateIncreasingSize()
        {
            var n = 0L;
            using (var odb = Open(DbName))
            {
                n = odb.Query<Dictionnary>().Count();
                var query = odb.Query<Dictionnary>();
                query.Descend("name").Constrain((object) "test2").Equal();
                var l = query.Execute<Dictionnary>();
                var dictionnary = l.GetFirst();
                dictionnary.SetMap(null);
                odb.Store(dictionnary);
            }

            using (var odb = Open(DbName))
            {
                AssertEquals(n, odb.Query<Dictionnary>().Count());
                var query = odb.Query<Dictionnary>();
                query.Descend("name").Constrain((object) "test2").Equal();
                var dic = query.Execute<Dictionnary>().GetFirst();
                AssertNull(dic.GetMap());
            }

            using (var odb = Open(DbName))
            {
                var query = odb.Query<Dictionnary>();
                query.Descend("name").Constrain((object) "test2").Equal();
                
                var dic = query.Execute<Dictionnary>().GetFirst();
                dic.AddEntry("olivier", "Smadja");
                odb.Store(dic);
            }
            
            using (var odb = Open(DbName))
            {
                var query = odb.Query<Dictionnary>();
                query.Descend("name").Constrain((object) "test2").Equal();
                var dic = query.Execute<Dictionnary>().GetFirst();
                AssertNotNull(dic.GetMap());
                AssertEquals("Smadja", dic.GetMap()["olivier"]);
            }
        }

        [Test]
        public virtual void Test6updateChangingKeyValue()
        {
            // to monitor updates
            var odb = Open(DbName);
            var n = odb.Query<Dictionnary>().Count();
            IQuery query = odb.Query<Dictionnary>();
            query.Descend("name").Constrain((object) "test2").Equal();
            var l = query.Execute<Dictionnary>();
            var dictionnary = l.GetFirst();
            dictionnary.GetMap()["f1"] = "changed function";
            odb.Store(dictionnary);
            odb.Close();

            odb = Open(DbName);
            AssertEquals(n, odb.Query<Dictionnary>().Count());
            query = odb.Query<Dictionnary>();
            query.Descend("name").Constrain((object) "test2").Equal();
            var dic = query.Execute<Dictionnary>().GetFirst();
            AssertEquals("changed function", dic.GetMap()["f1"]);
            odb.Close();
        }

        [Test]
        public virtual void Test6updateDecreasingSize()
        {
            var odb = Open(DbName);
            var n = odb.Query<Dictionnary>().Count();
            IQuery query = odb.Query<Dictionnary>();
            query.Descend("name").Constrain((object) "test2").Equal();
            var l = query.Execute<Dictionnary>();
            var dictionnary = l.GetFirst();
            var mapSize = dictionnary.GetMap().Count;
            dictionnary.GetMap().Remove("f1");
            odb.Store(dictionnary);
            odb.Close();

            odb = Open(DbName);
            AssertEquals(n, odb.Query<Dictionnary>().Count());
            query = odb.Query<Dictionnary>();
            query.Descend("name").Constrain((object) "test2").Equal();
            var dic = query.Execute<Dictionnary>().GetFirst();
            AssertEquals(mapSize - 1, dic.GetMap().Count);
            odb.Close();
        }
    }
}
