<%@ control language="C#" autoeventwireup="true" inherits="AdministratorS_CreatingFormsControls_UserEditControl, App_Web_dpqzgjiu" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

 <asp:Panel ID="UserEditPanel" runat="server" Width="408px" BorderWidth="1px">
    <asp:Table runat="server" ID="UserEditTable" Width="400px" CellSpacing="15">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="Label1" runat="server" Text="Логин:"/>
            </asp:TableCell>
            <asp:TableCell Width="70%">
                <asp:TextBox runat="server" ID="LoginTextBox" Width="100%"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="Label2" runat="server" Text="Пароль:"/>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox runat="server" ID="PasswortTextBox" Width="100%"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="Label3" runat="server" Text="Тип пользователя:"/>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList runat="server" ID="UserTypeDropDownList" Width="101%"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="Label4" runat="server" Text="Роль пользователя:"/>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList runat="server" ID="UserRoleDropDownList" Width="101%"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="Label5" runat="server" Text="Организация:"/>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList runat="server" ID="OrganizationNameDropDownList" Width="101%"/>                
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:ImageButton ID="ChangeDrName_OK" OnClick="ChangeDrName_OK_Click" runat="server" ToolTip="Применить" ImageUrl="~/images/icons/button_ok.png" Height="40"/>
    <asp:ImageButton ID="ChangeDrName_Cancel" OnClick="ChangeDrName_Cancel_Click" runat="server" ToolTip="Отмена" ImageUrl="~/images/icons/button_cancel.png" Height="40"/>
    <asp:ConfirmButtonExtender ConfirmText="Изменить учетную информацию пользователя. Вы уверены?"
        runat="server" TargetControlID="ChangeDrName_OK"  ID="ConfirmButtonExtender1"/>
    <h3><asp:Label ID="Status" runat="server" /></h3>
    <asp:HiddenField ID="OldNameHF" runat="server" Visible />
    <asp:HiddenField ID="OldPassHF" runat="server" Visible />
</asp:Panel>