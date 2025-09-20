<%@ Page Title="Batch Details" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BatchDetails.aspx.cs" Inherits="CompileCrew1.BatchDetails" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-2xl font-bold text-blue-700 mb-4">Batch Details</h2>

    <!-- Select Batch -->
    <div class="mb-4">
        <asp:Label ID="lblBatch" runat="server" Text="Select Batch:" CssClass="block mb-1 font-semibold" />
        <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="true" 
            OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
            CssClass="border p-2 rounded w-64"></asp:DropDownList>
    </div>

    <!-- Entry Form -->
    <div class="grid grid-cols-3 gap-4 mb-4">
        <div>
            <asp:Label ID="lblIngredient" runat="server" Text="Ingredient:" CssClass="block mb-1 font-semibold" />
            <asp:TextBox ID="txtIngredient" runat="server" CssClass="border p-2 rounded w-full"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblQuantity" runat="server" Text="Quantity:" CssClass="block mb-1 font-semibold" />
            <asp:TextBox ID="txtQuantity" runat="server" CssClass="border p-2 rounded w-full"></asp:TextBox>
        </div>
        <div class="flex items-end">
            <asp:Button ID="btnAddDetail" runat="server" Text="➕ Add Detail" 
                OnClick="btnAddDetail_Click"
                CssClass="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg" />
        </div>
    </div>

    <!-- Success/Error Message -->
    <asp:Label ID="lblMessage" runat="server" CssClass="text-green-600 font-semibold mb-4 block"></asp:Label>

    <!-- Details Grid -->
    <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False"
    CssClass="table table-bordered table-hover"
    OnRowEditing="gvDetails_RowEditing"
    OnRowUpdating="gvDetails_RowUpdating"
    OnRowCancelingEdit="gvDetails_RowCancelingEdit"
    OnRowDeleting="gvDetails_RowDeleting"
    DataKeyNames="DetailId">

    <Columns>
        <asp:BoundField DataField="DetailId" HeaderText="ID" ReadOnly="True" />
        <asp:BoundField DataField="Ingredient" HeaderText="Ingredient" />
        <asp:BoundField DataField="Quantity" HeaderText="Quantity" DataFormatString="{0:N2}" />

        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" 
            EditText="✏️ Edit" DeleteText="🗑️ Delete" 
            UpdateText="💾 Save" CancelText="❌ Cancel" />
    </Columns>
</asp:GridView>
</asp:Content>
