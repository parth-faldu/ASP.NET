<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="settings.aspx.cs" Inherits="Budget_Budddy.pages.settings" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Settings - Profile Management</title>
    <script type="text/javascript">
        function confirmDelete() {
            return confirm("Are you sure you want to permanently delete your account? This action cannot be undone.");
        }
    </script>
    <!-- External Stylesheets -->
    <link rel="stylesheet" href="../Styles/settings.css?v=1.1" />
    <link rel="stylesheet" href="../Styles/header.css" />
    <link rel="stylesheet" href="../Styles/sidebar.css" />
    <link rel="stylesheet" href="../Styles/baseStyle.css" />
    <link rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.7.2/css/all.min.css"
          integrity="sha512-Evv84Mr4kqVGRNSgIGL/F/aIDqQb7xQ2vcrdIwxfjThSH8CSR7PBEakCr51Ck+w+/U6swU2Im1vVX0SVk9ABhg=="
          crossorigin="anonymous"
          referrerpolicy="no-referrer" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- Sidebar -->
        <div class="sidebar">
            <ul>
                <li><a href="dashboard.aspx"><i class="fa-solid fa-chart-line"></i> Dashboard</a></li>
                <li><a href="addExpense.aspx"><i class="fa-solid fa-wallet"></i> Add Expenses</a></li>
                <li><a href="manageExpense.aspx"><i class="fa-solid fa-wallet"></i> Manage Expenses</a></li>
                <li><a href="budgetSuggestion.aspx"><i class="fa-solid fa-coins"></i> Budget AI</a></li>
                <li><a href="reports.aspx"><i class="fa-solid fa-file-alt"></i> Reports</a></li>
                <li><a href="settings.aspx"><i class="fa-solid fa-cog"></i> Settings</a></li>
                <li>
                    <asp:LinkButton ID="btnLogoutSidebar" runat="server" OnClick="btnLogout_Click">
                        <i class="fa-solid fa-sign-out-alt"></i> Logout
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
        <!-- Main Content -->
        <div class="main-content">
            <!-- Header -->
            <div class="header">
                <h2>Budget Buddy</h2>
                <div class="user-settings-con">
                    <h2>Welcome, <asp:Literal ID="litUsername" runat="server"></asp:Literal></h2>
                </div>
            </div>
            <!-- Settings Form Container -->
            <div class="container">
                <h2>Profile Settings</h2>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                
                <!-- Row Layout for Update and Delete Sections -->
                <div class="row">
                    <!-- Update Profile Section -->
                    <div class="update-section">
                        <h3>Update Profile</h3>
                        <div class="form-group">
                            <label for="txtUsername">Username:</label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtEmail">Email:</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="input-field"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtPassword">New Password :</label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-field"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnUpdate" runat="server" Text="Update Profile" OnClick="btnUpdate_Click" CssClass="action-btn" />
                        </div>
                    </div>
                    
                    <!-- Delete Profile Section -->
                    <div class="delete-section">
                        <h3>Delete Profile</h3>
                        <p>This action is irreversible. Your account and all related data will be permanently deleted.</p>
                        <div class="form-group">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete Account" OnClick="btnDelete_Click"
                                OnClientClick="return confirmDelete();" CssClass="delete-btn" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
