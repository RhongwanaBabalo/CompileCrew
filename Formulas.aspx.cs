using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CompileCrew1
{
    public partial class Formulas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadProducts();
        }

        private void LoadProducts()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Product FROM ProductFormula ORDER BY Product", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlProduct.DataSource = dt;
                ddlProduct.DataTextField = "Product";
                ddlProduct.DataValueField = "Product";
                ddlProduct.DataBind();

                ddlProduct.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Product --", ""));
            }
        }

        protected void btnViewIngredients_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlProduct.SelectedValue))
                return;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT Ingredient AS IngredientName, Quantity FROM ProductFormula WHERE Product=@Product", con);
                da.SelectCommand.Parameters.AddWithValue("@Product", ddlProduct.SelectedValue);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvIngredients.DataSource = dt;
                gvIngredients.DataBind();

                ViewState["CustomIngredients"] = null;
            }
        }

        protected void btnAddCustomIngredient_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomIngredient.Text) || string.IsNullOrEmpty(txtCustomQuantity.Text))
                return;

            DataTable dt;
            if (ViewState["CustomIngredients"] != null)
                dt = (DataTable)ViewState["CustomIngredients"];
            else
            {
                dt = new DataTable();
                dt.Columns.Add("IngredientName");
                dt.Columns.Add("Quantity", typeof(decimal));
            }

            DataRow row = dt.NewRow();
            row["IngredientName"] = txtCustomIngredient.Text.Trim();
            row["Quantity"] = decimal.Parse(txtCustomQuantity.Text.Trim());
            dt.Rows.Add(row);

            ViewState["CustomIngredients"] = dt;

            DataTable merged = GetMergedIngredients(dt);
            gvIngredients.DataSource = merged;
            gvIngredients.DataBind();

            txtCustomIngredient.Text = "";
            txtCustomQuantity.Text = "";
        }

        private DataTable GetMergedIngredients(DataTable customIngredients)
        {
            DataTable standard = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT Ingredient AS IngredientName, Quantity FROM ProductFormula WHERE Product=@Product", con);
                da.SelectCommand.Parameters.AddWithValue("@Product", ddlProduct.SelectedValue);
                da.Fill(standard);
            }

            if (customIngredients != null)
                foreach (DataRow row in customIngredients.Rows)
                    standard.Rows.Add(row["IngredientName"], row["Quantity"]);

            return standard;
        }

        protected void btnCalculateBatch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBatchSize.Text))
                return;

            decimal batchSize = decimal.Parse(txtBatchSize.Text);

            DataTable standard = GetMergedIngredients(ViewState["CustomIngredients"] as DataTable);

            DataTable dtBatchCalc = new DataTable();
            dtBatchCalc.Columns.Add("IngredientName");
            dtBatchCalc.Columns.Add("RequiredQuantity", typeof(decimal));

            foreach (DataRow row in standard.Rows)
            {
                string ingredient = row["IngredientName"].ToString();
                decimal qty = Convert.ToDecimal(row["Quantity"]);
                dtBatchCalc.Rows.Add(ingredient, qty * batchSize);
            }

            gvBatchCalc.DataSource = dtBatchCalc;
            gvBatchCalc.DataBind();

            ViewState["BatchCalculation"] = dtBatchCalc;

            lblMessage.Text = "✅ Batch calculation completed successfully.";
        }

        protected void btnSaveBatch_Click(object sender, EventArgs e)
        {
            if (ViewState["BatchCalculation"] == null || string.IsNullOrEmpty(ddlProduct.SelectedValue) || string.IsNullOrEmpty(txtBatchSize.Text))
            {
                lblMessage.Text = "⚠️ Please calculate the batch first.";
                return;
            }

            DataTable dtBatchCalc = (DataTable)ViewState["BatchCalculation"];
            decimal batchSize = decimal.Parse(txtBatchSize.Text);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString))
            {
                con.Open();

                string insertBatch = @"INSERT INTO Batches (Product, Size, StartDate, Status) 
                                       OUTPUT INSERTED.BatchId
                                       VALUES (@Product, @Size, GETDATE(), 'In Progress')";
                SqlCommand cmd = new SqlCommand(insertBatch, con);
                cmd.Parameters.AddWithValue("@Product", ddlProduct.SelectedValue);
                cmd.Parameters.AddWithValue("@Size", batchSize);

                int batchId = (int)cmd.ExecuteScalar();

                foreach (DataRow row in dtBatchCalc.Rows)
                {
                    string insertIng = @"INSERT INTO BatchDetails (BatchId, IngredientName, RequiredQuantity) 
                                         VALUES (@BatchId, @IngredientName, @RequiredQuantity)";
                    SqlCommand cmdIng = new SqlCommand(insertIng, con);
                    cmdIng.Parameters.AddWithValue("@BatchId", batchId);
                    cmdIng.Parameters.AddWithValue("@IngredientName", row["IngredientName"]);
                    cmdIng.Parameters.AddWithValue("@RequiredQuantity", row["RequiredQuantity"]);
                    cmdIng.ExecuteNonQuery();
                }

                con.Close();
            }

            lblMessage.Text = "✅ Batch and ingredients saved successfully.";
        }
    }
}
