<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Budget_Budddy.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget Buddy - Login</title>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="style.css"/>
</head>
<body class="money-bg">
    <form id="form1" runat="server">
        <div class="login-container">
            <h2>Login to Budget Buddy</h2>

            <asp:Label ID="lblError" runat="server" CssClass="error-label" ForeColor="Red"></asp:Label>

            <div class="form-group">
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" placeholder="Username" autocomplete="off"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder="Password" autocomplete="off"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Button ID="btnLogin" runat="server" CssClass="btn-login" Text="Login" OnClick="btnLogin_Click" />
            </div>

            <div class="footer-text">
                <p>Don't have an account? <asp:HyperLink ID="lnkSignUp" runat="server" NavigateUrl="pages/SignUp.aspx">Sign Up</asp:HyperLink>&nbsp;</p>
            </div>
        </div>
    </form>
</body>
</html>
