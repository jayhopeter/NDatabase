namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    public class Class1
    {
        private string name;

        public Class1(string name)
        {
            this.name = name;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
