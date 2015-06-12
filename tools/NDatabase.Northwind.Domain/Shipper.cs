namespace NDatabase.Northwind.Domain
{
    public class Shipper
    {
        public static readonly string PK = "ShipperID";

        public long ShipperID { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }
    }
}