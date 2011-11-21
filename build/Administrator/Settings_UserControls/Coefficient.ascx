<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Settings_UserControls_Coefficient, App_Web_bnbe51gs" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Panel ID="Vehicles" runat="server" Width="100%" ScrollBars="None">
    <link rel="Stylesheet" type="text/css" href="~/css/custom-theme/ui.jqgrid.css" id="style" runat="server" visible="false" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <asp:Panel ID="KoeffGridPanel" runat="server" CssClass = "ui-jqgrid">
                <asp:DataGrid ID="KoeffGrid" runat="server"  Width="100%"
                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                    AutoGenerateColumns="false" ItemStyle-Font-Size="10" CellSpacing="0" CellPadding="3" rules="all" BorderColor="#CCC" border="0"
                    OnEditCommand="KoefDataGrid_Edit" OnCancelCommand="KoefDataGrid_Cancel" OnUpdateCommand="KoefDataGrid_Update">
                    <Columns>
                        <asp:BoundColumn DataField="KEY_ID" HeaderText="ID" ReadOnly="true"  />
                        <asp:BoundColumn DataField="KEY_NAME" HeaderText="Название критерия" ReadOnly="true"  />
                        <asp:BoundColumn DataField="MEASURE_NAME" HeaderText="Единица измерения" ReadOnly="true"  />
                        <asp:BoundColumn DataField="KEY_VALUE_MIN" HeaderText="Мин. значение"/>
                        <asp:BoundColumn DataField="KEY_VALUE_MAX" HeaderText="Макс. значение"/>
                        <asp:BoundColumn DataField="KEY_NOTE" HeaderText="Комментарий"/>
                        <asp:EditCommandColumn EditText="Править" CancelText="Отмена" UpdateText="Применить" />                  
                    </Columns>
                </asp:DataGrid>
            </asp:Panel>
        </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Panel>    