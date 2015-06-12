namespace Test.NDatabase.Odb.Test.Cache
{
    public class MyObjectWithMyHashCode2
    {
        private readonly long myLong;

        public MyObjectWithMyHashCode2(long myLong)
        {
            this.myLong = myLong;
        }

        public virtual long GetMyLong()
        {
            return myLong;
        }

        public override int GetHashCode()
        {
            return myLong.GetHashCode();
        }
    }
}
