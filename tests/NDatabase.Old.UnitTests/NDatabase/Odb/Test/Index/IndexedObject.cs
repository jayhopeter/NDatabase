using System;

namespace Test.NDatabase.Odb.Test.Index
{
    
    public class IndexedObject : IComparable
    {
        private DateTime creation;
        private int duration;
        private string name;

        public IndexedObject()
        {
        }

        public IndexedObject(string name, int duration, DateTime creation)
        {
            this.name = name;
            this.duration = duration;
            this.creation = creation;
        }

        #region IComparable Members

        public virtual int CompareTo(object @object)
        {
            if (@object == null || !(@object is IndexedObject))
                return -1000;
            var io = (IndexedObject) @object;
            return name.CompareTo(io.name);
        }

        #endregion

        public virtual DateTime GetCreation()
        {
            return creation;
        }

        public virtual void SetCreation(DateTime creation)
        {
            this.creation = creation;
        }

        public virtual int GetDuration()
        {
            return duration;
        }

        public virtual void SetDuration(int duration)
        {
            this.duration = duration;
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
