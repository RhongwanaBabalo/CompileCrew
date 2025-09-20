using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CompileCrew1
{
    public partial class DeliveryTracking : System.Web.UI.Page
    {

        protected string GetStatusCssClass(string status)
        {
            switch (status)
            {
                case "Pending":
                    return "bg-yellow-100 text-yellow-800 px-2 py-1 rounded-full text-xs";
                case "Delivered (On Time)":
                    return "bg-green-100 text-green-800 px-2 py-1 rounded-full text-xs";
                case "Delivered (Late)":
                    return "bg-red-100 text-red-800 px-2 py-1 rounded-full text-xs";
                default:
                    return "bg-gray-100 text-gray-800 px-2 py-1 rounded-full text-xs";
            }
        }

        string connStr = ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPOs();
                LoadDeliveries();
            }
        }

        private void LoadPOs()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT POId, POId AS DisplayPO FROM PurchaseOrders ORDER BY POId DESC", con);
                con.Open();
                ddlPOs.DataSource = cmd.ExecuteReader();
                ddlPOs.DataTextField = "DisplayPO";
                ddlPOs.DataValueField = "POId";
                ddlPOs.DataBind();
                ddlPOs.Items.Insert(0, new ListItem("-- Select PO --", "0"));
            }
        }

        private void LoadDeliveries()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT d.DeliveryId, d.POId, d.DeliveryDate, d.Status, p.ExpectedDelivery FROM Deliveries d INNER JOIN PurchaseOrders p ON d.POId = p.POId ORDER BY d.DeliveryId DESC",
                    con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add LeadTimeCompliance column
                dt.Columns.Add("LeadTimeCompliance", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    if (row["DeliveryDate"] != DBNull.Value && row["ExpectedDelivery"] != DBNull.Value)
                    {
                        DateTime deliveryDate = Convert.ToDateTime(row["DeliveryDate"]);
                        DateTime expected = Convert.ToDateTime(row["ExpectedDelivery"]);
                        row["LeadTimeCompliance"] = (deliveryDate <= expected.AddDays(7)) ? "Yes" : "No";
                    }
                    else
                        row["LeadTimeCompliance"] = "Pending";
                }

                gvDeliveries.DataSource = dt;
                gvDeliveries.DataBind();
            }
        }

        protected void btnUpdateDelivery_Click(object sender, EventArgs e)
        {
            if (ddlPOs.SelectedValue == "0") return;
            DateTime deliveryDate;
            if (!DateTime.TryParse(txtDeliveryDate.Text, out deliveryDate)) return;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"UPDATE Deliveries SET DeliveryDate=@DeliveryDate, Status='Pending' 
                                 WHERE POId=@POId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DeliveryDate", deliveryDate);
                cmd.Parameters.AddWithValue("@POId", ddlPOs.SelectedValue);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            LoadDeliveries();
            txtDeliveryDate.Text = "";
            ddlPOs.SelectedIndex = 0;
        }

        protected void gvDeliveries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "MarkDelivered")
            {
                int deliveryId = Convert.ToInt32(e.CommandArgument);
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = "UPDATE Deliveries SET Status='Delivered', DeliveryDate=GETDATE() WHERE DeliveryId=@DeliveryId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@DeliveryId", deliveryId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadDeliveries();
            }
        }

        protected void ddlPOs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPOs.SelectedValue == "0")
            {
                txtDeliveryDate.Text = "";
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT DeliveryDate FROM Deliveries WHERE POId=@POId", con);
                cmd.Parameters.AddWithValue("@POId", ddlPOs.SelectedValue);
                con.Open();
                var result = cmd.ExecuteScalar();
                txtDeliveryDate.Text = (result != DBNull.Value && result != null)
                    ? Convert.ToDateTime(result).ToString("yyyy-MM-dd")
                    : "";
            }
        }

        // Optional: Highlight lead-time compliance in GridView
        protected void gvDeliveries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string compliance = DataBinder.Eval(e.Row.DataItem, "LeadTimeCompliance").ToString();
                if (compliance == "No")
                {
                    e.Row.BackColor = System.Drawing.Color.LightCoral; // Missed lead time
                }
                else if (compliance == "Yes")
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen; // Complied
                }
            }
        }
    }
}
