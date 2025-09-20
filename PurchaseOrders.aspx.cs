using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CompileCrew1
{
    public partial class PurchaseOrders : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadPOs();
        }

        private void LoadPOs()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM PurchaseOrders ORDER BY POId DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvPurchaseOrders.DataSource = dt;
                gvPurchaseOrders.DataBind();
            }
        }

        protected void btnSavePO_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                decimal quantity = string.IsNullOrEmpty(txtQuantity.Text) ? 0 : decimal.Parse(txtQuantity.Text);
                decimal unitCost = string.IsNullOrEmpty(txtUnitCost.Text) ? 0 : decimal.Parse(txtUnitCost.Text);
                decimal totalCost = quantity * unitCost;

                if (string.IsNullOrEmpty(hfPOId.Value)) // New PO
                {
                    query = @"INSERT INTO PurchaseOrders 
                              (Supplier, ItemName, Quantity, Unit, UnitCost, TotalCost, ExpectedDelivery, Status) 
                              VALUES (@Supplier, @ItemName, @Quantity, @Unit, @UnitCost, @TotalCost, @ExpectedDelivery, 'Pending')";
                }
                else // Edit existing PO
                {
                    query = @"UPDATE PurchaseOrders SET 
                              Supplier=@Supplier, ItemName=@ItemName, Quantity=@Quantity, Unit=@Unit, 
                              UnitCost=@UnitCost, TotalCost=@TotalCost, ExpectedDelivery=@ExpectedDelivery
                              WHERE POId=@POId";
                    cmd.Parameters.AddWithValue("@POId", hfPOId.Value);
                }

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@Supplier", ddlSupplier.SelectedValue);
                cmd.Parameters.AddWithValue("@ItemName", txtItemName.Text.Trim());
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Unit", ddlUnit.SelectedValue);
                cmd.Parameters.AddWithValue("@UnitCost", unitCost);
                cmd.Parameters.AddWithValue("@TotalCost", totalCost);
                cmd.Parameters.AddWithValue("@ExpectedDelivery", string.IsNullOrEmpty(txtExpectedDelivery.Text) ? (object)DBNull.Value : DateTime.Parse(txtExpectedDelivery.Text));

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            ClearForm();
            LoadPOs();
        }

        private void ClearForm()
        {
            hfPOId.Value = "";
            txtItemName.Text = "";
            txtQuantity.Text = "";
            txtUnitCost.Text = "";
            txtExpectedDelivery.Text = "";
            ddlSupplier.SelectedIndex = 0;
            ddlUnit.SelectedIndex = 0;
        }

        protected void gvPurchaseOrders_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int poId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditPO")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = "SELECT * FROM PurchaseOrders WHERE POId=@POId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@POId", poId);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfPOId.Value = dr["POId"].ToString();
                        ddlSupplier.SelectedValue = dr["Supplier"].ToString();
                        txtItemName.Text = dr["ItemName"].ToString();
                        txtQuantity.Text = dr["Quantity"].ToString();
                        ddlUnit.SelectedValue = dr["Unit"].ToString();
                        txtUnitCost.Text = dr["UnitCost"].ToString();
                        txtExpectedDelivery.Text = dr["ExpectedDelivery"] == DBNull.Value ? "" : Convert.ToDateTime(dr["ExpectedDelivery"]).ToString("yyyy-MM-dd");
                    }
                    con.Close();
                }
            }
            else if (e.CommandName == "ReceivePO")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = "UPDATE PurchaseOrders SET Status='Received' WHERE POId=@POId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@POId", poId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                LoadPOs();
            }
        }
    }
}
