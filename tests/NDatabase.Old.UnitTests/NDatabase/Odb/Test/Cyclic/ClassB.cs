namespace Test.NDatabase.Odb.Test.Cyclic
{
    /// <author>olivier</author>
    public class ClassB
    {
        internal ClassA classA;
        internal string name;

        public ClassB()
        {
        }

        public ClassB(ClassA classA, string name)
        {
            this.classA = classA;
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

        public virtual ClassA GetClassA()
        {
            return classA;
        }

        public virtual void SetClassA(ClassA classA)
        {
            this.classA = classA;
        }
    }
}
