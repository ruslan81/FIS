<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Adminisration_UserControls_UsersTab_UserControl, App_Web_hjrox150" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>

<asp:Panel ID="UsersDataPanel" runat="server" Width="100%" ScrollBars="None" Height="100%">
    <asp:Panel runat="server" ID="ButtonsPanel">
        <asp:UpdatePanel ID="ButtonsUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>            
                <asp:Table ID="UserButtonsTable" runat="server" Width="100%" GridLines="None">
                    <asp:TableRow>
                        <asp:TableCell Width="150px" HorizontalAlign="Left">
                            <uc2:BlueButton ID="NewUserButton" Text="Новый пользователь" CausesValidation="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="EditUserButton" Text="Редактировать" CausesValidation="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="SaveUserButton" Text="Сохранить" CausesValidation="false" Enabled="false"  runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="CancelUserButton" Text="Отмена" CausesValidation="false" Visible="false" runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell />
                        <asp:TableCell Width="100px">
                            <uc2:BlueButton ID="DeleteUserButton" Text="Удалить" runat="server" CausesValidation="false" 
                                OnClientClick="if(confirm('Вы уверены, что хотите удалить этого пользователя? Если это водитель, это удалит всю информацию об этом водителе.')) { return true; } else { return false; }"/>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="DataPanel" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="tabs">                
                <ul style="height:30px;">
		            <li><asp:LinkButton ID="UsersTab" runat="server" Text="Пользователи" href="#tabs-1"/></li>
		            <li><asp:LinkButton ID="DetailedInfoTab" runat="server" Text="Детальная информация" href="#tabs-2"/></li>
	                </ul>
                    <div id="tabs-1">
                        <asp:UpdatePanel ID="UsersDataGridUpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" if="UsersFilterPanel" runat="server" Width="100%" GroupingText="Фильтр" DefaultButton="ApplyUsersFilter">
                                    <asp:Table ID="UsersFilterTable" GridLines="None" runat="server" Width="100%">
                                        <asp:TableRow>
                                            <asp:TableCell HorizontalAlign="Right" Width="70px">
                                                <asp:Label ID="UsersFilterTable_DealerLabel" runat="server" Font-Size="Small" Text="Дилер" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="220px">
                                                <asp:DropDownList ID="UsersFilterTable_DealerDropDownList" runat="server" Width="80%"/>
                                            </asp:TableCell> 
                                               <asp:TableCell HorizontalAlign="Right"  Width="70px">
                                                <asp:Label ID="UsersFilterTable_TypeLabel" runat="server" Font-Size="Small" Text="Тип" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="220px">
                                                <asp:DropDownList ID="UsersFilterTable_TypeDropDownList" runat="server" Width="80%"/>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Right"  Width="70px">
                                                <asp:Label ID="UsersFilterTable_RoleLabel" runat="server" Font-Size="Small" Text="Роль" />
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="220px">
                                                <asp:DropDownList ID="UsersFilterTable_RoleDropDownList" runat="server" Width="80%"/>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left">
                                                <asp:Button runat="server" ID="ApplyUsersFilter" CausesValidation="false" Text="Применить" OnClick="ApplyUsersFilter_Click" />
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:Panel>            
                                <div class="ui-jqgrid">
                                    <asp:DataGrid ID="UsersDataGrid" runat="server" Width="100%"  AutoGenerateColumns="false" 
                                        HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                                        CellSpacing="0" CellPadding="1" rules="all" BorderColor="#CCC" border="0"
                                        ItemStyle-Font-Size="8">
                                        <Columns>
                                            <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15px">
                                                <ItemTemplate>                                                                                    
                                                    <asp:RadioButton ID="UsersDataGrid_RadioButton" onclick="javascript:CheckOtherIsChecked(this);"
                                                        runat="server" AutoPostBack="true" OnCheckedChanged="UsersDataGrid_RadioButton_Checked" CausesValidation="false"
                                                    /> 
                                                </ItemTemplate> 
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="DEALER" HeaderText="Дилер" ReadOnly="true"  />
                                            <asp:BoundColumn DataField="SURNAME" HeaderText="Фамилия" ReadOnly="true"  />
                                            <asp:BoundColumn DataField="NAME" HeaderText="Имя" ReadOnly="true"  />
                                            <asp:BoundColumn DataField="PATRONYMIC" HeaderText="Отчество"/>
                                            <asp:BoundColumn DataField="LOGIN" HeaderText="Логин"/>
                                            <asp:BoundColumn DataField="REG_DATE" HeaderText="Дата регистрации"/>
                                            <asp:BoundColumn DataField="USER_TYPE" HeaderText="Тип пользователя"/>
                                            <asp:BoundColumn DataField="ROLE" HeaderText="Роль"/>
                                            <asp:BoundColumn DataField="STATE" HeaderText="Состояние"/>
                                        </Columns>
                                    </asp:DataGrid>
                                </div>
                                <asp:HiddenField ID="Selected_UsersDataGrid_Index" runat="server" />
                            </ContentTemplate> 
                        </asp:UpdatePanel>   
                    </div>
                    <div id="tabs-2">
                        <asp:UpdatePanel ID="DetailedInformationUpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="DetailedInformationPanel" runat="server" Enabled="false">
                                    <asp:Panel ID="Panel2" runat="server" BorderWidth="1px" BorderColor="LightGray">
                                        <asp:Table runat="server" ID="DetailedInfoMainTable" Width="100%" CellPadding="5">
                                            <asp:TableRow>
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_SurName_Label" Text="Фамилия" /> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_SurName_TextBox" BackColor="LightGreen" Width="100%"/> 
                                                    <asp:RequiredFieldValidator ID="DetailedInfo_SurName_TextBox_ReqFVal" runat="server" 
                                                        ErrorMessage="<b>Поле пустое!</b><p>Введите фамилию пользователя.</p>" ControlToValidate="DetailedInfo_SurName_TextBox" Display="None"/>
                                                    <asp:ValidatorCalloutExtender ID="DetailedInfo_SurName_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_SurName_TextBox_ReqFVal"
                                                        HighlightCssClass="validatorCalloutHighlight"/>
                                                </asp:TableCell>
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_Name_Label" Text="Имя"/> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_Name_TextBox" Width="100%" BackColor="LightGreen"/> 
                                                    <asp:RequiredFieldValidator ID="DetailedInfo_Name_TextBox_ReqFVal" runat="server" 
                                                        ErrorMessage="<b>Поле пустое!</b><p>Введите имя пользователя.</p>" ControlToValidate="DetailedInfo_Name_TextBox" Display="None"/>
                                                    <asp:ValidatorCalloutExtender ID="DetailedInfo_Name_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_Name_TextBox_ReqFVal"
                                                        HighlightCssClass="validatorCalloutHighlight"/>
                                                </asp:TableCell>
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_Patronymic_Label" Text="Отчество"/> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_Patronymic_TextBox" Width="100%" BackColor="LightGreen"/> 
                                                    <asp:RequiredFieldValidator ID="DetailedInfo_Patronymic_TextBox_ReqFVal" runat="server" 
                                                        ErrorMessage="<b>Поле пустое!</b><p>Введите отчество пользователя.</p>" ControlToValidate="DetailedInfo_Patronymic_TextBox" Display="None"/>
                                                    <asp:ValidatorCalloutExtender ID="DetailedInfo_Patronymic_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_Patronymic_TextBox_ReqFVal"
                                                        HighlightCssClass="validatorCalloutHighlight"/>
                                                </asp:TableCell>
                                                <asp:TableCell Width="10%">
                                                    <asp:CheckBox ID="DetailedInfo_ONOFF_CheckBox" runat="server" Text="Включен" />
                                                </asp:TableCell>
                                                <asp:TableCell />
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_RegDate_Label" Text="Дата регистрации"/> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_RegDate_TextBox" Width="100%" BackColor="LightGray" Enabled="false"/> 
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <asp:Label runat="server" ID="DetailedInfo_Login_Label" Text="Логин" /> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_Login_TextBox" Width="100%"/>    
                                                    <asp:RequiredFieldValidator ID="DetailedInfo_Login_TextBox_ReqFVal" runat="server" 
                                                        ErrorMessage="<b>Поле пустое!</b><p>Введите имя для входа.</p>" ControlToValidate="DetailedInfo_Login_TextBox" Display="None"/>
                                                    <asp:ValidatorCalloutExtender ID="DetailedInfo_Login_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_Login_TextBox_ReqFVal"
                                                        HighlightCssClass="validatorCalloutHighlight"/>                         
                                                </asp:TableCell>
                                                <asp:TableCell />
                                                <asp:TableCell />
                                                <asp:TableCell />
                                                <asp:TableCell />
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_RegEndDate_Label" Text="Срок окончания регистрации"/> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_RegEndDate_TextBox" Width="100%" BackColor="LightGray" Enabled="false"/>                                               
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <asp:Label runat="server" ID="DetailedInfo_Password_Label" Text="Пароль" /> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_Password_TextBox" TextMode="Password" Width="100%"/>                             
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label runat="server" ID="DetailedInfo_PasswordConfirm_Label" Text="Пароль(подтверждение)" /> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_PasswordConfirm_TextBox" TextMode="Password" Width="100%"/>                             
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                        <asp:Table runat="server" ID="DetailedInfoDealerRoleTable" Width="100%" CellPadding="5">
                                            <asp:TableRow>
                                                <asp:TableCell Width="41%">
                                                    <asp:Label runat="server" ID="DetailedInfo_Dealer_Label" Text="Дилер" /> 
                                                    <asp:DropDownList runat="server" ID="DetailedInfo_Dealer_DropDown" BackColor="Coral" Width="100%"/> 
                                                </asp:TableCell>
                                                <asp:TableCell/>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <asp:Label runat="server" ID="DetailedInfo_UserRole_Label" Text="Роль пользователя" /> 
                                                    <asp:DropDownList runat="server" ID="DetailedInfo_UserRole_DropDown" BackColor="Coral" Width="100%"/> 
                                                </asp:TableCell>
                                                <asp:TableCell Width="41%">
                                                    <asp:Label runat="server" ID="DetailedInfo_UserType_Label" Text="Тип пользователя" /> 
                                                    <asp:DropDownList runat="server" ID="DetailedInfo_UserType_DropDown" BackColor="Coral" Width="100%"/> 
                                                </asp:TableCell>
                                                <asp:TableCell/>
                                            </asp:TableRow>
                                        </asp:Table>  
                                    </asp:Panel> 
                                    <asp:Panel ID="Panel3" runat="server" BorderWidth="1px" BorderColor="LightGray">
                                        <asp:Table runat="server" ID="DetailedInfoCountryTable" Width="100%" CellPadding="5">
                                            <asp:TableRow>
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_Country_Label" Text="Страна"/> 
                                                    <asp:DropDownList runat="server" ID="DetailedInfo_Country_DropDown" Width="100%"/>                                  
                                                </asp:TableCell>
                                                 <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_Region_Label" Text="Город"/> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_Region_TextBox" Width="100%"/> 
                                                    <asp:RequiredFieldValidator ID="DetailedInfo_Region_TextBox_ReqFVal" runat="server" 
                                                        ErrorMessage="<b>Поле пустое!</b><p>Введите город.</p>" ControlToValidate="DetailedInfo_Region_TextBox" Display="None"/>
                                                    <asp:ValidatorCalloutExtender ID="DetailedInfo_Region_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_Region_TextBox_ReqFVal"
                                                        HighlightCssClass="validatorCalloutHighlight"/>                                 
                                                </asp:TableCell>
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_ZipCode_Label" Text="Почтовый код"/> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_ZipCode_TextBox" Width="100%"/> 
                                                    <asp:RequiredFieldValidator ID="DetailedInfo_ZipCode_TextBox_ReqFVal" runat="server" 
                                                        ErrorMessage="<b>Поле пустое!</b><p>Введите почтовый код.</p>" ControlToValidate="DetailedInfo_ZipCode_TextBox" Display="None"/>
                                                    <asp:ValidatorCalloutExtender ID="DetailedInfo_ZipCode_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_ZipCode_TextBox_ReqFVal"
                                                        HighlightCssClass="validatorCalloutHighlight"/>                                 
                                                </asp:TableCell>
                                                <asp:TableCell />
                                            </asp:TableRow>
                                        </asp:Table>
                                        <asp:Table runat="server" ID="DetailedInfoTimezoneAddressTable" Width="100%" CellPadding="5">
                                            <asp:TableRow>
                                                <asp:TableCell Width="60%">
                                                    <asp:Label runat="server" ID="DetailedInfo_TimeZone_Label" Text="Часовая зона"/> 
                                                    <asp:DropDownList runat="server" ID="DetailedInfo_TimeZone_DropDown" Width="101%"/>                                  
                                                </asp:TableCell>
                                                <asp:TableCell />
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell Width="60%">
                                                    <asp:Label runat="server" ID="DetailedInfo_AddressOne_Label" Text="Адрес(1)"/> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_AddressOne_TextBox" Width="100%"/>   
                                                    <asp:RequiredFieldValidator ID="DetailedInfo_AddressOne_TextBox_ReqFVal" runat="server" 
                                                        ErrorMessage="<b>Поле пустое!</b><p>Введите адрес номер один(обязательный).</p>" ControlToValidate="DetailedInfo_AddressOne_TextBox" Display="None"/>
                                                    <asp:ValidatorCalloutExtender ID="DetailedInfo_AddressOne_TextBox_ReqFValE" runat="server" TargetControlID="DetailedInfo_AddressOne_TextBox_ReqFVal"
                                                        HighlightCssClass="validatorCalloutHighlight"/>                               
                                                </asp:TableCell>
                                                <asp:TableCell />
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell Width="60%">
                                                    <asp:Label runat="server" ID="DetailedInfo_AddressTwo_Label" Text="Адрес(2)"/> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_AddressTwo_TextBox" Width="100%"/>                                                                           
                                                </asp:TableCell>
                                                <asp:TableCell />
                                            </asp:TableRow>
                                        </asp:Table>
                                        <asp:Table runat="server" ID="DetailedInfoPhoneNumbersTable" Width="100%" CellPadding="5">
                                            <asp:TableRow>
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_PhoneNumb_Label" Text="Телефон" /> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_PhoneNumb_TextBox" Text="+" Width="100%"/> 
                                                </asp:TableCell>
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_Fax_Label" Text="Факс"/> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_Fax_TextBox" Width="100%"/> 
                                                </asp:TableCell>
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_Email_Label" Text="E-Mail"/> 
                                                    <asp:TextBox runat="server" ID="DetailedInfo_Email_TextBox" ForeColor="Blue" Text="@" Width="100%"/> 
                                                </asp:TableCell>
                                                <asp:TableCell />
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:Panel>                      
                                    <asp:Panel ID="Panel4" runat="server" BorderWidth="1px" BorderColor="LightGray">
                                         <asp:Table runat="server" ID="Table1" Width="100%" CellPadding="5">
                                            <asp:TableRow>
                                                <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_SiteLang_Label" Text="Язык (экран)"/> 
                                                    <asp:DropDownList runat="server" ID="DetailedInfo_SiteLang_DropDown" Width="100%"/>                                  
                                                </asp:TableCell>
                                                 <asp:TableCell Width="20%">
                                                    <asp:Label runat="server" ID="DetailedInfo_ReportsLang_Label" Text="Язык (отчеты)"/> 
                                                    <asp:DropDownList runat="server" ID="DetailedInfo_ReportsLang_DropDown" Width="100%"/>                                  
                                                </asp:TableCell>
                                                <asp:TableCell />
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
