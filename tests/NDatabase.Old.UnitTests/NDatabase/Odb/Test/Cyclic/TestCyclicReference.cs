using NDatabase;
using NDatabase.Api;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Country;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Cyclic
{
    [TestFixture]
    public class TestCyclicReference : ODBTest
    {
        #region Setup/Teardown

        [TearDown]
        public override void TearDown()
        {
            DeleteBase("cyclic.ndb");
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DeleteBase("cyclic.ndb");
            var odb = Open("cyclic.ndb");
            for (var i = 0; i < 1; i++)
            {
                var brasilia = new City("Brasilia" + i);
                var brasil = new Country2("Brasil" + i);
                brasilia.SetCountry(brasil);
                brasil.SetCapital(brasilia);
                brasil.SetPopulation(450000);
                odb.Store(brasil);
            }
            odb.Store(new User("name", "email", new Profile("profile")));
            odb.Close();
        }

        #endregion

        [Test]
        public virtual void Test1()
        {
            var odb = Open("cyclic.ndb");
            var query = odb.Query<Country2>();
            var l = query.Execute<Country2>(true);
            var country = l.GetFirst();
            AssertEquals("Brasil0", country.GetName());
            AssertEquals("Brasilia0", country.GetCapital().GetName());
            odb.Close();
        }

        [Test]
        public virtual void Test10()
        {
            IOdb odb = null;
            try
            {
                var baseName = GetBaseName();
                DeleteBase(baseName);
                odb = Open(baseName);
                var ca = new ClassA();
                var cb = new ClassB(ca, "b");
                ca.SetClassb(cb);
                ca.SetName("a");
                odb.Store(ca);
                var ci = ((global::NDatabase.Odb)odb).GetStorageEngine().GetSession().GetMetaModel().GetClassInfo(typeof(ClassA), true);
                AssertTrue(ci.HasCyclicReference());
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
        }

        [Test]
        public virtual void Test11()
        {
            IOdb odb = null;
            try
            {
                odb = Open("cyclic.ndb");
                var ci = ((global::NDatabase.Odb)odb).GetStorageEngine().GetSession().GetMetaModel().GetClassInfo(typeof(User), true);
                AssertFalse(ci.HasCyclicReference());
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
        }

        [Test]
        public virtual void Test15()
        {
            Println("-------------------");
            // LogUtil.logOn(ObjectWriter.LOG_ID, true);
            // LogUtil.logOn(ObjectReader.LOG_ID, true);
            var odb = Open("cyclic.ndb");
            var query1 = odb.Query<Country2>();
            var l = query1.Execute<Country2>(true);
            var country = l.GetFirst();
            var city = country.GetCapital();
            city.SetName("rio de janeiro");
            country.SetCapital(city);
            odb.Store(country);
            odb.Close();
            odb = Open("cyclic.ndb");
            var query2 = odb.Query<Country2>();
            l = query2.Execute<Country2>(true);
            country = l.GetFirst();
            AssertEquals("rio de janeiro", country.GetCapital().GetName());
            var query = odb.Query<City>();
            query.Descend("name").Constrain((object) "rio de janeiro").Equal();
            var cities = query.Execute<City>();
            AssertEquals(1, cities.Count);
            var cities2 = odb.Query<City>().Execute<City>();
            AssertEquals(1, cities2.Count);
            odb.Close();
        }

        [Test]
        public virtual void Test2()
        {
            var odb = Open("cyclic.ndb");
            var query1 = odb.Query<Country2>();
            var l = query1.Execute<Country2>(true);
            var country = l.GetFirst();
            var city = new City("rio de janeiro");
            country.SetCapital(city);
            odb.Store(country);
            odb.Close();
            odb = Open("cyclic.ndb");
            var query2 = odb.Query<Country2>();
            l = query2.Execute<Country2>(true);
            country = l.GetFirst();
            AssertEquals("rio de janeiro", country.GetCapital().GetName());
            var query = odb.Query<City>();
            query.Descend("name").Constrain((object) "rio de janeiro").Equal();
            var cities = query.Execute<City>();
            AssertEquals(1, cities.Count);

            var cities2 = odb.Query<City>().Execute<City>();
            AssertEquals(2, cities2.Count);
            odb.Close();
        }

        [Test]
        public virtual void TestUniqueInstance1()
        {
            var odb = Open("cyclic.ndb");
            var query = odb.Query<City>();
            var cities = query.Execute<City>(true);
            var query1 = odb.Query<Country2>();
            var countries = query1.Execute<Country2>(true);
            var country = countries.GetFirst();
            var city = cities.GetFirst();
            AssertTrue(country == city.GetCountry());
            AssertTrue(city == country.GetCities()[0]);
            AssertTrue(city == country.GetCapital());
            odb.Close();
        }

        [Test]
        public virtual void TestUniqueInstance2()
        {
            var odb = Open("cyclic.ndb");
            var query1 = odb.Query<Country2>();
            var countries = query1.Execute<Country2>(true);
            var query = odb.Query<City>();
            var cities = query.Execute<City>(true);
            var country = countries.GetFirst();
            var city = cities.GetFirst();
            AssertTrue(country == city.GetCountry());
            AssertTrue(city == country.GetCities()[0]);
            AssertTrue(city == country.GetCapital());
            odb.Close();
        }
    }
}
