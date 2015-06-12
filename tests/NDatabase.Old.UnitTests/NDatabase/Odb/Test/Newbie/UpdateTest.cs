using NUnit.Framework;
using Test.NDatabase.Odb.Test.Newbie.VO;

namespace Test.NDatabase.Odb.Test.Newbie
{
    /// <summary>
    ///   It is just a simple test to help the newbies
    /// </summary>
    public class UpdateTest : ODBTest
    {
        protected static readonly string NewbieOdb = "newbie.ndb";

        [Test]
        public virtual void TestUpdate()
        {
            DeleteBase(NewbieOdb);

            using (var odb = Open(NewbieOdb))
            {
                var marcelo = new Driver("marcelo");
                var car = new Car("car1", 4, "ranger", marcelo);
                odb.Store(car);

                var query = odb.Query<Car>();
                query.Descend("Driver.Name").Constrain((object) "marcelo").Equal();
                var newCar = query.Execute<Car>().GetFirst();

                newCar.Driver = new Driver("dani");
                odb.Store(newCar);
            }

            using (var odb = Open(NewbieOdb))
            {
                var query = odb.Query<Car>();
                query.Descend("Driver.Name").Constrain((object) "dani").Equal();
                AssertEquals(1, query.Execute<Car>().Count);
            }

            DeleteBase(NewbieOdb);
        }
    }
}
