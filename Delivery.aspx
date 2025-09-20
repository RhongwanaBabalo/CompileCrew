<%@ Page Title="Delivery Tracking" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeliveryTracking.aspx.cs" Inherits="CompileCrew1.DeliveryTracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="text-3xl font-bold text-gray-800 mb-6">Delivery Tracking</h2>

   
    <div class="bg-white rounded-xl shadow-lg p-6 mb-6">
        <h3 class="text-xl font-semibold mb-4">Update Delivery</h3>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">

            <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Select PO</label>
                <asp:DropDownList ID="ddlPOs" runat="server" CssClass="w-full p-3 border border-gray-300 rounded-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlPOs_SelectedIndexChanged"></asp:DropDownList>
            </div>

            <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Delivery Date</label>
                <asp:TextBox ID="txtDeliveryDate" runat="server" TextMode="Date" CssClass="w-full p-3 border border-gray-300 rounded-lg" />
            </div>

            <div class="md:col-span-3">
                <asp:Button ID="btnUpdateDelivery" runat="server" Text="Update Delivery" CssClass="bg-green-600 hover:bg-green-700 text-white px-6 py-3 rounded font-medium transition-colors" OnClick="btnUpdateDelivery_Click" />
            </div>
        </div>
    </div>

   
    <div class="bg-white rounded-xl shadow-lg p-6">
        <h3 class="text-xl font-semibold mb-4">Delivery Records</h3>
        <div class="overflow-x-auto">
            <asp:GridView ID="gvDeliveries" runat="server" AutoGenerateColumns="false"
                CssClass="w-full border-collapse"
                HeaderStyle-CssClass="bg-gray-50 text-left text-sm font-medium text-gray-700"
                RowStyle-CssClass="divide-y divide-gray-200"
                OnRowCommand="gvDeliveries_RowCommand">

                <Columns>
                    <asp:BoundField DataField="DeliveryId" HeaderText="ID" />
                    <asp:BoundField DataField="POId" HeaderText="PO #" />
                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" DataFormatString="{0:yyyy-MM-dd}" />

                    
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' CssClass='<%# GetStatusCssClass(Eval("Status").ToString()) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                  
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnMarkDelivered" runat="server" Text="Mark Delivered"
                                CommandName="MarkDelivered" CommandArgument='<%# Eval("DeliveryId") %>'
                                CssClass="bg-blue-600 hover:bg-blue-700 text-white px-4 py-1 rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
