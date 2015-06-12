namespace Test.NDatabase.Odb.Test.Cyclic
{
    /// <author>olivier</author>
    public class ClassA
    {
        internal ClassB classb;
        internal string name;

        public ClassA()
        {
        }

        public ClassA(ClassB classb, string name)
        {
            this.classb = classb;
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

        public virtual ClassB GetClassb()
        {
            return classb;
        }

        public virtual void SetClassb(ClassB classb)
        {
            this.classb = classb;
        }
    }
}
