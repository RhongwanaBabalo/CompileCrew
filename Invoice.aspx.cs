using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CompileCrew1
{
    [Serializable]
    public class InvoiceData
    {
        public int BatchId { get; set; }
        public string Product { get; set; }
        public string Customer { get; set; }
        public string OrderRef { get; set; }
        public DataTable Ingredients { get; set; } // Stores itemized ingredients including editable unit price
        public decimal DeliveryCost { get; set; }
        public decimal FinalPrice { get; set; }
    }

    public partial class Invoice : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadBatches();
        }

        private void LoadBatches()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT BatchId, Product + ' - ' + CAST(Size AS NVARCHAR) AS BatchName FROM Batches ORDER BY BatchId DESC", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlBatch.DataSource = dt;
                ddlBatch.DataTextField = "BatchName";
                ddlBatch.DataValueField = "BatchId";
                ddlBatch.DataBind();

                ddlBatch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Batch --", ""));
            }
        }

        protected void btnGenerateInvoice_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlBatch.SelectedValue))
            {
                lblMessage.Text = "⚠️ Please select a batch.";
                return;
            }

            if (string.IsNullOrEmpty(txtCustomer.Text.Trim()) || string.IsNullOrEmpty(txtOrderRef.Text.Trim()))
            {
                lblMessage.Text = "⚠️ Please enter Customer and Order Reference.";
                return;
            }

            int batchId = int.Parse(ddlBatch.SelectedValue);
            string product = ddlBatch.SelectedItem.Text;
            string customer = txtCustomer.Text.Trim();
            string orderRef = txtOrderRef.Text.Trim();

            // Fetch batch ingredients and costs
            DataTable dtIngredients = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT Ingredient AS IngredientName, Quantity AS RequiredQuantity, 0 AS UnitPrice FROM ProductFormula WHERE Product=@Product", con);
                da.SelectCommand.Parameters.AddWithValue("@Product", product.Split('-')[0].Trim());
                da.Fill(dtIngredients);
            }

            // Bind GridView
            gvInvoiceItems.DataSource = dtIngredients;
            gvInvoiceItems.DataBind();

            decimal ingredientsTotal = 0;
            foreach (DataRow row in dtIngredients.Rows)
                ingredientsTotal += Convert.ToDecimal(row["RequiredQuantity"]) * Convert.ToDecimal(row["UnitPrice"]);

            decimal deliveryCost = string.IsNullOrEmpty(txtDeliveryCost.Text) ? 0 : decimal.Parse(txtDeliveryCost.Text);
            decimal finalPrice = ingredientsTotal + deliveryCost;

            lblBatchName.Text = product;
            pnlInvoice.Visible = true;
            btnDownloadPDF.Visible = true;

            // Store in ViewState
            InvoiceData invoice = new InvoiceData
            {
                BatchId = batchId,
                Product = product,
                Customer = customer,
                OrderRef = orderRef,
                Ingredients = dtIngredients,
                DeliveryCost = deliveryCost,
                FinalPrice = finalPrice
            };

            ViewState["InvoiceData"] = invoice;
        }

        protected void btnDownloadPDF_Click(object sender, EventArgs e)
        {
            if (ViewState["InvoiceData"] == null) return;
            InvoiceData invoice = (InvoiceData)ViewState["InvoiceData"];

            // Update ingredients prices from GridView
            UpdateGridPrices(invoice.Ingredients);

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Header
                Paragraph header = new Paragraph("Best Results Manufacturing", FontFactory.GetFont("Arial", 18, Font.BOLD))
                { Alignment = Element.ALIGN_CENTER };
                doc.Add(header);
                doc.Add(new Paragraph("Adams Mission, Durban", FontFactory.GetFont("Arial", 12)) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("Tel: (031) 123-4567", FontFactory.GetFont("Arial", 12)) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph(" ")); // Spacer

                // Invoice info
                Paragraph invoiceType = new Paragraph("PROFORMA INVOICE", FontFactory.GetFont("Arial", 14, Font.BOLD)) { Alignment = Element.ALIGN_CENTER };
                doc.Add(invoiceType);
                doc.Add(new Paragraph(" "));

                PdfPTable infoTable = new PdfPTable(2) { WidthPercentage = 100 };
                infoTable.SetWidths(new float[] { 50f, 50f });
                infoTable.AddCell(new PdfPCell(new Phrase("Invoice #: INV-001")) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase($"Date: {DateTime.Now:yyyy-MM-dd}")) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase($"Customer: {invoice.Customer}")) { Border = 0 });
                infoTable.AddCell(new PdfPCell(new Phrase($"Order Ref: {invoice.OrderRef}")) { Border = 0 });
                doc.Add(infoTable);
                doc.Add(new Paragraph(" "));

                // Ingredients table
                PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 50f, 15f, 15f, 20f });

                string[] headers = { "Item", "Qty", "Price", "Total" };
                foreach (string h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, FontFactory.GetFont("Arial", 12, Font.BOLD)))
                    { BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER };
                    table.AddCell(cell);
                }

                decimal ingredientsTotal = 0;
                foreach (DataRow row in invoice.Ingredients.Rows)
                {
                    string ingredient = row["IngredientName"].ToString();
                    decimal qty = Convert.ToDecimal(row["RequiredQuantity"]);
                    decimal price = Convert.ToDecimal(row["UnitPrice"]);
                    decimal total = qty * price;
                    ingredientsTotal += total;

                    table.AddCell(new PdfPCell(new Phrase(ingredient)) { Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase(qty.ToString("N2"))) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(price.ToString("C"))) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(total.ToString("C"))) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
                }

                // Totals
                PdfPCell totalLabel = new PdfPCell(new Phrase("Total Ingredients Cost", FontFactory.GetFont("Arial", 12, Font.BOLD)))
                { Colspan = 3, HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 };
                table.AddCell(totalLabel);
                table.AddCell(new PdfPCell(new Phrase(ingredientsTotal.ToString("C"), FontFactory.GetFont("Arial", 12, Font.BOLD)))
                { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });

                doc.Add(table);

                // Delivery and final
                PdfPTable finalTable = new PdfPTable(2) { WidthPercentage = 100, SpacingBefore = 20f };
                finalTable.SetWidths(new float[] { 70f, 30f });

                void AddRow(string label, decimal value)
                {
                    finalTable.AddCell(new PdfPCell(new Phrase(label)) { Border = 0, Padding = 5 });
                    finalTable.AddCell(new PdfPCell(new Phrase(value.ToString("C"))) { Border = 0, Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
                }

                AddRow("Delivery", invoice.DeliveryCost);
                AddRow("Total", invoice.FinalPrice);

                doc.Add(finalTable);
                doc.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=Invoice.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }

        private void UpdateGridPrices(DataTable dtIngredients)
        {
            for (int i = 0; i < gvInvoiceItems.Rows.Count; i++)
            {
                var row = gvInvoiceItems.Rows[i];
                var txtPrice = (System.Web.UI.WebControls.TextBox)row.FindControl("txtUnitPrice");
                if (txtPrice != null)
                    dtIngredients.Rows[i]["UnitPrice"] = decimal.TryParse(txtPrice.Text, out decimal val) ? val : 0;
            }
        }

        protected void btnSaveProforma_Click(object sender, EventArgs e)
        {
            SaveInvoice("Proforma");
        }

        protected void btnSaveFinal_Click(object sender, EventArgs e)
        {
            SaveInvoice("Final");
        }

        private void SaveInvoice(string invoiceType)
        {
            if (ViewState["InvoiceData"] == null) return;
            InvoiceData invoice = (InvoiceData)ViewState["InvoiceData"];

            UpdateGridPrices(invoice.Ingredients);

            decimal ingredientsTotal = 0;
            foreach (DataRow row in invoice.Ingredients.Rows)
                ingredientsTotal += Convert.ToDecimal(row["RequiredQuantity"]) * Convert.ToDecimal(row["UnitPrice"]);

            decimal deliveryCost = string.IsNullOrEmpty(txtDeliveryCost.Text) ? 0 : decimal.Parse(txtDeliveryCost.Text);
            decimal finalPrice = ingredientsTotal + deliveryCost;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Invoices 
                                 (BatchId, Product, Customer, OrderRef, InvoiceType, IngredientsCost, DeliveryCost, FinalPrice, CreatedAt) 
                                 VALUES (@BatchId, @Product, @Customer, @OrderRef, @InvoiceType, @IngredientsCost, @DeliveryCost, @FinalPrice, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BatchId", invoice.BatchId);
                cmd.Parameters.AddWithValue("@Product", invoice.Product);
                cmd.Parameters.AddWithValue("@Customer", invoice.Customer);
                cmd.Parameters.AddWithValue("@OrderRef", invoice.OrderRef);
                cmd.Parameters.AddWithValue("@InvoiceType", invoiceType);
                cmd.Parameters.AddWithValue("@IngredientsCost", ingredientsTotal);
                cmd.Parameters.AddWithValue("@DeliveryCost", deliveryCost);
                cmd.Parameters.AddWithValue("@FinalPrice", finalPrice);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            lblMessage.Text = $"✅ {invoiceType} Invoice saved successfully.";
        }
    }
}
