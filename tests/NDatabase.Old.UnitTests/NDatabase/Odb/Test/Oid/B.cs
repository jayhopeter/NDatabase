namespace Test.NDatabase.Odb.Test.Oid
{
    public class B
    {
        private readonly string name;

        public B(string name)
        {
            this.name = name;
        }

        public virtual string GetName()
        {
            return name;
        }
    }
}
