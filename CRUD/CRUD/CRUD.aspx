<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRUD.aspx.cs" Inherits="CRUD.CRUD" EnableEventValidation="false"%>

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
                <asp:DropDownList ID="DropDownList1" runat="server" Width="136px">
                    <asp:ListItem >- - - Select - - -</asp:ListItem>
                    <asp:ListItem>Rajkot</asp:ListItem>
                    <asp:ListItem>Surat</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div>
            <asp:Button ID="Button1" runat="server" Text="Save" OnClick="Button1_Click" />

                <br />
                <br />

                <asp:Label ID="Label5" runat="server" Text=""></asp:Label>

                <br />
                <br />

                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Height="29px" style="margin-top: 6px" Width="16px">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="Button4" runat="server" BackColor="#33CC33" CommandArgument='<%# Eval("Id") %>' ForeColor="White" Height="43px" Text="Select" Width="165px" OnClick="Button4_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:Button ID="Button5" runat="server" BackColor="Red" CommandArgument='<%# Eval("Id") %>' ForeColor="White" Height="39px" Text="Delete" Width="178px" OnClick="Button5_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#7C6F57" />
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
