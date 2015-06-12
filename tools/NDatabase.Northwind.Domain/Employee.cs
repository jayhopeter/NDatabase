using System;

namespace NDatabase.Northwind.Domain
{
    public class Employee
    {
        public static readonly string PK = "EmployeeID";

        public long EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TitleOfCourtesy { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        public string Title { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string HomePhone { get; set; }

        public string Extension { get; set; }

        public string Notes { get; set; }

        public Byte[] Photo { get; set; }

        public string PhotoPath { get; set; }

        public Employee ReportsTo { get; set; }
    }
}