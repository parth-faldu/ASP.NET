<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="Budget_Budddy.SignUp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget Buddy - Sign Up</title>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="style.css"/>
</head>
<body class="money-bg">
    <form id="form1" runat="server">
        <div class="sign-up-container">
            <h2>Sign Up for Budget Buddy</h2>

            <div class="form-group">
                <input type="text" id="txtUsername" runat="server" placeholder="Username" required autocomplete="off" />
            </div>

            <div class="form-group">
                <input type="email" id="txtEmail" runat="server" placeholder="Email" required autocomplete="off"/>
            </div>

            <div class="form-group">
                <input type="password" id="txtPassword" runat="server" placeholder="Password" required autocomplete="off"/>
            </div>

            <div class="form-group">
                <input type="password" id="txtConfirmPassword" runat="server" placeholder="Confirm Password" required autocomplete="off"/>
            </div>

            <button type="submit" class="btn-sign-up" id="btnSignUp" runat="server" OnClick="btnSignUp_Click">Sign Up</button>

            <div class="footer-text">
                <p>Already have an account? <a href="index.aspx">Login here</a></p>
            </div>
        </div>
    </form>
</body>
</html>
