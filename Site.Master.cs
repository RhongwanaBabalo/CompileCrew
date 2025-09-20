using System;

namespace CompileCrew1
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEmail"] != null)
            {
                lblUserName.Text = Session["UserEmail"].ToString();
                lblUserRole.Text = Session["UserRole"]?.ToString();
            }
            else
            {
                // Force back to login if not authenticated
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }
    }
}
