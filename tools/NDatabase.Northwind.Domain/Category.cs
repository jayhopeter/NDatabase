using System;

namespace NDatabase.Northwind.Domain
{
    public class Category
    {
        public static readonly string PK = "CategoryID";

        public long CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public Byte[] Picture { get; set; }
    }
}