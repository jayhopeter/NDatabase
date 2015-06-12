using System.Collections;

namespace Test.NDatabase.Odb.Test.VO.Country
{
    public class Country
    {
        private readonly IList cities;
        private string name;

        private int population;

        public Country()
        {
            cities = new ArrayList();
        }

        public Country(string name) : this()
        {
            this.name = name;
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
            return name;
        }

        public virtual void AddCity(City city)
        {
            cities.Add(city);
        }

        public virtual IList GetCities()
        {
            return cities;
        }

        public virtual int GetPopulation()
        {
            return population;
        }

        public virtual void SetPopulation(int population)
        {
            this.population = population;
        }
    }
}
