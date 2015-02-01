<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AspNetCaching.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset>
                <legend>
                    <asp:Label ID="lblCustomers" runat="server" Text="Customers"></asp:Label></legend>
                <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="#000066" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                </asp:GridView>
                <div id="divInfo" style="background-color:beige;width:416px">
                    Number of Customers:
                    <asp:Label ID="lblNumberOfCustomers" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <br />
                    Data retrieved from:
                    <asp:Label ID="lblDataRetrievedFrom" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <br />
                    Time elapsed:
                    <asp:Label ID="lblTimeElapsed" runat="server" ForeColor="Red" Text=""></asp:Label>
                </div>
                <br />
                <asp:Button ID="btnReload" runat="server" Text="Reload" OnClick="btnReload_Click" />
            </fieldset>

            <br />
        </div>
    </form>
</body>
</html>
