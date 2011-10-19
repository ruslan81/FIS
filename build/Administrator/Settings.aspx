<%@ page language="C#" masterpagefile="~/MasterPage/MasterPage.Master" autoeventwireup="true" inherits="Administrator_Settings, App_Web_adafxnuj" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Settings_UserControls/GeneralTab.ascx" TagName="GeneralTab" TagPrefix="uc1" %>
<%@ Register Src="Settings_UserControls/UserGroupsTab.ascx" TagName="UserGroupsTab" TagPrefix="uc2" %>
<%@ Register Src="Settings_UserControls/UserDriversTab.ascx" TagName="UserDriversTab" TagPrefix="uc3" %>
<%@ Register Src="Settings_UserControls/UserVehicleTab.ascx" TagName="UserVehicleTab" TagPrefix="uc4" %>
<%@ Register Src="Settings_UserControls/ReminderOverSpeedingTab.ascx" TagName="ReminderOverSpeedingTab" TagPrefix="uc5" %>
<%@ Register Src="Settings_UserControls/Coefficient.ascx" TagName="Coefficient" TagPrefix="uc6" %>
<%@ Register Src="Settings_UserControls/EmailSheduler.ascx" TagName="EmailSheduler" TagPrefix="uc7" %>
<%@ Register src="../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>

<asp:Content ID="AccordionContent" ContentPlaceHolderID="VerticalOutlookMenu_PlaceHolder"
    runat="server">
    
    <asp:HiddenField ID="AccordionSelectedPane" Visible="true" runat="server" Value="0" /> 
    
     <script type="text/javascript" language="javascript">
         function acordionIndexSwitch(accIndex) {
             document.getElementById("<% =AccordionSelectedPane.ClientID %>").value = accIndex;
         }

         $(window).resize(function() {
             resizeAllMaster();
             $("#accordion").accordion("resize");
         });
    </script>   
    
    <div id="accordion" style="width: 5">
        <h3><asp:LinkButton ID="AccordionHeader1_Users" CausesValidation="false" runat="server" OnClientClick="acordionIndexSwitch(0);" PostBackUrl="#" Text="Предприятие" /></h3>
            <div >
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;" 
                                runat="server" BorderWidth="1px" BorderColor="LightGray">
                                <asp:TreeView ID="UsersTreeView" ShowCheckBoxes="None" runat="server" Width="100%"
                                    SelectedNodeStyle-ForeColor="Firebrick" ForeColor="Black" Font-Bold="true"
                                    ImageSet="News" OnSelectedNodeChanged="UsersTreeView_NodeChanged" />
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="UsersTreeView" EventName="SelectedNodeChanged"/>
                        </Triggers>
                    </asp:UpdatePanel>
            </div>
        <h3><asp:LinkButton ID="AccordionHeader2_Reminders" CausesValidation="false" runat="server" OnClientClick="acordionIndexSwitch(1);" PostBackUrl="#" Text="Напоминания" /></h3>
            <div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="Panel2" style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;" 
                            runat="server" BorderWidth="1px" BorderColor="LightGray">
                            <asp:TreeView ID="ReminderTreeView" ShowCheckBoxes="None" runat="server" Width="100%"
                                SelectedNodeStyle-ForeColor="Firebrick" ForeColor="Black" Font-Bold="true"
                                ImageSet="News" OnSelectedNodeChanged="ReminderTreeView_NodeChanged" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>    
            <h3><asp:LinkButton ID="AccordionHeader3_Additional" CausesValidation="false" runat="server" OnClientClick="acordionIndexSwitch(2);" PostBackUrl="#" Text="Дополнительно" /></h3>
            <div>
              <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="Panel3" style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;" 
                            runat="server" BorderWidth="1px" BorderColor="LightGray">
                            <asp:TreeView ID="AdditionalTreeView" ShowCheckBoxes="None" runat="server" Width="100%"
                                SelectedNodeStyle-ForeColor="Firebrick" ForeColor="Black" Font-Bold="true"
                                ImageSet="News" OnSelectedNodeChanged="AdditionalTreeView_NodeChanged" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>    
    </div> 
    
</asp:Content>    
<asp:Content ID="ChoisesContent" ContentPlaceHolderID="MainConditions_PlaceHolder" runat="server">
    <asp:UpdatePanel ID="ChoisesContentUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <h1>
            <asp:Label ID="SettingName" runat="server" /></h1>
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>
<asp:Content ID="DataContent" ContentPlaceHolderID="Reports_PlaceHolder" runat="server">
<asp:UpdatePanel ID="DataContentUpdatePanel" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Panel id="DataContentPanel" runat="server" ScrollBars="Auto">
            <uc1:GeneralTab ID="GeneralTab1" runat="server" Visible="false" />
            <uc2:UserGroupsTab ID="UserGroupsTab1" runat="server" />
            <uc3:UserDriversTab ID="UserDriversTab1" runat="server" />
            <uc4:UserVehicleTab ID="UserVehicleTab1" runat="server" />
            <uc5:ReminderOverSpeedingTab ID="ReminderOverSpeedingTab1" runat="server" />
            <uc6:Coefficient ID="Coefficient1" runat="server" />
            <uc7:EmailSheduler ID="EmailSheduler1" runat="server" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

        <script language="javascript">
            function CheckOtherIsChecked(spanChk) {
                var IsChecked = spanChk.checked;
                var CurrentRdbID = spanChk.id;
                var Chk = spanChk;
                Parent = Chk.form.elements;
                for (i = 0; i < Parent.length; i++) {
                    if (Parent[i].id != CurrentRdbID && Parent[i].type == "radio") {
                        if (Parent[i].checked) {
                            Parent[i].checked = false;
                        }
                    }
                }
            }

    </script>
</asp:Content>
<asp:Content ID="DecisionContent1" ContentPlaceHolderID="Decision_PlaceHolder" runat="server">
<asp:UpdatePanel ID="DecisionUpdatePanel" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Table runat="server"  ID="resultActionButtonsTable" CellPadding="3" GridLines="None" Width="100%">
            <asp:TableRow>
                <asp:TableCell Width="15%">
                    <uc2:BlueButton ID="Choises_DELETE_Button" Text="Удалить" runat="server"
                        OnClientClick="if(confirm('Вы уверены, что хотите удалить эту рассылку?')) { return true; } else { return false; }"/>                    
                </asp:TableCell>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell Width="15%">
                    <uc2:BlueButton ID="Choises_CANCEL_Button" Text="Отменить" runat="server" CausesValidation="false" />
                </asp:TableCell>
                <asp:TableCell Width="15%">        
                    <uc2:BlueButton ID="Choises_ADD_Button" Text="Добавить" runat="server" CausesValidation="true"/>
                </asp:TableCell>
                <asp:TableCell Width="15%">        
                    <uc2:BlueButton ID="Choises_EDIT_Button" Text="Редактировать" runat="server" CausesValidation="true"/>
                </asp:TableCell>
                <asp:TableCell Width="15%">        
                    <uc2:BlueButton ID="Choises_SAVE_Button" Text="Сохранить" runat="server" CausesValidation="true"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

<asp:Content ID="BottomContent1" ContentPlaceHolderID="Bottom_PlaceHolder" runat="server">
<asp:UpdatePanel ID="StatusUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <h3><asp:Label ID="Status" runat="server" Visible="false" /></h3>
    </ContentTemplate>
</asp:UpdatePanel>    
</asp:Content>