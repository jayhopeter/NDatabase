namespace Test.NDatabase.Odb.Test.Performance
{
    public class Class1
    {
        private string ccc;
        private int ww;

        public Class1(int ww, string ccc)
        {
            this.ww = ww;
            this.ccc = ccc;
        }

        public virtual int GetWw()
        {
            return ww;
        }

        public virtual void SetWw(int ww)
        {
            this.ww = ww;
        }

        public virtual string GetCcc()
        {
            return ccc;
        }

        public virtual void SetCcc(string ccc)
        {
            this.ccc = ccc;
        }
    }
}
