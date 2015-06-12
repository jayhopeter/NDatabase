namespace Test.NDatabase.Odb.Test.Trigger
{
    public class ID
    {
        private long id;
        private string idName;

        public ID(string name, long id)
        {
            idName = name;
            this.id = id;
        }

        public virtual long GetNext()
        {
            id++;
            return id;
        }
    }
}
