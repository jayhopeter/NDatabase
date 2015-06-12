using System;

namespace Test.NDatabase.Odb.Test.List
{
    public class MyInt : IComparable
    {
        private readonly int value;

        public MyInt(int value)
        {
            this.value = value;
        }

        #region IComparable Members

        public virtual int CompareTo(object @object)
        {
            if (@object == null || !(@object is MyInt))
                return -10;
            var ml = (MyInt) @object;
            return (value - ml.value);
        }

        #endregion

        public virtual int IntValue()
        {
            return value;
        }
    }
}
