namespace Test.NDatabase.Odb.Test.VO.Country
{
    public class City
    {
        private Country country;
        private string name;

        public City()
        {
        }

        public City(string name)
        {
            this.name = name;
        }

        public virtual Country GetCountry()
        {
            return country;
        }

        public virtual void SetCountry(Country country)
        {
            this.country = country;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name + " - Country = " + (country != null
                                                 ? country.GetName()
                                                 : "null");
        }
    }
}
