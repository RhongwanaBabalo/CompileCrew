<%@ Page Title="Invoice Generator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="CompileCrew1.Invoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="text-3xl font-bold text-gray-800 mb-6">Invoice Generator</h2>

    <!-- Batch Selection -->
    <div class="mb-6 grid grid-cols-1 md:grid-cols-2 gap-4">
        <asp:DropDownList ID="ddlBatch" runat="server" CssClass="p-2 border rounded w-full"></asp:DropDownList>
        <asp:TextBox ID="txtDeliveryCost" runat="server" CssClass="p-2 border rounded w-full" Placeholder="Delivery Cost" oninput="updateTotals()"></asp:TextBox>
    </div>

    <!-- Customer and Order Ref -->
    <div class="mb-6 grid grid-cols-1 md:grid-cols-2 gap-4">
        <asp:TextBox ID="txtCustomer" runat="server" CssClass="p-2 border rounded w-full" Placeholder="Customer Name"></asp:TextBox>
        <asp:TextBox ID="txtOrderRef" runat="server" CssClass="p-2 border rounded w-full" Placeholder="Order Reference"></asp:TextBox>
    </div>

    <!-- Buttons -->
    <div class="mb-6 flex gap-4 flex-wrap">
        <asp:Button ID="btnGenerateInvoice" runat="server" Text="Generate Invoice" CssClass="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700" OnClick="btnGenerateInvoice_Click" />
        <asp:Button ID="btnDownloadPDF" runat="server" Text="Download PDF" CssClass="bg-green-600 text-white px-6 py-2 rounded hover:bg-green-700" OnClick="btnDownloadPDF_Click" Visible="false" />
        <asp:Button ID="btnSaveProforma" runat="server" Text="Save Proforma" CssClass="bg-gray-600 text-white px-6 py-2 rounded hover:bg-gray-700" OnClick="btnSaveProforma_Click" />
        <asp:Button ID="btnSaveFinal" runat="server" Text="Save Final" CssClass="bg-gray-800 text-white px-6 py-2 rounded hover:bg-gray-900" OnClick="btnSaveFinal_Click" />
    </div>

    <!-- Message -->
    <asp:Label ID="lblMessage" runat="server" CssClass="text-red-600 font-semibold"></asp:Label>

    <!-- Invoice Preview Panel -->
    <asp:Panel ID="pnlInvoice" runat="server" Visible="false" CssClass="mt-6 border p-4 rounded bg-gray-100">

        <h3 class="text-xl font-bold mb-4">Invoice Preview</h3>

        <p><strong>Batch:</strong> <asp:Label ID="lblBatchName" runat="server" Text=""></asp:Label></p>

        <!-- Totals -->
        <p><strong>Ingredients Total:</strong> <span id="spanIngredientsTotal">R0.00</span></p>
        <p><strong>Delivery Cost:</strong> <asp:TextBox ID="txtDeliveryCostPreview" runat="server" CssClass="p-2 border w-32" oninput="updateTotals()" /></p>
        <p><strong>Final Price:</strong> <span id="spanFinalTotal">R0.00</span></p>

        <hr class="my-4 border-gray-300" />

        <!-- Editable GridView -->
        <asp:GridView ID="gvInvoiceItems" runat="server" AutoGenerateColumns="False" CssClass="w-full border border-gray-300 rounded-lg text-sm"
            GridLines="Both">
            <Columns>
                <asp:BoundField DataField="IngredientName" HeaderText="Ingredient" HeaderStyle-CssClass="bg-gray-200 p-2" ItemStyle-CssClass="p-2" />
                <asp:BoundField DataField="RequiredQuantity" HeaderText="Quantity" DataFormatString="{0:N2}" HeaderStyle-CssClass="bg-gray-200 p-2 text-right" ItemStyle-CssClass="p-2 text-right" />

                <asp:TemplateField HeaderText="Unit Price (R)">
                    <HeaderStyle CssClass="bg-gray-200 p-2 text-right" />
                    <ItemStyle CssClass="p-2 text-right" />
                    <ItemTemplate>
                        <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="p-1 border w-24 text-right" Text='<%# Eval("UnitPrice") %>' oninput="updateTotals()" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Total (R)">
                    <HeaderStyle CssClass="bg-gray-200 p-2 text-right" />
                    <ItemStyle CssClass="p-2 text-right" />
                    <ItemTemplate>
                        <asp:Label ID="lblLineTotal" runat="server" CssClass="line-total">
                            <%# (Convert.ToDecimal(Eval("RequiredQuantity")) * Convert.ToDecimal(Eval("UnitPrice"))).ToString("C") %>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </asp:Panel>

    <!-- Live Totals Script -->
    <script>
        function updateTotals() {
            let grid = document.getElementById('<%= gvInvoiceItems.ClientID %>');
            let ingredientsTotal = 0;

            for (let i = 1; i < grid.rows.length; i++) { // Skip header row
                let qtyCell = grid.rows[i].cells[1];
                let priceInput = grid.rows[i].cells[2].querySelector('input');
                let lineTotalLabel = grid.rows[i].cells[3].querySelector('span, label');

                let qty = parseFloat(qtyCell.innerText) || 0;
                let price = parseFloat(priceInput.value) || 0;
                let lineTotal = qty * price;

                lineTotalLabel.innerText = 'R' + lineTotal.toFixed(2);
                ingredientsTotal += lineTotal;
            }

            document.getElementById('spanIngredientsTotal').innerText = 'R' + ingredientsTotal.toFixed(2);

            let deliveryCost = parseFloat(document.getElementById('<%= txtDeliveryCost.ClientID %>').value) || 0;
            let finalTotal = ingredientsTotal + deliveryCost;
            document.getElementById('spanFinalTotal').innerText = 'R' + finalTotal.toFixed(2);
        }

        window.onload = updateTotals;
    </script>

</asp:Content>
