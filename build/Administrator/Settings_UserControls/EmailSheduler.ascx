<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Settings_UserControls_EmailSheduler, App_Web_roolmgow" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Panel ID="Shedules" runat="server" Width="100%" ScrollBars="None">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="ShedulesDataGridPanel" runat="server" CssClass="ui-jqgrid">
                <asp:DataGrid ID="ShedulesDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                    CellSpacing="0" CellPadding="3" rules="all" BorderColor="#CCC" border="0">
                    <Columns>
                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="15px">
                            <ItemTemplate>
                                <asp:RadioButton ID="SheduleDataGrid_RadioButton" onclick="javascript:CheckOtherIsChecked(this);"
                                    runat="server" AutoPostBack="true" OnCheckedChanged="SheduleDataGrid_RadioButton_Checked" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn HeaderText="Код" DataField="SheduleId" HeaderStyle-HorizontalAlign="Center" Visible="false"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundColumn HeaderText="Имя карты" DataField="CardsName" />
                        <asp:BoundColumn HeaderText="Название отчета" DataField="ReportName" />
                        <asp:BoundColumn HeaderText="Дата последней отправки" DataField="LastSendDate" />
                        <asp:BoundColumn HeaderText="Период" DataField="Period" />
                        <asp:BoundColumn HeaderText="Email получателя" DataField="EmailAddress" />
                    </Columns>
                </asp:DataGrid>
            </asp:Panel>
            <asp:HiddenField ID="Selected_SheduleDataGrid_Index" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="ShedulesEdit_UpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="ShedulesEdit" runat="server" Width="750px" Visible="false">
                <asp:Label runat="server" ID="SelectReportPeriodLabel" Text="Выберите, за какой период вы хотите получать отчет:" />
                <p>
                </p>
                <asp:Table ID="EditTable" runat="server" Width="100%" GridLines="None">
                    <asp:TableRow>
                        <asp:TableCell Width="50%">
                            <div style="float: left; width: 70%;">
                                <asp:DropDownList runat="server" ID="PeriodTypeDropDown" Width="100%" />
                            </div>
                            <div style="float: right;">
                                <asp:Label ID="PeriodAmmountLabel" runat="server" Text="Количество:" />
                            </div>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="PeriodTextBox" runat="server" Width="70%" Text="1"/>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <p>
                                <asp:Label runat="server" ID="ReportTypeLabel" Text="Тип отчета:" /></p>
                            <p>
                                <asp:DropDownList runat="server" ID="ReportTypeDropDown" Width="70%"
                                    OnSelectedIndexChanged="ReportTypeDropDown_SelectedIndexChanged" AutoPostBack="true" />
                            </p>
                        </asp:TableCell><asp:TableCell>
                            <p>
                                <asp:Label ID="CardNameLabel" runat="server" Text="Имя карты:" /></p>
                            <p>
                                <asp:DropDownList runat="server" ID="CardNameDropDown" Width="70%" />
                            </p>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="ReportNameLabel" runat="server" Text="Название отчета:" />
                            <p>
                                <asp:DropDownList ID="ReportNameDropDown" runat="server" Width="70%" />
                            </p>
                        </asp:TableCell><asp:TableCell>
                            <asp:Label ID="DeviceNumber_PLFONLY_Label" runat="server" Text="Номер устройства(для PLF):" />
                            <p>
                                <asp:DropDownList ID="DeviceNumber_PLFONLY_DropDown" runat="server" Width="70%" Enabled="false"/>
                            </p>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="EmailAddressLabel" runat="server" Text="Адрес электронной почты:" />
                            <p>
                                <asp:TextBox ID="EmailAddressTextBox" runat="server" Width="70%" ForeColor="Blue"/>
                            </p>
                        </asp:TableCell>
                        <asp:TableCell></asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
