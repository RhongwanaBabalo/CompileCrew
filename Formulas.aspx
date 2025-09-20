<%@ Page Title="Formulas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Formulas.aspx.cs" Inherits="CompileCrew1.Formulas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="text-3xl font-bold text-gray-800 mb-6">Formula Management</h2>

    <!-- Select Product -->
    <div class="mb-6 grid grid-cols-1 md:grid-cols-3 gap-4">
        <asp:DropDownList ID="ddlProduct" runat="server" CssClass="p-2 border rounded"></asp:DropDownList>
        <asp:Button ID="btnViewIngredients" runat="server" Text="View Ingredients"
            CssClass="bg-blue-600 text-white px-4 py-2 rounded" OnClick="btnViewIngredients_Click" />
    </div>

    <!-- Add Custom Ingredient -->
    <div class="mb-6 grid grid-cols-1 md:grid-cols-3 gap-4">
        <asp:TextBox ID="txtCustomIngredient" runat="server" CssClass="p-2 border rounded" Placeholder="Custom Ingredient"></asp:TextBox>
        <asp:TextBox ID="txtCustomQuantity" runat="server" CssClass="p-2 border rounded" Placeholder="Quantity per Unit"></asp:TextBox>
        <asp:Button ID="btnAddCustomIngredient" runat="server" Text="Add Custom Ingredient"
            CssClass="bg-green-600 text-white px-4 py-2 rounded" OnClick="btnAddCustomIngredient_Click" />
    </div>

    <!-- Ingredient Grid -->
    <asp:GridView ID="gvIngredients" runat="server" AutoGenerateColumns="false"
        CssClass="w-full border border-gray-300 rounded-lg text-sm mb-6">
        <Columns>
            <asp:BoundField DataField="IngredientName" HeaderText="Ingredient"
                ItemStyle-CssClass="px-4 py-2" HeaderStyle-CssClass="px-4 py-2" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity per Unit"
                ItemStyle-CssClass="px-4 py-2 text-center" HeaderStyle-CssClass="px-4 py-2 text-center" />
        </Columns>
    </asp:GridView>

    <!-- Batch Size & Calculation -->
    <div class="mb-6 grid grid-cols-1 md:grid-cols-3 gap-4">
        <asp:TextBox ID="txtBatchSize" runat="server" CssClass="p-2 border rounded" Placeholder="Batch Size"></asp:TextBox>
        <asp:Button ID="btnCalculateBatch" runat="server" Text="Calculate Batch"
            CssClass="bg-purple-600 text-white px-4 py-2 rounded" OnClick="btnCalculateBatch_Click" />
        <asp:Button ID="btnSaveBatch" runat="server" Text="Save Batch"
            CssClass="bg-green-600 text-white px-4 py-2 rounded" OnClick="btnSaveBatch_Click" />
    </div>

    <!-- Final Batch Calculation -->
    <asp:GridView ID="gvBatchCalc" runat="server" AutoGenerateColumns="false"
        CssClass="w-full border border-gray-300 rounded-lg text-sm">
        <Columns>
            <asp:BoundField DataField="IngredientName" HeaderText="Ingredient"
                ItemStyle-CssClass="px-4 py-2" HeaderStyle-CssClass="px-4 py-2" />
            <asp:BoundField DataField="RequiredQuantity" HeaderText="Required Quantity"
                ItemStyle-CssClass="px-4 py-2 text-center" HeaderStyle-CssClass="px-4 py-2 text-center" />
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMessage" runat="server" CssClass="text-green-600 font-semibold mt-4 block"></asp:Label>

</asp:Content>
