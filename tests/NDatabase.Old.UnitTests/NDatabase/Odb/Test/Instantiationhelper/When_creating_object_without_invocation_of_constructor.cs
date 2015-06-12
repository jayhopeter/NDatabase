using System.Runtime.Serialization;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Instantiationhelper
{
    public class When_creating_object_without_invocation_of_constructor
    {
        [Test]
        public virtual void It_should_have_the_default_state()
        {
            var car = (Car) FormatterServices.GetUninitializedObject(typeof (Car));

            Assert.That(car.GetModel(), Is.Null);
            Assert.That(car.GetYear(), Is.EqualTo(0));
        }
    }
}
