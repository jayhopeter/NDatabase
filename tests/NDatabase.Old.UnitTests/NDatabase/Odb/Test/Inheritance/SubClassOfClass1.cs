namespace Test.NDatabase.Odb.Test.Inheritance
{
    public class SubClassOfClass1 : Class1
    {
        private long l1;

        public SubClassOfClass1(string name, long l1) : base(name)
        {
            this.l1 = l1;
        }

        public override string GetName()
        {
            return base.GetName();
        }

        public virtual long GetL1()
        {
            return l1;
        }

        public virtual void SetL1(long l1)
        {
            this.l1 = l1;
        }
    }
}
