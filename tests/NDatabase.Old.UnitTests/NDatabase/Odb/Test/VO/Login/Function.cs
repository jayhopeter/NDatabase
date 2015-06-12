namespace Test.NDatabase.Odb.Test.VO.Login
{
    
    public class Function
    {
        private string name;

        public Function()
        {
        }

        public Function(string name)
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
