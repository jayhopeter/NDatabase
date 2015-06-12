using System.Collections;

namespace Test.NDatabase.Odb.Test.Arraycollectionmap
{
    internal class ClassWithNonGenericMap
    {
        private readonly IDictionary map;
        private readonly string name;

        public ClassWithNonGenericMap(string name)
        {
            this.name = name;
            map = new Hashtable();
        }

        public void Add(object key, object value)
        {
            map.Add(key, value);
        }

        public object Get(object key)
        {
            return map[key];
        }

        public int Size()
        {
            return map.Count;
        }

        public string GetName()
        {
            return name;
        }
    }
}
