<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserGroupsTab.ascx.cs" Inherits="Administrator_Settings_UserControls_UserGroupsTab" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Panel ID="User_Groups" runat="server" Width="100%" ScrollBars="None">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate> 
            <asp:Panel ID="GroupsDataGridPanel" CssClass="ui-jqgrid" runat="server">
                <asp:DataGrid ID="GroupsDataGrid"  runat="server"  Width="100%" AutoGenerateColumns="false"                    
                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                    CellSpacing="0" CellPadding="3" BorderColor="#CCC" border="0">
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>                                                                                    
                                <asp:RadioButton ID="GroupsDataGrid_RadioButton" Width="8" GroupName="GVRow"  onclick="javascript:CheckOtherIsChecked(this);"
                                    runat="server" AutoPostBack="true" CausesValidation="false" OnCheckedChanged="GroupsDataGrid_RadioButton_Checked"
                                /> 
                            </ItemTemplate>                                                
                         </asp:TemplateColumn>
                         <asp:BoundColumn DataField="№" HeaderText="№" ReadOnly="true"  />
                         <asp:BoundColumn DataField="Название" HeaderText="Название" ReadOnly="true" />
                         <asp:BoundColumn DataField="Комментарий" HeaderText="Комментарий" ReadOnly="true" />
                   </Columns>    
                </asp:DataGrid>
            </asp:Panel> 
            <asp:HiddenField ID="Selected_GroupsDataGrid_Index" runat="server" />
        </ContentTemplate>        
    </asp:UpdatePanel>
        
    <asp:Panel ID="EditPanel" runat="server" Width="100%" Visible="false">
        <asp:Table ID="EditTable" runat="server" Width="100%" CellPadding="5">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label ID="Edit_GroupNameLabel" runat="server" Text="Название"/>
                </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                    <asp:TextBox ID="Edit_GroupNameTextBox" runat="server" Width="50%"/>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell  HorizontalAlign="Right" VerticalAlign="Top">
                    <asp:Label ID="Edit_GroupCommentLabel" runat="server" Text="Комментарий"/>
                </asp:TableCell><asp:TableCell  HorizontalAlign="Left">
                    <asp:TextBox ID="Edit_GroupCommentTextBox" runat="server" TextMode="MultiLine" Rows="7" Width="50%"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
</asp:Panel>