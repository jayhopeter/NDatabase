using System;

namespace Test.NDatabase.Odb.Test.List
{
    public class MyLong : IComparable
    {
        private readonly long value;

        public MyLong(long value)
        {
            this.value = value;
        }

        #region IComparable Members

        public virtual int CompareTo(object @object)
        {
            if (@object == null || !(@object is MyLong))
                return -10;
            var ml = (MyLong) @object;
            return (int) (value - ml.value);
        }

        #endregion

        public virtual long LongValue()
        {
            return value;
        }
    }
}
