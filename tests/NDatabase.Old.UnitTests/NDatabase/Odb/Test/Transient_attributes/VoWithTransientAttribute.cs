using System.Collections.Generic;
using NDatabase.Api;

namespace Test.NDatabase.Odb.Test.Transient_attributes
{
    public class VoWithTransientAttribute
    {
        [NonPersistent]
        private IList<string> keys;

        private string name;

        public VoWithTransientAttribute(string name)
        {
            this.name = name;
        }

        public virtual void AddKey(string key)
        {
            if (keys == null)
                keys = new List<string>();
            keys.Add(key);
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual IList<string> GetKeys()
        {
            return keys;
        }

        public virtual void SetKeys(IList<string> keys)
        {
            this.keys = keys;
        }
    }
}
