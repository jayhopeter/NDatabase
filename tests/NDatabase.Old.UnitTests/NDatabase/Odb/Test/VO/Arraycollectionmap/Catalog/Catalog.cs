using System.Collections.Generic;

namespace Test.NDatabase.Odb.Test.VO.Arraycollectionmap.Catalog
{
    /// <author>olivier</author>
    public class Catalog
    {
        private IList<ProductCategory> categories;
        private string name;

        public Catalog(string name)
        {
            this.name = name;
            categories = new List<ProductCategory>();
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual IList<ProductCategory> GetCategories()
        {
            return categories;
        }

        public virtual void SetCategories(IList<ProductCategory> categories)
        {
            this.categories = categories;
        }
    }
}
