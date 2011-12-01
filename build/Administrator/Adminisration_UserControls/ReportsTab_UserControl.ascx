<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Adminisration_UserControls_ReportsTab_UserControl, App_Web_44gcsth0" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>

<asp:Panel ID="ReportsPanel" runat="server" Width="100%">
    <asp:UpdatePanel ID="ReportsUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate> 
            <div id="tabs">                
                <ul style="height:30px;">
		            <li><asp:LinkButton ID="ReportsTab" runat="server" Text="Отчеты" href="#tabs-1"/></li>
		            <li><asp:LinkButton ID="PreviewTab" runat="server" Text="Просмотр" href="#tabs-2"/></li>
		            <li><asp:LinkButton ID="AccessPanel1" runat="server" Text="Доступ" href="#tabs-3"/></li>
                </ul>
                <div id="tabs-1">
                    <asp:Table ID="ReportsTab_GeneralTable" runat="server" Width="100%" GridLines="None">
                        <asp:TableRow>
                            <asp:TableCell Height="30px"> 
                                <asp:UpdatePanel ID="ReportsTab_ButtonsUpdateTable" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Table ID="ReportsTab_ButtonsTable" runat="server" Width="100%" Height="10%" GridLines="None">
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Middle" Width="150px">
                                                    <uc2:BlueButton ID="ReportsTab_AddReportButton" runat="server" Text="Добавить отчет" Enabled="false"
                                                        OnClientClick="if(confirm('При добавлении вам будет выставлен счет на оплату. Вы точно хотите добавить этот отчет?')) { return true; } else { return false; }"/>
                                                </asp:TableCell>
                                                <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle">
                                                    <asp:Button ID="ReportsTab_DelReportButton_only4Test" runat="server" Text="Удалить отчет(только тест)"
                                                        OnClick="ReportsTab_DelReportButton_only4Test_Click" Enabled="true"
                                                        BackColor="White" BorderColor="Black" BorderWidth="2px"
                                                        />
                                                </asp:TableCell>
                                                <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Width="150px">
                                                    <uc2:BlueButton ID="ReportsTab_PrintReportButton" runat="server" Text="Распечатать отчет" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>         
                                    </ContentTemplate>
                                </asp:UpdatePanel>                     
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell VerticalAlign="Top">
                                <asp:UpdatePanel ID="ReportsDataGridUpdatePanel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="ui-jqgrid">
                                            <asp:DataGrid ID="ReportsDataGrid" runat="server" GridLines="None" Width="100%" 
                                                CssClass="" HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                                                AutoGenerateColumns="false" CellSpacing="0" CellPadding="3" BorderColor="#CCC"
                                                ItemStyle-Font-Size="8">
                                                <Columns>
                                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="15px">
                                                        <ItemTemplate>
                                                            <asp:RadioButton ID="ReportsDataGrid_RadioButton" runat="server" AutoPostBack="true" onclick="javascript:CheckOtherIsChecked(this);"
                                                                OnCheckedChanged="ReportsDataGrid_RadioButton_Checked" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="CODE" HeaderText="Код отчета" ReadOnly="true" />
                                                    <asp:BoundColumn DataField="TYPE" HeaderText="Тип отчета" ReadOnly="true" />
                                                    <asp:BoundColumn DataField="NAME" HeaderText="Название" ReadOnly="true" />
                                                    <asp:BoundColumn DataField="PRICE" HeaderText="Стоимость добавления" ReadOnly="true" />
                                                    <asp:BoundColumn DataField="UPDATE_DATE" HeaderText="Дата изменения" ReadOnly="true" />
                                                    <asp:BoundColumn DataField="BEGIN_DATE" HeaderText="Дата добавления" ReadOnly="true" />
                                                    <asp:BoundColumn DataField="END_DATE" HeaderText="Срок действия" ReadOnly="true" />
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                        <asp:HiddenField ID="Selected_ReportsDataGrid_Index" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>                            
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:UpdatePanel ID="ReportNoteUpdatePanel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel1" runat="server" Height="100%" HorizontalAlign="Center" GroupingText="Описание отчета" Width="100%">
                                            <asp:Label ID="ReportAboutLabel" runat="server" Text="Окно описания отчета. Выберите отчет, чтобы просмотреть его описание."/>
                                        </asp:Panel> 
                                    </ContentTemplate>
                                </asp:UpdatePanel>                                                                
                            </asp:TableCell>
                        </asp:TableRow>
                        </asp:Table>
                </div>
                <div id="tabs-2">
                    <asp:Image ID="ReportExampleImage" Height="500px" ImageUrl="~/images/ReportExampleScreenShots/ScreenShotExample.jpg" runat="server" />
                </div>
                <div id="tabs-3">
                        <asp:Table ID="AccessTab_GeneralTable" runat="server" Width="100%" Height="585px" GridLines="None">
                            <asp:TableRow>
                                <asp:TableCell Height="30px"> 
                                    <asp:UpdatePanel ID="AccessTab_ButtonsUpdateTable" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Table ID="AccessTab_ButtonsTable" runat="server" Width="100%" Height="10%" GridLines="None">
                                                <asp:TableRow>
                                                    <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle" Width="160px">
                                                        <asp:Button ID="AccessTab_SaveButton" Width="95%" runat="server" Enabled="false"
                                                            OnClick="AccessTab_SaveButton_Click" Text="Сохранить изменения"/>
                                                    </asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle" Width="160px">
                                                        <asp:Button ID="AccessTab_AvailableForAllButton" Width="95%" runat="server" Enabled="false"
                                                            OnClick="AccessTab_AvailableForAllButton_Click" Text="Доступен для всех" />
                                                    </asp:TableCell>                                                
                                                    <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle" Width="160px">
                                                        <asp:Button ID="AccessTab_UnAvailableForAllButton" Width="95%" runat="server" Enabled="false"
                                                            OnClick="AccessTab_UnAvailableForAllButton_Click" Text="Недоступен для всех" />
                                                    </asp:TableCell>
                                                    <asp:TableCell ></asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>         
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                     
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                            <asp:TableCell Height="475px" VerticalAlign="Top">
                                <asp:UpdatePanel ID="AccessTab_DataGridUpdateTable" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DataGrid ID="AccessDataGrid" runat="server" GridLines="Both" Width="100%" AlternatingItemStyle-BackColor="AliceBlue"
                                            HeaderStyle-BackColor="ControlDark" AutoGenerateColumns="false" CellPadding="3"
                                            ItemStyle-Font-Size="8">
                                            <Columns>            
                                                <asp:BoundColumn DataField="USER_TYPE_ID" ItemStyle-Width="35%" Visible="false" />
                                                <asp:BoundColumn DataField="USERTYPE" HeaderText="Тип пользователя" ReadOnly="true" />
                                                <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                     HeaderText="Доступность отчета">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="AccessDataGrid_CheckBox" runat="server" AutoPostBack="false"/>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="ENABLEDFROM" ItemStyle-Width="45%" HeaderText="Доступен с" ReadOnly="true" />
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:HiddenField ID="Selected_AccessDataGrid_Index" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>                            
                            </asp:TableCell>
                        </asp:TableRow>
                        </asp:Table>
                </div>
            </div>    
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
