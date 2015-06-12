using System;

namespace Test.NDatabase.Odb.Test.Update
{
    public class MyObject
    {
        private DateTime date;
        private string name;
        private int size;

        public MyObject(int size, string name)
        {
            this.size = size;
            this.name = name;
        }

        public override string ToString()
        {
            return "size=" + size + " - name=" + name + " - time=" + (date == null
                                                                          ? "null"
                                                                          : string.Empty + date.Millisecond);
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual int GetSize()
        {
            return size;
        }

        public virtual void SetSize(int size)
        {
            this.size = size;
        }

        public virtual DateTime GetDate()
        {
            return date;
        }

        public virtual void SetDate(DateTime date)
        {
            this.date = date;
        }
    }
}
