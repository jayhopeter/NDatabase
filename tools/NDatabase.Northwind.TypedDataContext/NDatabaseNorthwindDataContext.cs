using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NDatabase.Northwind.Domain;

namespace NDatabase.Northwind.TypedDataContext
{
    public class NDatabaseNorthwindDataContext
    {
        static NDatabaseNorthwindDataContext()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("DbFilePath"))
            {
                DbName = ConfigurationManager.AppSettings["DbFilePath"];    
            }
            else
            {
                throw new ConfigurationErrorsException(
                    "You are missing key 'DbFilePath' in your <appSettings>. Key has to contain valid db file path");
            }
        }

        private static readonly string DbName;

        private static readonly IList<string> PropertyNames = new List<string>
                                                                  {
                                                                      "Customers",
                                                                      "Employees",
                                                                      "Categories",
                                                                      "Orders",
                                                                      "Products",
                                                                      "Regions",
                                                                      "Shippers",
                                                                      "Suppliers",
                                                                      "Territories",
                                                                      "CustomerCustomerDemos",
                                                                      "CustomerDemographicses",
                                                                      "EmployeeTerritories",
                                                                      "OrderDetails"
                                                                  };

        public IEnumerable<string> Names
        {
            get { return PropertyNames; }
        }

        public IEnumerable<Customer> Customers
        {
            get
            {
                IList<Customer> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<Customer>().Execute<Customer>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<Employee> Employees
        {
            get
            {
                IList<Employee> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<Employee>().Execute<Employee>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<Category> Categories
        {
            get
            {
                IList<Category> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<Category>().Execute<Category>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<Order> Orders
        {
            get
            {
                IList<Order> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<Order>().Execute<Order>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<Product> Products
        {
            get
            {
                IList<Product> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<Product>().Execute<Product>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<Region> Regions
        {
            get
            {
                IList<Region> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<Region>().Execute<Region>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<Shipper> Shippers
        {
            get
            {
                IList<Shipper> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<Shipper>().Execute<Shipper>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<Supplier> Suppliers
        {
            get
            {
                IList<Supplier> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<Supplier>().Execute<Supplier>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<Territory> Territories
        {
            get
            {
                IList<Territory> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<Territory>().Execute<Territory>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<CustomerCustomerDemo> CustomerCustomerDemos
        {
            get
            {
                IList<CustomerCustomerDemo> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<CustomerCustomerDemo>().Execute<CustomerCustomerDemo>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<CustomerDemographics> CustomerDemographicses
        {
            get
            {
                IList<CustomerDemographics> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<CustomerDemographics>().Execute<CustomerDemographics>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<EmployeeTerritory> EmployeeTerritories
        {
            get
            {
                IList<EmployeeTerritory> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<EmployeeTerritory>().Execute<EmployeeTerritory>().ToList();
                }
                return result;
            }
        }

        public IEnumerable<OrderDetail> OrderDetails
        {
            get
            {
                IList<OrderDetail> result;
                using (var odb = OdbFactory.Open(DbName))
                {
                    result = odb.Query<OrderDetail>().Execute<OrderDetail>().ToList();
                }
                return result;
            }
        }
    }
}