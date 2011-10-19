<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreatingForms.aspx.cs" Inherits="AdministratorS_CreatingForms" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="CreatingFormsControls/GeneralTab_CretingFormsControl.ascx" TagName="GeneralTab_CretingFormsControl"
    TagPrefix="uc1" %>
<%@ Register Src="CreatingFormsControls/UserEditControl.ascx" TagName="UserEditControl"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="" />
    <meta name="description" content="" />
    <link href="../styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="formAdmS" enctype="multipart/form-data" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" />
    <div id="header">
        <asp:UpdatePanel ID="headerUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Table runat="server" Width="100%">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Status" runat="server" ForeColor="DarkBlue" Font-Bold="true" Font-Size="Medium" />
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Right" Width="10%">
                            <asp:LinkButton ID="ExitButt" Text="Выход" PostBackUrl="~/loginPage.aspx" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="width: 99%; min-height: 800px; margin: 2px 5px; padding: 5px 5px 10px 5px;">
        <asp:TabContainer ID="TabContainer1" Width="100%" runat="server" ActiveTabIndex="1">
            <asp:TabPanel ID="TabPanel1" HeaderText="Предприятия" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="OrgUpdatePanel">
                        <ContentTemplate>
                            <asp:Panel ID="ButtonPanel1" runat="server" Width="100%" BackColor="DarkCyan">
                                <asp:Button ID="AddEnterpriseBtn" runat="server" Text="Добавить" OnClick="AddEnterpriseBtnClick" />
                                <asp:Label ID="EnterpriseGeneralButton" runat="server" Font-Size="Large" ForeColor="White"
                                    Text="Управление предприятиями" />
                            </asp:Panel>
                            <asp:DataGrid ID="EnterPriseDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                AlternatingItemStyle-BackColor="LightCyan" GridLines="None" OnItemCommand="EditOrgLinkBtn_Click">
                                <HeaderStyle BackColor="LightGray" />
                                <Columns>
                                    <asp:BoundColumn DataField="#" HeaderText="#" />
                                    <asp:BoundColumn DataField="Название организации" HeaderText="Название организации" />
                                    <asp:BoundColumn DataField="Тип организации" HeaderText="Тип организации" />
                                    <asp:BoundColumn DataField="Страна" HeaderText="Страна" />
                                    <asp:BoundColumn DataField="Регион" HeaderText="Регион" />
                                    <asp:TemplateColumn HeaderText="Флаг">
                                        <ItemTemplate>
                                            <asp:Image ID="Image1" runat="server" Width="50px" ImageUrl="~/images/icons/zimbabwe-flag2.png" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Редактирование" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="EditOrgLinkBtn" Text="Править" runat="server" CommandName="Edit" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Удаление" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="DeleteOrgLinkBtn" Text="Удалить" runat="server" CommandName="Delete" />
                                            <asp:ConfirmButtonExtender ConfirmText="Удалить учетную запись организации. Вы уверены?"
                                                runat="server" TargetControlID="DeleteOrgLinkBtn" ID="ConfirmButtonExtender2" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            <asp:Panel runat="server" ID="DragPanelOrg" Visible="false">
                                <asp:Panel runat="server" ID="DragPanelOrg_Header" BackColor="Gray" BorderWidth="1px">
                                    <asp:Label ID="Label1_org" runat="server" ForeColor="White" Text="Добавление/Редактирование Организации" />
                                </asp:Panel>
                                <asp:Panel runat="server" ID="DragPanelOrg_Body" BackColor="LightGray">
                                    <uc1:GeneralTab_CretingFormsControl ID="GeneralTab_CreatingFormsControl1" runat="server" />
                                </asp:Panel>
                            </asp:Panel>
                            <asp:DragPanelExtender ID="DragPanelExtender2" runat="server" TargetControlID="DragPanelOrg"
                                DragHandleID="DragPanelOrg_Header">
                            </asp:DragPanelExtender>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="AddEnterpriseBtn" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel2" HeaderText="Пользователи" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UsersUpdatePanel1">
                        <ContentTemplate>
                            <asp:Panel ID="Panel2" runat="server" Width="100%" BackColor="DarkCyan">
                                <asp:Button ID="AddUserBtn" Text="Добавить" runat="server" OnClick="AddNewUser" />
                                <asp:Label ID="UsersGeneralLabel" runat="server" Font-Size="Large" ForeColor="White"
                                    Text="Управление пользователями" />
                            </asp:Panel>
                            <asp:DataGrid ID="UsersDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                AlternatingItemStyle-BackColor="LightCyan" OnItemCommand="EditUserLinkBtn_Click"
                                GridLines="None">
                                <HeaderStyle BackColor="LightGray" />
                                <Columns>
                                    <asp:BoundColumn HeaderText="#" DataField="#" />
                                    <asp:BoundColumn HeaderText="Имя" DataField="Имя" />
                                    <asp:BoundColumn HeaderText="Пароль" DataField="Пароль" />
                                    <asp:BoundColumn HeaderText="Тип пользователя" DataField="Тип пользователя" />
                                    <asp:BoundColumn HeaderText="Роль пользователя" DataField="Роль пользователя" />
                                    <asp:BoundColumn HeaderText="Время последней аутентификации" DataField="Время последней аутентификации" />
                                    <asp:BoundColumn HeaderText="Название организации" DataField="Название организации" />
                                    <asp:TemplateColumn HeaderText="Редактирование" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="EditUserLinkBtn" Text="Править" runat="server" CommandName="Edit" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Удаление" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="DeleteUserLinkBtn" Text="Удалить" runat="server" CommandName="Delete" />
                                            <asp:ConfirmButtonExtender ConfirmText="Удалить учетную запись пользователя. Вы уверены?"
                                                runat="server" TargetControlID="DeleteUserLinkBtn" ID="ConfirmButtonExtender1" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            <asp:Panel runat="server" ID="DragPanel">
                                <asp:Panel runat="server" ID="DragPanel_Header" BackColor="Gray" BorderWidth="1px">
                                    <asp:Label ID="DragPanel_Header_Label" runat="server" ForeColor="White" Text="Добавление/Редактирование учетной записи пользователя" />
                                </asp:Panel>
                                <asp:Panel runat="server" ID="DragPanel_Body" BackColor="LightGray">
                                    <uc2:UserEditControl ID="UserEditControl1" runat="server" />
                                </asp:Panel>
                            </asp:Panel>
                            <asp:DragPanelExtender ID="DragPanelExtender1" runat="server" TargetControlID="DragPanel"
                                DragHandleID="DragPanel_Header">
                            </asp:DragPanelExtender>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="AddUserBtn" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel3" HeaderText="Управление Строками(fd_string)" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="FdStringUpdatePanel">
                        <ContentTemplate>
                            <asp:DataGrid ID="FD_stringDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                AlternatingItemStyle-BackColor="LightCyan" GridLines="None" OnEditCommand="FD_stringDataGrid_Edit"
                                OnCancelCommand="FD_stringDataGrid_Cancel" OnUpdateCommand="FD_stringDataGrid_Update">
                                <HeaderStyle BackColor="LightGray" />
                                <Columns>
                                    <asp:BoundColumn DataField="STRING_ID" HeaderText="ID" ReadOnly="true" />
                                    <asp:BoundColumn DataField="STRING_EN" HeaderText="English" />
                                    <asp:BoundColumn DataField="STRING_RU" HeaderText="Русский" />
                                    <asp:EditCommandColumn EditText="Править" CancelText="Отмена" UpdateText="Применить" />
                                </Columns>
                            </asp:DataGrid>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel4" HeaderText="Информация о предприятии " runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="OrgInfoUpdatePanel">
                        <ContentTemplate>
                            <asp:Panel ID="ButtonPanel4" runat="server" Width="100%" BackColor="DarkCyan">
                                <asp:Button ID="ShowAddOrgInfoBtn" runat="server" Text="Добавить" OnClick="ShowAddOrgInfoBtnClick" />
                                <asp:Label ID="OrgGeneralLabel" runat="server" Font-Size="Large" ForeColor="White"
                                    Text="Справочник характеристик организаций" />
                            </asp:Panel>
                            <asp:DataGrid ID="OrgInfosDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                AlternatingItemStyle-BackColor="LightCyan" GridLines="None" OnItemCommand="OrgInfoLinkBtn_Click">
                                <HeaderStyle BackColor="LightGray" />
                                <Columns>
                                    <asp:BoundColumn DataField="ORG_INFO_ID" HeaderText="ID" ReadOnly="true" />
                                    <asp:BoundColumn DataField="ORG_INFO_NAME" HeaderText="Название параметра" />
                                    <asp:TemplateColumn HeaderText="Удаление">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="DeleteOrgInfoLinkBtn" Text="Удалить" runat="server" CommandName="Delete" />
                                            <asp:ConfirmButtonExtender ConfirmText="Удалить поле информации об организации. Вы уверены?"
                                                runat="server" TargetControlID="DeleteOrgInfoLinkBtn" ID="DeleteOrgInfoLinkBtn_ConfirmButtonExtender" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            <asp:Panel runat="server" ID="AddOrgInfoPanel" Visible="false">
                                <asp:Label runat="server" Text="Название нового параметра: " />
                                <asp:TextBox runat="server" ID="NewOrgInfoName" />
                                <asp:Button runat="server" ID="AddOrgInfoButton" OnClick="AddOrgInfoBtnClick" Text="Сохранить" />
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="AddOrgInfoButton" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel5" HeaderText="Типы ТС/топлива/устройств" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="VehicleTypesUpdatePanel">
                        <ContentTemplate>
                            <asp:Table runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Panel ID="Panel1" runat="server" Width="100%" BackColor="DarkCyan">
                                            <asp:Button ID="ShowAddVehicleTypeButton" runat="server" Text="Добавить" OnClick="ShowAddVehicleTypeButton_Click" />
                                            <asp:Label ID="AddVehicleGeneralLabel" runat="server" Font-Size="Large" ForeColor="White"
                                                Text="Тип транспортного средства" />
                                        </asp:Panel>
                                        <asp:DataGrid ID="VehicleTypesDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                            AlternatingItemStyle-BackColor="LightCyan" GridLines="None" OnItemCommand="VehTypeDelLinkBtn_Click">
                                            <HeaderStyle BackColor="LightGray" />
                                            <Columns>
                                                <asp:BoundColumn DataField="VEHICLE_TYPE_ID" HeaderText="ID" ReadOnly="true" />
                                                <asp:BoundColumn DataField="STRID_VEHICLE_TYPE_NAME" HeaderText="Название типа" />
                                                <asp:BoundColumn DataField="FUEL_TYPE" HeaderText="Тип топлива" />
                                                <asp:TemplateColumn HeaderText="Редактирование" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="EditVehicleTypesLinkBtn" Text="Править" runat="server" CommandName="Edit" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Удаление">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="DeleteVehTypeLinkBtn" Text="Удалить" runat="server" CommandName="Delete" />
                                                        <asp:ConfirmButtonExtender ConfirmText="Удалить тип транспортного средства. Вы уверены?"
                                                            runat="server" TargetControlID="DeleteVehTypeLinkBtn" ID="DeleteVehTypeLinkBtn_ConfirmButtonExtender" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:Panel runat="server" ID="NewVehicleTypesPanel" Visible="false">
                                            <asp:Label ID="NewVehicleTypeLabel" runat="server" Text="Название нового типа ТС: " />
                                            <asp:TextBox runat="server" ID="NewVehicleTypeTextBox" />
                                            <asp:DropDownList runat="server" ID="AddNewVehicleTypeDropDownList" />
                                            <asp:Button runat="server" ID="EditNewVehicleTypeButton" OnClick="EditVehicleTypeButtonClick"
                                                Text="Обновить" />
                                            <asp:Button runat="server" ID="AddNewVehicleTypeButton" OnClick="AddNewVehicleTypeButtonClick"
                                                Text="Сохранить" />
                                            <asp:Button runat="server" ID="CancelNewVehicleTypeButton" OnClick="CancelNewVehicleTypeButtonClick"
                                                Text="Отменить" />
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="30px">
                                    <asp:TableCell VerticalAlign="Middle">
                                        <hr id="Hr4" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Panel ID="Panel3" runat="server" Width="100%" BackColor="DarkCyan">
                                            <asp:Button ID="ShowAddFuelTypeButton" runat="server" Text="Добавить" OnClick="ShowAddFuelTypeButton_Click" />
                                            <asp:Label ID="AddFuelTypeGeneralLabel" runat="server" Font-Size="Large" ForeColor="White"
                                                Text="Тип топлива" />
                                        </asp:Panel>
                                        <asp:DataGrid ID="FuelTypeDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                            AlternatingItemStyle-BackColor="LightCyan" GridLines="None" OnItemCommand="FuelTypeDelLinkBtn_Click">
                                            <HeaderStyle BackColor="LightGray" />
                                            <Columns>
                                                <asp:BoundColumn DataField="FUEL_TYPE_ID" HeaderText="ID" ReadOnly="true" />
                                                <asp:BoundColumn DataField="STRID_FUEL_TYPE_NAME" HeaderText="Название типа" />
                                                <asp:TemplateColumn HeaderText="Удаление">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="DeleteFuelTypeLinkBtn" Text="Удалить" runat="server" CommandName="Delete" />
                                                        <asp:ConfirmButtonExtender ConfirmText="Удалить выбранный тип топлива. Вы уверены?"
                                                            runat="server" TargetControlID="DeleteFuelTypeLinkBtn" ID="DeleteFuelTypeLinkBtn_ConfirmButtonExtender" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:Panel runat="server" ID="AddFuelTypePanel" Visible="false">
                                            <asp:Label ID="AddFuelTypeLabel" runat="server" Text="Название нового типа топлива: " />
                                            <asp:TextBox runat="server" ID="AddFuelTypeTextBox" />
                                            <asp:Button runat="server" ID="AddFuelTypeButton" OnClick="AddFuelTypeButtonClick"
                                                Text="Сохранить" />
                                            <asp:Button runat="server" ID="CancelFuelTypeButton" OnClick="CancelFuelTypeButtonClick"
                                                Text="Отменить" />
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="30px">
                                    <asp:TableCell VerticalAlign="Middle">
                                        <hr id="Hr2" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Panel ID="Panel6" runat="server" Width="100%" BackColor="DarkCyan">
                                            <asp:Button ID="ShowAddDeviceTypeButton" runat="server" Text="Добавить" OnClick="ShowAddDeviceTypeButton_Click" />
                                            <asp:Label ID="AddDeviceTypeGeneralLabel" runat="server" Font-Size="Large" ForeColor="White"
                                                Text="Тип устройства" />
                                        </asp:Panel>
                                        <asp:DataGrid ID="DeviceTypeDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                            AlternatingItemStyle-BackColor="LightCyan" GridLines="None" OnItemCommand="DeviceTypeDelLinkBtn_Click">
                                            <HeaderStyle BackColor="LightGray" />
                                            <Columns>
                                                <asp:BoundColumn DataField="DEVICE_TYPE_ID" HeaderText="ID" ReadOnly="true" />
                                                <asp:BoundColumn DataField="STRID_DEVICE_TYPE_NAME" HeaderText="Название типа" />
                                                <asp:TemplateColumn HeaderText="Удаление">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="DeleteDeviceTypeLinkBtn" Text="Удалить" runat="server" CommandName="Delete" />
                                                        <asp:ConfirmButtonExtender ConfirmText="Удалить выбранный тип устройства. Вы уверены?"
                                                            runat="server" TargetControlID="DeleteDeviceTypeLinkBtn" ID="DeleteDeviceTypeLinkBtn_ConfirmButtonExtender" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:Panel runat="server" ID="AddDeviceTypePanel" Visible="false">
                                            <asp:Label ID="AddDeviceTypeLabel" runat="server" Text="Название нового типа устройств: " />
                                            <asp:TextBox runat="server" ID="AddDeviceTypeTextBox" />
                                            <asp:Button runat="server" ID="AddDeviceTypeButton" OnClick="AddDeviceTypeButtonClick"
                                                Text="Сохранить" />
                                            <asp:Button runat="server" ID="CancelDeviceTypeButton" OnClick="CancelDeviceTypeButtonClick"
                                                Text="Отменить" />
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel6" HeaderText="Справочник критериев/ед измерения" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="CriteriaListUpdatePanel">
                        <ContentTemplate>
                            <asp:Table ID="Table1" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Panel ID="Panel4" runat="server" Width="100%" BackColor="DarkCyan">
                                            <asp:Button ID="ShowAddCriteriaButton" runat="server" Text="Добавить" OnClick="ShowAddCriteriaButton_Click" />
                                            <asp:Label ID="AddCriteriaGeneralLabel" runat="server" Font-Size="Large" ForeColor="White"
                                                Text="Справочник критериев" />
                                        </asp:Panel>
                                        <asp:DataGrid ID="CriteriaListDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                            AlternatingItemStyle-BackColor="LightCyan" GridLines="None" OnItemCommand="CriteriaListDelLinkBtn_Click">
                                            <HeaderStyle BackColor="LightGray" />
                                            <Columns>
                                                <asp:BoundColumn DataField="KEY_ID" HeaderText="ID" ReadOnly="true" />
                                                <asp:BoundColumn DataField="KEY_NAME" HeaderText="Название критерия" />
                                                <asp:BoundColumn DataField="MEASURE_NAME" HeaderText="Единица измерения" />
                                                <asp:BoundColumn DataField="KEY_VALUE_MIN" HeaderText="Мин. значение" />
                                                <asp:BoundColumn DataField="KEY_VALUE_MAX" HeaderText="Макс. значение" />
                                                <asp:BoundColumn DataField="KEY_NOTE" HeaderText="Комментарий" />
                                                <asp:TemplateColumn HeaderText="Редактирование" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="EditCriteriaListLinkBtn" Text="Править" runat="server" CommandName="Edit" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Удаление">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="DeleteCriteriaListLinkBtn" Text="Удалить" runat="server" CommandName="Delete" />
                                                        <asp:ConfirmButtonExtender ConfirmText="Удалить критерий. Вы уверены?" runat="server"
                                                            TargetControlID="DeleteCriteriaListLinkBtn" ID="DeleteCriteriaListLinkBtn_ConfirmButtonExtender" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:Panel runat="server" ID="AddNewCriteriaPanel" Visible="false">
                                            <asp:Label ID="Label1" runat="server" Text="Название: " />
                                            <asp:TextBox runat="server" ID="CriteriaNameTextBox" />
                                            <asp:Label ID="Label5" runat="server" Text="Ед.изм.: " />
                                            <asp:DropDownList runat="server" ID="CriteriaMeasureDropDownList" />
                                            <asp:Label ID="Label2" runat="server" Text="Мин значение: " />
                                            <asp:TextBox runat="server" ID="CriteriaMinValueTextBox" Width="30px" />
                                            <asp:Label ID="Label3" runat="server" Text="Макс значение: " />
                                            <asp:TextBox runat="server" ID="CriteriaMaxValueTextBox" Width="30px" />
                                            <asp:Label ID="Label4" runat="server" Text="Комментарий: " />
                                            <asp:TextBox runat="server" ID="CriteriaCommentTextBox" />
                                            <asp:Button runat="server" ID="EditCriteriaButton" OnClick="EditCriteriaButtonClick"
                                                Text="Обновить" />
                                            <asp:Button runat="server" ID="AddCriteriaButton" OnClick="AddCriteriaButtonClick"
                                                Text="Сохранить" />
                                            <asp:Button runat="server" ID="CancelCriteriaButton" OnClick="CancelCriteriaButtonClick"
                                                Text="Отменить" />
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="30px">
                                    <asp:TableCell VerticalAlign="Middle">
                                        <hr id="Hr1" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Panel ID="Panel5" runat="server" Width="100%" BackColor="DarkCyan">
                                            <asp:Button ID="ShowAddMeasureButton" runat="server" Text="Добавить" OnClick="ShowAddMeasureButton_Click" />
                                            <asp:Label ID="AddMeasureGeneralLabel" runat="server" Font-Size="Large" ForeColor="White"
                                                Text="Справочник единиц измерения" />
                                        </asp:Panel>
                                        <asp:DataGrid ID="MeasureListDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                            AlternatingItemStyle-BackColor="LightCyan" GridLines="None" OnEditCommand="FD_MeasureDataGrid_Edit"
                                            OnCancelCommand="FD_MeasureDataGrid_Cancel" OnUpdateCommand="FD_MeasureDataGrid_Update"
                                            OnDeleteCommand="MeasureListDelLinkBtn_Click">
                                            <HeaderStyle BackColor="LightGray" />
                                            <Columns>
                                                <asp:BoundColumn DataField="MEASURE_ID" HeaderText="ID" ReadOnly="true" />
                                                <asp:BoundColumn DataField="MEASURE_NAME" HeaderText="Сокращенное название единицы измерения" />
                                                <asp:BoundColumn DataField="MEASURE_FULL_NAME" HeaderText="Полное название единицы измерения" />
                                                <asp:EditCommandColumn EditText="Править" CancelText="Отмена" UpdateText="Применить" />
                                                <asp:TemplateColumn HeaderText="Удаление">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="DeleteMeasureListLinkBtn" Text="Удалить" runat="server" CommandName="Delete" />
                                                        <asp:ConfirmButtonExtender ConfirmText="Удалить единицу измерения. Вы уверены?" runat="server"
                                                            TargetControlID="DeleteMeasureListLinkBtn" ID="DeleteMeasureListLinkBtn_ConfirmButtonExtender" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:Panel runat="server" ID="AddNewMeasurePanel" Visible="false">
                                            <asp:Label ID="Label7" runat="server" Text="Сокращенное название: " />
                                            <asp:TextBox runat="server" ID="MeasureShortNameTextBox" />
                                            <asp:Label ID="Label8" runat="server" Text="Полное название: " />
                                            <asp:TextBox runat="server" ID="MeasureFullNameTextBox" />
                                            <asp:Button runat="server" ID="AddMeasureButton" OnClick="AddMeasureButtonClick"
                                                Text="Сохранить" />
                                            <asp:Button runat="server" ID="CancelMeasureButton" OnClick="CancelMeasureButtonClick"
                                                Text="Отменить" />
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel7" HeaderText="Справочник отчетов" runat="server">
                <ContentTemplate>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="ReportTypeListUpdatePanel">
                        <ContentTemplate>
                            <asp:Table ID="Table2" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Panel ID="Panel7" runat="server" Width="100%" BackColor="DarkCyan">
                                            <asp:Button ID="ShowAddReportButton" runat="server" Text="Добавить" OnClick="ShowAddReportButton_Click" />
                                            <asp:Label ID="AddReportGeneralLabel" runat="server" Font-Size="Large" ForeColor="White"
                                                Text="Справочник отчетов" />
                                        </asp:Panel>
                                        <asp:DataGrid ID="ReportListDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                            AlternatingItemStyle-BackColor="LightCyan" GridLines="None" OnEditCommand="FD_ReportDataGrid_Edit"
                                            OnCancelCommand="FD_ReportDataGrid_Cancel" OnUpdateCommand="FD_ReportDataGrid_Update"
                                            OnDeleteCommand="ReportListDelLinkBtn_Click">
                                            <HeaderStyle BackColor="LightGray" />
                                            <Columns>
                                                <asp:BoundColumn DataField="CODE" HeaderText="Код отчета" ReadOnly="true" />
                                                <asp:BoundColumn DataField="TYPE" HeaderText="Тип отчета" ReadOnly="true" />
                                                <asp:BoundColumn DataField="NAME" HeaderText="Название" ReadOnly="true" />
                                                <asp:BoundColumn DataField="PRICE" HeaderText="Стоимость добавления" ReadOnly="true" />
                                                <asp:BoundColumn DataField="CREATE_DATE" HeaderText="Дата создания" ReadOnly="true" />
                                                <asp:BoundColumn DataField="UPDATE_DATE" HeaderText="Дата изменения" ReadOnly="true" />
                                                <asp:BoundColumn DataField="NOTE" HeaderText="Описание" ReadOnly="true" />
                                                <asp:EditCommandColumn EditText="Править" HeaderText="Правка" CancelText="Отмена" UpdateText="Применить" />
                                                <asp:TemplateColumn HeaderText="Удаление">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="DeleteReportListLinkBtn" Text="Удалить" runat="server" CommandName="Delete" />
                                                        <asp:ConfirmButtonExtender ConfirmText="Удалить отчет. Вы уверены?" runat="server"
                                                            TargetControlID="DeleteReportListLinkBtn" ID="DeleteReportListLinkBtn_ConfirmButtonExtender" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:Panel runat="server" ID="AddNewReportPanel" Visible="false">
                                            <asp:Label ID="Label9" runat="server" Text="Тип отчета: " />
                                            <asp:DropDownList runat="server" ID="ReportTypeDropDown" />
                                            <asp:Label ID="Label10" runat="server" Text="Название: " />
                                            <asp:TextBox runat="server" ID="ReportNameTextBox" />
                                            <asp:Label ID="Label6" runat="server" Text="Стоимость добавления: " />
                                            <asp:TextBox runat="server" ID="ReportPriceTextBox" Width="30px"/>
                                             <asp:Label ID="Label11" runat="server" Text="Описание: " />
                                            <asp:TextBox runat="server" ID="ReportNoteTextBox" TextMode="MultiLine" Width="250px" Columns="3"/>
                                            <asp:Button runat="server" ID="AddReportButton" OnClick="AddReportButtonClick" Text="Сохранить" />
                                            <asp:Button runat="server" ID="CancelReportButton" OnClick="CancelReportButtonClick"
                                                Text="Отменить" />
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                                 <asp:TableRow Height="30px">
                                    <asp:TableCell VerticalAlign="Middle">
                                        <hr id="Hr3" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Panel ID="Panel8" runat="server" Width="100%" BackColor="DarkCyan">
                                            <asp:Button ID="ShowAddReportTypeButton" runat="server" Text="Добавить" OnClick="ShowAddReportTypeButton_Click" />
                                            <asp:Label ID="AddReportTypeGeneralLabel" runat="server" Font-Size="Large" ForeColor="White"
                                                Text="Справочник типов отчетов" />
                                        </asp:Panel>
                                        <asp:DataGrid ID="ReportTypeListDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                            AlternatingItemStyle-BackColor="LightCyan" GridLines="None" OnDeleteCommand="ReportTypeListDelLinkBtn_Click"
                                            OnEditCommand="FD_ReportTypeDataGrid_Edit" OnCancelCommand="FD_ReportTypeDataGrid_Cancel" 
                                            OnUpdateCommand="FD_ReportTypeDataGrid_Update">
                                            <HeaderStyle BackColor="LightGray" />
                                            <Columns>
                                                <asp:BoundColumn DataField="CODE" HeaderText="Код типа" ReadOnly="true" />
                                                <asp:BoundColumn DataField="NAME" HeaderText="Название" />
                                                <asp:BoundColumn DataField="SHORTNAME" HeaderText="Короткое название"/>
                                                <asp:BoundColumn DataField="FULLNAME" HeaderText="Полное название"/>
                                                <asp:BoundColumn DataField="PRINTNAME" HeaderText="Название на печать"/>
                                                <asp:EditCommandColumn EditText="Править" CancelText="Отмена" UpdateText="Применить" />
                                                <asp:TemplateColumn HeaderText="Удаление">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="DeleteReportTypeListLinkBtn" Text="Удалить" runat="server" CommandName="Delete" />
                                                        <asp:ConfirmButtonExtender ConfirmText="Удалить тип отчета. Вы уверены?" runat="server"
                                                            TargetControlID="DeleteReportTypeListLinkBtn" ID="DeleteReportTypeListLinkBtn_ConfirmButtonExtender" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:Panel runat="server" ID="AddNewReportTypePanel" Visible="false">
                                            <asp:Label ID="Label13" runat="server" Text="Название: " />
                                            <asp:TextBox runat="server" ID="ReportTypeNameTextBox" />
                                            <asp:Label ID="Label14" runat="server" Text="Короткое название: " />
                                            <asp:TextBox runat="server" ID="ReportTypeShortNameTextBox" />
                                            <asp:Label ID="Label15" runat="server" Text="Полное название: " />
                                            <asp:TextBox runat="server" ID="ReportTypeFullNameTextBox"/>
                                            <asp:Label ID="Label12" runat="server" Text="Название для печати: " />
                                            <asp:TextBox runat="server" ID="ReportTypePrintNameTextBox"/>
                                            <asp:Button runat="server" ID="Button2" OnClick="AddReportTypeButtonClick" Text="Сохранить" />
                                            <asp:Button runat="server" ID="Button3" OnClick="CancelReportTypeButtonClick"
                                                Text="Отменить" />
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
    </form>
</body>
</html>

<script type="text/javascript">
    function setBodyHeightToContentHeight() {
        document.body.style.height = Math.max(document.documentElement.scrollHeight, document.body.scrollHeight) + "px";
    }
    setBodyHeightToContentHeight();
    window.attachEvent('onresize', setBodyHeightToContentHeight);
</script>

