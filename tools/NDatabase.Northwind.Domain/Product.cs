namespace NDatabase.Northwind.Domain
{
    public class Product
    {
        public static readonly string PK = "ProductID";

        public long ProductID { get; set; }

        public string ProductName { get; set; }

        public Supplier Supplier { get; set; }

        public Category Category { get; set; }

        public string QuantityPerUnit { get; set; }

        public double UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

        public int UnitsOnOrder { get; set; }

        public int ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}