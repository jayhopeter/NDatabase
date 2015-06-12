namespace NDatabase.Northwind.Domain
{
    public class Territory
    {
        public static readonly string PK = "TerritoryID";

        public string TerritoryID { get; set; }

        public string TerritoryDescription { get; set; }

        public Region Region { get; set; }
    }
}