using System;
using System.Linq;
using System.Web;
using Domain;
using NDatabase;

namespace TutorialWebSite
{
    public partial class Employees : System.Web.UI.Page
    {
        private const string DbPath = "~/App_Data/employees.ndb";

        protected void Page_Load(object sender, EventArgs e)
        {
            var path = HttpContext.Current.Server.MapPath(DbPath);

            using (var odb = OdbFactory.Open(path))
            {
                var employees = from employee in odb.AsQueryable<Employee>()
                                select new
                                           {
                                               ID = odb.GetObjectId(employee).ObjectId,
                                               employee.Name,
                                               employee.City,
                                               employee.Age,
                                               Employment_Date = employee.EmploymentDate.ToString("yyyy-MM-dd")
                                           };

                EmployeesGridView.DataSource = employees.ToList();
                EmployeesGridView.DataBind();
            }
        }
    }
}