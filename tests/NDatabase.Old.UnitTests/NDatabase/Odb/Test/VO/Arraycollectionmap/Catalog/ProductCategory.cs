using System.Collections.Generic;

namespace Test.NDatabase.Odb.Test.VO.Arraycollectionmap.Catalog
{
    /// <author>olivier</author>
    public class ProductCategory
    {
        private string name;

        private IList<Product> products;

        public ProductCategory(string name)
        {
            this.name = name;
            products = new List<Product>();
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual IList<Product> GetProducts()
        {
            return products;
        }

        public virtual void SetProducts(IList<Product> products)
        {
            this.products = products;
        }
    }
}
