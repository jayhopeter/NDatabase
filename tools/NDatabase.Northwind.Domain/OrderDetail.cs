namespace NDatabase.Northwind.Domain
{
    public class OrderDetail
    {
        public string OrderDetailID
        {
            get { return Order.OrderID.ToString() + "-" + Product.ProductID.ToString(); }
        }

        public Order Order { get; set; }

        public Product Product { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }
    }
}