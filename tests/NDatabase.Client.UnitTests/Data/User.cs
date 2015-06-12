namespace NDatabase.Client.UnitTests.Data
{
    public class User
    {
        public User(string name, int age, Address address)
        {
            Name = name;
            Age = age;
            Address = address;
        }

        public string Name { get; private set; }
        public int Age { get; private set; }

        public Address Address { get; private set; }

        protected bool Equals(User other)
        {
            return string.Equals(Name, other.Name) && Age == other.Age && Equals(Address, other.Address);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Age;
                hashCode = (hashCode*397) ^ (Address != null ? Address.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}