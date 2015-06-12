using System;

namespace Test.NDatabase.Odb.Test.Query.Criteria
{
    public class MyDates
    {
        private DateTime date1;

        private DateTime date2;
        private int i;

        public virtual DateTime GetDate1()
        {
            return date1;
        }

        public virtual void SetDate1(DateTime date1)
        {
            this.date1 = date1;
        }

        public virtual DateTime GetDate2()
        {
            return date2;
        }

        public virtual void SetDate2(DateTime date2)
        {
            this.date2 = date2;
        }

        public virtual int GetI()
        {
            return i;
        }

        public virtual void SetI(int i)
        {
            this.i = i;
        }
    }
}
