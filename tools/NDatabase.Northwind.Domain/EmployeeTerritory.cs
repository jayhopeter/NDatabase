namespace NDatabase.Northwind.Domain
{
    public class EmployeeTerritory
    {
        public string EmployeeTerritoryID
        {
            get { return Employee.EmployeeID.ToString() + "-" + Territory.TerritoryID; }
        }

        public Employee Employee { get; set; }

        public Territory Territory { get; set; }
    }
}