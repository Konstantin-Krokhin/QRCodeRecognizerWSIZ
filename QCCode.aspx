<%@ Page Language="C#" AutoEventWireup="true"
   CodeBehind="QCCode.aspx.cs"
   Inherits="QRCodeSample.QCCode" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
   <head runat="server">
      <title>Generator QR kodów dla pracowników WSIiZu</title>
      <style type ="text/css">
        .scrollable tbody { display: block; height: 100px; overflow: auto; }
      </style>
   </head>
    
   <body>
      <form id="QCFrom" runat="server">
         <div>
            <hr/>
             <asp:Label Text = "Panel do Drukowania Kart" Font-Size ="15" runat = "server" /> <br/>
            <asp:Button ID="btnQCGenerate" runat="server"
               Text="Utwórz QR kody i zapisz do pliku"
               OnClick="btnQCGenerate_Click" />
             <asp:Button ID="btnload" runat="server"
               Text="Ładuj z Bazy"
               OnClick="btnload_Click" />
             <asp:GridView ID="GridView" runat="server" Height="261px" Width="1287px">
                 <Columns>
                 <asp:TemplateField HeaderText="Select Data">  
                    <EditItemTemplate>  
                        <asp:CheckBox ID="CheckBox1" runat="server" />  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:CheckBox ID="CheckBox1" runat="server" />  
                    </ItemTemplate>  
                </asp:TemplateField>
                     </Columns>
             </asp:GridView>

             <br /><br />
            
               <br /><br />
         </div>
      </form>
   </body>
   </html>