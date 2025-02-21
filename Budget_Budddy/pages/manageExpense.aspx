<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageExpense.aspx.cs" Inherits="Budget_Budddy.pages.manageExpense" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Expenses</title>
    <link rel="stylesheet" href="../Styles/manageExpense.css" />
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
        <div class="sidebar">
            <ul>
                <li><a href="dashboard.aspx"><i class="fa-solid fa-chart-line"></i> Dashboard</a></li>
                <li><a href="addExpense.aspx"><i class="fa-solid fa-wallet"></i> Add Expenses</a></li>
                <li><a href="manageExpense.aspx"><i class="fa-solid fa-wallet"></i> Manage Expenses</a></li>
                <li><a href="budgetSuggestion.aspx"><i class="fa-solid fa-coins"></i>Budegt AI</a></li>
                <li><a href="reports.aspx"><i class="fa-solid fa-file-alt"></i> Reports</a></li>
                <li><a href="settings.aspx"><i class="fa-solid fa-cog"></i> Settings</a></li>
                <li>
                    <%-- Logout button (commented out) --%>
                </li>
            </ul>
        </div>
        <div class="main-content">
            <div class="header">
                <h2>Budget Buddy</h2>
                <div class="user-settings-con">
                    <h2>Welcome, <asp:Literal ID="litUsername" runat="server"></asp:Literal></h2>
                    <!-- Additional header content -->
                </div>
            </div>
            <div class="table-container">
                <h3>Your Expenses</h3>
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
                            <ItemTemplate>
                                <%# Eval("ID") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- Category Column --%>
                        <asp:TemplateField HeaderText="Category">
                            <ItemTemplate>
                                <%# Eval("Category") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCategoryEdit" runat="server" CssClass="input-field" 
                                    Text='<%# Bind("Category") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%-- Amount Column --%>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <%# String.Format("{0:C}", Eval("Amount")) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAmountEdit" runat="server" CssClass="input-field" 
                                    Text='<%# Bind("Amount") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%-- Description Column --%>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <%# Eval("Description") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescriptionEdit" runat="server" CssClass="input-field" 
                                    Text='<%# Bind("Description") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%-- Expense Date Column --%>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <%# Eval("ExpenseDate", "{0:yyyy-MM-dd}") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDateEdit" runat="server" CssClass="input-field" 
                                    Text='<%# Bind("ExpenseDate", "{0:yyyy-MM-dd}") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%-- Action Column --%>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEdit" runat="server" CommandName="Edit" CssClass="action-btn" ToolTip="Edit">
                                    <i class="fa fa-pencil"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="lbDelete" runat="server" CommandName="Delete" CssClass="action-btn delete-btn" 
                                    OnClientClick="return confirm('Are you sure you want to delete this expense?');"
                                    ToolTip="Delete">
                                    <i class="fa-solid fa-trash"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lbUpdate" runat="server" CommandName="Update" CssClass="action-btn update-btn" ToolTip="Update">
                                    <i class="fa fa-check"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="lbCancel" runat="server" CommandName="Cancel" CssClass="action-btn cancel-btn" ToolTip="Cancel">
                                    <i class="fa fa-times"></i>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
