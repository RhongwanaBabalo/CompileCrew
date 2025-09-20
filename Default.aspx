<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CompileCrew2.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Formula Viewer</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Select a Product</h2>
            <asp:DropDownList ID="ddlProducts" runat="server" AutoPostBack="true" 
                OnSelectedIndexChanged="ddlProducts_SelectedIndexChanged"></asp:DropDownList>
            
            <h3>Ingredients</h3>
            <asp:GridView ID="gvIngredients" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Ingredient" HeaderText="Ingredient" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
