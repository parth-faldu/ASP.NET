<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="Budget_Budddy.SignUp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Budget Buddy - Sign Up</title>
    <link
      href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap"
      rel="stylesheet" />
      <script type="text/javascript">
          var rbUserID = '<%= rbUser.ClientID %>';
          var rbAdminID = '<%= rbAdmin.ClientID %>';
      </script>

    <!-- Use the same CSS file as your login page or copy the same rules -->
    <link rel="stylesheet" href="../Styles/signup.css" />
  </head>
  <body>
    <form id="form1" runat="server">
      <div class="login-container">
        <h2>Create an Account</h2>
        
        <!-- Error Message Label -->
        <asp:Label
          ID="lblError"
          runat="server"
          CssClass="error-label"
          ForeColor="Red"></asp:Label>
        
        <!-- Toggle Account Type (User/Admin) -->
        <div class="login-type-container">
          <!-- Add 'active' class by default to the User label -->
          <label class="toggle-option active" for="<%= rbUser.ClientID %>">User</label>
          <asp:RadioButton
            ID="rbUser"
            runat="server"
            GroupName="AccountType"
            CssClass="toggle-input"
            Checked="True"
            style="display:none;" />
          <label class="toggle-option" for="<%= rbAdmin.ClientID %>">Admin</label>
          <asp:RadioButton
            ID="rbAdmin"
            runat="server"
            GroupName="AccountType"
            CssClass="toggle-input"
            style="display:none;" />
        </div>
        
        <!-- Username Field -->
        <div class="form-group">
          <asp:TextBox
            ID="txtUsername"
            runat="server"
            CssClass="input-field"
            placeholder="Username"
            autocomplete="off"></asp:TextBox>
        </div>
        
        <!-- Email Field -->
        <div class="form-group">
          <asp:TextBox
            ID="txtEmail"
            runat="server"
            CssClass="input-field"
            TextMode="Email"
            placeholder="Email"
            autocomplete="off"></asp:TextBox>
        </div>
        
        <!-- Password Field -->
        <div class="form-group">
          <asp:TextBox
            ID="txtPassword"
            runat="server"
            CssClass="input-field"
            TextMode="Password"
            placeholder="Password"
            autocomplete="off"></asp:TextBox>
        </div>
        
        <!-- Confirm Password Field -->
        <div class="form-group">
          <asp:TextBox
            ID="txtConfirmPassword"
            runat="server"
            CssClass="input-field"
            TextMode="Password"
            placeholder="Confirm Password"
            autocomplete="off"></asp:TextBox>
        </div>
        
        <!-- Sign Up Button -->
        <div class="form-group">
          <asp:Button
            ID="btnSignUp"
            runat="server"
            CssClass="btn-login"
            Text="Sign Up"
            OnClick="btnSignUp_Click" />
        </div>
        
        <!-- Footer Text -->
        <div class="footer-text">
          <p>
            Already have an account?
            <asp:HyperLink
              ID="lnkLogin"
              runat="server"
              NavigateUrl="../index.aspx">Login</asp:HyperLink>
          </p>
        </div>
      </div>
    </form>
    
    <!-- JavaScript to toggle the active state of the labels -->
    <script type="text/javascript" src="../javascript/signup.js"></script>
  </body>
</html>
