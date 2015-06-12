namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    public class Class2
    {
        private Class1 class1;

        public Class2()
        {
        }

        public Class2(Class1 class1)
        {
            // TODO Auto-generated constructor stub
            this.class1 = class1;
        }

        public virtual Class1 GetClass1()
        {
            return class1;
        }

        public virtual void SetClass1(Class1 class1)
        {
            this.class1 = class1;
        }
    }
}
