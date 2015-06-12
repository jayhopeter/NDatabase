using System;
using System.Web;
using Domain;
using NDatabase;

namespace TutorialWebSite
{
    public partial class AddEmployee : System.Web.UI.Page
    {
        private const string DbPath = "~/App_Data/employees.ndb";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            Name.Text = string.Empty;
            City.Text = string.Empty;
            Age.Text = "-1";
            EmploymentDate.SelectedDate = DateTime.Now;
        }
        protected void AddButton_Click(object sender, EventArgs e)
        {
            var dbPath = HttpContext.Current.Server.MapPath(DbPath);

            var name = Name.Text;
            var city = City.Text;
            var age = Convert.ToInt32(Age.Text);
            var employmentDate = EmploymentDate.SelectedDate;

            var newEmployee = new Employee { Name = name, City = city, Age = age, EmploymentDate = employmentDate };

            using (var odb = OdbFactory.Open(dbPath))
            {
                odb.Store(newEmployee);
            }

            Name.Text = string.Empty;
            City.Text = string.Empty;
            Age.Text = "-1";
            EmploymentDate.SelectedDate = DateTime.Now;
        }
    }
}