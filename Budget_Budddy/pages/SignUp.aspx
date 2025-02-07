<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="Budget_Budddy.SignUp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget Buddy - Sign Up</title>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="../style.css"/>
</head>
<body class="money-bg">
    <form id="form1" runat="server">
        <div class="login-container">
            <h2>Create an Account</h2>

            <!-- 🔹 Error Message Label -->
            <asp:Label ID="lblError" runat="server" CssClass="error-label" ForeColor="Red"></asp:Label>

            <!-- 🔹 Username Field -->
            <div class="form-group">
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" placeholder="Username" autocomplete="off"></asp:TextBox>
            </div>

            <!-- 🔹 Email Field -->
            <div class="form-group">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" TextMode="Email" placeholder="Email" autocomplete="off"></asp:TextBox>
            </div>

            <!-- 🔹 Password Field -->
            <div class="form-group">
                <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder="Password" autocomplete="off"></asp:TextBox>
            </div>

            <!-- 🔹 Confirm Password Field -->
            <div class="form-group">
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder="Confirm Password" autocomplete="off"></asp:TextBox>
            </div>

            <!-- 🔹 Sign Up Button -->
            <div class="form-group">
                <asp:Button ID="btnSignUp" runat="server" CssClass="btn-login" Text="Sign Up" OnClick="btnSignUp_Click" />
            </div>

            <div class="footer-text">
                <p>Already have an account? <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="../index.aspx">Login</asp:HyperLink></p>
            </div>
        </div>
    </form>
</body>
</html>
