using System;

namespace Test.NDatabase.Odb.Test.Performance
{
    public class SimpleObject
    {
        public static int nbgc = 0;

        private DateTime date;
        private int duration;
        private string name;

        public SimpleObject(string name, int duration, DateTime date)
        {
            this.name = name;
            this.duration = duration;
            this.date = date;
        }

        public SimpleObject()
        {
        }

        public virtual DateTime GetDate()
        {
            return date;
        }

        public virtual void SetDate(DateTime date)
        {
            this.date = date;
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

        ~SimpleObject()
        {
            nbgc++;
        }
    }
}
