<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Budget_Budddy.Authentication" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget Buddy</title>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="style.css"/>
</head>
<body class="money-bg">
    <form id="form1" runat="server">
        <div class="login-container">
            <h2>Login to Budget Buddy</h2>
            
            <div class="form-group">
                <input type="text" id="txtUsername" runat="server" placeholder="Username" required autocomplete="off"/>
            </div>
            
            <div class="form-group">
                <input type="password" id="txtPassword" runat="server" placeholder="Password" required autocomplete="off" />
            </div>
            
            <button type="submit" class="btn-login" id="btnLogin" runat="server" OnClick="btnLogin_Click">Login</button>

            <div class="footer-text">
                <p>Don't have an account? <a href="SignUp.aspx">Sign Up</a></p>
            </div>
        </div>
    </form>

</body>
</html>
