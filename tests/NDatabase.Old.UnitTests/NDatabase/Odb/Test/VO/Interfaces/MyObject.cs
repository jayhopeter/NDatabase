namespace Test.NDatabase.Odb.Test.VO.Interfaces
{
    /// <author>olivier</author>
    
    public class MyObject
    {
        private string name;

        public MyObject(string name)
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
    }
}
