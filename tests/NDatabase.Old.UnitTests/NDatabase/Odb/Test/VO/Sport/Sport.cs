namespace Test.NDatabase.Odb.Test.VO.Sport
{
    public class Sport
    {
        private string name;

        public Sport(string name)
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
