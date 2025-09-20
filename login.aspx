<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CompileCrew1.Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Best Results - Login</title>

    <!-- Tailwind -->
    <script src="https://cdn.tailwindcss.com"></script>

    <style>
        body { font-family: 'Inter', sans-serif; }
        .auth-bg {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        }
        .glass-effect {
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(10px);
            border: 1px solid rgba(255, 255, 255, 0.2);
        }
    </style>
</head>
<body class="auth-bg min-h-screen flex items-center justify-center">
    <form id="form1" runat="server" class="w-full max-w-md glass-effect rounded-2xl shadow-2xl p-8 fade-in">

        <!-- Logo -->
        <div class="text-center mb-8">
            <div class="w-20 h-20 bg-blue-600 rounded-full flex items-center justify-center mx-auto mb-4">
                <span class="text-white font-bold text-3xl">BR</span>
            </div>
            <h1 class="text-3xl font-bold text-gray-800 mb-2">Welcome Back</h1>
            <p class="text-gray-600">Sign in to Best Results Manufacturing System</p>
        </div>

        <!-- Email -->
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Email Address</label>
            <asp:TextBox ID="txtEmail" runat="server"
                CssClass="w-full p-4 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500"
                placeholder="admin@bestresults.com"></asp:TextBox>
        </div>

        <!-- Password -->
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                CssClass="w-full p-4 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500"
                placeholder="Enter your password"></asp:TextBox>
        </div>

        <!-- Role -->
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">User Role</label>
            <asp:DropDownList ID="ddlRole" runat="server"
                CssClass="w-full p-4 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500">
                <asp:ListItem Text="Admin" Value="admin"></asp:ListItem>
                <asp:ListItem Text="Operator" Value="operator"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- Remember me + Forgot -->
        <div class="flex items-center justify-between mb-4">
            <label class="flex items-center">
                <asp:CheckBox ID="chkRemember" runat="server" CssClass="text-blue-600" />
                <span class="ml-2 text-sm text-gray-600">Remember me</span>
            </label>
            <asp:LinkButton ID="lnkForgot" runat="server"
                CssClass="text-sm text-blue-600 hover:text-blue-800">Forgot password?</asp:LinkButton>
        </div>

        <!-- Login Button -->
        <asp:Button ID="btnLogin" runat="server" Text="Sign In"
            CssClass="w-full bg-blue-600 hover:bg-blue-700 text-white py-4 rounded-xl font-semibold transition-all duration-300 transform hover:scale-105"
            OnClick="btnLogin_Click" />

        <!-- Error Message -->
        <asp:Label ID="lblMessage" runat="server" CssClass="block text-center text-red-600 mt-4"></asp:Label>

        <!-- Register Redirect -->
        <div class="mt-6 text-center">
            <p class="text-gray-600">Don’t have an account?
                <asp:HyperLink ID="lnkRegister" runat="server" 
                    NavigateUrl="~/Register.aspx" 
                    CssClass="text-blue-600 hover:text-blue-800 font-semibold">
                    Register here
                </asp:HyperLink>
            </p>
        </div>

        <!-- Forgot Password Modal -->
        <div id="forgotModal" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 hidden">
            <div class="bg-white rounded-xl p-6 w-full max-w-sm relative">
                <h2 class="text-xl font-bold mb-4">Reset Password</h2>
                <p class="text-gray-600 mb-4">Enter your registered email to reset your password.</p>
                <asp:TextBox ID="txtForgotEmail" runat="server" 
                    CssClass="w-full p-3 border border-gray-300 rounded-xl mb-4"
                    placeholder="Email Address"></asp:TextBox>
                <div class="flex justify-end gap-2">
                    <button type="button" onclick="closeForgotModal()" 
                        class="px-4 py-2 bg-gray-300 rounded-xl hover:bg-gray-400">Cancel</button>
                    <asp:Button ID="btnForgot" runat="server" Text="Submit" OnClick="btnForgot_Click"
                        CssClass="px-4 py-2 bg-blue-600 text-white rounded-xl hover:bg-blue-700"/>
                </div>
            </div>
        </div>

    </form>

    <!-- JavaScript for modal -->
    <script>
        // Show modal
        document.getElementById('<%= lnkForgot.ClientID %>').addEventListener('click', function(e){
            e.preventDefault();
            document.getElementById('forgotModal').classList.remove('hidden');
        });

        // Close modal
        function closeForgotModal() {
            document.getElementById('forgotModal').classList.add('hidden');
        }
    </script>
</body>
</html>
