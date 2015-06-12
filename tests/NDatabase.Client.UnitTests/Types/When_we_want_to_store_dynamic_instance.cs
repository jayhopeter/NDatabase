using System;
using System.Numerics;
using NDatabase.Client.UnitTests.Data;
using NUnit.Framework;

namespace NDatabase.Client.UnitTests.Types
{
    public class When_we_want_to_store_dynamic_instance
    {
        [Test]
        public void It_should_be_avialable_to_store_dynamic_instance()
        {
            dynamic val = new User("j", 2, null);

            OdbFactory.Delete("dynamic.ndb");

            using (var odb = OdbFactory.Open("dynamic.ndb"))
            {
                odb.Store(val);
            }

            using (var odb = OdbFactory.Open("dynamic.ndb"))
            {
                var objectSet = odb.Query<User>().Execute<object>();
                var first = objectSet.GetFirst();

                Assert.That(first, Is.EqualTo(val));
            }
        }

        public class TypeWithBigIntegerAndComples
        {
            public BigInteger BigInteger { get; set; }
            public Complex Complex { get; set; }
            public TimeSpan TimeSpan { get; set; }

            public Guid Guid { get; set; }

            protected bool Equals(TypeWithBigIntegerAndComples other)
            {
                return BigInteger.Equals(other.BigInteger) && Complex.Equals(other.Complex) && TimeSpan.Equals(other.TimeSpan) && Guid.Equals(other.Guid);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TypeWithBigIntegerAndComples) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = BigInteger.GetHashCode();
                    hashCode = (hashCode*397) ^ Complex.GetHashCode();
                    hashCode = (hashCode*397) ^ TimeSpan.GetHashCode();
                    hashCode = (hashCode*397) ^ Guid.GetHashCode();
                    return hashCode;
                }
            }
        }

        [Test]
        public void It_should_be_avialable_to_store_BigInteger_instance()
        {
            dynamic val = new TypeWithBigIntegerAndComples
                              {
                                  BigInteger = new BigInteger(123123123123123123),
                                  TimeSpan = TimeSpan.FromMinutes(10),
                                  Complex = new Complex(2.2, 2.1),
                                  Guid = Guid.NewGuid()
                              };

            OdbFactory.Delete("dynamic.ndb");

            using (var odb = OdbFactory.Open("dynamic.ndb"))
            {
                odb.Store(val);
            }

            using (var odb = OdbFactory.Open("dynamic.ndb"))
            {
                var objectSet = odb.Query<TypeWithBigIntegerAndComples>().Execute<object>();
                var first = objectSet.GetFirst();

                Assert.That(first, Is.EqualTo(val));
            }
        }
    }
}