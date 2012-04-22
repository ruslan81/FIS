<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="Administration.aspx.cs" Inherits="Administrator_Administration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Adminisration_UserControls/GeneralData_UserControl.ascx" TagName="GeneralData_UserControl"
    TagPrefix="uc1" %>
<%@ Register src="Adminisration_UserControls/UsersTab_UserControl.ascx" tagname="UsersTab_UserControl" tagprefix="uc2" %>
<%@ Register src="Adminisration_UserControls/DealersTab_UserControl.ascx" tagname="DealersTab_UserControl" tagprefix="uc3" %>
<%@ Register src="Adminisration_UserControls/LogTab_UserControl.ascx" tagname="LogTab_UserControl" tagprefix="uc4" %>
<%@ Register src="Adminisration_UserControls/InvoicesTab_UserControl.ascx" tagname="InvoicesTab_UserControl" tagprefix="uc5" %>
<%@ Register src="Adminisration_UserControls/ReportsTab_UserControl.ascx" tagname="ReportsTab_UserControl" tagprefix="uc6" %>
<%@ Register src="Adminisration_UserControls/ClientsTab_UserControl.ascx" tagname="ClientsTab_UserControl" tagprefix="uc7" %>
<%@ Register src="Adminisration_UserControls/AccountsTab_UserControl.ascx" tagname="AccountsTab_UserControl" tagprefix="uc8" %>
<%@ Register src="../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>

<asp:Content ID="AccordionContent" ContentPlaceHolderID="VerticalOutlookMenu_PlaceHolder" runat="server">

  <link type="text/css" href="../css/custom-theme/jquery.wijmo.wijcombobox.css" rel="stylesheet" />
  <link type="text/css" href="../css/DetailedData.css" rel="stylesheet" />

  <script src="../js/custom/Adminsitration.js" type="text/javascript"></script>
  <script src="../js/jquery.ui.datepicker-ru.js" type="text/javascript"></script>
  <script src="../js/jquery.wijmo.wijcombobox.js" type="text/javascript"></script>
  <asp:HiddenField ID="AccordionSelectedPane" Visible="true" runat="server" Value="0" /> 
  
  <script type="text/javascript">    
      //run on page load
      $(function () {
          var mode = "";
          var tabIndex = 0;
          loadGeneralData();

          $("#accordion").accordion({
              change: function (event, ui) {

                  $("#ContentContainer").empty();
                  if ($("a", ui.newHeader).text() == "Общие сведения") {
                      loadGeneralData();
                  };
                  if ($("a", ui.newHeader).text() == "Счета") {
                      loadInvoiceData();
                  };
                  if ($("a", ui.newHeader).text() == "Журнал") {
                      loadJournalData();
                  };

              }
          });
      });

      function pageLoad() {
          $('#accordion').bind('accordionchange', function() {
              onACESelectedIndexChanged(null, null);
          });
      }

      function onACESelectedIndexChanged(sender, eventArgs) {
          var actionBtn = "<%= InvisibleAccordionButton.ClientID %>";
          document.getElementById(actionBtn).click();
      }

      function onNewAccordionSelectedIndexChanged(accIndex) {
          document.getElementById("<% =AccordionSelectedPane.ClientID %>").value = accIndex;
          //  var actionBtn = "<%= InvisibleAccordionButton.ClientID %>";
          //  document.getElementById(actionBtn).click();
      }

      function resizeReports() {
          var vertHeightSTR = document.getElementById('vertical-menu').style.height;
          vertHeightSTR = vertHeightSTR.substr(0, vertHeightSTR.length - 2);
          document.getElementById('outputId').style.height = (vertHeightSTR - 20) + "px";
          var oneAccPanelHeight = document.getElementById('firstAccordionPanel').style.height;
          oneAccPanelHeight = oneAccPanelHeight.substring(0, oneAccPanelHeight.length - 2);
          document.getElementById('AccountsOverFlowPanel').style.height = oneAccPanelHeight - 2 + "px";
      }

      $(document).ready(function() {
          resizeReports();
      });

      $(window).resize(function() {
          resizeAllMaster();
          resizeReports();
          $("#accordion").accordion("resize");
      });
    
    </script>

    <!-- TEMPLATES-->

    <script id="tmplGeneralData" type="text/x-jquery-tmpl">
        <label>Текущее подключение с </label><label>{{html connectDate}}</label><br>
        <label>Тип лицензии </label><label>{{html licenseType}}</label><br>
        <label>Дата регистрации в системе </label><label>{{html registerDate}}</label><br>
        <label>Срок окончания регистрации </label><label>{{html endDate}}</label><br>
    </script>

    <script id="tmplGeneralDetailedData" type="text/x-jquery-tmpl">
        <table style="width: 100%;">
        <label>Аккаунт</label></br><div style="width: 80%;"><input id="orgName" value="{{html orgName}}"/></div></br>
        <label>Пользователь</label></br><div style="width: 40%;"><input id="orgLogin" value="{{html orgLogin}}"/></div></br>

        <table style="width:81%;">
        <tr><td><label>Пароль </label></td><td><label>Пароль (Подтверждение) </label></td></tr>
        <tr><td><input id="pass1" value="{{html password}}"/></td><td><input id="pass2" value="{{html password}}"/></td></tr>
        </table>

        <hr>

        <table style="width:100%;">
        <tr><td><label>Страна </label></td><td><label>Город </label></td><td><label>Почтовый индекс </label></td></tr>
        <tr><td><select id="country" countryId="{{html country}}" onchange="this.countryId=this.value;"></select></td><td>
        <input id="city" value="{{html city}}"/></td><td><input id="index" value="{{html index}}"/></td></tr>
        </table><br>

        <label>Часовая зона</label></br>
        <div style="width:50%;"><select id="timeZoneSelector" timeZoneId="{{html timeZone}}" onchange="this.timeZoneId=this.value;"></select></div><br>


        <label>Адрес1</label><br><div style="width: 80%;"><input id="addr1" value="{{html address1}}"/></div><br>
        <label>Адрес2</label><br><div style="width: 80%;"><input id="addr2" value="{{html address2}}"/></div><br>

        <table style="width:100%;">
        <tr><td><label>Телефон </label></td><td><label>Факс </label></td><td><label>e-mail </label></td></tr>
        <tr><td><input id="phone" value="{{html phone}}"/></td><td><input id="fax" value="{{html fax}}"/></td><td><input id="mail" value="{{html mail}}"/></td></tr>
        </table>

        <hr>

        
    </script>

    <script id="GeneralData" type="text/x-jquery-tmpl">
     <div id="tabs">
            <ul>
                <li><a href="#tabs-1">Общие сведения</a></li>
		        <li><a href="#tabs-2">Детальные сведения</a></li>
	        </ul>
            <div id="tabs-1">
                <div id="commonData" style="overflow: auto;">
                 <table style="width:100%;">
            <tr id="firstGeneralRow"> 
            </tr>
            <tr><td>
                <table id="statisticTable"  style="width:100%;border: 1px solid #0000FF;border-radius: 3px;border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                    border="0" cellpadding="0" cellspacing="0">
                    <thead id="statisticTableHeader"></thead>
                    <tbody id="statisticTableBody" class="ui-widget-content wijmo-wijgrid-data">
                    </tbody>
                </table>
            </td>
            <td>
                <table id="messageTable"  style="width:100%;border: 1px solid #0000FF;border-radius: 3px;border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                    border="0" cellpadding="0" cellspacing="0">
                    <thead id="messageTableHeader"></thead>
                    <tbody id="messageTableBody" class="ui-widget-content wijmo-wijgrid-data">
                    </tbody>
                </table>
            </td></tr>
            </table>
                </div>
            </div>
            <div id="tabs-2">
                <div id="detailedData" style="overflow: hidden;">
                </div>
            </div>
    </div>           
    </script>

    <script id="InvoiceData" type="text/x-jquery-tmpl">

            <div id="filter" style="border: 1px solid #0000FF;border-radius: 3px;">
            <table>
            <tr><td><label><h3>Фильтр</h3></label></td><td></td><td></td></tr>
            <tr><td><label>Начальная дата </label><input id="startDatePicker" type="text"/>
            <td><label>Конечная дата </label><input id="endDatePicker" type="text"/><td></td></tr><br>

            <div id="dateErrorBlock" class="error-block">
            <label class="error" id="dateErrorLabel"> Ошибка: Укажите начальную и конечную дату!</label>
            </div>

            <tr><td style="height:"><label>Тип </label><select id="invoiceStatusSelector" statusType="0" onchange="this.statusType=this.value;"></select></td>
            <td><button id="buildButton">Применить</button></td></tr>
            </table>
            </div>

            <table id="invoiceTable"  style="border-collapse: separate;width:100%;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                border="0" cellpadding="0" cellspacing="0">
                <thead id="invoiceTableHeader"></thead>
                <tbody id="invoiceTableBody" class="ui-widget-content wijmo-wijgrid-data">
                </tbody>
            </table>
    </script>
    
    <script id="JournalData" type="text/x-jquery-tmpl">

            <div id="filter" style="border: 1px solid #0000FF;border-radius: 3px;">
            <table>
            <tr><td><label><h3>Фильтр</h3></label></td><td></td><td></td></tr>
            <tr><td><label>Начальная дата </label><input id="startDatePicker" type="text"/>
            <td><label>Конечная дата </label><input id="endDatePicker" type="text"/><td></td></tr><br>

            <div id="dateErrorBlock" class="error-block">
            <label class="error" id="dateErrorLabel"> Ошибка: Укажите начальную и конечную дату!</label>
            </div>

            <tr><td style="height:"><label>Событие </label><select id="eventSelector" event="-1" onchange="this.event=this.value;"></select></td>
            <td><label>Текст в описании </label><input id="textInput" value=""/></td>
            <td><button id="buildButton">Применить</button></td></tr>
            </table>
            </div>
            <table id="journalTable"  style="border-collapse: separate;width:100%;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                border="0" cellpadding="0" cellspacing="0">
                <thead id="journalTableHeader"></thead>
                <tbody id="journalTableBody" class="ui-widget-content wijmo-wijgrid-data">
                </tbody>
            </table>
    </script>
    
    <script id="tmplJournalTableContent" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                   <input value="{{html dateTime}}" class="inputField-readonly input" readonly="readonly"/>
                    </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <input value="{{html user}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <input value="{{html note}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
        </tr>
    </script>

    <script id="tmplInvoiceTableContent" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <input value="{{html name}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <input value="{{html beginDate}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <input value="{{html endDate}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <input value="{{html status}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <input value="{{html payDate}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
        </tr>
    </script>

    <script id="tmplHeadColumn" type="text/x-jquery-tmpl">
        <th class="ui-widget wijmo-c1basefield ui-state-default wijmo-c1field" style="{{html style}}height:30px;">
            <div class="wijmo-wijgrid-innercell">
                <span class="wijmo-wijgrid-headertext">{{html text}}</span>
            </div>
        </th>
    </script>

    <script id="tmplOption" type="text/x-jquery-tmpl">
        <option value="{{html Key}}">{{html Value}}</option>
    </script>

        <asp:UpdatePanel ID="InvisibleUpdatePanel" runat="server" UpdateMode="Always">  
            <ContentTemplate>
            
            <div style="display:none;">
                <asp:Button ID="InvisibleAccordionButton" runat="server" CausesValidation="false"
                    OnClick="InvisibleAccordionButton_Click" Visible="true"/>
            </div>
            
            </ContentTemplate>
        </asp:UpdatePanel>
            
        <div id="accordion" style="width: 5">
            <h3><asp:LinkButton ID="GeneralDataAccordionPane1" runat="server" CausesValidation="false" PostBackUrl="#" OnClientClick="onNewAccordionSelectedIndexChanged(0);" Text="Общие сведения" /></h3>
                <div id="firstAccordionPanel">                   
                </div>
            <h3 id="AccountsAccordionPane2_Header" runat="server"><asp:LinkButton ID="AccountsAccordionPane2" runat="server" CausesValidation="false" PostBackUrl="#" OnClientClick="onNewAccordionSelectedIndexChanged(7);" Text="Аккаунты" /></h3>
                <div>
                    <div id="AccountsOverFlowPanel" style="overflow:auto; border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px; border: 1px solid #AFCBDE;">
                        <asp:UpdatePanel ID="AccountsTreeUpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TreeView ID="AccountsTreeView" runat="server" ForeColor="Black" HoverNodeStyle-ForeColor="Firebrick"
                                SelectedNodeStyle-ForeColor="Firebrick" RootNodeStyle-Font-Bold="true" SelectedNodeStyle-Font-Underline="true"
                                NodeStyle-HorizontalPadding="5" NodeIndent="20" ShowLines="true" OnSelectedNodeChanged="AccountsTreeView_NodeChanged" />    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>     
            <h3><asp:LinkButton ID="UsersAccordionPane3" runat="server" CausesValidation="false" PostBackUrl="#" OnClientClick="onNewAccordionSelectedIndexChanged(2);" Text="Пользователи" /></h3>
                <div>
                </div>
            <h3><asp:LinkButton ID="ReportsAccordionPane4" runat="server" CausesValidation="false" PostBackUrl="#" OnClientClick="onNewAccordionSelectedIndexChanged(3);" Text="Отчеты" /></h3>
                <div>
                </div>
            <h3><asp:LinkButton ID="BillsAccordionPane5" runat="server" CausesValidation="false" PostBackUrl="#" OnClientClick="onNewAccordionSelectedIndexChanged(4);" Text="Счета" /></h3>
                <div>
                </div>
            <h3><asp:LinkButton ID="LogAccordionPane6" runat="server" CausesValidation="false" PostBackUrl="#" OnClientClick="onNewAccordionSelectedIndexChanged(5);" Text="Журнал" /></h3>
                <div>
                </div>
        </div>
            
</asp:Content>

<asp:Content ID="DataContent" ContentPlaceHolderID="Reports_PlaceHolder" runat="server">
    <div id="ContentContainer">
    </div>
    <!--<asp:UpdatePanel id="DataContentUpdatePanel" runat="server" UpdateMode="Always"  OnDataBinding="GeneralDataLoad">
        <ContentTemplate>
            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(asss);
                function asss() {
                    $('#tabs').tabs();
                }
            </script>       
        
            <uc1:GeneralData_UserControl ID="GeneralData_UserControl1" runat="server"/>

            <uc2:UsersTab_UserControl ID="UsersTab_UserControl1" runat="server"  Visible="false" />
            <uc3:DealersTab_UserControl ID="DealersTab_X_UserControl1" runat="server"  Visible="false"/>
            <uc4:LogTab_UserControl ID="LogTab_UserControl1" runat="server" />
            <uc5:InvoicesTab_UserControl ID="InvoicesTab_UserControl1" runat="server" />
            <uc6:ReportsTab_UserControl ID="ReportsTab_UserControl1" runat="server" />
            <uc7:ClientsTab_UserControl ID="ClientsTab_X_UserControl2" runat="server" Visible="false" />
            <uc8:AccountsTab_UserControl ID="AccountsTab_UserControl1" runat="server" />
            
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
    
    <asp:UpdatePanel ID="StatusUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label runat="server" ID="Status" Font-Size="Large" ForeColor="DarkBlue" />
        </ContentTemplate>
    </asp:UpdatePanel>
        -->
</asp:Content>
<asp:Content ID="DecisionContent1" ContentPlaceHolderID="Decision_PlaceHolder" runat="server">

    <script id="сontrolsGeneralDetailed" type="text/x-jquery-tmpl">
        <button id="edit">Редактировать</button>
        <div style="float:right">
            <button id="save">Сохранить</button>
            <button id="cancel">Отмена</button>
        </div>
    </script>

    <div id="userControls">
    </div>
</asp:Content>