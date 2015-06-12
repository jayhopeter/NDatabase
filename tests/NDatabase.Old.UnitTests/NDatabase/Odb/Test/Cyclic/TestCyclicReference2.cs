using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Country;

namespace Test.NDatabase.Odb.Test.Cyclic
{
    [TestFixture]
    public class TestCyclicReference2 : ODBTest
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DeleteBase("cyclic.ndb");
            var odb = Open("cyclic.ndb");
            var brasil = new Country("Brasil");
            for (var i = 0; i < 10; i++)
            {
                var city = new City("city" + i);
                city.SetCountry(brasil);
                brasil.AddCity(city);
            }
            odb.Store(brasil);
            odb.Close();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteBase("cyclic.ndb");
        }

        #endregion

        [Test]
        public virtual void Test1()
        {
            var odb = Open("cyclic.ndb");
            var query = odb.Query<Country>();
            var l = query.Execute<Country>(true);
            var country = l.GetFirst();
            AssertEquals("Brasil", country.GetName());
            odb.Close();
        }

        [Test]
        public virtual void Test2()
        {
            Println("-------------------");
            // LogUtil.logOn(ObjectWriter.LOG_ID, true);
            // LogUtil.logOn(ObjectReader.LOG_ID, true);
            var odb = Open("cyclic.ndb");
            var query = odb.Query<Country>();
            var l = query.Execute<Country>(true);
            var country = l.GetFirst();
            AssertEquals(10, country.GetCities().Count);
            odb.Close();
        }
    }
}
