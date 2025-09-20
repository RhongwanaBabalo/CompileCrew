<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CompileCrew1.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

   
    <h2 class="text-3xl font-bold text-gray-800 mb-6">Manufacturing Dashboard</h2>

   
    <div class="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
        <div class="bg-white p-6 rounded-xl shadow-lg card-hover">
            <p class="text-gray-600 text-sm">Active Orders</p>
            <asp:Label ID="lblOrders" runat="server" CssClass="text-3xl font-bold text-blue-600"></asp:Label>
        </div>
        <div class="bg-white p-6 rounded-xl shadow-lg card-hover">
            <p class="text-gray-600 text-sm">Batches in Production</p>
            <asp:Label ID="lblBatches" runat="server" CssClass="text-3xl font-bold text-green-600"></asp:Label>
        </div>
    </div>

   
    <div class="bg-white p-6 rounded-xl shadow-lg mb-8">
        <h3 class="text-xl font-semibold mb-4">Recent Orders</h3>
        <asp:GridView ID="gvRecentOrders" runat="server" AutoGenerateColumns="false"
            CssClass="w-full text-sm border border-gray-200 text-left"
            HeaderStyle-CssClass="bg-gray-100 text-gray-700 font-semibold"
            RowStyle-CssClass="border-t border-gray-200"
            AlternatingRowStyle-CssClass="bg-gray-50"
           
            DataKeyNames="OrderId">

            <Columns>
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
                        <asp:Button ID="btnCompleteRecent" runat="server" Text="Complete"
                            CommandName="CompleteOrder" CommandArgument='<%# Eval("OrderId") %>'
                            CssClass="bg-green-600 hover:bg-green-700 text-white px-3 py-1 rounded-lg" />
                    </ItemTemplate>
                    <ItemStyle CssClass="px-4 py-2 text-center" />
                    <HeaderStyle CssClass="px-4 py-2 text-center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

   
   <div class="bg-white p-6 rounded-xl shadow-lg">
    <h3 class="text-xl font-semibold mb-4">Production Overview</h3>
    <!-- Smaller canvas -->
    <canvas id="productionChart" width="500" height="250"></canvas>
</div>

<script>
    document.addEventListener("DOMContentLoaded", () => {
        const ctx = document.getElementById('productionChart').getContext('2d');
        new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: ['Dishwashing Liquid', 'Pine Gel', 'Other Products'],
                datasets: [{
                    data: [45, 35, 20], // Placeholder, can be filled from DB later
                    backgroundColor: ['#3b82f6', '#10b981', '#f59e0b'],
                    borderWidth: 0
                }]
            },
            options: {
                plugins: {
                    legend: { position: 'bottom', labels: { boxWidth: 20, padding: 30 } }
                },
                responsive: false, // Keep chart size fixed
                maintainAspectRatio: false
            }
        });
    });
</script>


</asp:Content>
