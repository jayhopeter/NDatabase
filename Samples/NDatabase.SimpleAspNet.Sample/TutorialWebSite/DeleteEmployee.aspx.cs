using System;
using System.Web;
using NDatabase;
using NDatabase.Oid;

namespace TutorialWebSite
{
    public partial class DeleteEmployee : System.Web.UI.Page
    {
        private const string DbPath = "~/App_Data/employees.ndb";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            ID.Text = "-1";
        }

        protected void RemoveButton_Click(object sender, EventArgs e)
        {
            var dbPath = HttpContext.Current.Server.MapPath(DbPath);
            var id = Convert.ToInt64(ID.Text);

            try
            {
                using (var odb = OdbFactory.Open(dbPath))
                {
                    odb.DeleteObjectWithId(OIDFactory.BuildObjectOID(id));
                }

                ID.Text = "-1";
                ErrorMessage.Text = string.Empty;
            }
            catch (Exception exception)
            {
                ErrorMessage.Text = exception.Message;
            }
        }
    }
}