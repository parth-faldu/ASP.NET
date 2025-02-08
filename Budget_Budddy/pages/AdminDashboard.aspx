<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="Budget_Budddy.AdminDashboard" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Admin Dashboard - Budget Buddy</title>
  <!-- Reference the dashboard.css file which is based on your provided CSS -->
  <link rel="stylesheet" type="text/css" href="../Styles/dashboard.css" />
  <link rel="stylesheet" type="text/css" href="../Styles/admin.css" />
</head>
<body>
  <form id="form1" runat="server">
    <!-- Header Section -->
    <div class="header">
      <h2>Budget Buddy - Admin Dashboard</h2>
      <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="logout-button" OnClick="btnLogout_Click" />
    </div>
    
    <!-- Main Dashboard Container -->
    <div class="dashboard-container">
      <!-- Left Section: User List -->
      <div class="left-section">
        <h3>User List</h3>
        <div class="table-container">
          <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="id"
              OnRowCommand="gvUsers_RowCommand">
            <Columns>
              <asp:BoundField DataField="id" HeaderText="ID" />
              <asp:BoundField DataField="username" HeaderText="Username" />
              <asp:BoundField DataField="email" HeaderText="Email" />
              <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                  <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteUser"
                      CommandArgument='<%# Eval("id") %>' 
                      OnClientClick="return confirm('Are you sure you want to delete this user?');">
                    Delete
                  </asp:LinkButton>
                </ItemTemplate>
              </asp:TemplateField>
            </Columns>
          </asp:GridView>
        </div>
      </div>
      
      <!-- Right Section: Add New User -->
      <div class="right-section">
        <h3>Add New User</h3>
        <div class="add-user-panel">
          <asp:TextBox ID="txtNewUsername" runat="server" CssClass="input-field" placeholder="Username"></asp:TextBox>
          <asp:TextBox ID="txtNewEmail" runat="server" CssClass="input-field" TextMode="Email" placeholder="Email"></asp:TextBox>
          <asp:TextBox ID="txtNewPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder="Password"></asp:TextBox>
          <asp:Button ID="btnAddUser" runat="server" Text="Add User" CssClass="btn-add" OnClick="btnAddUser_Click" />
        </div>
      </div>
    </div>
    
    <!-- Footer Section -->
    <div class="footer">
      <p>&copy; 2025 Budget Buddy. All rights reserved.</p>
    </div>
  </form>
</body>
</html>
