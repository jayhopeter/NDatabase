namespace NDatabase.Northwind.Domain
{
    public class CustomerDemographics
    {
        public static readonly string PK = "CustomerTypeID";

        public string CustomerTypeID { get; set; }

        public string CustomerDesc { get; set; }
    }
}