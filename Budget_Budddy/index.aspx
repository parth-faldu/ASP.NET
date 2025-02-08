<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Budget_Budddy.Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Budget Buddy - Login</title>
    <link
      href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap"
      rel="stylesheet" />
    <link rel="stylesheet" href="Styles/index.css" />
    <!-- Pass radio button client IDs as global variables -->
    <script type="text/javascript">
        var rbUserID = '<%= rbUser.ClientID %>';
      var rbAdminID = '<%= rbAdmin.ClientID %>';
    </script>
    <script type="text/javascript" src="javascript/index.js"></script>
  </head>
  <body>
    <form id="form1" runat="server">
      <div class="login-container">
        <h2>Login to Budget Buddy</h2>
        
        <asp:Label ID="lblError" runat="server" CssClass="error-label" ForeColor="Red"></asp:Label>
        
        <!-- Toggle Login Type -->
        <div class="login-type-container">
          <label class="toggle-option" for="<%= rbUser.ClientID %>">User</label>
          <asp:RadioButton ID="rbUser" runat="server" GroupName="LoginType" CssClass="toggle-input" Checked="True" style="display:none;" />
          <label class="toggle-option" for="<%= rbAdmin.ClientID %>">Admin</label>
          <asp:RadioButton ID="rbAdmin" runat="server" GroupName="LoginType" CssClass="toggle-input" style="display:none;" />
        </div>
        
        <!-- Username Field -->
        <div class="form-group">
          <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" placeholder="Username" autocomplete="off"></asp:TextBox>
        </div>
        
        <!-- Password Field -->
        <div class="form-group">
          <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder="Password" autocomplete="off"></asp:TextBox>
        </div>
        
        <!-- Login Button -->
        <div class="form-group">
          <asp:Button ID="btnLogin" runat="server" CssClass="btn-login" Text="Login" OnClick="btnLogin_Click" />
        </div>
        
        <!-- Footer Text -->
        <div class="footer-text">
          <p>
            Don't have an account?
            <asp:HyperLink ID="lnkSignUp" runat="server" NavigateUrl="pages/SignUp.aspx">Sign Up</asp:HyperLink>
          </p>
        </div>
      </div>
    </form>
  </body>
</html>
