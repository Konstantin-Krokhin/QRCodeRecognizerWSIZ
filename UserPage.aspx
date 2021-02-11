<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="QRCodeSample.UserPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
            <asp:Label Text = "Panel do Pobierania Obecności" Font-Size ="15" runat = "server" /> <br />
            <asp:Button ID="btnQCGenerate" runat="server"
               Width ="260" Text="Pobierz obecnosc i zapisz do pliku"
               OnClick="btnQCGenerate_Click" />
            <asp:GridView ID="GridView2" runat="server" Height="261px" Width="1287px">
                 <Columns>
                     </Columns>
             </asp:GridView>
    </form>
</body>
</html>
