namespace Test.NDatabase.Odb.Test.Inheritance
{
    public class Class2
    {
        private IInterface interface1;
        private int nb;

        public Class2(int nb, IInterface interface1)
        {
            this.nb = nb;
            this.interface1 = interface1;
        }

        public virtual IInterface GetInterface1()
        {
            return interface1;
        }

        public virtual void SetInterface1(IInterface interface1)
        {
            this.interface1 = interface1;
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
