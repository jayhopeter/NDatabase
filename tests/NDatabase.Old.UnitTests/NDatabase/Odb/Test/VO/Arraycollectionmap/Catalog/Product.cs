using System;

namespace Test.NDatabase.Odb.Test.VO.Arraycollectionmap.Catalog
{
    /// <author>olivier</author>
    public class Product
    {
        private string name;

        private Decimal price;

        public Product(string name, Decimal price)
        {
            this.name = name;
            this.price = price;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual Decimal GetPrice()
        {
            return price;
        }

        public virtual void SetPrice(Decimal price)
        {
            this.price = price;
        }
    }
}
