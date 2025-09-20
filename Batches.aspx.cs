using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace CompileCrew1
{
    public partial class Batches : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
                LoadBatches();

                if (!string.IsNullOrEmpty(Request.QueryString["product"]))
                    ddlProduct.SelectedValue = Request.QueryString["product"];
                if (!string.IsNullOrEmpty(Request.QueryString["size"]))
                    txtBatchSize.Text = Request.QueryString["size"];
            }
        }

        // Load products into dropdown
        private void LoadProducts()
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                // Fixed column name and ordering for DISTINCT
                SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Product FROM ProductFormula ORDER BY Product", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlProduct.DataSource = dt;
                ddlProduct.DataTextField = "Product";
                ddlProduct.DataValueField = "Product";
                ddlProduct.DataBind();
            }
            ddlProduct.Items.Insert(0, new ListItem("-- Select Product --", ""));
        }

        // Schedule batch
        protected void btnScheduleBatch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlProduct.SelectedValue) || string.IsNullOrEmpty(txtBatchSize.Text) || string.IsNullOrEmpty(txtStartDate.Text))
                return;

            decimal batchSize = decimal.Parse(txtBatchSize.Text);
            DateTime startDate = DateTime.Parse(txtStartDate.Text);

            int batchId;

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                con.Open();

                // Insert new batch and get the auto-incremented BatchId
                string query = "INSERT INTO Batches (Product, Size, StartDate, Status) " +
                               "OUTPUT INSERTED.BatchId " +
                               "VALUES (@Product, @Size, @StartDate, 'In Progress')";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Product", ddlProduct.SelectedValue);
                cmd.Parameters.AddWithValue("@Size", batchSize);
                cmd.Parameters.AddWithValue("@StartDate", startDate);

                batchId = (int)cmd.ExecuteScalar();
                con.Close();
            }

            // Refresh GridView to show the new batch immediately
            LoadBatches();

            lblMessage.Text = $"✅ Batch scheduled successfully with Batch ID: BTC{batchId:D3}!";
        }


        // Load scheduled batches with optional filters
        private void LoadBatches(string productFilter = "", string statusFilter = "", string dateFilter = "")
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                string query = "SELECT * FROM Batches WHERE 1=1";

                if (!string.IsNullOrEmpty(productFilter))
                    query += " AND Product LIKE @Product";
                if (!string.IsNullOrEmpty(statusFilter))
                    query += " AND Status=@Status";
                if (!string.IsNullOrEmpty(dateFilter))
                    query += " AND StartDate=@StartDate";

                // **Sort by BatchId descending so newest batch is on top**
                query += " ORDER BY BatchId DESC";

                SqlCommand cmd = new SqlCommand(query, con);

                if (!string.IsNullOrEmpty(productFilter))
                    cmd.Parameters.AddWithValue("@Product", "%" + productFilter + "%");
                if (!string.IsNullOrEmpty(statusFilter))
                    cmd.Parameters.AddWithValue("@Status", statusFilter);
                if (!string.IsNullOrEmpty(dateFilter))
                    cmd.Parameters.AddWithValue("@StartDate", DateTime.Parse(dateFilter));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvBatches.DataSource = dt;
                gvBatches.DataBind();
            }
        }



        // Complete batch
        protected void gvBatches_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CompleteBatch")
            {
                int batchId = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
                {
                    string query = "UPDATE Batches SET Status='Completed' WHERE BatchId=@BatchId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@BatchId", batchId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                LoadBatches();
            }
        }

        // Filter batches
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string product = txtSearchProduct.Text.Trim();
            string status = ddlSearchStatus.SelectedValue;
            string date = txtSearchDate.Text.Trim();

            LoadBatches(product, status, date);
        }
    }
}
