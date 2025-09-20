using System;
using System.Data;
using System.Data.SqlClient;

namespace CompileCrew1
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardData();
                LoadRecentOrders(); // call recent orders separately
            }
        }

        private void LoadDashboardData()
        {
            using (SqlConnection con = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                con.Open();

                // Active Orders
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Orders WHERE Status='In Progress'", con);
                lblOrders.Text = cmd.ExecuteScalar().ToString();

                // Batches
                cmd = new SqlCommand("SELECT COUNT(*) FROM Batches WHERE Status='In Progress'", con);
                lblBatches.Text = cmd.ExecuteScalar().ToString();

                // Deliveries (optional)
                // cmd = new SqlCommand("SELECT COUNT(*) FROM Deliveries WHERE Status='Pending'", con);
                // lblDeliveries.Text = cmd.ExecuteScalar().ToString();

                // Monthly Revenue (optional)
                // cmd = new SqlCommand("SELECT ISNULL(SUM(TotalAmount),0) FROM Invoices WHERE MONTH(Date)=MONTH(GETDATE())", con);
                // lblRevenue.Text = "R" + cmd.ExecuteScalar().ToString();
            }
        }

        private void LoadRecentOrders()
        {
            using (SqlConnection con = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                // Include OrderId for the Complete button
                string query = "SELECT TOP 5 OrderId, Customer, Product, Quantity, DueDate, Status FROM Orders ORDER BY DueDate DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvRecentOrders.DataSource = dt;
                gvRecentOrders.DataBind();
            }
        }

    }
}

