<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="t4.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Table</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Table ID="EmployeeTable" runat="server" CellSpacing="10" CellPadding="8" GridLines="Both" BorderWidth="1px" BorderColor="Black">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>ID</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Salary</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                
                <asp:TableRow>
                    <asp:TableCell>100</asp:TableCell>
                    <asp:TableCell>Rishab</asp:TableCell>
                    <asp:TableCell>7000</asp:TableCell>
                </asp:TableRow>
                
                <asp:TableRow>
                    <asp:TableCell>101</asp:TableCell>
                    <asp:TableCell>Dharani</asp:TableCell>
                    <asp:TableCell>7800</asp:TableCell>
                </asp:TableRow>
                
                <asp:TableRow>
                    <asp:TableCell>102</asp:TableCell>
                    <asp:TableCell>Joseph</asp:TableCell>
                    <asp:TableCell>8500</asp:TableCell>
                </asp:TableRow>
                
                <asp:TableRow>
                    <asp:TableCell>103</asp:TableCell>
                    <asp:TableCell>Yamuna</asp:TableCell>
                    <asp:TableCell>9500</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </form>
</body>
</html>