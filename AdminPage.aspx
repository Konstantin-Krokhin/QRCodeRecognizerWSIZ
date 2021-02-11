<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="QRCodeSample.AdminPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label Text = "Panel do Zarządzania Kontami Użytkowników (Skróty: admin - Administrator, print - Konto do Drukowania, user - Konto do pobierania Listy Obecnosci)" Font-Size ="15" runat = "server" /> <br/>

            <asp:GridView ID="GridView1" runat="server" onrowdatabound="gridView_RowDataBound" AutoGenerateColumns="false" DataKeyNames="Login" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">  
                    <Columns>  

                        <asp:BoundField DataField="Login" HeaderText="Login" />


                        <asp:TemplateField HeaderText="Role">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Role") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlRoles" DataTextField="Role" DataValueField="Role">
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:CommandField ShowEditButton="true" />  
                        <asp:CommandField ShowDeleteButton="true" /> 

                    </Columns>  
                </asp:GridView>

        </div>
    </form>
</body>
</html>
