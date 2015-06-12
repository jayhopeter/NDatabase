namespace Test.NDatabase.Odb.Test.Inheritance
{
    public class Class1 : IInterface
    {
        private readonly string name;

        public Class1(string name)
        {
            this.name = name;
        }

        #region IInterface Members

        public virtual string GetName()
        {
            return name;
        }

        #endregion
    }
}
