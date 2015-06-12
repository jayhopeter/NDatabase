namespace Test.NDatabase.Odb.Test.Other
{
    public class Person1
    {
        private string address;

        private int age;
        private string name;

        public Person1(string address, string name, int age)
        {
            // TODO Auto-generated constructor stub
            this.address = address;
            this.name = name;
            this.age = age;
        }

        public virtual string GetAddress()
        {
            return address;
        }

        public virtual void SetAddress(string address)
        {
            this.address = address;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }
    }
}
