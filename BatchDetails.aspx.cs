using System;
using System.Data;
using System.Data.SqlClient;

namespace CompileCrew1
{
    public partial class BatchDetails : System.Web.UI.Page
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBatches();
            }
        }

        private void LoadBatches()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT BatchId, Product FROM Batches", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlBatch.DataSource = dt;
                ddlBatch.DataTextField = "Product";
                ddlBatch.DataValueField = "BatchId";
                ddlBatch.DataBind();
                ddlBatch.Items.Insert(0, "-- Select Batch --");
            }
        }

        private void LoadDetails()
        {
            if (ddlBatch.SelectedIndex > 0)
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT DetailId, Ingredient, Quantity FROM BatchDetails WHERE BatchId=@BatchId", con);
                    da.SelectCommand.Parameters.AddWithValue("@BatchId", ddlBatch.SelectedValue);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                }
            }
            else
            {
                gvDetails.DataSource = null;
                gvDetails.DataBind();
            }
        }

        protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDetails();
        }

        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            if (ddlBatch.SelectedIndex <= 0)
            {
                lblMessage.Text = "⚠ Please select a batch.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtIngredient.Text) || string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                lblMessage.Text = "⚠ Please enter both Ingredient and Quantity.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "INSERT INTO BatchDetails (BatchId, Ingredient, Quantity) VALUES (@BatchId, @Ingredient, @Quantity)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BatchId", ddlBatch.SelectedValue);
                cmd.Parameters.AddWithValue("@Ingredient", txtIngredient.Text.Trim());
                cmd.Parameters.AddWithValue("@Quantity", decimal.Parse(txtQuantity.Text));

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                lblMessage.Text = "✅ Detail added successfully!";
                txtIngredient.Text = "";
                txtQuantity.Text = "";

                LoadDetails();
            }
        }

        // ----------- GRIDVIEW CRUD EVENTS -----------

        protected void gvDetails_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvDetails.EditIndex = e.NewEditIndex;
            LoadDetails();
        }

        protected void gvDetails_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvDetails.EditIndex = -1;
            LoadDetails();
        }

        protected void gvDetails_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int detailId = Convert.ToInt32(gvDetails.DataKeys[e.RowIndex].Value);

            string ingredient = (gvDetails.Rows[e.RowIndex].Cells[1].Controls[0] as System.Web.UI.WebControls.TextBox).Text;
            string qtyText = (gvDetails.Rows[e.RowIndex].Cells[2].Controls[0] as System.Web.UI.WebControls.TextBox).Text;

            if (decimal.TryParse(qtyText, out decimal quantity))
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = "UPDATE BatchDetails SET Ingredient=@Ingredient, Quantity=@Quantity WHERE DetailId=@DetailId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Ingredient", ingredient);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@DetailId", detailId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                lblMessage.Text = "✅ Detail updated successfully!";
                gvDetails.EditIndex = -1;
                LoadDetails();
            }
            else
            {
                lblMessage.Text = "⚠ Invalid quantity entered.";
            }
        }

        protected void gvDetails_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int detailId = Convert.ToInt32(gvDetails.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "DELETE FROM BatchDetails WHERE DetailId=@DetailId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DetailId", detailId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            lblMessage.Text = "🗑️ Detail deleted successfully!";
            LoadDetails();
        }
    }
}
