<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="budgetSuggestion.aspx.cs" Inherits="Budget_Budddy.pages.budgetSuggestion" Async="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget Suggestion</title>
    <link rel="stylesheet" href="../Styles/budegtSuggestion.css" />
    <link rel="stylesheet" href="../Styles/header.css" />
    <link rel="stylesheet" href="../Styles/sidebar.css" />
    <link rel="stylesheet" href="../Styles/baseStyle.css" />
    <link rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.7.2/css/all.min.css"
          integrity="sha512-Evv84Mr4kqVGRNSgIGL/F/aIDqQb7xQ2vcrdIwxfjThSH8CSR7PBEakCr51Ck+w+/U6swU2Im1vVX0SVk9ABhg=="
          crossorigin="anonymous"
          referrerpolicy="no-referrer" />
    <script type="text/javascript">
        var hiddenExpenseDataClientID = "<%= hiddenExpenseData.ClientID %>";
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="sidebar">
            <ul>
                <li><a href="dashboard.aspx"><i class="fa-solid fa-chart-line"></i> Dashboard</a></li>
                <li><a href="addExpense.aspx"><i class="fa-solid fa-wallet"></i> Add Expenses</a></li>
                <li><a href="manageExpense.aspx"><i class="fa-solid fa-wallet"></i> Manage Expenses</a></li>
                <li><a href="budget.aspx"><i class="fa-solid fa-coins"></i> Budget AI</a></li>
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
            <div class="header">
                <h2>Budget Buddy</h2>
                <div class="user-settings-con">
                    <h2>
                        Welcome, <asp:Literal ID="litUsername" runat="server"></asp:Literal>
                    </h2>
                </div>
            </div>
            <div class="suggestions-section">
                <asp:UpdatePanel ID="upBudgetSuggestion" runat="server">
                    <ContentTemplate>
                        <!-- Input for next year's target budget -->
                        <div class="budget-input">
                            <label for="txtNextYearBudget" style="color:#cecece;">Enter Your Target Budget for Next Year:</label>
                            <asp:TextBox ID="txtNextYearBudget" runat="server" CssClass="input-box" />
                        </div>
                        <!-- Button to trigger suggestion generation -->
                        <asp:LinkButton 
                            ID="lnkGetBudgetSuggestion" 
                            runat="server" 
                            Text="Get Budget Suggestion" 
                            CssClass="suggestion-btn"
                            OnClick="lnkGetBudgetSuggestion_Click"
                            CausesValidation="false" />
                        <br /><br />
                        <asp:Literal ID="budgetSuggestionsLiteral" runat="server"></asp:Literal>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkGetBudgetSuggestion" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <!-- UpdateProgress with DisplayAfter set to 0 so it shows immediately -->
                <asp:UpdateProgress ID="upProgress" runat="server" 
                    AssociatedUpdatePanelID="upBudgetSuggestion" DisplayAfter="0">
                    <ProgressTemplate>
                        <div class="loader-container">
                            <span class="loader"></span>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </div>
        <asp:HiddenField ID="hiddenExpenseData" runat="server" />
    </form>
</body>
</html>
ody>
</html>
