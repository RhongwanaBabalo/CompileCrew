using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CompileCrew2
{
    public partial class Default : System.Web.UI.Page
    {
        private string connStr;

        protected void Page_Load(object sender, EventArgs e)
        {
            // ✅ Safely fetch connection string
            var connSettings = ConfigurationManager.ConnectionStrings["CompileCrew2"];
            if (connSettings == null)
            {
                throw new Exception("Connection string 'CompileCrew2' not found in Web.config.");
            }
            connStr = connSettings.ConnectionString;

            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT DISTINCT Product FROM ProductFormula ORDER BY Product";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                ddlProducts.DataSource = cmd.ExecuteReader();
                ddlProducts.DataTextField = "Product";
                ddlProducts.DataValueField = "Product";
                ddlProducts.DataBind();
            }

            ddlProducts.Items.Insert(0, "-- Select a Product --");
        }

        protected void ddlProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProducts.SelectedIndex > 0) // Skip placeholder
            {
                LoadIngredients(ddlProducts.SelectedValue);
            }
            else
            {
                gvIngredients.DataSource = null;
                gvIngredients.DataBind();
            }
        }

        private void LoadIngredients(string product)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Ingredient, Quantity FROM ProductFormula WHERE Product = @Product";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Product", product);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvIngredients.DataSource = dt;
                gvIngredients.DataBind();
            }
        }
    }
}
