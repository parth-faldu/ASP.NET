<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs"
    Inherits="Budget_Budddy.pages.dashboard" Async="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Budget Buddy - Dashboard</title>
    <!-- Google Fonts & Chart.js -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <link rel="stylesheet" href="../Styles/header.css" />
    <link rel="stylesheet" href="../Styles/sidebar.css?v=1.0" />
    <link rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.7.2/css/all.min.css"
          integrity="sha512-Evv84Mr4kqVGRNSgIGL/F/aIDqQb7xQ2vcrdIwxfjThSH8CSR7PBEakCr51Ck+w+/U6swU2Im1vVX0SVk9ABhg=="
          crossorigin="anonymous"
          referrerpolicy="no-referrer" />
    <link rel="stylesheet" href="../Styles/dashboard.css" />
    <link rel="stylesheet" href="../Styles/baseStyle.css" />

    <!-- Inline Script Block to Pass Server IDs as Global Variables -->
    <script type="text/javascript">
        var hiddenExpenseDataClientID = "<%= hiddenExpenseData.ClientID %>";
        var hiddenChartImageClientID = "<%= hiddenChartImage.ClientID %>";
    </script>

    <!-- External JavaScript File -->
    <script type="text/javascript" src="../javascript/dashboard.js?v=1.0"></script>
  </head>
  <body>
    <form id="form1" runat="server">
      <!-- ScriptManager is required for AJAX-enabled controls (UpdatePanel, UpdateProgress) -->
      <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <!-- Sidebar -->
    <div class="sidebar">
        <ul>
            <li><a href="dashboard.aspx"><i class="fa-solid fa-chart-line"></i> Dashboard</a></li>
            <li><a href="addExpense.aspx"><i class="fa-solid fa-wallet"></i>Add Expenses</a></li>
            <li><a href="manageExpense.aspx"><i class="fa-solid fa-wallet"></i>Manage Expenses</a></li>
            <li><a href="budgetSuggestion.aspx"><i class="fa-solid fa-coins"></i>Budegt AI</a></li>
            <li><a href="reports.aspx"><i class="fa-solid fa-file-alt"></i> Reports</a></li>
            <li><a href="settings.aspx"><i class="fa-solid fa-cog"></i> Settings</a></li>
            <li>
                <asp:LinkButton ID="btnLogoutSidebar" runat="server" OnClick="btnLogout_Click">
                    <i class="fa-solid fa-sign-out-alt"></i> Logout
                </asp:LinkButton>
            </li>
        </ul>
    </div>

    <div class="main-content">
      <!-- Header Section -->
      <div class="header">
        <h2>Budget Buddy</h2>
        <div class="user-settings-con">
          <h2>
            Welcome, <asp:Literal ID="litUsername" runat="server"></asp:Literal>
          </h2>
        </div>
      </div>

      <!-- Main Dashboard Container -->
      <div class="dashboard-container">
        <!-- Left: Expense Tracker Form -->
        

        <!-- Right: Expense Overview (Chart) -->
        <div class="right-section">
          <h3>Expense Overview</h3>
          <div class="chart-container">
            <canvas id="expenseChart"></canvas>
            <p id="chartMessage" style="display:none; color:#fff; text-align:center; padding-top: 180px;">
              Please add data
            </p>
          </div>
        </div>

        <!-- Hidden fields for chart data and available budget -->
        <asp:HiddenField ID="hiddenExpenseData" runat="server" />
        <asp:HiddenField ID="hiddenChartImage" runat="server" />
        <asp:HiddenField ID="hiddenBudgetAmount" runat="server" />
      </div>

      <!-- Footer -->
      <div class="footer">
        <p>&copy; 2025 Budget Buddy. All Rights Reserved.</p>
      </div>
    </div>
    </form>
  </body>
</html>
