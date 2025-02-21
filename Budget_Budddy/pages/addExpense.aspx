<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addExpense.aspx.cs" Inherits="Budget_Budddy.pages.addExpense" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../Styles/addExpense.css" />
    <link rel="stylesheet" href="../Styles/sidebar.css" />
    <link rel="stylesheet" href="../Styles/header.css" />
    <link rel="stylesheet" href="../Styles/baseStyle.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap" rel="stylesheet" />
    <link rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.7.2/css/all.min.css"
          integrity="sha512-Evv84Mr4kqVGRNSgIGL/F/aIDqQb7xQ2vcrdIwxfjThSH8CSR7PBEakCr51Ck+w+/U6swU2Im1vVX0SVk9ABhg=="
          crossorigin="anonymous"
          referrerpolicy="no-referrer" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="sidebar">
        <ul>
            <li><a href="dashboard.aspx"><i class="fa-solid fa-chart-line"></i> Dashboard</a></li>
            <li><a href="addExpense.aspx"><i class="fa-solid fa-wallet"></i>Add Expenses</a></li>
            <li><a href="manageExpense.aspx"><i class="fa-solid fa-wallet"></i>Manage Expenses</a></li>
            <li><a href="budgetSuggestion.aspx"><i class="fa-solid fa-coins"></i>Budegt AI</a></li>
            <li><a href="reports.aspx"><i class="fa-solid fa-file-alt"></i> Reports</a></li>
            <li><a href="settings.aspx"><i class="fa-solid fa-cog"></i> Settings</a></li>
            <li>
                <%--<asp:LinkButton ID="btnLogoutSidebar" runat="server" OnClick="btnLogout_Click">
                    <i class="fa-solid fa-sign-out-alt"></i> Logout
                </asp:LinkButton> --%>
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
          <!-- Settings Dropdown replacing the Logout button -->
        </div>
      </div>
            <div class="container">
                <div class="left-section">
                    <h3>Add Expense</h3>
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
        </div>
            </div>
          
    </form>
</body>
</html>
