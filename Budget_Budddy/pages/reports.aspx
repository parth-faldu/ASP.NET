<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reports.aspx.cs" Inherits="Budget_Budddy.pages.reports" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget Buddy - Reports</title>
    <link rel="stylesheet" href="../Styles/reports.css?v=1.0" />
    <link rel="stylesheet" href="../Styles/header.css?v=1.0" />
    <link rel="stylesheet" href="../Styles/sidebar.css?v=1.0" />
    <link rel="stylesheet" href="../Styles/baseStyle.css?v=1.0" />
    <link rel="stylesheet" 
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.7.2/css/all.min.css" 
          integrity="sha512-Evv84Mr4kqVGRNSgIGL/F/aIDqQb7xQ2vcrdIwxfjThSH8CSR7PBEakCr51Ck+w+/U6swU2Im1vVX0SVk9ABhg==" 
          crossorigin="anonymous" referrerpolicy="no-referrer" />
    <!-- Include Chart.js library -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <!-- Set client-side variables for your hidden fields -->
    <script type="text/javascript">
        var hiddenExpenseDataClientID = '<%= hiddenExpenseData.ClientID %>';
        var hiddenChartImageClientID = '<%= hiddenChartImage.ClientID %>';
    </script>
    <!-- Reference your external JavaScript file (dashboard.js) that includes your chart logic -->
    <script type="text/javascript" src="../javascript/dashboard.js?v=1.0"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Sidebar Navigation -->
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
            <div class="header">
                <h2>Budget Buddy</h2>
                <div class="user-settings-con">
                    <h2>Welcome, <asp:Literal ID="litUsername" runat="server"></asp:Literal></h2>
                </div>
            </div>

            <!-- Export Buttons -->
            <div class="export-buttons">
                <asp:LinkButton
                    ID="lnkExportPDF"
                    runat="server"
                    Text="Export as PDF"
                    CssClass="expense-btn"
                    OnClick="btnExportPDF_Click"
                    OnClientClick="return captureChartForExport();"
                    CausesValidation="false" />
                <asp:LinkButton
                    ID="lnkExportExcel"
                    runat="server"
                    Text="Export as Excel"
                    CssClass="expense-btn"
                    OnClick="btnExportExcel_Click"
                    OnClientClick="return captureChartForExport();"
                    CausesValidation="false" />
            </div>

            <!-- Chart Container (fixed small size) -->
            <div class="chart-container">
                <canvas id="expenseChart" width="200" height="200"></canvas>
                <p id="chartMessage" style="display:none; color:#fff; text-align:center; padding-top: 180px;">
                    Please add data
                </p>
            </div>

            <!-- Hidden Fields for Expense Data and Chart Image -->
            <asp:HiddenField ID="hiddenExpenseData" runat="server" />
            <asp:HiddenField ID="hiddenChartImage" runat="server" />
            <asp:HiddenField ID="hiddenBudgetAmount" runat="server" />

            <!-- GridView to display expense details -->
            <asp:GridView ID="gvExpenses" runat="server" AutoGenerateColumns="false" CssClass="expense-grid">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" />
                    <asp:BoundField DataField="Category" HeaderText="Category" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="ExpenseDate" HeaderText="Expense Date" DataFormatString="{0:yyyy-MM-dd}" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
