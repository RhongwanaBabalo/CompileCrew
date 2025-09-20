using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CompileCrew1
{
    public partial class CostingTool : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT Product FROM ProductFormula";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                ddlProduct.DataSource = cmd.ExecuteReader();
                ddlProduct.DataTextField = "Product";
                ddlProduct.DataValueField = "Product";
                ddlProduct.DataBind();
            }
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadIngredients(ddlProduct.SelectedValue);
        }

        private void LoadIngredients(string product)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Ingredient, Quantity FROM ProductFormula WHERE Product = @Product";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Product", product);
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvIngredients.DataSource = dt;
                gvIngredients.DataBind();
            }
        }

        protected void btnCalculateCost_Click(object sender, EventArgs e)
        {
            int batchSize = string.IsNullOrEmpty(txtBatchSize.Text) ? 0 : int.Parse(txtBatchSize.Text);

            if (batchSize == 0)
            {
                lblTotalCost.Text = "Please enter a valid batch size.";
                return;
            }

            decimal totalIngredientCost = 0;

            // Loop through GridView to read ingredient prices entered by admin
            foreach (GridViewRow row in gvIngredients.Rows)
            {
                // Access quantity via a Label inside a TemplateField
                Label lblQuantity = (Label)row.FindControl("lblQuantity");
                decimal quantity = decimal.Parse(lblQuantity.Text);

                TextBox txtPrice = (TextBox)row.FindControl("txtPricePerUnit");
                decimal price = string.IsNullOrEmpty(txtPrice.Text) ? 0 : decimal.Parse(txtPrice.Text);

                totalIngredientCost += (quantity * price * batchSize);
            }

            decimal containerCost = string.IsNullOrEmpty(txtContainerCost.Text) ? 0 : decimal.Parse(txtContainerCost.Text) * batchSize;
            decimal labelCost = string.IsNullOrEmpty(txtLabelCost.Text) ? 0 : decimal.Parse(txtLabelCost.Text) * batchSize;
            decimal transportCost = string.IsNullOrEmpty(txtTransportCost.Text) ? 0 : decimal.Parse(txtTransportCost.Text);

            decimal totalManufacturingCost = totalIngredientCost + containerCost + labelCost + transportCost;
            decimal costPerUnit = totalManufacturingCost / batchSize;
            decimal sellingPrice = costPerUnit * 1.5m;
            decimal totalRevenue = sellingPrice * batchSize;

            // Display
            lblIngredientCost.Text = "R " + totalIngredientCost.ToString("N2");
            lblContainerCost.Text = "R " + containerCost.ToString("N2");
            lblLabelCost.Text = "R " + labelCost.ToString("N2");
            lblTransportCost.Text = "R " + transportCost.ToString("N2");

            lblTotalCost.Text = "R " + totalManufacturingCost.ToString("N2");
            lblCostPerUnit.Text = "R " + costPerUnit.ToString("N2");
            lblSellingPrice.Text = "R " + sellingPrice.ToString("N2");
            lblTotalRevenue.Text = "R " + totalRevenue.ToString("N2");
        }
    }
}
