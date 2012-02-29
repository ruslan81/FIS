<%@ control language="C#" autoeventwireup="true" inherits="AdministratorS_CreatingFormsControls_GeneralTab_CretingFormsControl, App_Web_oxldwgmx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Panel ID="GeneralTablePanel" Width="600px" Height="350px" runat="server">
    <asp:Table ID="GeneralTable" runat="server" Width="600px" CellPadding="10">
        <asp:TableRow >
            <asp:TableCell HorizontalAlign="Right" Width="35%">
                <asp:Label ID="OrganizationFullNameLabel" runat="server" Text="Полное название предприятия"/>
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="OrganizationFullNameTextBox" runat="server" Width="100%"/>
            </asp:TableCell></asp:TableRow><asp:TableRow >
            <asp:TableCell HorizontalAlign="Right" Width="35%">
                <asp:Label ID="OrganizationTypeLabel" runat="server" Text="Тип предприятия"/>
            </asp:TableCell><asp:TableCell>
                <asp:DropDownList ID="OrganizationTypeDropDown" runat="server" Width="100%"/>
            </asp:TableCell></asp:TableRow>
                                <asp:TableRow >
                                    <asp:TableCell HorizontalAlign="Right" Width="35%">
                                        <asp:Label ID="OrgCountryLabel" runat="server" Text="Страна"/>
                                    </asp:TableCell>
                                    <asp:TableCell> 
                                        <asp:DropDownList ID="OrgCountryDropDownList" runat="server" Width="100%"
                                            OnSelectedIndexChanged="OrgCountryDropDownList_SelectedIndexChanged" AutoPostBack="true"
                                        />
                                    </asp:TableCell>
                                    <asp:TableCell Width="10%">
                                        <asp:Image ID="ContryImage" AlternateText="Нет флага" runat="server" Width="100%" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow >
                                    <asp:TableCell HorizontalAlign="Right" Width="35%">
                                        <asp:Label ID="OrgRegionLabel" runat="server" Text="Регион"/>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList ID="OrgRegionDropDownList" runat="server" Width="100%"/>
                                    </asp:TableCell>
                                </asp:TableRow>
   </asp:Table>
            <asp:Table runat="server" Width="100%">
                <asp:TableRow>                    
                    <asp:TableCell HorizontalAlign = "Center">
                        <asp:ImageButton ID="ChangeDrName_OK" OnClick="ChangeDrName_OK_Click" runat="server" ToolTip="Применить" ImageUrl="~/images/icons/button_ok.png" Height="40"/>
                        <asp:ImageButton ID="ChangeDrName_Cancel" OnClick="ChangeDrName_Cancel_Click" runat="server" ToolTip="Отмена" ImageUrl="~/images/icons/button_cancel.png" Height="40"/>
                        <asp:ConfirmButtonExtender ConfirmText="Создать/изменить предприятие. Вы уверены?"
                            runat="server" TargetControlID="ChangeDrName_OK"  ID="ConfirmButtonExtender1"
                        />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign = "Right">
                        <asp:Panel runat="server" ID="GeneralLogoPanel" GroupingText="Логотип" Width="230px">
                            <asp:Image ID="Image1" ImageUrl="~/images/LogoSmartFIS_medium.png" runat="server" Width="200px"/>
                            <br />
                            <asp:Button ID="Button6" runat="server" Text="Открыть"/> 
                        </asp:Panel>    
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
    <asp:HiddenField ID="currentHiddenOrgId" runat="server" />
    <asp:HiddenField ID="OldOrgName" runat="server" />
    <h3><asp:Label ID="Status" runat="server" /></h3>
</asp:Panel>