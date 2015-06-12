namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    public class ClassWithInt
    {
        private int i;

        private string name;

        public ClassWithInt(int i, string name)
        {
            this.i = i;
            this.name = name;
        }

        public virtual int GetI()
        {
            return i;
        }

        public virtual void SetI(int i)
        {
            this.i = i;
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
