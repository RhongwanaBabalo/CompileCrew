<%@ Page Title="Purchase Orders" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="PurchaseOrders.aspx.cs" Inherits="CompileCrew1.PurchaseOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Purchase Orders Section -->
    <div id="purchase" class="section fade-in">
        <h2 class="text-3xl font-bold text-gray-800 mb-6">Purchase Order Management</h2>
        
        <!-- Create / Edit Purchase Order Form -->
        <div class="bg-white rounded-xl shadow-lg p-6 mb-6">
            <h3 class="text-xl font-semibold mb-4">Create / Edit Purchase Order</h3>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <asp:HiddenField ID="hfPOId" runat="server" />

                <div>
                    <label class="block text-sm font-medium text-gray-700 mb-2">Supplier</label>
                    <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500">
                        <asp:ListItem>ChemSupply Co.</asp:ListItem>
                        <asp:ListItem>Industrial Chemicals Ltd</asp:ListItem>
                        <asp:ListItem>Packaging Solutions</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div>
                    <label class="block text-sm font-medium text-gray-700 mb-2">Item Name</label>
                    <asp:TextBox ID="txtItemName" runat="server" CssClass="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
                </div>

                <div>
                    <label class="block text-sm font-medium text-gray-700 mb-2">Quantity</label>
                    <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number" CssClass="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
                </div>

                <div>
                    <label class="block text-sm font-medium text-gray-700 mb-2">Unit</label>
                    <asp:DropDownList ID="ddlUnit" runat="server" CssClass="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500">
                        <asp:ListItem>Liters</asp:ListItem>
                        <asp:ListItem>Kilograms</asp:ListItem>
                        <asp:ListItem>Units</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div>
                    <label class="block text-sm font-medium text-gray-700 mb-2">Unit Cost</label>
                    <asp:TextBox ID="txtUnitCost" runat="server" TextMode="Number" CssClass="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
                </div>

                <div>
                    <label class="block text-sm font-medium text-gray-700 mb-2">Expected Delivery</label>
                    <asp:TextBox ID="txtExpectedDelivery" runat="server" TextMode="Date" CssClass="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500" />
                </div>

                <div class="md:col-span-2">
                    <asp:Button ID="btnSavePO" runat="server" Text="Save Purchase Order"
                        OnClick="btnSavePO_Click"
                        CssClass="bg-indigo-600 hover:bg-indigo-700 text-white px-6 py-3 rounded-lg font-medium transition-colors" />
                </div>
            </div>
        </div>

        <!-- Purchase Orders List -->
        <div class="bg-white rounded-xl shadow-lg p-6">
            <h3 class="text-xl font-semibold mb-4">Purchase Orders</h3>
            <div class="overflow-x-auto">
                <asp:GridView ID="gvPurchaseOrders" runat="server" AutoGenerateColumns="false" DataKeyNames="POId"
                    OnRowCommand="gvPurchaseOrders_RowCommand"
                    CssClass="w-full border-collapse"
                    HeaderStyle-CssClass="bg-gray-50 text-left text-sm font-medium text-gray-700"
                    RowStyle-CssClass="divide-y divide-gray-200">

                    <Columns>
                        <asp:BoundField DataField="POId" HeaderText="PO #" />
                        <asp:BoundField DataField="Supplier" HeaderText="Supplier" />
                        <asp:BoundField DataField="ItemName" HeaderText="Item" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                        <asp:BoundField DataField="Unit" HeaderText="Unit" />
                        <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="ExpectedDelivery" HeaderText="Expected Delivery" DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditPO" CommandArgument='<%# Eval("POId") %>'
                                    CssClass="text-blue-600 hover:text-blue-800 mr-2" />
                                <asp:Button ID="btnReceive" runat="server" Text="Receive" CommandName="ReceivePO" CommandArgument='<%# Eval("POId") %>'
                                    CssClass="text-green-600 hover:text-green-800" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
