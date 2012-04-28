<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Adminisration_UserControls_AccountsTab_UserControl, App_Web_a50yprwt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>


<asp:Panel ID="AccountsDataPanel" runat="server" Width="100%" ScrollBars="Auto" Height="100%">
    <asp:Panel runat="server" ID="ButtonsPanel">
        <asp:UpdatePanel ID="ButtonsUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="button">
                <asp:Table ID="UserButtonsTable" runat="server" Width="100%" GridLines="None">
                    <asp:TableRow>
                        <asp:TableCell Width="150px" HorizontalAlign="Left">
                            <uc2:BlueButton ID="NewAccountButton" Text="Новый аккаунт" CausesValidation="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="EditAccountButton" Text="Редактировать" CausesValidation="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="SaveAccountButton" Text="Сохранить" Enabled="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="CancelAccountButton" Text="Отмена" Visible="false" CausesValidation="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell />
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="DeleteAccountButton" Text="Удалить" runat="server" CausesValidation="false" 
                                OnClientClick="if(confirm('Вы уверены, что хотите удалить этот аккаунт?')) { return true; } else { return false; }"/>
                          
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="SaveAccountButton" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="DataPanel" runat="server">
                            <asp:UpdatePanel ID="DetailedInformationUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:HiddenField ID="Selected_AccountsDataGrid_Index" runat="server" />
                                    <asp:Panel ID="DetailedInformationPanel" runat="server" Enabled="false">
                                        <asp:Panel ID="Panel1" runat="server" BorderWidth="1px" style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;" BorderColor="LightGray">
                                            <asp:Table runat="server" ID="DetailedInfoMainTable" Width="100%" CellPadding="5">
                                                <asp:TableRow>
                                                    <asp:TableCell Width="40%">
                                                        <asp:Label runat="server" ID="DetailedInfo_AccountName_Label" Text="Наименование аккаунта" />
                                                        <asp:TextBox runat="server" ID="DetailedInfo_AccountName_TextBox" BackColor="LightGreen" Width="100%" />
                                                        <asp:RequiredFieldValidator ID="DetailedInfo_AccountName_TextBox_ReqFVal" runat="server" 
                                                            ErrorMessage="<b>Поле пустое!</b><p>Введите название аккаунта.</p>" ControlToValidate="DetailedInfo_AccountName_TextBox" Display="None"/>
                                                        <asp:ValidatorCalloutExtender ID="DetailedInfo_AccountName_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_AccountName_TextBox_ReqFVal"
                                                            HighlightCssClass="validatorCalloutHighlight"/>
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="20%">
                                                        <asp:Label runat="server" ID="AccountTypeLabel" Text="Тип аккаунта" />
                                                        <asp:DropDownList runat="server" ID="AccountTypeDropDown" Width="100%"/>
                                                    </asp:TableCell>
                                                    <asp:TableCell Width="10%">
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
                                        <br />
                                        <asp:Panel ID="Panel2" runat="server" BorderWidth="1px" style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;" BorderColor="LightGray">
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
                                        <br />
                                        <asp:Panel ID="Panel3" runat="server" BorderWidth="1px" style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;" BorderColor="LightGray">
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
    </asp:Panel>
</asp:Panel>
