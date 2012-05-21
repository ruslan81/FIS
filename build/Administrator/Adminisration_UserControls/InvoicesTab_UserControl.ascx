<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Adminisration_UserControls_InvoicesTab_UserControl, App_Web_0ptxwcqi" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>
    
    
<asp:Panel ID="InvoicesPanel" runat="server" Width="100%">
    <asp:UpdatePanel ID="InvoicesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>  
            <div id="tabs">                
                <ul style="height:30px;">
		            <li><asp:LinkButton ID="InvoiceTab" runat="server" Text="Счета" href="#tabs-1"/></li>
		            <li><asp:LinkButton ID="DetailedInfoTab" runat="server" Text="Детальная информация" href="#tabs-2"/></li>
                </ul>
                <div id="tabs-1">
                      <asp:Table ID="InvoicecsTab_GeneralTable" runat="server" Width="100%" GridLines="None">
                            <asp:TableRow>
                                <asp:TableCell Height="30px"> 
                                    <asp:UpdatePanel ID="InvoicesTab_ButtonsUpdateTable" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Table ID="InvoicesTab_ButtonsTable" runat="server" Width="100%" Height="10%" GridLines="None">
                                                <asp:TableRow>
                                                    <asp:TableCell HorizontalAlign="Left" Width="150px" VerticalAlign="Middle">
                                                        <uc2:BlueButton ID="InvoicesTab_PayInvoiceButton" runat="server" Text="Оплатить" Enabled="false"
                                                            OnClientClick="if(confirm('Оплатить этот счет?')) { return true; } else { return false; }"/>
                                                    </asp:TableCell>
                                                    <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center">
                                                        <asp:Button ID="Button1" runat="server" Text="Отменить оплату счета(тест)"
                                                            BackColor="White" BorderColor="Black" BorderWidth="2px"
                                                            OnClick="InvoicesTab_UNPayInvoiceButton_Click" Enabled="true"/>
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                    </asp:TableCell>
                                                    <asp:TableCell HorizontalAlign="Left" Width="150px" VerticalAlign="Middle">
                                                        <uc2:BlueButton ID="InvoicesTab_PrintInvoiceButton" runat="server" Text="Распечатать счет" Enabled="true" />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>         
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                     
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell VerticalAlign="Top">
                                    <asp:UpdatePanel ID="InvoicesDataGridUpdatePanel" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="ui-jqgrid">
                                                <asp:DataGrid ID="InvoicesDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
                                                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                                                    CellSpacing="0" CellPadding="3" rules="all" BorderColor="#CCC" border="0"
                                                    ItemStyle-Font-Size="8" AllowSorting="true" OnSortCommand="InvoicesDataGrid_Sort">
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="15px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="InvoicesDataGrid_RadioButton" runat="server" AutoPostBack="true" onclick="javascript:CheckOtherIsChecked(this);"
                                                                    OnCheckedChanged="InvoicesDataGrid_RadioButton_Checked" CausesValidation="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="NAME" HeaderText="Наименование" ReadOnly="true" />
                                                        <asp:BoundColumn DataField="INVOICEDATE" HeaderText="Дата выставления" ReadOnly="true" />
                                                        <asp:BoundColumn DataField="PAYTERMDATE" HeaderText="Срок оплаты" ReadOnly="true" />
                                                        <asp:BoundColumn DataField="STATUS" HeaderText="Статус" ReadOnly="true" />
                                                        <asp:BoundColumn DataField="PAYDATE" HeaderText="Дата оплаты" ReadOnly="true" />
                                                    </Columns>
                                                </asp:DataGrid>
                                            </div>    
                                            <asp:HiddenField ID="Selected_InvoicesDataGrid_Index" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                            
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                </div>
                <div id="tabs-2">
                </div>                
            </div>    
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
                
