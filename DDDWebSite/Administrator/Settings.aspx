﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true"
    CodeFile="Settings.aspx.cs" Inherits="Administrator_Settings" %>

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
    
    <link type="text/css" href="../css/custom-theme/jquery.wijmo.wijcombobox.css" rel="stylesheet" />

    <script src="../js/custom/Settings.js" type="text/javascript"></script>
    <script src="../js/jquery.wijmo.wijcombobox.js" type="text/javascript"></script>   

    <asp:HiddenField ID="AccordionSelectedPane" Visible="true" runat="server" Value="0" />

    <script type="text/javascript" language="javascript">

        var mode = "";
        
        //run on page load
        $(function () {
            buildTree();

            $("#accordion").accordion({
                change: function (event, ui) {
                    if ($("a", ui.newHeader).text() == "Организация") {
                        loadGeneralSettings();
                    };
                    if ($("a", ui.newHeader).text() == "Напоминания") {
                    }
                    if ($("a", ui.newHeader).text() == "Дополнительно") {
                    }
                }
            });
        }); 

        $(window).resize(function () {
            resizeAllMaster();
            $("#accordion").accordion("resize");
        });
    </script>  
    
    <script id="tmplGeneralSettings" type="text/x-jquery-tmpl">
        <tr>
            <td class="key" key="${Key}" style="font-size:12px;">
                ${Value.Key}
            </td>
            <td class="value" style="padding-left:50px;">
                <input value="${Value.Value}" class="inputField-readonly" readonly="readonly"/>
            </td>
        </tr>
    </script> 

    <script id="userControlsGeneral" type="text/x-jquery-tmpl">
        <button id="edit">Редактировать</button>
        <div style="float:right">
            <button id="save">Сохранить</button>
            <button id="cancel">Отмена</button>
        </div>
    </script>
    
    <script id="userControlsGroups" type="text/x-jquery-tmpl">
        <button id="edit">Редактировать</button>
        <button id="delete">Удалить</button>
        <button id="create">Создать</button>
        <div id="deletedialog" title="Удаление групп" style="display: none;">
	        <p>Вы действительно хотите удалить выделенные элементы?</p>
        </div>
        <div style="float:right">
            <button id="save">Сохранить</button>
            <button id="cancel">Отмена</button>
        </div>
    </script>

     <script id="userControlsDefault" type="text/x-jquery-tmpl">
        <button id="edit">Редактировать</button>
        <div style="float:right">
            <button id="save">Сохранить</button>
            <button id="cancel">Отмена</button>
        </div>
    </script>

    <script id="tmplContentTable" type="text/x-jquery-tmpl">
        <div id="contentTableWrapper">
            <table id="contentTable" style="border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                border="0" cellpadding="0" cellspacing="0">
                <thead id="contentTableHeader">
                </thead>
                <tbody id="contentTableBody" class="ui-widget-content wijmo-wijgrid-data">
                </tbody>
            </table>
         </div>
    </script>

    <script id="tmplHeadColumn" type="text/x-jquery-tmpl">
        <th class="ui-widget wijmo-c1basefield ui-state-default wijmo-c1field" style="{{html style}}height:30px;">
            <div class="wijmo-wijgrid-innercell">
                <span class="wijmo-wijgrid-headertext">{{html text}}</span>
            </div>
        </th>
    </script>

    <script id="tmplGroupTableContent" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell" style="margin-left:5px;">
                    <input type="checkbox" id="checkbox{{html grID}}" key="{{html grID}}"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                    {{html Number}}
                    </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input id="nameinput{{html grID}}" value="{{html Name}}" class="inputField-readonly" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input id="commentinput{{html grID}}" value="{{html Comment}}" class="inputField-readonly" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <select id="groupSelector{{html grID}}" name="groupSelector" card="{{html cardType}}" onchange="this.card=this.value;">
                        <option value="1">Водитель</option>
                        <option value="2">ТС</option>
                    </select>
                </div>
            </td>
        </tr>
    </script>

    <script id="NewGroup" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell" style="margin-left:5px;">
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <input id="newNameinputGroup" value="" class="inputField"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <input id="newCommentinputGroup" value="" class="inputField"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <select id="newGroupSelector" name="groupSelector" card="1" onchange="this.card=this.value;">
                        <option value="1">Водитель</option>
                        <option value="2">ТС</option>
                    </select>
                </div>
            </td>
        </tr>
    </script>

    <script id="tmplOption" type="text/x-jquery-tmpl">
        <option value="{{html Key}}">{{html Value}}</option>
    </script>

    <script id="tmplCardTableContent" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell" style="margin-left:5px;">
                    <input type="checkbox" id="checkbox{{html grID}}" key="{{html grID}}"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                   <input id="numberinput{{html grID}}" value="{{html Number}}" class="inputField-readonly" readonly="readonly"/>
                    </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input id="nameinput{{html grID}}" value="{{html Name}}" class="inputField-readonly" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input id="commentinput{{html grID}}" value="{{html Comment}}" class="inputField-readonly" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <select id="groupSelector{{html grID}}" name="groupSelector" group="{{html groupID}}" onchange="this.group=this.value;">
                    </select>
                </div>
            </td>
        </tr>
    </script>

    <script id="newCard" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell" style="margin-left:5px;">
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                   <input id="newCardNumber" value="" class="inputField"/>
                    </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <input id="newCardName" value="" class="inputField"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <input id="newCardComment" value="" class="inputField"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <select id="newCardGroupSelector" name="groupSelector" group="1" onchange="this.group=this.value;">
                    </select>
                </div>
            </td>
        </tr>
    </script>
    
    <script id="tmplDefaultSettingsTable" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell" style="margin-left:5px;">
                    <input type="checkbox" key="{{html keyID}}"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html CriteriaName}}" id="CriteriaName{{html keyID}}" class="inputField-readonly" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html MeasureName}}" id="MeasureName{{html keyID}}" class="inputField-readonly" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html MinValue}}" id="MinValue{{html keyID}}" class="inputField-readonly" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html MaxValue}}" id="MaxValue{{html keyID}}" class="inputField-readonly" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html CriteriaNote}}" id="CriteriaNote{{html keyID}}" class="inputField-readonly" readonly="readonly"/>
                </div>
            </td>
        </tr>
    </script>

    <div id="accordion" style="width: 5">
        <h3><asp:LinkButton CausesValidation="false" runat="server" PostBackUrl="#" Text="Организация"/></h3>
        <div>
            <!--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Panel ID="Panel1" Style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;"
                        runat="server" BorderWidth="1px" BorderColor="LightGray">
                        <asp:TreeView ID="UsersTreeView" ShowCheckBoxes="None" runat="server" Width="100%"
                            SelectedNodeStyle-ForeColor="Firebrick" ForeColor="Black" Font-Bold="true" ImageSet="News"
                            OnSelectedNodeChanged="UsersTreeView_NodeChanged" />
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="UsersTreeView" EventName="SelectedNodeChanged" />
                </Triggers>
            </asp:UpdatePanel>-->
            <div>
                <ul id="tree">
                    <li class="folder" id="general"><a><span key="General">Общие</span></a>
                    </li>
                    <li class="folder"><a><span key="Groups">Группы</span></a>
                    </li>
                    <li class="folder"><a><span key="Drivers">Водители</span></a>
                    </li>
                    <li class="folder"><a><span key="Transport">ТС</span></a>
                    </li>
                    <li class="folder"><a><span key="Default">Установки по умолчанию</span></a>
                    </li>
                </ul>
            </div>
        </div>
        <div id="reminders">
        <h3>
            <asp:LinkButton ID="AccordionHeader2_Reminders" CausesValidation="false" runat="server"
                OnClientClick="acordionIndexSwitch(1);" PostBackUrl="#" Text="Напоминания"/></h3>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Panel ID="Panel2" Style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;"
                        runat="server" BorderWidth="1px" BorderColor="LightGray">
                        <asp:TreeView ID="ReminderTreeView" ShowCheckBoxes="None" runat="server" Width="100%"
                            SelectedNodeStyle-ForeColor="Firebrick" ForeColor="Black" Font-Bold="true" ImageSet="News"
                            OnSelectedNodeChanged="ReminderTreeView_NodeChanged" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <h3>
            <asp:LinkButton ID="AccordionHeader3_Additional" CausesValidation="false" runat="server"
                OnClientClick="acordionIndexSwitch(2);" PostBackUrl="#" Text="Дополнительно" /></h3>
        <div>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Panel ID="Panel3" Style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;"
                        runat="server" BorderWidth="1px" BorderColor="LightGray">
                        <asp:TreeView ID="AdditionalTreeView" ShowCheckBoxes="None" runat="server" Width="100%"
                            SelectedNodeStyle-ForeColor="Firebrick" ForeColor="Black" Font-Bold="true" ImageSet="News"
                            OnSelectedNodeChanged="AdditionalTreeView_NodeChanged" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ChoisesContent" ContentPlaceHolderID="MainConditions_PlaceHolder" runat="server">
    <div id="headerSettings">
        Общие настройки
    </div>
    <!--<asp:UpdatePanel ID="ChoisesContentUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <h1>
            <asp:Label ID="SettingName" runat="server" /></h1>
        </ContentTemplate>
    </asp:UpdatePanel>-->
</asp:Content>
<asp:Content ID="DataContent" ContentPlaceHolderID="Reports_PlaceHolder" runat="server">
    <div>
    
        <table cellpadding="5" style="margin-left:30px; width: 90%">
            <tbody id="contentSettings">
            </tbody>
        </table>
    </div>
    <div id="contentSettingsPlace">
    </div>
    <asp:UpdatePanel ID="DataContentUpdatePanel" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <!--<asp:Panel id="DataContentPanel" runat="server" ScrollBars="Auto">
            <uc1:GeneralTab ID="GeneralTab1" runat="server" Visible="false" />
            <uc2:UserGroupsTab ID="UserGroupsTab1" runat="server" />
            <uc3:UserDriversTab ID="UserDriversTab1" runat="server" />
            <uc4:UserVehicleTab ID="UserVehicleTab1" runat="server" />
            <uc5:ReminderOverSpeedingTab ID="ReminderOverSpeedingTab1" runat="server" />
            <uc6:Coefficient ID="Coefficient1" runat="server" />
            <uc7:EmailSheduler ID="EmailSheduler1" runat="server" />
        </asp:Panel>-->
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
    <div id="userControls">
    </div>
<!--<asp:UpdatePanel ID="DecisionUpdatePanel" runat="server" UpdateMode="Always">
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
</asp:UpdatePanel>-->
</asp:Content>

<asp:Content ID="BottomContent1" ContentPlaceHolderID="Bottom_PlaceHolder" runat="server">
<asp:UpdatePanel ID="StatusUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <h3><asp:Label ID="Status" runat="server" Visible="false" /></h3>
    </ContentTemplate>
</asp:UpdatePanel>    
</asp:Content>