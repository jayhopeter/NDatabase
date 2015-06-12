using NUnit.Framework;

namespace NDatabase.UnitTests.Base
{
    public class When_we_store_nullable_types
    {
         private class ClassWithNullableTypes
         {
             private decimal? _price;
             public int? Size { get; set; }
             
             public decimal? Price
             {
                 get { return _price; }
                 set { _price = value; }
             }

             public short? Measure { get; set; }
         }

        [Test]
        public void It_should_store_object_with_all_nullable_fields_set_to_null()
        {
            var classWithNullableTypes = new ClassWithNullableTypes();
            OdbFactory.Delete("nullable1.ndb");

            using (var odb = OdbFactory.Open("nullable1.ndb"))
            {
                odb.Store(classWithNullableTypes);
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<ClassWithNullableTypes>();
                var objectToTest = query.Execute<ClassWithNullableTypes>().GetFirst();

                Assert.That((object) objectToTest.Size, Is.Null);
                Assert.That((object) objectToTest.Price, Is.Null);
                Assert.That((object) objectToTest.Measure, Is.Null);
            }
        }

        [Test]
        public void It_should_store_object_with_all_nullable_fields_with_values()
        {
            var classWithNullableTypes = new ClassWithNullableTypes {Size = 33, Measure = 2, Price = 33.33m};
            OdbFactory.Delete("nullable2.ndb");

            using (var odb = OdbFactory.Open("nullable2.ndb"))
            {
                odb.Store(classWithNullableTypes);
            }

            using (var odb = OdbFactory.OpenLast())
            {
                var query = odb.Query<ClassWithNullableTypes>();
                var objectToTest = query.Execute<ClassWithNullableTypes>().GetFirst();

                Assert.That((object) objectToTest.Size, Is.EqualTo(33));
                Assert.That((object) objectToTest.Price, Is.EqualTo(33.33m));
                Assert.That((object) objectToTest.Measure, Is.EqualTo(2));
            }
        }
    }
}