<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs"
    Inherits="Budget_Budddy.pages.dashboard" Async="true"%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Budget Buddy - Dashboard</title>
    <!-- Google Fonts & Chart.js -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <link rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.7.2/css/all.min.css"
          integrity="sha512-Evv84Mr4kqVGRNSgIGL/F/aIDqQb7xQ2vcrdIwxfjThSH8CSR7PBEakCr51Ck+w+/U6swU2Im1vVX0SVk9ABhg=="
          crossorigin="anonymous"
          referrerpolicy="no-referrer" />
    <link rel="stylesheet" href="../Styles/dashboard.css?v=1.0" />

    <!-- Inline Script Block to Pass Server IDs as Global Variables -->
    <script type="text/javascript">
        var hiddenExpenseDataClientID = "<%= hiddenExpenseData.ClientID %>";
        var hiddenChartImageClientID = "<%= hiddenChartImage.ClientID %>";
        var btnSettingsClientID = "<%= btnSettings.ClientID %>";
        var hiddenBudgetAmountClientID = "<%= hiddenBudgetAmount.ClientID %>"; // New hidden field for available budget
    </script>

    <!-- External JavaScript File -->
    <script type="text/javascript" src="../javascript/dashboard.js"></script>
  </head>
  <body>
    <form id="form1" runat="server">
      <!-- Header Section -->
      <div class="header">
        <h2>Budget Buddy</h2>
        <div class="user-settings-con">
          <h2>
            Welcome, <asp:Literal ID="litUsername" runat="server"></asp:Literal>
          </h2>
          <!-- Settings Dropdown replacing the Logout button -->
          <div class="dropdown">
            <asp:LinkButton 
              ID="btnSettings" 
              runat="server"
              CssClass="settings-button"
              OnClientClick="toggleDropdown(); return false;"
              CausesValidation="false">
              <i class="fa-solid fa-gear"></i>
            </asp:LinkButton>
            <div id="settingsDropdown" class="dropdown-content">
              <!-- Dropdown menu items -->
              <asp:LinkButton
                ID="btnUpdateProfile"
                runat="server"
                Text="Update Profile"
                OnClick="btnUpdateProfile_Click"
                CssClass="dropdown-item"
                CausesValidation="false" />
              <asp:LinkButton
                ID="btnDeleteAccount"
                runat="server"
                Text="Delete Account"
                OnClick="btnDeleteAccount_Click"
                CssClass="dropdown-item"
                CausesValidation="false"
                OnClientClick="return confirm('Are you sure you want to delete your account?');" />
              <asp:LinkButton
                ID="btnLogout"
                runat="server"
                Text="Logout"
                OnClick="btnLogout_Click"
                CssClass="dropdown-item"
                CausesValidation="false" />
            </div>
          </div>
        </div>
      </div>

      <!-- Main Dashboard Container -->
      <div class="dashboard-container">
        <!-- Left: Expense Tracker Form -->
        <div class="left-section">
          <h3>Expense Tracker</h3>
          <div class="form-group">
            <label for="txtCategory">Category:</label>
            <asp:TextBox
              ID="txtCategory"
              runat="server"
              CssClass="input-field"
              placeholder="Enter Category"
              required
              autocomplete="off" />
          </div>
          <div class="form-group">
            <label for="txtAmount">Amount:</label>
            <asp:TextBox
              ID="txtAmount"
              runat="server"
              CssClass="input-field amount-input"
              TextMode="Number"
              placeholder="Enter Amount"
              required="required"
              autocomplete="off" />
          </div>
          <div class="form-group">
            <label for="txtDescription">Description:</label>
            <asp:TextBox
              ID="txtDescription"
              runat="server"
              CssClass="input-field"
              placeholder="Enter Description"
              autocomplete="off" />
          </div>
          <div class="form-group">
            <label for="txtDate">Date:</label>
            <asp:TextBox
              ID="txtDate"
              runat="server"
              CssClass="input-field"
              TextMode="Date"
              required
              autocomplete="off" />
          </div>
          <div id="Buttons">
            <asp:Button
              ID="btnAddExpense"
              runat="server"
              CssClass="expense-btn"
              Text="Add"
              OnClick="btnAddExpense_Click" />
          </div>
        </div>

        <!-- Right: Expense Overview (Chart) -->
        <div class="right-section">
          <h3>Expense Overview</h3>
          <div class="chart-container">
            <canvas id="expenseChart"></canvas>
            <p id="chartMessage" style="display:none; color:#fff; text-align:center; padding-top: 180px;">Please add data</p>
          </div>
        </div>

        <!-- Hidden fields for chart data and available budget -->
        <asp:HiddenField ID="hiddenExpenseData" runat="server" />
        <asp:HiddenField ID="hiddenChartImage" runat="server" />
        <asp:HiddenField ID="hiddenBudgetAmount" runat="server" /> <!-- New hidden field for user's available budget -->
      </div>


      <!-- Expenses Grid -->
      <div class="table-container">
        <h3>Your Expenses</h3>
        <!-- Export Buttons -->
        <div class="export-buttons">
          <asp:Button
            ID="btnExportPDF"
            runat="server"
            Text="Export as PDF"
            CssClass="expense-btn"
            OnClick="btnExportPDF_Click"
            OnClientClick="return captureChartForExport();"
            CausesValidation="false" />
          <asp:Button
            ID="btnExportExcel"
            runat="server"
            Text="Export as Excel"
            CssClass="expense-btn"
            OnClick="btnExportExcel_Click"
            OnClientClick="return captureChartForExport();"
            CausesValidation="false" />
        </div>
        <asp:GridView
          ID="gvExpenses"
          runat="server"
          AutoGenerateColumns="False"
          DataKeyNames="ID"
          OnRowEditing="gvExpenses_RowEditing"
          OnRowUpdating="gvExpenses_RowUpdating"
          OnRowCancelingEdit="gvExpenses_RowCancelingEdit"
          OnRowDeleting="gvExpenses_RowDeleting">
          <Columns>
            <%-- ID Column (read-only) --%>
            <asp:TemplateField HeaderText="ID">
              <ItemTemplate> <%# Eval("ID") %> </ItemTemplate>
            </asp:TemplateField>
            <%-- Category Column --%>
            <asp:TemplateField HeaderText="Category">
              <ItemTemplate> <%# Eval("Category") %> </ItemTemplate>
              <EditItemTemplate>
                <asp:TextBox
                  ID="txtCategoryEdit"
                  runat="server"
                  CssClass="input-field"
                  Text='<%# Bind("Category") %>' />
              </EditItemTemplate>
            </asp:TemplateField>
            <%-- Amount Column --%>
            <asp:TemplateField HeaderText="Amount">
              <ItemTemplate>
                <%# String.Format("{0:C}", Eval("Amount")) %>
              </ItemTemplate>
              <EditItemTemplate>
                <asp:TextBox
                  ID="txtAmountEdit"
                  runat="server"
                  CssClass="input-field"
                  Text='<%# Bind("Amount") %>' />
              </EditItemTemplate>
            </asp:TemplateField>
            <%-- Description Column --%>
            <asp:TemplateField HeaderText="Description">
              <ItemTemplate> <%# Eval("Description") %> </ItemTemplate>
              <EditItemTemplate>
                <asp:TextBox
                  ID="txtDescriptionEdit"
                  runat="server"
                  CssClass="input-field"
                  Text='<%# Bind("Description") %>' />
              </EditItemTemplate>
            </asp:TemplateField>
            <%-- Expense Date Column --%>
            <asp:TemplateField HeaderText="Date">
              <ItemTemplate>
                <%# Eval("ExpenseDate", "{0:yyyy-MM-dd}") %>
              </ItemTemplate>
              <EditItemTemplate>
                <asp:TextBox
                  ID="txtDateEdit"
                  runat="server"
                  CssClass="input-field"
                  Text='<%# Bind("ExpenseDate", "{0:yyyy-MM-dd}") %>' />
              </EditItemTemplate>
            </asp:TemplateField>
            <%-- Action Column --%>
            <asp:TemplateField HeaderText="Actions">
              <ItemTemplate>
                <asp:LinkButton
                  ID="lbEdit"
                  runat="server"
                  CommandName="Edit"
                  CssClass="action-btn"
                  ToolTip="Edit">
                  <i class="fa fa-pencil"></i>
                </asp:LinkButton>
                <asp:LinkButton
                  ID="lbDelete"
                  runat="server"
                  CommandName="Delete"
                  CssClass="action-btn"
                  OnClientClick="return confirm('Are you sure you want to delete this expense?');"
                  ToolTip="Delete">
                  <i class="fa-solid fa-trash"></i>
                </asp:LinkButton>
              </ItemTemplate>
              <EditItemTemplate>
                <asp:LinkButton
                  ID="lbUpdate"
                  runat="server"
                  CommandName="Update"
                  CssClass="action-btn"
                  ToolTip="Update">
                  <i class="fa fa-check"></i>
                </asp:LinkButton>
                <asp:LinkButton
                  ID="lbCancel"
                  runat="server"
                  CommandName="Cancel"
                  CssClass="action-btn"
                  ToolTip="Cancel">
                  <i class="fa fa-times"></i>
                </asp:LinkButton>
              </EditItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      </div>

     <!-- Budget Suggestions Section -->
    <div class="suggestions-section">
        <h3>Budget Suggestions</h3>
        <asp:Literal ID="budgetSuggestionsLiteral" runat="server" Text="Loading suggestions..." Mode="PassThrough"></asp:Literal>
    </div>
      <!-- Footer -->
      <div class="footer">
        <p>&copy; 2025 Budget Buddy. All Rights Reserved.</p>
      </div>
    </form>
  </body>
</html>
