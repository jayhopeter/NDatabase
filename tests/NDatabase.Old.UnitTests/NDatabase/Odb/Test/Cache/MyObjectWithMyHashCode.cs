namespace Test.NDatabase.Odb.Test.Cache
{
    public class MyObjectWithMyHashCode
    {
        private long myLong;

        public MyObjectWithMyHashCode(long myLong)
        {
            this.myLong = myLong;
        }

        public virtual long GetMyLong()
        {
            return myLong;
        }

        public virtual void SetMyLong(long myLong)
        {
            this.myLong = myLong;
        }

        public override int GetHashCode()
        {
            return myLong.GetHashCode();
        }
    }
}
