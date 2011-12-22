<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Adminisration_UserControls_GeneralData_UserControl, App_Web_wr2iylfa" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register src="../../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>

<asp:Panel ID="GeneralData" runat="server"> 
    <div id="tabs">
        <ul style="height:30px;">
		    <li><asp:LinkButton ID="GeneralDataTab" runat="server" Text="Обшие сведения" href="#tabs-1"/></li>
		    <li><asp:LinkButton ID="RegistrationDataTab" runat="server" Text="Регистрационные данные" href="#tabs-2"/></li>
	    </ul>
	    <div id="tabs-1">
		    <div style="width:100%; height:100%; padding:2px;">
                    <asp:Panel runat="server"  ID="GeneralDataTab_Panel1" BorderColor="LightGray" Height="62px" BorderWidth="1px">
                        <asp:Table ID="GeneralDataTab_Table1" runat="server" Width="100%" Height="100%">
                            <asp:TableRow>
                                <asp:TableCell Width="30%">
                                    <asp:Label ID="GeneralDataTab_Table_VersionLabel" runat="server" Text="SmartFIS v.0.1" Font-Bold="true" Font-Size="X-Large" />
                                </asp:TableCell>
                            </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="GeneralDataTab_Table_CurConnectDateLabel" runat="server" Text="Текущее подключение с:"/>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="GeneralDataTab_Table_CurConnectDateValue" Font-Underline="true" runat="server"/>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="GeneralDataTab_Panel2" BorderColor="LightGray" Height="62px" BorderWidth="1px">
                        <asp:Table ID="GeneralDataTab_Table2" runat="server" Width="100%" Height="100%">
                            <asp:TableRow>
                                <asp:TableCell Width="30%">
                                    <asp:Label ID="GeneralDataTab_Table_RegDateLabel" runat="server" Text="Дата регистрации в системе:"/>
                                </asp:TableCell>
                                <asp:TableCell  Width="30%">
                                    <asp:Label ID="GeneralDataTab_Table_RegDateValue"  Font-Underline="true"  runat="server"/>
                                </asp:TableCell>
                                <asp:TableCell  Width="20%" HorizontalAlign="Center">
                                    <asp:Label ID="GeneralDataTab_Table_LicenseTypeLabel" runat="server" Text="Тип лицензии:"/>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="Left">
                                    <asp:Label ID="GeneralDataTab_Table_LicenseTypeValue"  runat="server" Font-Bold="true" Font-Underline="true"/>
                                </asp:TableCell>
                            </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="GeneralDataTab_Table_EndRegDateLabel" runat="server" Text="Срок окончания регистрации: "/>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="GeneralDataTab_Table_EndRegDateValue" runat="server" Font-Bold="true" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                </div>
	    </div>
	    <div id="tabs-2">
	        <asp:UpdatePanel ID="FirstUpdatePanel" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>  
                        <div style="width:100%; height:100%; padding:2px;">
                            <asp:Panel runat="server"  ID="RegistrationDataTab_Panel1" BorderColor="LightGray" Height="32px" BorderWidth="1px">
                                <asp:Table ID="RegistrationDataTab_Table1" runat="server" Width="100%" Height="100%">
                                    <asp:TableRow>
                                        <asp:TableCell Width="150px">
                                            <asp:Label ID="RegistrationDataTab_RegCodeLabel" runat="server" Text="Регистрационный код"/>
                                        </asp:TableCell>
                                        <asp:TableCell Width="100px">
                                            <asp:Label ID="RegistrationDataTab_RegCodeValue" Font-Bold="true" runat="server"/>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Right">
                                            <uc2:BlueButton ID="RegistrationDataTab_SaveButton" Text="Сохранить" runat="server" BtnWidth="80"/>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="RegistrationDataTab_Panel2" BorderColor="LightGray" Height="93px" BorderWidth="1px">
                                <asp:Table ID="RegistrationDataTab_Table2" runat="server" Width="100%" Height="100%">
                                    <asp:TableRow Height="8px">
                                        <asp:TableCell Width="200px">
                                            <asp:Label ID="RegistrationDataTab_CityLabel" runat="server" Text="Страна" Font-Size="8" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="200px">
                                            <asp:Label ID="RegistrationDataTab_TownLabel" runat="server" Text="Город" Font-Size="8" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="300px">
                                            <asp:Label ID="RegistrationDataTab_AddressLabel" runat="server" Text="Адрес" Font-Size="8" />
                                        </asp:TableCell>
                                        <asp:TableCell></asp:TableCell>
                                    </asp:TableRow>                                
                                     <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:DropDownList ID="RegistrationDataTab_CityDropDown" OnSelectedIndexChanged="RegistrationDataTab_TownDropDown_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="100%" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:DropDownList ID="RegistrationDataTab_TownDropDown" runat="server"  Width="100%"/>
                                        </asp:TableCell>
                                         <asp:TableCell>
                                            <asp:TextBox ID="RegistrationDataTab_AddressTextBox" runat="server"  Width="95%"/>
                                            <asp:RequiredFieldValidator ID="RegistrationDataTab_AddressTextBox_ReqFVal" runat="server" 
                                                ErrorMessage="<b>Поле пустое!</b><p>Введите адрес.</p>" ControlToValidate="RegistrationDataTab_AddressTextBox" Display="None"/>
                                            <asp:ValidatorCalloutExtender ID="RegistrationDataTab_AddressTextBox_ReqFValE" runat="server" TargetControlID="RegistrationDataTab_AddressTextBox_ReqFVal"
                                                HighlightCssClass="validatorCalloutHighlight"/>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow  Height="8px">
                                        <asp:TableCell>
                                            <asp:Label id="RegistrationDataTab_LanguageLabel" runat="server" Text="Язык" Font-Size="8" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label id="RegistrationDataTab_SaveDataPeriodLabel" runat="server" Text="Срок хранения данных(мес.)" Font-Size="8" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:DropDownList id="RegistrationDataTab_LanguageDropDown" runat="server" Width="100%"/>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox id="RegistrationDataTab_SaveDataPeriodTextBox" runat="server" Width="98%"/>
                                            <asp:RequiredFieldValidator ID="RegistrationDataTab_SaveDataPeriodTextBox_ReqFVal" runat="server" 
                                                ErrorMessage="<b>Поле пустое!</b><p>Введите срок хранения данных(количество месяцев).</p>" ControlToValidate="RegistrationDataTab_SaveDataPeriodTextBox" Display="None"/>
                                            <asp:ValidatorCalloutExtender ID="RegistrationDataTab_SaveDataPeriodTextBox_ReqFValE" runat="server" TargetControlID="RegistrationDataTab_SaveDataPeriodTextBox_ReqFVal"
                                                HighlightCssClass="validatorCalloutHighlight"/>
                                            <asp:FilteredTextBoxExtender ID="RegistrationDataTab_SaveDataPeriodTextBox_FilteredExtender" FilterType="Numbers"
                                                TargetControlID="RegistrationDataTab_SaveDataPeriodTextBox" runat="server"/>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>                       
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="RegistrationDataTab_CityDropDown" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
		</div>	
    </div>
    <asp:Table ID="SecondStageTable" runat="server" GridLines="None" Width="100%">
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" Width="35%">
                <asp:Panel GroupingText="Статистика" runat="server" >
                    <div class="ui-jqgrid">
                        <asp:DataGrid  ID="StatisticGrid" runat="server"  Width="100%" AutoGenerateColumns="true"
                         HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                         CellSpacing="0" CellPadding="3" rules="all" BorderColor="#CCC" border="0"/>
                         </div>
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:Panel runat="server" GroupingText="Сообщения">
                    <div class="ui-jqgrid">
                        <asp:DataGrid  ID="MessagesGrid" runat="server"  Width="100%" AutoGenerateColumns="true"
                            HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                            CellSpacing="0" CellPadding="3" rules="all" BorderColor="#CCC" border="0">
                            <Columns>
                                <asp:TemplateColumn>
                                    <ItemTemplate>                                                                                    
                                        <asp:ImageButton ID="RemoveMessageButton" CommandName="RemoveData"
                                            ImageUrl="~/images/icons/X.png" Width="15" runat="server" CausesValidation = "false"/>
                                        <asp:ConfirmButtonExtender ID="cbe" runat="server"
                                            TargetControlID="RemoveMessageButton" 
                                            ConfirmText="Вы уверены ?"/>                                              
                                        </ItemTemplate>                                                
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div> 
                </asp:Panel>                       
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Label ID="Status" runat="server" Font-Size="Large" ForeColor="Blue" />
</asp:Panel>