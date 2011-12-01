<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Adminisration_UserControls_DealersTab_UserControl, App_Web_44gcsth0" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>

<asp:Panel ID="DealersDataPanel" runat="server" Width="100%" ScrollBars="Auto" Height="100%">
    <asp:Panel runat="server" ID="ButtonsPanel">
        <asp:UpdatePanel ID="ButtonsUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="button">
                <asp:Table ID="UserButtonsTable" runat="server" Width="100%" GridLines="None">
                    <asp:TableRow>
                        <asp:TableCell Width="150px" HorizontalAlign="Left">
                            <uc2:BlueButton ID="NewDealerButton" Text="Новый дилер" CausesValidation="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="EditDealerButton" Text="Редактировать" CausesValidation="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="SaveDealerButton" Text="Сохранить" Enabled="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="CancelDealerButton" Text="Отмена" Visible="false" CausesValidation="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell />
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="DeleteDealerButton" Text="Удалить" runat="server" CausesValidation="false" 
                                OnClientClick="if(confirm('Вы уверены, что хотите удалить этого дилера?')) { return true; } else { return false; }"/>
                          
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="DataPanel" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="tabs">                
                    <ul style="height:30px;">
		                <li><asp:LinkButton ID="DealersTab" runat="server" Text="Дилеры" href="#tabs-1"/></li>
		                <li><asp:LinkButton ID="DetailedInfoTab" runat="server" Text="Детальная информация" href="#tabs-2"/></li>
	                </ul>
                    <div id="tabs-1">
                            <asp:UpdatePanel ID="DealersDataGridUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="ui-jqgrid">
                                        <asp:DataGrid ID="DealersDataGrid" runat="server" Width="100%" AutoGenerateColumns="false" 
                                            HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                                            CellSpacing="0" CellPadding="3" rules="all" BorderColor="#CCC" border="0"
                                            ItemStyle-Font-Size="8">
                                            <Columns>
                                                <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="15px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="DealersDataGrid_RadioButton" runat="server" AutoPostBack="true"
                                                            OnCheckedChanged="DealersDataGrid_RadioButton_Checked" CausesValidation="false"/>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="DEALERNAME" HeaderText="Дилер" ReadOnly="true" />
                                                <asp:BoundColumn DataField="LOGIN" HeaderText="Логин" />
                                                <asp:BoundColumn DataField="REG_DATE" HeaderText="Дата регистрации" />
                                                <asp:BoundColumn DataField="ENDREG_DATE" HeaderText="Окончание регистрации" />
                                                <asp:BoundColumn DataField="COUNTRY" HeaderText="Страна" />
                                                <asp:BoundColumn DataField="CITY" HeaderText="Город" />
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>    
                                    <asp:HiddenField ID="Selected_DealersDataGrid_Index" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                    </div>
                    <div id="tabs-2">
                            <asp:UpdatePanel ID="DetailedInformationUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="DetailedInformationPanel" runat="server" Enabled="false">
                                        <asp:Panel ID="Panel1" runat="server" BorderWidth="1px" BorderColor="LightGray">
                                            <asp:Table runat="server" ID="DetailedInfoMainTable" Width="100%" CellPadding="5">
                                                <asp:TableRow>
                                                    <asp:TableCell Width="60%">
                                                        <asp:Label runat="server" ID="DetailedInfo_DealerName_Label" Text="Наименование дилера" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_DealerName_TextBox" BackColor="LightGreen" Width="100%" />
                                                        <asp:RequiredFieldValidator ID="DetailedInfo_DealerName_TextBox_ReqFVal" runat="server" 
                                                            ErrorMessage="<b>Поле пустое!</b><p>Введите название дилера.</p>" ControlToValidate="DetailedInfo_DealerName_TextBox" Display="None"/>
                                                        <asp:ValidatorCalloutExtender ID="DetailedInfo_DealerName_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_DealerName_TextBox_ReqFVal"
                                                            HighlightCssClass="validatorCalloutHighlight"/>
                                                    </asp:TableCell><asp:TableCell Width="10%">
                                                        <asp:CheckBox ID="DetailedInfo_ONOFF_CheckBox" runat="server" Text="Включен" />
                                                    </asp:TableCell><asp:TableCell />
                                                    <asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_RegDate_Label" Text="Дата регистрации" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_RegDate_TextBox" Width="100%" BackColor="LightGray"
                                                            Enabled="false" />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                            <asp:Table runat="server" ID="DetailedInfoLogPassTable" Width="100%" CellPadding="5">
                                                <asp:TableRow>
                                                    <asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_Login_Label" Text="Логин" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_Login_TextBox" Width="100%" />
                                                        <asp:RequiredFieldValidator ID="DetailedInfo_Login_ReqFVal" runat="server" 
                                                            ErrorMessage="<b>Поле пустое!</b><p>Введите имя для входа(логин).</p>" ControlToValidate="DetailedInfo_Login_TextBox" Display="None"/>
                                                        <asp:ValidatorCalloutExtender ID="DetailedInfo_Login_ReqFValE" runat="server" TargetControlID="DetailedInfo_Login_ReqFVal"
                                                            HighlightCssClass="validatorCalloutHighlight"/>
                                                    </asp:TableCell>
                                                    <asp:TableCell />
                                                    <asp:TableCell />
                                                    <asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_RegEndDate_Label" Text="Срок окончания регистрации" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_RegEndDate_TextBox" Width="100%" BackColor="LightGray"
                                                            Enabled="false" />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_Password_Label" Text="Пароль" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_Password_TextBox" TextMode="Password"
                                                            Width="100%" />
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_PasswordConfirm_Label" Text="Пароль(подтверждение)" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_PasswordConfirm_TextBox" TextMode="Password"
                                                            Width="100%" />
                                                    </asp:TableCell>
                                                    <asp:TableCell />
                                                    <asp:TableCell />
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel2" runat="server" BorderWidth="1px" BorderColor="LightGray">
                                            <asp:Table runat="server" ID="DetailedInfoCountryTable" Width="100%" CellPadding="5">
                                                <asp:TableRow>
                                                    <asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_Country_Label" Text="Страна" />
                                                        <asp:DropDownList runat="server" ID="DetailedInfo_Country_DropDown" Width="100%" />
                                                    </asp:TableCell><asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_Region_Label" Text="Город" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_Region_TextBox" Width="100%" />
                                                        <asp:RequiredFieldValidator ID="DetailedInfo_Region_TextBox_ReqFVal" runat="server" 
                                                            ErrorMessage="<b>Поле пустое!</b><p>Введите город.</p>" ControlToValidate="DetailedInfo_Region_TextBox" Display="None"/>
                                                        <asp:ValidatorCalloutExtender ID="DetailedInfo_Region_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_Region_TextBox_ReqFVal"
                                                            HighlightCssClass="validatorCalloutHighlight"/>
                                                    </asp:TableCell><asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_ZipCode_Label" Text="Почтовый код" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_ZipCode_TextBox" Width="100%" />
                                                        <asp:RequiredFieldValidator ID="DetailedInfo_ZipCode_TextBox_ReqFVal" runat="server" 
                                                            ErrorMessage="<b>Поле пустое!</b><p>Введите почтовый код.</p>" ControlToValidate="DetailedInfo_ZipCode_TextBox" Display="None"/>
                                                        <asp:ValidatorCalloutExtender ID="DetailedInfo_ZipCode_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_ZipCode_TextBox_ReqFVal"
                                                            HighlightCssClass="validatorCalloutHighlight"/>
                                                    </asp:TableCell><asp:TableCell />
                                                </asp:TableRow>
                                            </asp:Table>
                                            <asp:Table runat="server" ID="DetailedInfoTimezoneAddressTable" Width="100%" CellPadding="5">
                                                <asp:TableRow>
                                                    <asp:TableCell Width="60%">
                                                        <asp:Label runat="server" ID="DetailedInfo_TimeZone_Label" Text="Часовая зона" />
                                                        <asp:DropDownList runat="server" ID="DetailedInfo_TimeZone_DropDown" Width="101%" />
                                                    </asp:TableCell><asp:TableCell />
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell Width="60%">
                                                        <asp:Label runat="server" ID="DetailedInfo_AddressOne_Label" Text="Адрес(1)" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_AddressOne_TextBox" Width="100%" />
                                                        <asp:RequiredFieldValidator ID="DetailedInfo_AddressOne_TextBox_ReqFVal" runat="server" 
                                                            ErrorMessage="<b>Поле пустое!</b><p>Введите адрес номер один.</p>" ControlToValidate="DetailedInfo_AddressOne_TextBox" Display="None"/>
                                                        <asp:ValidatorCalloutExtender ID="DetailedInfo_AddressOne_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_AddressOne_TextBox_ReqFVal"
                                                            HighlightCssClass="validatorCalloutHighlight"/>
                                                    </asp:TableCell><asp:TableCell />
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell Width="60%">
                                                        <asp:Label runat="server" ID="DetailedInfo_AddressTwo_Label" Text="Адрес(2)" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_AddressTwo_TextBox" Width="100%" />                                                      
                                                    </asp:TableCell><asp:TableCell />
                                                </asp:TableRow>
                                            </asp:Table>
                                            <asp:Table runat="server" ID="DetailedInfoPhoneNumbersTable" Width="100%" CellPadding="5">
                                                <asp:TableRow>
                                                    <asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_PhoneNumb_Label" Text="Телефон" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_PhoneNumb_TextBox" Text="+" Width="100%" />
                                                    </asp:TableCell><asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_Fax_Label" Text="Факс" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_Fax_TextBox" Width="100%" />
                                                    </asp:TableCell><asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_Email_Label" Text="E-Mail" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_Email_TextBox" ForeColor="Blue" Text="@"
                                                            Width="100%" />  
                                                        <asp:RegularExpressionValidator ID="EmailRegexValidator" runat="server"  ControlToValidate="DetailedInfo_Email_TextBox"
                                                            ErrorMessage="<b>Ошибка!</b><p>Проверьте введенный E-mail адрес</p>" ValidationExpression=".*@.{2,}\..{2,}" Display="None"/>                                                    
                                                        <asp:ValidatorCalloutExtender ID="EmailRegexValidatorE" runat="server" TargetControlID="EmailRegexValidator"
                                                            HighlightCssClass="validatorCalloutHighlight"/>
                                                    </asp:TableCell><asp:TableCell />
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel3" runat="server" BorderWidth="1px" BorderColor="LightGray">
                                            <asp:Table runat="server" ID="Table1" Width="100%" CellPadding="5">
                                                <asp:TableRow>
                                                    <asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_SiteLang_Label" Text="Язык (экран)" />
                                                        <asp:DropDownList runat="server" ID="DetailedInfo_SiteLang_DropDown" Width="100%" />
                                                    </asp:TableCell><asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="DetailedInfo_ReportsLang_Label" Text="Язык (отчеты)" />
                                                        <asp:DropDownList runat="server" ID="DetailedInfo_ReportsLang_DropDown" Width="100%" />
                                                    </asp:TableCell><asp:TableCell />
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:Panel>
                                    </asp:Panel>
                                    <asp:HiddenField ID="NewOrEditUser_hdnField" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>        
        </asp:UpdatePanel>      
    </asp:Panel>
</asp:Panel>
