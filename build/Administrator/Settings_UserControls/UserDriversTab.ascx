<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Settings_UserControls_UserDriversTab, App_Web_ptdfnxy5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Panel ID="Drivers" runat="server" Width="100%" ScrollBars="None">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <asp:Panel id="DriversDataGridPanel" runat="server" CssClass="ui-jqgrid">
                <asp:DataGrid ID="DriversDataGrid" runat="server"  Width="100%" AutoGenerateColumns="false" 
                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                    CellSpacing="0" CellPadding="3" rules="all" BorderColor="#CCC" border="0">
                    <Columns>
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15px">
                            <ItemTemplate>                                                                                    
                                <asp:RadioButton ID="DriversDataGrid_RadioButton" onclick="javascript:CheckOtherIsChecked(this);"
                                    runat="server" AutoPostBack="true" OnCheckedChanged="DriversDataGrid_RadioButton_Checked"
                                /> 
                            </ItemTemplate> 
                        </asp:TemplateColumn>
                        <asp:BoundColumn HeaderText="№" DataField="#" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundColumn HeaderText="ФИО" DataField="ФИО" />
                        <asp:BoundColumn HeaderText="Номер водителя" DataField="Номер водителя" />
                        <asp:BoundColumn HeaderText="Комментарий" DataField="Комментарий" />
                   </Columns>    
                </asp:DataGrid>
            </asp:Panel>   
            <asp:HiddenField ID="Selected_DriversDataGrid_Index" runat="server" />
        </ContentTemplate>        
    </asp:UpdatePanel>
    
    <asp:Panel ID="EditPanel" runat="server" Width="100%" Visible="false">
    <asp:Table runat="server" Width="100%">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Table ID="EditTable" runat="server" Width="100%" CellPadding="10">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right"  Width="35%">
                    <asp:Label ID="Edit_SurnameLabel" runat="server" Text="Фамилия"/>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:TextBox ID="Edit_SurnameTextBox" runat="server" Width="100%"/>
                    <asp:RequiredFieldValidator ID="SurnameTextBoxReqFVal" runat="server" 
                        ErrorMessage="Введите Фамилию водителя!" ControlToValidate="Edit_SurnameTextBox" Display="None"/>
                    <asp:ValidatorCalloutExtender ID="SurnameTextBoxReqFValE" runat="server" TargetControlID="SurnameTextBoxReqFVal"
                        HighlightCssClass="validatorCalloutHighlight"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label ID="Edit_NameLabel" runat="server" Text="Имя"/>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:TextBox ID="Edit_NameTextBox" runat="server" Width="100%"/>
                    <asp:RequiredFieldValidator ID="NameTextBoxReqFVal" runat="server" 
                        ErrorMessage="Введите имя водителя!" ControlToValidate="Edit_NameTextBox" Display="None"/>
                    <asp:ValidatorCalloutExtender ID="NameTextBoxReqFValE" runat="server" TargetControlID="NameTextBoxReqFVal"
                        HighlightCssClass="validatorCalloutHighlight"/>                    
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label ID="Edit_PatronymicLabel" runat="server" Text="Отчество"/>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:TextBox ID="Edit_PatronymicTextBox" runat="server" Width="100%"/>
                    <asp:RequiredFieldValidator ID="PatronymicTextBoxReqFVal" runat="server" 
                        ErrorMessage="Введите отчество водителя!" ControlToValidate="Edit_PatronymicTextBox" Display="None"/>
                    <asp:ValidatorCalloutExtender ID="PatronymicTextBoxReqFValE" runat="server" TargetControlID="PatronymicTextBoxReqFVal"
                        HighlightCssClass="validatorCalloutHighlight"/>                    
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label ID="Edit_DriversCertificateLabel" runat="server" Text="Водительское удостоверение"/>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:TextBox ID="Edit_DriversCertificateTextBox" runat="server" Width="100%"/>
                    <asp:RequiredFieldValidator ID="DriversCertificateTextBoxReqFVal" runat="server" 
                        ErrorMessage="Введите номер водительского удостоверения!" ControlToValidate="Edit_DriversCertificateTextBox" Display="None"/>
                    <asp:ValidatorCalloutExtender ID="DriversCertificateTextBoxReqFValE" runat="server" TargetControlID="DriversCertificateTextBoxReqFVal"
                        HighlightCssClass="validatorCalloutHighlight"/>     
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label ID="Edit_DriversCardLabel" runat="server" Text="Карточка"/>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:TextBox ID="Edit_DriversCardTextBox" runat="server" Width="100%" MaxLength="16"/>
                     <asp:RequiredFieldValidator ID="DriversCardTextBoxReqFVal" runat="server" 
                        ErrorMessage="Введите номер карты водителя!" ControlToValidate="Edit_DriversCardTextBox" Display="None"/>
                    <asp:ValidatorCalloutExtender ID="DriversCardTextBoxReqFValE" runat="server" TargetControlID="DriversCardTextBoxReqFVal"
                        HighlightCssClass="validatorCalloutHighlight"/>                          
                    <asp:FilteredTextBoxExtender ID="Edit_DriversCardTextBox_FilteredExtender" FilterType="Numbers"
                        TargetControlID="Edit_DriversCardTextBox" runat="server"/>                         
                </asp:TableCell>
            </asp:TableRow>            
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label ID="Edit_DriversPhoneNumberLabel" runat="server" Text="Номер телефона"/>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:TextBox ID="Edit_DriversPhoneNumberTextBox" runat="server" Width="100%"/>                  
                   
                    <asp:RequiredFieldValidator ID="DriversPhoneNumberTextBoxReqFVal" runat="server" 
                        ErrorMessage="Введите номер контактного телефона водителя!" ControlToValidate="Edit_DriversPhoneNumberTextBox" Display="None"/>
                    <asp:ValidatorCalloutExtender ID="DriversPhoneNumberTextBoxReqFValE" runat="server" TargetControlID="DriversPhoneNumberTextBoxReqFVal"
                        HighlightCssClass="validatorCalloutHighlight"/>    
                                        
                    <asp:RegularExpressionValidator runat="server" ID="DriversPhoneNumberTextBoxRegExVal"
                        ControlToValidate="Edit_DriversPhoneNumberTextBox" Display="None"
                        ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"
                        ErrorMessage="Пожалуйста введите номер в формате:<br />(###) ###-####" />
                    <asp:ValidatorCalloutExtender runat="Server" ID="DriversPhoneNumberTextBoxRegExValE"
                        TargetControlID="DriversPhoneNumberTextBoxRegExVal" HighlightCssClass="validatorCalloutHighlight" />
                   
                   
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label ID="Edit_DriversBirthDayLabel" runat="server" Text="Дата рождения"/>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:TextBox ID="Edit_DriversBirthDayTextBox" runat="server" Width="100%"/>                    
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label ID="Edit_DriversNOTELabel" runat="server" Text="Коментарий"/>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:TextBox ID="Edit_DriversNOTETextBox" runat="server" Width="100%"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label ID="Edit_DriversGroupSelectLabel" runat="server" Text="Группа"/>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:RadioButtonList runat="server" ID="radioButtonList1" Font-Size="X-Small" BackColor="LightBlue"
                        RepeatDirection="Horizontal" Width="100%" CellPadding ="1" RepeatColumns="4"            
                    /> 
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Bottom" Width="30%">
                <asp:Panel runat="server" ID="GeneralLogoPanel" GroupingText="Фото водителя" >
                    <asp:Image ID="DriversPhotoImage" ImageUrl="~/images/unknown_person.jpg" runat="server" Width="100%"/>
                    <br />
                    <asp:UpdatePanel ID="FileUploadUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:Table runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell Width="70%">
                            <asp:FileUpload ID="MyFileUpload"   runat="server" />
                                    </asp:TableCell>
                                    <asp:TableCell Width="25%">
                            <asp:Button ID="ImageUploadButton" runat="server"  Text="Обновить" CausesValidation="false" OnClick="Upload_Click"/> 
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
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
    <asp:ValidationSummary ID="ValidationSummaryControl" runat="server"
            HeaderText="Ошибки заполнения полей(можно убрать этот список):"  DisplayMode="BulletList"/> 
    </asp:Panel>
</asp:Panel>