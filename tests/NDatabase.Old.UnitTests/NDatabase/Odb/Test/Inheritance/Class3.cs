namespace Test.NDatabase.Odb.Test.Inheritance
{
    public class Class3
    {
        private Class1 class1;
        private int nb;

        public Class3(int nb, Class1 class1)
        {
            this.nb = nb;
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

        public virtual int GetNb()
        {
            return nb;
        }

        public virtual void SetNb(int nb)
        {
            this.nb = nb;
        }
    }
}
