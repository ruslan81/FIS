<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Settings_UserControls_UserVehicleTab, App_Web_wtoelusk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Panel ID="Vehicles" runat="server" Width="100%" ScrollBars="None">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <link rel="Stylesheet" type="text/css" href="~/css/custom-theme/ui.jqgrid.css" id="style" runat="server" visible="false" />
            <asp:Panel ID="VehiclesDataGridPanel" runat="server" class="ui-jqgrid">
                <asp:DataGrid ID="VehiclesDataGrid" runat="server" Width="100%"  AutoGenerateColumns="false" 
                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                    CellSpacing="0" CellPadding="3" rules="all" BorderColor="#CCC" border="0">
                    <Columns>
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="15px">
                            <ItemTemplate>
                                <asp:RadioButton ID="VehiclesDataGrid_RadioButton" runat="server" AutoPostBack="true" onclick="javascript:CheckOtherIsChecked(this);"
                                    OnCheckedChanged="VehiclesDataGrid_RadioButton_Checked" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn HeaderText="Код" DataField="#" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundColumn HeaderText="Гос номер" DataField="Номер ТС" />
                        <asp:BoundColumn HeaderText="Идентификационный номер ТС" DataField="VIN" />
                        <asp:BoundColumn HeaderText="Комментарий" DataField="Комментарий" />
                    </Columns>
                </asp:DataGrid>
            </asp:Panel>   
            <asp:HiddenField ID="Selected_VehiclesDataGrid_Index" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="EditPanel" runat="server" Width="100%">        
        <asp:TabContainer ID="TabContainer1" runat="server" Width="100%" Height="450px" ActiveTabIndex="0">
            <asp:TabPanel HeaderText="Основные" runat="server" ID="GeneralTabPanel">
                <ContentTemplate>
                    <asp:Table runat="server" ID="GenearalTableInfonPhoto">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Table ID="GeneralVehicleInfoTable" runat="server" GridLines="None" CellPadding="10">
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="Right">
                                            <asp:Label ID="GeneralVehInfo_VinLabel" runat="server" Text="VIN" />
                                        </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                            <asp:TextBox ID="GeneralVehInfo_VinTextBox" runat="server" Width="300px" />
                                            <asp:RequiredFieldValidator ID="VinTextBoxReqFVal" runat="server" ErrorMessage="Введите идентификационный номер транспортного средства !"
                                                ControlToValidate="GeneralVehInfo_VinTextBox" Display="None" />
                                            <asp:ValidatorCalloutExtender ID="VinTextBoxReqFValE" runat="server" TargetControlID="VinTextBoxReqFVal"
                                                HighlightCssClass="validatorCalloutHighlight" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="Right" Width="200px">
                                            <asp:Label ID="GeneralVehInfo_RegNumbLabel" runat="server" Text="Регистрационный номер" />
                                        </asp:TableCell><asp:TableCell HorizontalAlign="Left" Width="300px">
                                            <asp:TextBox ID="GeneralVehInfo_RegNumbTextBox" runat="server" Width="300px" />
                                            <asp:RequiredFieldValidator ID="RegNumbTextBoxReqFVal" runat="server" ErrorMessage="Введите регистрационный номер ТС!"
                                                ControlToValidate="GeneralVehInfo_RegNumbTextBox" Display="None" />
                                            <asp:ValidatorCalloutExtender ID="RegNumbTextBoxReqFValE" runat="server" TargetControlID="RegNumbTextBoxReqFVal"
                                                HighlightCssClass="validatorCalloutHighlight" />
                                        </asp:TableCell>
                                    </asp:TableRow>                                   
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="Right">
                                            <asp:Label ID="GeneralVehInfo_DateBlocked_Label" runat="server" Text="Date Blocked" />
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Left">
                                            <asp:TextBox ID="GeneralVehInfo_DateBlocked_TextBox" runat="server" Width="300px" />
                                            <asp:RequiredFieldValidator ID="DateBlockedTextBoxReqFVal" runat="server" ErrorMessage="Введите дату блокировки транспортного средства !"
                                                ControlToValidate="GeneralVehInfo_DateBlocked_TextBox" Display="None" />
                                            <asp:ValidatorCalloutExtender ID="DateBlockedTextBoxReqFValE" runat="server" TargetControlID="DateBlockedTextBoxReqFVal"
                                                HighlightCssClass="validatorCalloutHighlight" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="Right">
                                            <asp:Label ID="GeneralVehInfo_Priority_Label" runat="server" Text="Priority" />
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Left">
                                            <asp:TextBox ID="GeneralVehInfo_Priority_TextBox" runat="server" Width="300px" MaxLength="3" />
                                            <asp:RequiredFieldValidator ID="PriorityTextBoxReqFVal" runat="server" ErrorMessage="Введите приоритет технического обсуживания транспортного средства !"
                                                ControlToValidate="GeneralVehInfo_Priority_TextBox" Display="None" />
                                            <asp:ValidatorCalloutExtender ID="PriorityTextBoxReqFValE" runat="server" TargetControlID="PriorityTextBoxReqFVal"
                                                HighlightCssClass="validatorCalloutHighlight" />
                                            <asp:FilteredTextBoxExtender ID="GeneralVehInfo_Priority_TextBox_FilteredExtender"
                                                FilterType="Numbers" TargetControlID="GeneralVehInfo_Priority_TextBox" runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="Right">
                                            <asp:Label ID="GeneralVehInfo_Manufacturer_Label" runat="server" Text="Manufacturer" />
                                        </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                            <asp:TextBox ID="GeneralVehInfo_Manufacturer_TextBox" runat="server" Width="300px" />
                                            <asp:RequiredFieldValidator ID="ManufacturerTextBoxReqFVal" runat="server" ErrorMessage="Введите производителя транспортного средства !"
                                                ControlToValidate="GeneralVehInfo_Manufacturer_TextBox" Display="None" />
                                            <asp:ValidatorCalloutExtender ID="ManufacturerTextBoxReqFValE" runat="server" TargetControlID="ManufacturerTextBoxReqFVal"
                                                HighlightCssClass="validatorCalloutHighlight" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="Right">
                                            <asp:Label ID="GeneralVehInfo_Comment_Label" runat="server" Text="Комментарий к ТС: " />
                                        </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                            <asp:TextBox ID="GeneralVehInfo_Comment_TextBox" runat="server" Width="300px" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                            <asp:TableCell VerticalAlign="Bottom" Width="30%">
                                <asp:Panel Width="320px" Height="200px" GroupingText="Фотография" runat="server" ID="VehFotoPanel">
                                    <asp:Image ID="VehiclesPhotoImage" ImageUrl="~/images/unknown_vehicle.jpg" runat="server" Width="100%"/>
                                    <br />
                                    <asp:UpdatePanel ID="FileUploadUpdatePanel" runat="server">
                                        <ContentTemplate>
                                            <asp:FileUpload ID="MyFileUpload"  runat="server" Width="70%" Font-Size="9"/>
                                            <asp:Button ID="ImageUploadButton"  runat="server" Text="Обновить" Width="25%"
                                                OnClick="Upload_Click" CausesValidation="false" Font-Size="9" /> 
                                            <asp:ConfirmButtonExtender ID="ImageUploadButton_ConfirmExtender" TargetControlID="ImageUploadButton" 
                                                ConfirmText="Вы уверены, что хотите обновить фотографию?" runat="server"/>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="ImageUploadButton" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel HeaderText="Дополнительные" runat="server" ID="AdditionalTabPanel">
                <ContentTemplate>
                    <asp:Table ID="AdditionalEditTable" runat="server" GridLines="None" CellPadding="10">
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="AdditionalEdit_NameLabel" runat="server" Text="Название" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="AdditionalEdit_MinValLabel" runat="server" Text="Минимальное значение" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="AdditionalEdit_MaxValLabel" runat="server" Text="Максимальное значение" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="AdditionalEdit_MeasureLabel" runat="server" Text="Единица измерения" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="AdditionalEdit_Bak_1_Label" runat="server" Text="Бак 1" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="AdditionalEdit_Bak_1_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="Bak_1_MinValTextBox_ReqFVal" runat="server" ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>"
                                    ControlToValidate="AdditionalEdit_Bak_1_MinValTextBox" Display="None" />
                                <asp:ValidatorCalloutExtender ID="Bak_1_MinValTextBox_ReqFValE" runat="server" TargetControlID="Bak_1_MinValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="AdditionalEdit_Bak_1_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="AdditionalEdit_Bak_1_MinValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="AdditionalEdit_Bak_1_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="Bak_1_MaxValTextBox_ReqFVal" runat="server" ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>"
                                    ControlToValidate="AdditionalEdit_Bak_1_MaxValTextBox" Display="None" />
                                <asp:ValidatorCalloutExtender ID="Bak_1_MaxValTextBox_ReqFValE" runat="server" TargetControlID="Bak_1_MaxValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="AdditionalEdit_Bak_1_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="AdditionalEdit_Bak_1_MaxValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="AdditionalEdit_Bak_1_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="AdditionalEdit_Bak_2_Label" runat="server" Text="Бак 2" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="AdditionalEdit_Bak_2_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="AdditionalEdit_Bak_2_MinValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="AdditionalEdit_Bak_2_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="AdditionalEdit_Bak_2_MinValTextBox_ReqFValE" runat="server"
                                    TargetControlID="AdditionalEdit_Bak_2_MinValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="AdditionalEdit_Bak_2_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="AdditionalEdit_Bak_2_MinValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="AdditionalEdit_Bak_2_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="AdditionalEdit_Bak_2_MaxValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="AdditionalEdit_Bak_2_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="AdditionalEdit_Bak_2_MaxValTextBox_ReqFValE" runat="server"
                                    TargetControlID="AdditionalEdit_Bak_2_MaxValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="AdditionalEdit_Bak_2_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="AdditionalEdit_Bak_2_MaxValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="AdditionalEdit_Bak_2_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="AdditionalEdit_LoadCcarrying_Label" runat="server" Text="Бак 2" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="AdditionalEdit_LoadCcarrying_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="AdditionalEdit_LoadCcarrying_MinValTextBox_ReqFVal"
                                    runat="server" ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="AdditionalEdit_LoadCcarrying_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="AdditionalEdit_LoadCcarrying_MinValTextBox_ReqFValE"
                                    runat="server" TargetControlID="AdditionalEdit_LoadCcarrying_MinValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="AdditionalEdit_LoadCcarrying_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="AdditionalEdit_LoadCcarrying_MinValTextBox"
                                    runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="AdditionalEdit_LoadCcarrying_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="AdditionalEdit_LoadCcarrying_MaxValTextBox_ReqFVal"
                                    runat="server" ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="AdditionalEdit_LoadCcarrying_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="AdditionalEdit_LoadCcarrying_MaxValTextBox_ReqFValE"
                                    runat="server" TargetControlID="AdditionalEdit_LoadCcarrying_MaxValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="AdditionalEdit_LoadCcarrying_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="AdditionalEdit_LoadCcarrying_MaxValTextBox"
                                    runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="AdditionalEdit_LoadCcarrying_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="AdditionalEdit_VehicleType_Label" runat="server" Text="Vehicle Type" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:DropDownList ID="AdditionalEdit_VehicleType_DropDown" runat="server" Width="108px" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left" Width="200px">
                                <asp:Label ID="AdditionalEdit_FuelType_Label" runat="server" Text="Fuel Type" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="AdditionalEdit_FuelType_DropDown" runat="server" Width="100px" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="AdditionalEdit_TO1_Label" runat="server" Text="Бак 2" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="AdditionalEdit_TO1_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="AdditionalEdit_TO1_MinValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="AdditionalEdit_TO1_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="AdditionalEdit_TO1_MinValTextBox_ReqFValE" runat="server"
                                    TargetControlID="AdditionalEdit_TO1_MinValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="AdditionalEdit_TO1_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="AdditionalEdit_TO1_MinValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="AdditionalEdit_TO1_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="AdditionalEdit_TO1_MaxValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="AdditionalEdit_TO1_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="AdditionalEdit_TO1_MaxValTextBox_ReqFValE" runat="server"
                                    TargetControlID="AdditionalEdit_TO1_MaxValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="AdditionalEdit_TO1_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="AdditionalEdit_TO1_MaxValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="AdditionalEdit_TO1_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="AdditionalEdit_TO2_Label" runat="server" Text="Бак 2" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="AdditionalEdit_TO2_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="AdditionalEdit_TO2_MinValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="AdditionalEdit_TO2_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="AdditionalEdit_TO2_MinValTextBox_ReqFValE" runat="server"
                                    TargetControlID="AdditionalEdit_TO2_MinValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="AdditionalEdit_TO2_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="AdditionalEdit_TO2_MinValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="AdditionalEdit_TO2_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="AdditionalEdit_TO2_MaxValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="AdditionalEdit_TO2_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="AdditionalEdit_TO2_MaxValTextBox_ReqFValE" runat="server"
                                    TargetControlID="AdditionalEdit_TO2_MaxValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="AdditionalEdit_TO2_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="AdditionalEdit_TO2_MaxValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="AdditionalEdit_TO2_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel HeaderText="Оборудование" runat="server" ID="HardwareTabPanel">
                <ContentTemplate>
                    <asp:Table ID="HardwareEditTable" runat="server" GridLines="None" CellPadding="10">
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                            </asp:TableCell>
                            <asp:TableCell>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="250px">
                                <asp:Label ID="HardwareEdit_DeviceTypeLabel" runat="server" Text="тип оборудования" />
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left" Width="400px">
                                <asp:Label ID="HardwareEdit_DeviceTypeTextBox" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="HardwareEdit_DeviceNameLabel" runat="server" Text="название оборудования" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="HardwareEdit_DeviceNameTextBox" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="HardwareEdit_DeviceNumberLabel" runat="server" Text="номер оборудования" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="HardwareEdit_DeviceNumberTextBox" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="HardwareEdit_DeviceProductionDateLabel" runat="server" Text="дата выпуска оборудования" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="HardwareEdit_DeviceProductionDateTextBox" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="HardwareEdit_DeviceFirmwareVersionLabel" runat="server" Text="Версия ПО оборудования" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="HardwareEdit_DeviceFirmwareVersionTextBox" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="HardwareEdit_DevicePhoneSimNumberLabel" runat="server" Text="Телефонный номер SIM" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="HardwareEdit_DevicePhoneSimNumberTextBox" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="HardwareEdit_LastCalibPurposeLabel" runat="server" Text="Причина последней калибровки" />
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="HardwareEdit_LastCalibPurposeTextBox" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="HardwareEdit_WhoCalibLabel" runat="server" Text="Кто калибровал" />
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="HardwareEdit_WhoCalibTextBox" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="HardwareEdit_WhoCalibCardNumberLabel" runat="server" Text="Номер карты калибровщика" />
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="HardwareEdit_WhoCalibCardNumberTextBox" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="HardwareEdit_NextCalibDateLabel" runat="server" Text="Следующая калибровка" />
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="HardwareEdit_NextCalibDateTextBox" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel HeaderText="Коэффициенты" runat="server" ID="KoeffTabPanel">
                <ContentTemplate>
                    <asp:Table ID="KoefEditTable" runat="server" CellPadding="10">
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="KoefEdit_NameLabel" runat="server" Text="Название" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="KoefEdit_MinValLabel" runat="server" Text="Минимальное значение" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="KoefEdit_MaxValLabel" runat="server" Text="Максимальное значение" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="KoefEdit_MeasureLabel" runat="server" Text="Единица измерения" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="KoefEditTable_NomRPM_Lable" runat="server" Text="Номинальные обороты" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_NomRPM_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_NomRPM_MinValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_NomRPM_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_NomRPM_MinValTextBox_ReqFValE" runat="server"
                                    TargetControlID="KoefEditTable_NomRPM_MinValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_NomRPM_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_NomRPM_MinValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_NomRPM_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_NomRPM_MaxValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_NomRPM_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_NomRPM_MaxValTextBox_ReqFValE" runat="server"
                                    TargetControlID="KoefEditTable_NomRPM_MaxValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_NomRPM_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_NomRPM_MaxValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="KoefEditTable_NomRPM_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="KoefEditTable_MaxSpeed_Label" runat="server" Text="Максимальная скорость" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_MaxSpeed_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_MaxSpeed_MinValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_MaxSpeed_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_MaxSpeed_MinValTextBox_ReqFValE"
                                    runat="server" TargetControlID="KoefEditTable_MaxSpeed_MinValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_MaxSpeed_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_MaxSpeed_MinValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_MaxSpeed_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_MaxSpeed_MaxValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_MaxSpeed_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_MaxSpeed_MaxValTextBox_ReqFValE"
                                    runat="server" TargetControlID="KoefEditTable_MaxSpeed_MaxValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_MaxSpeed_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_MaxSpeed_MaxValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="KoefEditTable_MaxSpeed_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="KoefEditTable_Manevr_Label" runat="server" Text="Маневрирование" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_Manevr_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_Manevr_MinValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_Manevr_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_Manevr_MinValTextBox_ReqFValE" runat="server"
                                    TargetControlID="KoefEditTable_Manevr_MinValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_Manevr_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_Manevr_MinValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_Manevr_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_Manevr_MaxValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_Manevr_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_Manevr_MaxValTextBox_ReqFValE" runat="server"
                                    TargetControlID="KoefEditTable_Manevr_MaxValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_Manevr_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_Manevr_MaxValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="KoefEditTable_Manevr_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="KoefEditTable_City_Label" runat="server" Text="Магистраль" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_City_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_City_MinValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_City_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_City_MinValTextBox_ReqFValE" runat="server"
                                    TargetControlID="KoefEditTable_City_MinValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_City_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_City_MinValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_City_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_City_MaxValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_City_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_City_MaxValTextBox_ReqFValE" runat="server"
                                    TargetControlID="KoefEditTable_City_MaxValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_City_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_City_MaxValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="KoefEditTable_City_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="KoefEditTable_Magistral_Label" runat="server" Text="Магистраль" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_Magistral_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_Magistral_MinValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_Magistral_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_Magistral_MinValTextBox_ReqFValE"
                                    runat="server" TargetControlID="KoefEditTable_Magistral_MinValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_Magistral_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_Magistral_MinValTextBox"
                                    runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_Magistral_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_Magistral_MaxValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_Magistral_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_Magistral_MaxValTextBox_ReqFValE"
                                    runat="server" TargetControlID="KoefEditTable_Magistral_MaxValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_Magistral_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_Magistral_MaxValTextBox"
                                    runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="KoefEditTable_Magistral_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="KoefEditTable_NomFuelConsumpion_Label" runat="server" Text="Номинальный расход" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_NomFuelConsumpion_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_NomFuelConsumpion_MinValTextBox_ReqFVal"
                                    runat="server" ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_NomFuelConsumpion_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_NomFuelConsumpion_MinValTextBox_ReqFValE"
                                    runat="server" TargetControlID="KoefEditTable_NomFuelConsumpion_MinValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_NomFuelConsumpion_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_NomFuelConsumpion_MinValTextBox"
                                    runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_NomFuelConsumpion_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_NomFuelConsumpion_MaxValTextBox_ReqFVal"
                                    runat="server" ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_NomFuelConsumpion_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_NomFuelConsumpion_MaxValTextBox_ReqFValE"
                                    runat="server" TargetControlID="KoefEditTable_NomFuelConsumpion_MaxValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_NomFuelConsumpion_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_NomFuelConsumpion_MaxValTextBox"
                                    runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="KoefEditTable_NomFuelConsumpion_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="KoefEditTable_ColdStart_Label" runat="server" Text="Холодный старт" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_ColdStart_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_ColdStart_MinValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_ColdStart_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_ColdStart_MinValTextBox_ReqFValE"
                                    runat="server" TargetControlID="KoefEditTable_ColdStart_MinValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_ColdStart_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_ColdStart_MinValTextBox"
                                    runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_ColdStart_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_ColdStart_MaxValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_ColdStart_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_ColdStart_MaxValTextBox_ReqFValE"
                                    runat="server" TargetControlID="KoefEditTable_ColdStart_MaxValTextBox_ReqFVal"
                                    HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_ColdStart_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_ColdStart_MaxValTextBox"
                                    runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="KoefEditTable_ColdStart_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right" Width="200px">
                                <asp:Label ID="KoefEditTable_HotStop_Label" runat="server" Text="Горячий стоп" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_HotStop_MinValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_HotStop_MinValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_HotStop_MinValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_HotStop_MinValTextBox_ReqFValE" runat="server"
                                    TargetControlID="KoefEditTable_HotStop_MinValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_HotStop_MinValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_HotStop_MinValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:TextBox ID="KoefEditTable_HotStop_MaxValTextBox" runat="server" Width="100px" />
                                <asp:RequiredFieldValidator ID="KoefEditTable_HotStop_MaxValTextBox_ReqFVal" runat="server"
                                    ErrorMessage="<b>Поле пустое!</b><p>Введите значение.</p>" ControlToValidate="KoefEditTable_HotStop_MaxValTextBox"
                                    Display="None" />
                                <asp:ValidatorCalloutExtender ID="KoefEditTable_HotStop_MaxValTextBox_ReqFValE" runat="server"
                                    TargetControlID="KoefEditTable_HotStop_MaxValTextBox_ReqFVal" HighlightCssClass="validatorCalloutHighlight" />
                                <asp:FilteredTextBoxExtender ID="KoefEditTable_HotStop_MaxValTextBox_FilteredExtender"
                                    FilterType="Numbers" TargetControlID="KoefEditTable_HotStop_MaxValTextBox" runat="server" />
                            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                                <asp:Label ID="KoefEditTable_HotStop_MeasureLabel" runat="server" />
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer></asp:Panel>
</asp:Panel>
