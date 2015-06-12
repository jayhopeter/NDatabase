namespace Test.NDatabase.Odb.Test.Index
{
    public class IndexedObject2
    {
        private string name;

        private IndexedObject @object;

        public IndexedObject2()
        {
        }

        public IndexedObject2(string name, IndexedObject @object)
        {
            this.name = name;
            this.@object = @object;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual IndexedObject GetObject()
        {
            return @object;
        }

        public virtual void SetObject(IndexedObject @object)
        {
            this.@object = @object;
        }
    }
}
