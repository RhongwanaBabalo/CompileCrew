
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace CompileCrew1
{
    public partial class Orders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders();
                LoadProducts(); // ✅ load products into dropdown
            }
        }

        private void LoadOrders()
        {
            using (SqlConnection con = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Orders ORDER BY DueDate ASC", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add a formatted OrderNumber column for display
                dt.Columns.Add("OrderNumber", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int id = Convert.ToInt32(dt.Rows[i]["OrderId"]);
                    dt.Rows[i]["OrderNumber"] = "ORD" + id.ToString("D3"); // ORD001, ORD002, ...
                }

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }

        // ✅ Load available products into the dropdown
        private void LoadProducts()
        {
            using (SqlConnection con = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                string query = "SELECT DISTINCT Product FROM ProductFormula ORDER BY Product";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlProduct.DataSource = reader;
                ddlProduct.DataTextField = "Product";
                ddlProduct.DataValueField = "Product";
                ddlProduct.DataBind();

                // Optional default item
                ddlProduct.Items.Insert(0, new ListItem("-- Select Product --", ""));
            }
        }

        protected void btnAddOrder_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                string query = "INSERT INTO Orders (Customer, Product, Quantity, DueDate, Status) " +
                               "VALUES (@Customer, @Product, @Quantity, @DueDate, 'In Progress')";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = txtCustomer.Text.Trim();
                    cmd.Parameters.Add("@Product", SqlDbType.NVarChar, 100).Value = ddlProduct.SelectedValue; // ✅ from dropdown
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = int.Parse(txtQuantity.Text);
                    cmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = DateTime.Parse(txtDueDate.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            lblMessage.Text = "✅ Order added successfully!";
            lblMessage.CssClass = "text-green-600 font-medium";

            // Clear input fields
            txtCustomer.Text = "";
            ddlProduct.SelectedIndex = 0; // ✅ reset dropdown
            txtQuantity.Text = "";
            txtDueDate.Text = "";

            LoadOrders();
        }

        protected void gvOrders_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CompleteOrder")
            {
                int orderId = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(
                    System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
                {
                    string query = "UPDATE Orders SET Status='Completed' WHERE OrderId=@OrderId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadOrders();
            }
        }
    }
}
