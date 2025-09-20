<%@ Page Title="Batches" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Batches.aspx.cs" Inherits="CompileCrew1.Batches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="text-3xl font-bold text-gray-800 mb-6">Batch Management</h2>

    <!-- Filter -->
    <div class="mb-6 grid grid-cols-1 md:grid-cols-4 gap-4">
        <asp:TextBox ID="txtSearchProduct" runat="server" CssClass="p-2 border rounded" Placeholder="Product"></asp:TextBox>
        <asp:DropDownList ID="ddlSearchStatus" runat="server" CssClass="p-2 border rounded">
            <asp:ListItem Text="All" Value=""></asp:ListItem>
            <asp:ListItem Text="In Progress" Value="In Progress"></asp:ListItem>
            <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="txtSearchDate" runat="server" CssClass="p-2 border rounded" TextMode="Date"></asp:TextBox>
        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="bg-blue-600 text-white px-4 py-2 rounded" OnClick="btnFilter_Click" />
    </div>

    <!-- Schedule Batch -->
    <div class="mb-6 grid grid-cols-1 md:grid-cols-3 gap-4">
        <asp:DropDownList ID="ddlProduct" runat="server" CssClass="p-2 border rounded"></asp:DropDownList>
        <asp:TextBox ID="txtBatchSize" runat="server" CssClass="p-2 border rounded" Placeholder="Batch Size"></asp:TextBox>
        <asp:TextBox ID="txtStartDate" runat="server" CssClass="p-2 border rounded" TextMode="Date"></asp:TextBox>
        <asp:Button ID="btnScheduleBatch" runat="server" Text="Schedule Batch" CssClass="bg-green-600 text-white px-4 py-2 rounded" OnClick="btnScheduleBatch_Click" />
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="text-green-600 font-semibold mb-4 block"></asp:Label>

    <!-- GridView -->
    <asp:GridView ID="gvBatches" runat="server" AutoGenerateColumns="false"
        CssClass="w-full border border-gray-300 rounded-lg text-sm"
        OnRowCommand="gvBatches_RowCommand">
        <Columns>
           <asp:TemplateField HeaderText="Batch ID">
    <ItemTemplate>
        BTC<%# Eval("BatchId", "{0:D3}") %>
    </ItemTemplate>
    <ItemStyle CssClass="px-4 py-2 text-center" />
    <HeaderStyle CssClass="px-4 py-2 text-center" />
</asp:TemplateField>


            <asp:BoundField DataField="Product" HeaderText="Product" 
                ItemStyle-CssClass="px-4 py-2" HeaderStyle-CssClass="px-4 py-2" />
            <asp:BoundField DataField="Size" HeaderText="Batch Size" 
                ItemStyle-CssClass="px-4 py-2 text-center" HeaderStyle-CssClass="px-4 py-2 text-center" />
            <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:yyyy-MM-dd}"
                ItemStyle-CssClass="px-4 py-2 text-center" HeaderStyle-CssClass="px-4 py-2 text-center" />
            <asp:BoundField DataField="Status" HeaderText="Status"
                ItemStyle-CssClass="px-4 py-2 text-center" HeaderStyle-CssClass="px-4 py-2 text-center" />

            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnComplete" runat="server" Text="Complete"
                        CommandName="CompleteBatch"
                        CommandArgument='<%# Eval("BatchId") %>'
                        CssClass="bg-green-500 text-white px-2 py-1 rounded" />
                </ItemTemplate>
                <ItemStyle CssClass="px-4 py-2 text-center" />
                <HeaderStyle CssClass="px-4 py-2 text-center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
