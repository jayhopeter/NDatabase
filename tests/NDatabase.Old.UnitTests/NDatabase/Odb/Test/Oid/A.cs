namespace Test.NDatabase.Odb.Test.Oid
{
    public class A
    {
        private readonly B b;
        private readonly string name;

        public A(string name, B b)
        {
            this.name = name;
            this.b = b;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual B GetB()
        {
            return b;
        }
    }
}
