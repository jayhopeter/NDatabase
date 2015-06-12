namespace Test.NDatabase.Odb.Test.VO.Country
{
    public class Country2 : Country
    {
        private City capital;

        public Country2()
        {
        }

        public Country2(string name) : base(name)
        {
        }

        public virtual City GetCapital()
        {
            return capital;
        }

        public virtual void SetCapital(City capital)
        {
            this.capital = capital;
            AddCity(capital);
        }

        public override string ToString()
        {
            return base.ToString() + " - Capital = " + (capital != null
                                                            ? capital.GetName()
                                                            : "null");
        }
    }
}
