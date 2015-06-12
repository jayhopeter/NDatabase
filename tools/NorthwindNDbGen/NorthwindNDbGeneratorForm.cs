using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NDatabase.Api;
using NDatabase.Northwind.Domain;
using NDatabase.Northwind.Generator.NorthwindDataSetTableAdapters;

namespace NDatabase.Northwind.Generator
{
    public partial class NorthwindNDbGeneratorForm : Form
    {
        private int _seconds;

        public NorthwindNDbGeneratorForm()
        {
            InitializeComponent();
        }

        private void GenerateNorthwindNDb()
        {
            const string ndbFile = "northwind.ndb";

            OdbFactory.Delete(ndbFile);

            var dataSet = new NorthwindDataSet();
            var odb = OdbFactory.Open(ndbFile);
            var progress = 0.0;

            try
            {
                LogMessage("Start processing...", true);

                CopyCustomers(odb);
                StepFinished(progress += 2.75);

                CopyCategories(odb);
                StepFinished(progress += 0.24);

                CopySuppliers(odb);
                StepFinished(progress += 0.88);

                CopyShippers(odb);
                StepFinished(progress += 0.09);

                CopyRegions(odb, dataSet);
                StepFinished(progress += 0.12);

                CopyEmployees(odb);
                StepFinished(progress += 0.27);

                CopyCustomerDemographics(odb);
                CopyCustomerCustomerDemo(odb);
                
                CopyTerritories(odb);
                StepFinished(progress += 1.6);

                CopyEmployeeTerritories(odb);
                StepFinished(progress += 1.48);

                CopyProducts(odb);
                StepFinished(progress += 2.33);

                CopyOrders(odb, ref progress);
                
                CopyOrderDetails(odb, ref progress);

                LogMessage("Northwind database is ready: " + ndbFile + ".", true);
                LogMessage("Work is done, good job :)", true);
            }
            catch (Exception e)
            {
                LogMessage(e.ToString(), true);
            }
            finally
            {
                Terminate(odb, dataSet);
            }
        }

        private void StepFinished(double i)
        {
            backgroundWorker1.ReportProgress((int) i);
        }

        public void Terminate(IOdb odb, NorthwindDataSet dataSet)
        {
            if (dataSet != null)
                dataSet.Dispose();

            if (odb == null) 
                return;

            odb.Close();
        }

        private void LogMessage(string msg, bool linefeed)
        {
            if (linefeed)
                msg = msg + Environment.NewLine;
            
            txtOutput.Invoke(() => txtOutput.AppendText(msg));
        }

        private void CopyCustomers(IOdb odb)
        {
            //Processing Customers
            LogMessage("Reading Customers...", false);
            var adapter1 = new CustomersTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                LogMessage("Customer: " + row.CustomerID + " ...", false);

                var c = new Customer
                            {
                                CustomerID = row.CustomerID,
                                CompanyName = row.CompanyName,
                                ContactName = row.IsContactNameNull() ? null : row.ContactName,
                                ContactTitle = row.IsContactTitleNull() ? null : row.ContactTitle,
                                Address = row.IsAddressNull() ? null : row.Address,
                                City = row.IsCityNull() ? null : row.City,
                                Region = row.IsRegionNull() ? null : row.Region,
                                PostalCode = row.IsPostalCodeNull() ? null : row.PostalCode,
                                Country = row.IsCountryNull() ? null : row.Country,
                                Phone = row.IsPhoneNull() ? null : row.Phone,
                                Fax = row.IsFaxNull() ? null : row.Fax
                            };

                odb.Store(c);
                LogMessage("saved", true);
            }
            odb.Commit();

            LogMessage("Commit done, starting create index ...", false);
            odb.IndexManagerFor<Customer>().AddUniqueIndexOn("Customer_CustomerID_PK_index", Customer.PK);
            odb.Commit();
            LogMessage(" index created.", true);

            long objectCount = NDbUtil.GetAllInstances<Customer>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with Customers" + Environment.NewLine, true);
        }

        private void CopyCategories(IOdb odb)
        {
            //Processing Categories
            LogMessage("Reading Categories...", false);
            var adapter1 = new CategoriesTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                LogMessage("Categories: " + row.CategoryID.ToString() + " ...", false);

                var c = new Category
                            {
                                CategoryID = row.CategoryID,
                                CategoryName = row.CategoryName,
                                Description = row.IsDescriptionNull() ? null : row.Description,
                                Picture = row.IsPictureNull() ? null : row.Picture
                            };

                odb.Store(c);
                LogMessage("saved", true);
            }
            odb.Commit();

            LogMessage("Commit done, starting create index ...", false);
            odb.IndexManagerFor<Category>().AddUniqueIndexOn("Category_CategoryID_PK_index", Category.PK);
            odb.Commit();
            LogMessage(" index created.", true);

            long objectCount = NDbUtil.GetAllInstances<Category>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with Categories" + Environment.NewLine, true);
        }

        private void CopySuppliers(IOdb odb)
        {
            //Processing Suppliers
            LogMessage("Reading Suppliers...", false);
            var adapter1 = new SuppliersTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                LogMessage("Supplier: " + row.SupplierID.ToString() + " ...", false);

                var s = new Supplier
                            {
                                SupplierID = row.SupplierID,
                                CompanyName = row.CompanyName,
                                ContactName = row.IsContactNameNull() ? null : row.ContactName,
                                ContactTitle = row.IsContactTitleNull() ? null : row.ContactTitle,
                                Address = row.IsAddressNull() ? null : row.Address,
                                City = row.IsCityNull() ? null : row.City,
                                Region = row.IsRegionNull() ? null : row.Region,
                                PostalCode = row.IsPostalCodeNull() ? null : row.PostalCode,
                                Country = row.IsCountryNull() ? null : row.Country,
                                Phone = row.IsPhoneNull() ? null : row.Phone,
                                Fax = row.IsFaxNull() ? null : row.Fax,
                                HomePage = row.IsHomePageNull() ? null : row.HomePage
                            };

                odb.Store(s);
                LogMessage("saved", true);
            }
            odb.Commit();

            LogMessage("Commit done, starting create index ...", false);
            odb.IndexManagerFor<Supplier>().AddUniqueIndexOn("Supplier_SupplierID_PK_index", Supplier.PK);
            odb.Commit();
            LogMessage(" index created.", true);

            long objectCount = NDbUtil.GetAllInstances<Supplier>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with Suppliers" + Environment.NewLine, true);
        }

        private void CopyShippers(IOdb odb)
        {
            //Processing Shippers
            LogMessage("Reading Shippers...", false);
            var adapter1 = new ShippersTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                LogMessage("Shippers: " + row.ShipperID.ToString() + " ...", false);

                var s = new Shipper
                            {
                                ShipperID = row.ShipperID,
                                CompanyName = row.CompanyName,
                                Phone = row.IsPhoneNull() ? null : row.Phone
                            };

                odb.Store(s);
                LogMessage("saved", true);
            }
            odb.Commit();

            LogMessage("Commit done, starting create index ...", false);
            odb.IndexManagerFor<Shipper>().AddUniqueIndexOn("Shipper_ShipperID_PK_index", Shipper.PK);
            odb.Commit();
            LogMessage(" index created.", true);

            long objectCount = NDbUtil.GetAllInstances<Shipper>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with Shippers" + Environment.NewLine, true);
        }

        public void CopyRegions(IOdb odb, NorthwindDataSet dataSet)
        {
            //Processing Regions
            LogMessage("Reading Regions...", false);
            var adapter1 = new RegionTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                LogMessage("Regions: " + row.RegionID.ToString() + " ...", false);

                var r = new Region {RegionID = row.RegionID, RegionDescription = row.RegionDescription};

                odb.Store(r);
                LogMessage("saved", true);
            }
            odb.Commit();

            LogMessage("Commit done, starting create index ...", false);
            odb.IndexManagerFor<Region>().AddUniqueIndexOn("Region_RegionID_PK_index", Domain.Region.PK);
            odb.Commit();
            LogMessage(" index created.", true);

            long objectCount = NDbUtil.GetAllInstances<Region>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with Regions" + Environment.NewLine, true);
        }

        private void CopyEmployees(IOdb odb)
        {
            var employees = new List<Employee>();
            var reportingEmployees = new Hashtable();
            //Processing Employees
            LogMessage("Reading Employees...", false);
            var adapter1 = new EmployeesTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                var e = new Employee
                            {
                                EmployeeID = row.EmployeeID,
                                FirstName = row.FirstName,
                                LastName = row.LastName,
                                Title = row.IsTitleNull() ? null : row.Title,
                                TitleOfCourtesy = row.IsTitleOfCourtesyNull() ? null : row.TitleOfCourtesy,
                                Address = row.IsAddressNull() ? null : row.Address,
                                City = row.IsCityNull() ? null : row.City,
                                Region = row.IsRegionNull() ? null : row.Region,
                                PostalCode = row.IsPostalCodeNull() ? null : row.PostalCode,
                                Country = row.IsCountryNull() ? null : row.Country,
                                HomePhone = row.IsHomePhoneNull() ? null : row.HomePhone,
                                Extension = row.IsExtensionNull() ? null : row.Extension,
                                Notes = row.IsNotesNull() ? null : row.Notes,
                                Photo = row.IsPhotoNull() ? null : row.Photo,
                                PhotoPath = row.IsPhotoPathNull() ? null : row.PhotoPath
                            };

                if (!row.IsBirthDateNull())
                    e.BirthDate = row.BirthDate;
                if (!row.IsHireDateNull())
                    e.HireDate = row.HireDate;
                if (!row.IsReportsToNull())
                    reportingEmployees.Add(e.EmployeeID, row.ReportsTo);

                employees.Add(e);
            }
            foreach (var e in employees)
            {
                LogMessage("Employee: " + e.EmployeeID.ToString() + " ...", false);
                if (reportingEmployees.ContainsKey(e.EmployeeID))
                {
                    LogMessage("linking member...", false);
                    var reportsToID = Convert.ToInt64(reportingEmployees[e.EmployeeID]);
                    var found = employees.Find(p => p.EmployeeID == reportsToID);
                    e.ReportsTo = found;
                }
                odb.Store(e);
                LogMessage("saved (" + e.EmployeeID.ToString() + ")", true);
            }
            odb.Commit();

            LogMessage("Commit done, starting create index ...", false);
            odb.IndexManagerFor<Employee>().AddUniqueIndexOn("Employee_EmployeeID_PK_index", Employee.PK);
            odb.Commit();
            LogMessage(" index created.", true);

            long objectCount = NDbUtil.GetAllInstances<Employee>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with Employees" + Environment.NewLine, true);
        }

        private void CopyCustomerDemographics(IOdb odb)
        {
            //Processing CustomerDemographics
            LogMessage("Reading CustomerDemographics...", false);
            var adapter1 = new CustomerDemographicsTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                LogMessage("CustomerDemographics: " + row.CustomerTypeID + " ...", false);

                var cd = new CustomerDemographics
                             {
                                 CustomerTypeID = row.CustomerTypeID,
                                 CustomerDesc = row.IsCustomerDescNull() ? null : row.CustomerDesc
                             };

                odb.Store(cd);
                LogMessage("saved", true);
            }
            odb.Commit();

            LogMessage("Commit done, starting create index ...", false);
            odb.IndexManagerFor<CustomerDemographics>().AddUniqueIndexOn(
                "CustomerDemographics_CustomerTypeID_PK_index", CustomerDemographics.PK);
            odb.Commit();
            LogMessage(" index created.", true);

            long objectCount = NDbUtil.GetAllInstances<CustomerDemographics>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with CustomerDemographics" + Environment.NewLine, true);
        }

        private void CopyCustomerCustomerDemo(IOdb odb)
        {
            //Processing CustomerCustomerDemo
            LogMessage("Reading CustomerCustomerDemo...", false);
            var adapter1 = new CustomerCustomerDemoTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                LogMessage("CustomerCustomerDemo: " + row.CustomerID + "/" + row.CustomerTypeID + " ...", false);

                var ccd = new CustomerCustomerDemo();
                LogMessage("linking members...", false);
                ccd.CustomerID = NDbUtil.GetByStringID<Customer>(odb, Customer.PK, row.CustomerID);
                ccd.CustomerTypeID = NDbUtil.GetByStringID<CustomerDemographics>(odb, CustomerDemographics.PK,
                                                                                 row.CustomerTypeID);

                odb.Store(ccd);
                LogMessage("saved (" + ccd.CustomerID.CustomerID + "/" + ccd.CustomerTypeID.CustomerTypeID + ")", true);
            }
            odb.Commit();

            long objectCount = NDbUtil.GetAllInstances<CustomerCustomerDemo>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with CustomerCustomerDemo" + Environment.NewLine, true);
        }

        private void CopyTerritories(IOdb odb)
        {
            //Processing Territories
            LogMessage("Reading Territories...", false);
            var adapter1 = new TerritoriesTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                LogMessage("Territories: " + row.TerritoryID + " ...", false);

                var t = new Territory {TerritoryID = row.TerritoryID, TerritoryDescription = row.TerritoryDescription};

                LogMessage("linking member...", false);
                t.Region = NDbUtil.GetByNumericalID<Region>(odb, Domain.Region.PK, row.RegionID);

                odb.Store(t);
                LogMessage("saved (" + t.TerritoryID + ")", true);
            }
            odb.Commit();

            LogMessage("Commit done, starting create index ...", false);
            odb.IndexManagerFor<Territory>().AddUniqueIndexOn("Territory_TerritoryID_PK_index", Territory.PK);
            odb.Commit();
            LogMessage(" index created.", true);

            long objectCount = NDbUtil.GetAllInstances<Territory>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with Territories" + Environment.NewLine, true);
        }

        private void CopyEmployeeTerritories(IOdb odb)
        {
            //Processing EmployeeTerritories
            LogMessage("Reading EmployeeTerritories...", false);
            var adapter1 = new EmployeeTerritoriesTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                LogMessage("EmployeeTerritories: " + row.EmployeeID.ToString() + "/" + row.TerritoryID + " ...", false);

                var et = new EmployeeTerritory();
                LogMessage("linking members...", false);
                et.Employee = NDbUtil.GetByNumericalID<Employee>(odb, Employee.PK, row.EmployeeID);
                et.Territory = NDbUtil.GetByStringID<Territory>(odb, Territory.PK, row.TerritoryID);

                odb.Store(et);
                LogMessage("saved (" + et.Employee.EmployeeID.ToString() + "/" + et.Territory.TerritoryID + ")",
                           true);
            }
            odb.Commit();

            long objectCount = NDbUtil.GetAllInstances<EmployeeTerritory>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with EmployeeTerritories" + Environment.NewLine, true);
        }

        private void CopyProducts(IOdb odb)
        {
            var products = new List<Product>();
            var suppliers = new Hashtable();
            var categories = new Hashtable();
            //Processing Products
            LogMessage("Reading Products...", false);
            var adapter1 = new ProductsTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                var p = new Product
                            {
                                ProductID = row.ProductID,
                                ProductName = row.ProductName,
                                QuantityPerUnit = row.IsQuantityPerUnitNull() ? null : row.QuantityPerUnit,
                                UnitPrice = row.IsUnitPriceNull() ? 0 : Convert.ToDouble(row.UnitPrice),
                                UnitsInStock = row.IsUnitsInStockNull() ? 0 : row.UnitsInStock,
                                UnitsOnOrder = row.IsUnitsOnOrderNull() ? 0 : row.UnitsOnOrder,
                                ReorderLevel = row.IsReorderLevelNull() ? 0 : row.ReorderLevel,
                                Discontinued = row.Discontinued
                            };

                if (!row.IsSupplierIDNull())
                    suppliers.Add(p.ProductID, row.SupplierID);
                if (!row.IsCategoryIDNull())
                    categories.Add(p.ProductID, row.CategoryID);

                products.Add(p);
            }
            foreach (var p in products)
            {
                LogMessage("Product: " + p.ProductID.ToString() + " ...", false);
                if (suppliers.ContainsKey(p.ProductID))
                {
                    LogMessage("linking member...", false);
                    var supplierID = Convert.ToInt64(suppliers[p.ProductID]);
                    var found = NDbUtil.GetByNumericalID<Supplier>(odb, Supplier.PK, supplierID);
                    p.Supplier = found;
                }
                if (categories.ContainsKey(p.ProductID))
                {
                    LogMessage("linking member...", false);
                    var categoryID = Convert.ToInt64(categories[p.ProductID]);
                    var found = NDbUtil.GetByNumericalID<Category>(odb, Category.PK, categoryID);
                    p.Category = found;
                }
                odb.Store(p);

                LogMessage("saved (" + p.ProductID.ToString() + ")", true);
            }
            odb.Commit();

            LogMessage("Commit done, starting create index ...", false);
            odb.IndexManagerFor<Product>().AddUniqueIndexOn("Product_ProductID_PK_index", Product.PK);
            odb.Commit();
            LogMessage(" index created.", true);

            long objectCount = NDbUtil.GetAllInstances<Product>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with Products" + Environment.NewLine, true);
        }

        private void CopyOrders(IOdb odb, ref double progress)
        {
            var orders = new List<Order>();
            var customers = new Hashtable();
            var employees = new Hashtable();
            var shippers = new Hashtable();
            //Processing Orders
            LogMessage("Reading Orders...", false);
            var adapter1 = new OrdersTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                var o = new Order
                            {
                                OrderID = row.OrderID,
                                Freight = row.IsFreightNull() ? 0 : Convert.ToDouble(row.Freight),
                                ShipName = row.IsShipNameNull() ? null : row.ShipName,
                                ShipAddress = row.IsShipAddressNull() ? null : row.ShipAddress,
                                ShipCity = row.IsShipCityNull() ? null : row.ShipCity,
                                ShipRegion = row.IsShipRegionNull() ? null : row.ShipRegion,
                                ShipPostalCode = row.IsShipPostalCodeNull() ? null : row.ShipPostalCode,
                                ShipCountry = row.IsShipCountryNull() ? null : row.ShipCountry
                            };

                if (!row.IsCustomerIDNull())
                    customers.Add(o.OrderID, row.CustomerID);
                if (!row.IsEmployeeIDNull())
                    employees.Add(o.OrderID, row.EmployeeID);
                if (!row.IsShipViaNull())
                    shippers.Add(o.OrderID, row.ShipVia);
                if (!row.IsOrderDateNull())
                    o.OrderDate = row.OrderDate;
                if (!row.IsRequiredDateNull())
                    o.RequiredDate = row.RequiredDate;
                if (!row.IsShippedDateNull())
                    o.ShippedDate = row.ShippedDate;

                orders.Add(o);
            }
            foreach (var o in orders)
            {
                LogMessage("Order: " + o.OrderID.ToString() + " ...", false);
                if (customers.ContainsKey(o.OrderID))
                {
                    LogMessage("linking member...", false);
                    var customerID = Convert.ToString(customers[o.OrderID]);
                    var found = NDbUtil.GetByStringID<Customer>(odb, Customer.PK, customerID);
                    o.Customer = found;
                }
                if (employees.ContainsKey(o.OrderID))
                {
                    LogMessage("linking member...", false);
                    var employeeID = Convert.ToInt64(employees[o.OrderID]);
                    var found = NDbUtil.GetByNumericalID<Employee>(odb, Employee.PK, employeeID);
                    o.Employee = found;
                }
                if (shippers.ContainsKey(o.OrderID))
                {
                    LogMessage("linking member...", false);
                    var shipperID = Convert.ToInt64(shippers[o.OrderID]);
                    var found = NDbUtil.GetByNumericalID<Shipper>(odb, Shipper.PK, shipperID);
                    o.ShipVia = found;
                }
                odb.Store(o);
                StepFinished(progress += 0.029);
                LogMessage("saved (" + o.OrderID.ToString() + ")", true);
            }
            odb.Commit();

            LogMessage("Commit done, starting create index ...", false);
            odb.IndexManagerFor<Order>().AddUniqueIndexOn("Order_OrderID_PK_index", Order.PK);
            odb.Commit();
            LogMessage(" index created.", true);

            long objectCount = NDbUtil.GetAllInstances<Order>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with Orders" + Environment.NewLine, true);
        }

        private void CopyOrderDetails(IOdb odb, ref double progress)
        {
            //Processing OrderDetails
            LogMessage("Reading OrderDetails...", false);
            var adapter1 = new Order_DetailsTableAdapter();
            var table1 = adapter1.GetData();
            LogMessage("processing " + table1.Count.ToString() + " rows", true);
            foreach (var row in table1)
            {
                LogMessage("OrderDetails: " + row.OrderID.ToString() + "/" + row.ProductID.ToString() + " ...", false);

                var od = new OrderDetail();
                LogMessage("linking members...", false);
                od.Order = NDbUtil.GetByNumericalID<Order>(odb, Order.PK, row.OrderID);
                od.Product = NDbUtil.GetByNumericalID<Product>(odb, Product.PK, row.ProductID);
                od.UnitPrice = Convert.ToDouble(row.UnitPrice);
                od.Quantity = row.Quantity;
                od.Discount = Convert.ToDouble(row.Discount);

                odb.Store(od);
                StepFinished(progress += 0.029);
                LogMessage("saved (" + od.Order.OrderID.ToString() + "/" + od.Product.ProductID.ToString() + ")",
                           true);
            }
            odb.Commit();

            long objectCount = NDbUtil.GetAllInstances<OrderDetail>(odb).Count;
            if (table1.Count == objectCount)
                LogMessage(table1.Count + " objects saved", true);
            else
                LogMessage("Error: " + table1.Count + " rows retrieved but " + objectCount + " objects were saved", true);
            LogMessage("Done with OrderDetails" + Environment.NewLine, true);
        }

        private void StartClick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            button1.Enabled = false;
            label1.Text = "IN PROGRESS";
            backgroundWorker1.RunWorkerAsync();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            OneSecondLeft();
        }

        private void OneSecondLeft()
        {
            _seconds++;
            lblTimer.Text = TimeSpan.FromSeconds(_seconds).ToString();
        }

        private void NorthwindNDbGeneratorForm_Load(object sender, EventArgs e)
        {
            txtOutput.BackColor = txtOutput.BackColor;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            GenerateNorthwindNDb();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 100;
            label1.Text = e.Error != null ? "FAILED" : "DONE";
            timer1.Enabled = false;
            button1.Enabled = true;
            button1.Text = "Restart";
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }
    }
}