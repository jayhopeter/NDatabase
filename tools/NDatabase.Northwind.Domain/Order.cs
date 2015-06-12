using System;

namespace NDatabase.Northwind.Domain
{
    public class Order
    {
        public static readonly string PK = "OrderID";

        public long OrderID { get; set; }

        public Customer Customer { get; set; }

        public Employee Employee { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public DateTime ShippedDate { get; set; }

        public Shipper ShipVia { get; set; }

        public double Freight { get; set; }

        public string ShipName { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCity { get; set; }

        public string ShipRegion { get; set; }

        public string ShipPostalCode { get; set; }

        public string ShipCountry { get; set; }
    }
}