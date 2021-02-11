<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="QRCodeSample.LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            text-align: center;
        }
    </style>
    <script>
        function loadAdmin()
        {
            location.href = 'AdminPage.aspx';
        }
        function loadPrint() {
            location.href = 'QCCode.aspx';
        }
        function loadUser() {
            location.href = 'UserPage.aspx';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="auto-style1">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Content/Banner.png" />
        </div>
        <div class="auto-style1">
        <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </div>
        <p class="auto-style1">
            <asp:Label ID="Label2" runat="server" Text="Hasło"></asp:Label>
            <asp:TextBox ID="TextBox2" runat="server" TextMode ="Password" Font-Names ="password font"/>
        </p>
        <p class="auto-style1">
        <asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" style="text-align: center;margin: auto" />
        </p>
    </form>
</body>
</html>
