 <%@ Page Title="Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="CompileCrew1.Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Heading -->
    <h2 class="text-3xl font-bold text-gray-800 mb-6">Order Management</h2>

    <!-- Order Entry Form -->
    <div class="bg-white p-6 rounded-xl shadow-lg mb-8">
        <h3 class="text-xl font-semibold mb-4">New Order</h3>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">

            <!-- Customer -->
            <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Customer</label>
                <asp:TextBox ID="txtCustomer" runat="server"
                    CssClass="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                    placeholder="Enter customer name"></asp:TextBox>
            </div>

          <!-- Product -->
<div>
    <label class="block text-sm font-medium text-gray-700 mb-2">Product</label>
    <asp:DropDownList ID="ddlProduct" runat="server"
        CssClass="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500">
    </asp:DropDownList>
</div>


            <!-- Quantity -->
            <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Quantity</label>
                <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number"
                    CssClass="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
                    placeholder="0"></asp:TextBox>
            </div>

            <!-- Due Date -->
            <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Due Date</label>
                <asp:TextBox ID="txtDueDate" runat="server" TextMode="Date"
                    CssClass="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"></asp:TextBox>
            </div>
        </div>

        <!-- Submit -->
        <div class="mt-6">
            <asp:Button ID="btnAddOrder" runat="server" Text="Add Order"
                CssClass="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg font-semibold transition-all duration-300 transform hover:scale-105"
                OnClick="btnAddOrder_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="ml-4 text-green-600 font-medium"></asp:Label>
        </div>
    </div>

    <!-- Orders List -->
    <div class="bg-white p-6 rounded-xl shadow-lg">
        <h3 class="text-xl font-semibold mb-4">Existing Orders</h3>

        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" DataKeyNames="OrderId"
            CssClass="w-full text-sm border border-gray-200 text-left"
            HeaderStyle-CssClass="bg-gray-100 text-gray-700 font-semibold"
            RowStyle-CssClass="border-t border-gray-200"
            AlternatingRowStyle-CssClass="bg-gray-50"
            OnRowCommand="gvOrders_RowCommand">

            <Columns>
                <asp:BoundField DataField="OrderNumber" HeaderText="Order ID"
    ItemStyle-CssClass="px-4 py-2 text-center" HeaderStyle-CssClass="px-4 py-2 text-center" />

                <asp:BoundField DataField="Customer" HeaderText="Customer"
                    ItemStyle-CssClass="px-4 py-2" HeaderStyle-CssClass="px-4 py-2" />
                <asp:BoundField DataField="Product" HeaderText="Product"
                    ItemStyle-CssClass="px-4 py-2" HeaderStyle-CssClass="px-4 py-2" />
                <asp:BoundField DataField="Quantity" HeaderText="Qty"
                    ItemStyle-CssClass="px-4 py-2 text-center" HeaderStyle-CssClass="px-4 py-2 text-center" />
                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:yyyy-MM-dd}"
                    ItemStyle-CssClass="px-4 py-2 text-center" HeaderStyle-CssClass="px-4 py-2 text-center" />
                <asp:BoundField DataField="Status" HeaderText="Status"
                    ItemStyle-CssClass="px-4 py-2" HeaderStyle-CssClass="px-4 py-2" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnComplete" runat="server" Text="Complete"
                            CommandName="CompleteOrder" CommandArgument='<%# Eval("OrderId") %>'
                            CssClass="bg-green-600 hover:bg-green-700 text-white px-3 py-1 rounded-lg" />
                    </ItemTemplate>
                    <ItemStyle CssClass="px-4 py-2 text-center" />
                    <HeaderStyle CssClass="px-4 py-2 text-center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>