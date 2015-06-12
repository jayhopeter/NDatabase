namespace NDatabase.Client.UnitTests.Data
{
    public class Address
    {
        public Address(string street, City city, int number)
        {
            Street = street;
            City = city;
            Number = number;
        }

        public string Street { get; private set; }
        public City City { get; private set; }
        public int Number { get; private set; }

        protected bool Equals(Address other)
        {
            return string.Equals(Street, other.Street) && City.Equals(other.City) && Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ City.GetHashCode();
                hashCode = (hashCode*397) ^ Number;
                return hashCode;
            }
        }
    }
}