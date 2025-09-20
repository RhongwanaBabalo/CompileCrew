<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CompileCrew1.Register" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Best Results - Register</title>

    <!-- Tailwind -->
    <script src="https://cdn.tailwindcss.com"></script>

    <style>
        body { font-family: 'Inter', sans-serif; }
        .auth-bg {
            background: linear-gradient(135deg, #43cea2 0%, #185a9d 100%);
        }
        .glass-effect {
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(10px);
            border: 1px solid rgba(255, 255, 255, 0.2);
        }
        .modal {
            display: none;
        }
    </style>
</head>
<body class="auth-bg min-h-screen flex items-center justify-center">
    <form id="form1" runat="server" class="w-full max-w-md glass-effect rounded-2xl shadow-2xl p-8 fade-in">

        <!-- Header -->
        <div class="text-center mb-6">
            <div class="w-20 h-20 bg-green-600 rounded-full flex items-center justify-center mx-auto mb-4">
                <span class="text-white font-bold text-3xl">BR</span>
            </div>
            <h1 class="text-2xl font-bold text-gray-800">Create Account</h1>
            <p class="text-gray-600">Register for Best Results Manufacturing System</p>
        </div>

        <!-- First Name -->
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">First Name</label>
            <asp:TextBox ID="txtFirstName" runat="server"
                CssClass="w-full p-4 border border-gray-300 rounded-xl focus:ring-2 focus:ring-green-500"
                placeholder="Enter first name"></asp:TextBox>
        </div>

        <!-- Last Name -->
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Last Name</label>
            <asp:TextBox ID="txtLastName" runat="server"
                CssClass="w-full p-4 border border-gray-300 rounded-xl focus:ring-2 focus:ring-green-500"
                placeholder="Enter last name"></asp:TextBox>
        </div>

        <!-- Email -->
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Email</label>
            <asp:TextBox ID="txtEmail" runat="server"
                CssClass="w-full p-4 border border-gray-300 rounded-xl focus:ring-2 focus:ring-green-500"
                placeholder="Enter email"></asp:TextBox>
        </div>

        <!-- Phone -->
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Phone</label>
            <asp:TextBox ID="txtPhone" runat="server"
                CssClass="w-full p-4 border border-gray-300 rounded-xl focus:ring-2 focus:ring-green-500"
                placeholder="Enter phone number"></asp:TextBox>
        </div>

        <!-- Password -->
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                CssClass="w-full p-4 border border-gray-300 rounded-xl focus:ring-2 focus:ring-green-500"
                placeholder="Enter password"></asp:TextBox>
        </div>

        <!-- Confirm Password -->
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Confirm Password</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"
                CssClass="w-full p-4 border border-gray-300 rounded-xl focus:ring-2 focus:ring-green-500"
                placeholder="Confirm password"></asp:TextBox>
        </div>

        <!-- Role -->
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">User Role</label>
            <asp:DropDownList ID="ddlRole" runat="server"
                CssClass="w-full p-4 border border-gray-300 rounded-xl focus:ring-2 focus:ring-green-500">
                <asp:ListItem Text="Admin" Value="admin"></asp:ListItem>
                <asp:ListItem Text="Operator" Value="operator"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- Agree Terms -->
        <div class="mb-4 flex items-center">
            <asp:CheckBox ID="chkAgreeTerms" runat="server" CssClass="mr-2" />
            <label for="chkAgreeTerms" class="text-sm text-gray-600">
                I agree to the 
                <a href="javascript:void(0)" onclick="showTerms()" class="text-green-600 hover:text-green-800">
                    Terms & Conditions
                </a>
            </label>
        </div>

        <!-- Register Button -->
        <asp:Button ID="btnRegister" runat="server" Text="Register"
            CssClass="w-full bg-green-600 hover:bg-green-700 text-white py-4 rounded-xl font-semibold transition-all duration-300 transform hover:scale-105"
            OnClick="btnRegister_Click" />

        <!-- Error Message -->
        <asp:Label ID="lblMessage" runat="server" CssClass="block text-center text-red-600 mt-4"></asp:Label>

        <!-- Link to Login -->
        <div class="mt-6 text-center">
            <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Login.aspx"
                CssClass="text-blue-600 hover:text-blue-800 text-sm">Already have an account? Sign in</asp:HyperLink>
        </div>
    </form>

    <!-- Terms Modal -->
    <div id="termsModal" class="modal fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
        <div class="bg-white rounded-xl shadow-2xl p-6 w-full max-w-lg">
            <h2 class="text-xl font-bold mb-4">Terms & Conditions</h2>
            <p class="text-gray-600 mb-4">
                By registering to Best Results Manufacturing System, you agree to comply with our 
                data policies, security rules, and user responsibilities. Misuse of the system 
                may result in account suspension or termination.
            </p>
            <button onclick="closeTerms()" class="w-full bg-green-600 hover:bg-green-700 text-white py-2 rounded-xl font-semibold">
                Close
            </button>
        </div>
    </div>

    <script>
        function showTerms() {
            document.getElementById("termsModal").style.display = "flex";
        }
        function closeTerms() {
            document.getElementById("termsModal").style.display = "none";
        }
    </script>
</body>
</html>
