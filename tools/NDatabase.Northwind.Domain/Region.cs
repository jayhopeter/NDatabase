namespace NDatabase.Northwind.Domain
{
    public class Region
    {
        public static readonly string PK = "RegionID";

        public long RegionID { get; set; }

        public string RegionDescription { get; set; }
    }
}