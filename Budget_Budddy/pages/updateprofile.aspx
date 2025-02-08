<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="updateprofile.aspx.cs" Inherits="Budget_Budddy.pages.updateprofile" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Update Profile</title>
    <link rel="stylesheet" type="text/css" href="../Styles/updateprofile.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Update Profile</h2>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            <div>
                <label for="txtUsername">Username:</label>
                <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            </div>

            <div>
                <label for="txtEmail">Email:</label>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            </div>

            <div>
                <label for="txtPassword">New Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>

            <div>
                <asp:Button ID="btnUpdate" runat="server" Text="Update Profile" OnClick="btnUpdate_Click" />
            </div>
            
            <!-- Back to Dashboard HyperLink -->
            <div>
                <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="dashboard.aspx" CssClass="back-link">
                    Back to Dashboard
                </asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>
