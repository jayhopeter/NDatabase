using System;

namespace Test.NDatabase.Odb.Test.Nativemap
{
    public class NativeLongMap
    {
        internal object[] array;
        internal int secondSize;
        internal int size;

        public NativeLongMap(int initialCapacity)
        {
            array = new object[initialCapacity];
            size = initialCapacity;
            secondSize = size / 10;
        }

        public virtual object Get(long key)
        {
            var tkey = (int) key % size;
            var entries = (Entry[]) array[tkey];
            if (entries == null)
                return null;
            var i = 0;
            while (i < entries.Length)
            {
                if (entries[i] == null)
                    return null;
                if (entries[i].key == key)
                    return entries[i].o;
            }
            return null;
        }

        public virtual void Put(long key, object o)
        {
            var tkey = (int) key % size;
            Entry[] entries = null;
            if (array[tkey] == null)
            {
                entries = new Entry[secondSize];
                entries[0] = new Entry(key, o);
                array[tkey] = entries;
                return;
            }
            var i = 0;
            while (i < entries.Length)
            {
                if (entries[i] == null)
                {
                    entries[i] = new Entry(key, o);
                    return;
                }
                i++;
            }
            throw new Exception("Second array explosion");
        }
    }

    internal class Entry
    {
        public long key;

        public object o;

        public Entry(long key, object o)
        {
            this.key = key;
            this.o = o;
        }
    }
}
