using NDatabase.Api;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Instantiationhelper
{
    /// <summary>
    ///   Test if the ODB retrieves objects without default constructor and null
    ///   arguments
    /// </summary>
    [TestFixture]
    public class TestInstanceHelper : ODBTest
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DeleteBase(Testdbname);
            odb = Open(Testdbname);
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteBase(Testdbname);
        }

        #endregion

        private IOdb odb;

        private static readonly string Testdbname = "instanceHelper.ndb";

        private void CheckCarRetrieval()
        {
            var query1 = odb.Query<Car>();
            var cars = query1.Execute<Car>();
            AssertEquals(1, cars.Count);
            var car = cars.GetFirst();
            AssertEquals(car.GetModel(), "Ranger");
            AssertEquals(car.GetYear(), 2006);
            var query = odb.Query<Car>();
            query.Descend("model").Constrain((object) "Ranger").Equal();
            cars = query.Execute<Car>();
            car = cars.GetFirst();
            AssertEquals(car.GetModel(), "Ranger");
        }

        private void CloseAndReopenDb()
        {
            odb.Close();
            odb = Open(Testdbname);
        }

        /// <summary>
        ///   Create, store and try retrieve the object without default constructor
        /// </summary>
        [Test]
        public virtual void TestWithoutHelperUsingNoConstructor()
        {
            // create a db and store a object that has not default constructor
            var car = new Car("Ranger", 2006);
            odb.Store(car);
            CloseAndReopenDb();
            CheckCarRetrieval();
            odb.Close();
        }
    }
}
