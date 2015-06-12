namespace NDatabase.Northwind.Domain
{
    public class CustomerCustomerDemo
    {
        public string CustomerCustomerDemoID
        {
            get { return CustomerID.CustomerID + "-" + CustomerTypeID.CustomerTypeID; }
        }

        public CustomerDemographics CustomerTypeID { get; set; }

        public Customer CustomerID { get; set; }
    }
}