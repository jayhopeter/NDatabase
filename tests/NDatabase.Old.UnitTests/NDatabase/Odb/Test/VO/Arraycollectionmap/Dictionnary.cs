using NDatabase.Tool.Wrappers;

namespace Test.NDatabase.Odb.Test.VO.Arraycollectionmap
{
    internal class Dictionnary
    {
        private readonly string name;

        private OdbHashMap<object, object> map;

        public Dictionnary() : this("default")
        {
        }

        public Dictionnary(string name)
        {
            this.name = name;
            map = null;
        }

        public virtual void AddEntry(object key, object value)
        {
            if (map == null)
                map = new OdbHashMap<object, object>();
            map.Add(key, value);
        }

        public override string ToString()
        {
            return name + " | " + map;
        }

        public virtual object Get(object key)
        {
            return map[key];
        }

        public virtual void SetMap(OdbHashMap<object, object> map)
        {
            this.map = map;
        }

        public virtual OdbHashMap<object, object> GetMap()
        {
            return map;
        }

        public virtual string GetName()
        {
            return name;
        }
    }
}
