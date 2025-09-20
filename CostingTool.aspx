<%@ Page Title="Costing Tool" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CostingTool.aspx.cs" Inherits="CompileCrew1.CostingTool" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="text-3xl font-bold text-gray-800 mb-6">Costing Tool</h2>

   
    <div class="mb-6 grid grid-cols-1 md:grid-cols-2 gap-4">
        <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="true" 
            OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" CssClass="p-2 border rounded w-full">
        </asp:DropDownList>

        <asp:TextBox ID="txtBatchSize" runat="server" CssClass="p-2 border rounded w-full" Placeholder="Batch Size (units)"></asp:TextBox>
    </div>

  
    <asp:GridView ID="gvIngredients" runat="server" AutoGenerateColumns="False"
        CssClass="w-full border border-gray-300 rounded-lg text-sm mb-6"
        GridLines="None">
        <Columns>
         
            <asp:BoundField DataField="Ingredient" HeaderText="Ingredient" 
                HeaderStyle-CssClass="bg-gray-200 p-2 text-left" ItemStyle-CssClass="p-2 text-left" />

           
            <asp:TemplateField HeaderText="Quantity (per unit)">
                <HeaderStyle CssClass="bg-gray-200 p-2 text-right" HorizontalAlign="Right" />
                <ItemStyle CssClass="p-2 text-right" HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity","{0:N2}") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

           
            <asp:TemplateField HeaderText="Price per Unit (R)">
                <HeaderStyle CssClass="bg-gray-200 p-2 text-right" HorizontalAlign="Right" />
                <ItemStyle CssClass="p-2 text-right" HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:TextBox ID="txtPricePerUnit" runat="server" CssClass="p-2 border rounded w-24 text-right"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

   
    <div class="mb-6 grid grid-cols-1 md:grid-cols-3 gap-4">
        <asp:TextBox ID="txtContainerCost" runat="server" CssClass="p-2 border rounded w-full" Placeholder="Container Cost (per unit)"></asp:TextBox>
        <asp:TextBox ID="txtLabelCost" runat="server" CssClass="p-2 border rounded w-full" Placeholder="Label/Sticker Cost (per unit)"></asp:TextBox>
        <asp:TextBox ID="txtTransportCost" runat="server" CssClass="p-2 border rounded w-full" Placeholder="Transport Cost (total)"></asp:TextBox>
    </div>

    <asp:Button ID="btnCalculateCost" runat="server" Text="Calculate Batch Cost" CssClass="bg-green-600 text-white px-6 py-3 rounded hover:bg-green-700" OnClick="btnCalculateCost_Click" />

    <div class="mt-6 border p-4 rounded bg-gray-100">
        <p class="flex justify-between"><strong>Ingredient Cost:</strong> <asp:Label ID="lblIngredientCost" runat="server" Text="R0.00"></asp:Label></p>
        <p class="flex justify-between"><strong>Container Cost:</strong> <asp:Label ID="lblContainerCost" runat="server" Text="R0.00"></asp:Label></p>
        <p class="flex justify-between"><strong>Label Cost:</strong> <asp:Label ID="lblLabelCost" runat="server" Text="R0.00"></asp:Label></p>
        <p class="flex justify-between"><strong>Transport Cost:</strong> <asp:Label ID="lblTransportCost" runat="server" Text="R0.00"></asp:Label></p>
        <hr class="my-2 border-gray-300" />
        <p class="flex justify-between"><strong>Total Manufacturing Cost:</strong> <asp:Label ID="lblTotalCost" runat="server" Text="R0.00"></asp:Label></p>
        <p class="flex justify-between"><strong>Cost per Unit:</strong> <asp:Label ID="lblCostPerUnit" runat="server" Text="R0.00"></asp:Label></p>
        <p class="flex justify-between"><strong>Selling Price (50% markup):</strong> <asp:Label ID="lblSellingPrice" runat="server" Text="R0.00"></asp:Label></p>
        <p class="flex justify-between"><strong>Total Revenue:</strong> <asp:Label ID="lblTotalRevenue" runat="server" Text="R0.00"></asp:Label></p>
    </div>

</asp:Content>
