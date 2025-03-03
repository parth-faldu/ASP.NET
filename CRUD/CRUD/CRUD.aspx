<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRUD.aspx.cs" Inherits="CRUD.CRUD" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .container{
            display:grid;
            gap:20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div>
                <asp:Label ID="Label1" runat="server" Text="Name"></asp:Label>
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="Label2" runat="server" Text="Gmail"></asp:Label>
                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>    
            </div>
            <div>
                <asp:Label ID="Label3" runat="server" Text="Gender"></asp:Label>
                <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                    <asp:ListItem>Male</asp:ListItem>
                    <asp:ListItem>Female</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div>
                <asp:Label ID="Label4" runat="server" Text="City"></asp:Label>
                <asp:DropDownList ID="DropDownList1" runat="server">
                    <asp:ListItem>Select</asp:ListItem>
                    <asp:ListItem>Rajkot</asp:ListItem>
                    <asp:ListItem>Surat</asp:ListItem>
                </asp:DropDownList>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlDataSource1" EmptyDataText="There are no data records to display.">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:BoundField DataField="Gmail" HeaderText="Gmail" SortExpression="Gmail" />
                        <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                        <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CRUD %>" DeleteCommand="DELETE FROM [emp] WHERE [Id] = @Id" InsertCommand="INSERT INTO [emp] ([Id], [Name], [Gmail], [Gender], [City]) VALUES (@Id, @Name, @Gmail, @Gender, @City)" ProviderName="<%$ ConnectionStrings:CRUD.ProviderName %>" SelectCommand="SELECT [Id], [Name], [Gmail], [Gender], [City] FROM [emp]" UpdateCommand="UPDATE [emp] SET [Name] = @Name, [Gmail] = @Gmail, [Gender] = @Gender, [City] = @City WHERE [Id] = @Id">
                    <DeleteParameters>
                        <asp:Parameter Name="Id" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="Id" Type="Int32" />
                        <asp:Parameter Name="Name" Type="String" />
                        <asp:Parameter Name="Gmail" Type="String" />
                        <asp:Parameter Name="Gender" Type="String" />
                        <asp:Parameter Name="City" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Name" Type="String" />
                        <asp:Parameter Name="Gmail" Type="String" />
                        <asp:Parameter Name="Gender" Type="String" />
                        <asp:Parameter Name="City" Type="String" />
                        <asp:Parameter Name="Id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </div>
            <div>
            <asp:Button ID="Button1" runat="server" Text="Submit" />
            <asp:Button ID="Button2" runat="server" Text="Update" />
            <asp:Button ID="Button3" runat="server" Text="Delete" />
            </div>
        </div>
    </form>
</body>
</html>
