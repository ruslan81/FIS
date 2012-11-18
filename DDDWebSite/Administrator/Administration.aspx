<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true"
    CodeFile="Administration.aspx.cs" Inherits="Administrator_Administration" %>

<%@ Register src="../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>

<asp:Content ID="AccordionContent" ContentPlaceHolderID="VerticalOutlookMenu_PlaceHolder" runat="server">

  <link type="text/css" href="../css/custom-theme/jquery.wijmo.wijcombobox.css" rel="stylesheet" />
  <link type="text/css" href="../css/DetailedData.css" rel="stylesheet" />
  <link type="text/css" href="../js/custom/jquery.search-tree.1.0.1.css" rel="stylesheet" />

  <script src="../js/custom/Adminsitration.js" type="text/javascript"></script>
  <script src="../js/jquery.ui.datepicker-ru.js" type="text/javascript"></script>
  <script src="../js/jquery.wijmo.wijcombobox.js" type="text/javascript"></script>
  <script src="../js/custom/jquery.search-tree.1.0.1.js" type="text/javascript"></script>

  <asp:HiddenField ID="AccordionSelectedPane" Visible="true" runat="server" Value="0" /> 
  
  <script type="text/javascript">
      //run on page load
      var mode = "";
      var tabIndex = 0;
      var radioIndex = -1;
      var citySelectors = null;
      var city = null;
      var dealerOrgID = $.cookie("CURRENT_ORG_ID");
      var dealerLevel = 0;
      var crtype = 0;

      $(function () {
          mode = "";
          tabIndex = 0;
          radioIndex = -1;

          buildOrgTree(0);
          //buildUserTree(0);
          loadGeneralData();

          $("#accordion").accordion({
              change: function (event, ui) {

                  $("#ContentContainer").empty();
                  if ($("a", ui.newHeader).text() == "�������") {
                      $("#userControls").empty();
                      mode = "";
                      tabIndex = 0;
                      radioIndex = -1;
                      buildOrgTree(0);
                      loadGeneralData();
                      createPeriodControls();
                  };
                  /*if ($("a", ui.newHeader).text() == "������") {
                  $("#userControls").empty();
                  mode = "";
                  tabIndex = 0;
                  radioIndex = -1;
                  loadDealersData();
                  resizeAdmin();
                  };*/
                  if ($("a", ui.newHeader).text() == "������������") {
                      $("#userControls").empty();
                      mode = "";
                      tabIndex = 0;
                      radioIndex = -1;
                      buildUserTree(0);
                      createPeriodControls();
                      //loadUsersData();
                  };
                  if ($("a", ui.newHeader).text() == "�����") {
                      $("#userControls").empty();
                      mode = "";
                      $("#dateErrorBlock").hide();
                      buildOrgTreeInvoices(0);
                      //loadInvoiceData();
                      resizeAdmin();
                  };
                  if ($("a", ui.newHeader).text() == "������") {
                      $("#userControls").empty();
                      mode = "";
                      $("#dateErrorBlock").hide();
                      buildOrgTreeJournal(0);
                      //loadJournalData();
                      resizeAdmin();
                  };
              }
          });

          resizeAdmin();
      });

      function resizeAdmin() {
          var vertHeightSTR = document.getElementById('vertical-menu').style.height;
          vertHeightSTR = vertHeightSTR.substr(0, vertHeightSTR.length - 2);
          document.getElementById('outputId').style.height = (vertHeightSTR - 30) + "px";
          document.getElementById('outputId-content').style.height = (vertHeightSTR - 30+5) + "px";
          if ($('#userControls:visible').length > 0) {
              var h = $('#outputId').height() - $('#main-conditions').height() - 34;
              $('#outputId').height(h);
              $('#outputId-content').height(h+5);
          }

          var outHeight = $("#main-content").height() - 90;
          if ($('.add-info-block:visible').length > 0) {
              outHeight = outHeight - 55;
          }

          try {
              if ($('#userControls:visible').length > 0) {
                  outHeight -= 34;
              }
              document.getElementById('commonData').style.height = outHeight + "px";
              document.getElementById('detailedData').style.height = outHeight + "px";
              /*document.getElementById('report').style.height = outHeight - 7 - 50 + "px";
              document.getElementById('chart').style.height = outHeight - 7 - 50 + "px";
              document.getElementById('chart').style.width = $('#outputId').width() - 35 + "px";
              document.getElementById('map').style.height = outHeight - 7 - 50 - 20 + "px";
              document.getElementById('map').style.width = $('#outputId').width() - 35 + "px";*/
          } catch (e) {

          }
      }

      $(window).resize(function() {
          resizeAllMaster();
          resizeAdmin();
          $("#accordion").accordion("resize");
      });
    
    </script>

    <!-- TEMPLATES-->
    <script id="tmplPeriodSelection" type="text/x-jquery-tmpl">
        <div id="periodSelection">
            <label>��������� ���� </label><input id="startDatePicker" type="text"/>
            <label>�������� ���� </label><input id="endDatePicker" type="text"/>
            <button id="buildButton">���������</button>
            <div id="dateErrorBlock" class="error-block">
                <label class="error" id="dateErrorLabel"> ������: ������� ��������� � �������� ����!</label>
            </div>
            <br/><br/>
        </div>
    </script>

    <script id="tmplGeneralData" type="text/x-jquery-tmpl">
    <div id="generalDataLabels">
        <div style="margin-top:2px;">
            <div style="float:left;margin-right:5px;">������� ����������� �:</div>
            <div style="font-weight:bold;"> {{html connectDate}}</div>
        </div>
        <div style="margin-top:2px;">
            <div style="float:left;margin-right:5px;">��� ��������:</div>
            <div style="font-weight:bold;"> {{html licenseType}}</div>
        </div>
        <!--<div style="margin-top:2px;">
            <div style="float:left;margin-right:5px;">���� ����������� � �������:</div>
            <div style="font-weight:bold;"> {{html registerDate}}</div>
        </div>
        <div style="margin-top:2px;">
            <div style="float:left;margin-right:5px;">���� ��������� �����������:</div>
            <div style="font-weight:bold;"> {{html endDate}}</div>
        </div>-->
    </div>
    </script>

    <script id="tmplGeneralDetailedData" type="text/x-jquery-tmpl">
        <table style="width: 100%;" cellpadding="0" cellspacing="0">
        <label>�����������</label></br><div style="width: 300px;"><input id="orgName" value="{{html orgName}}"/></div></br>
        <label>������������</label></br><div style="width: 300px;"><input id="orgLogin" value="{{html orgLogin}}"/></div></br>

        <label>�����</label></br>
        <div style="width:50%;"><select id="dealerSelector" style="width:100px;" dealerId="{{html dealerId}}" onchange="this.dealerId=this.value;"></select></div><br>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>������ </label></td><td><label>������ (�������������) </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><input id="pass1" value="{{html password}}"/></div>
                </td>
                <td>
                    <div style="width: 300px;"><input id="pass2" value="{{html password}}"/></div>
                </td>
            </tr>
        </table>

        <div style="margin:10px 0 10px 0; border-top:1px dashed #ccc;"></div>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>������ </label></td><td><label>����� </label></td><td><label>�������� ������ </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><select id="country" style="width:170px;" countryId="{{html country}}" onchange="this.countryId=this.value;"></select></div>
                </td>
                <td>
                    <div style="width: 300px;"><input id="city" value="{{html city}}"/></div>
                </td>
                <td>
                    <div style="width: 300px;"><input id="index" value="{{html index}}"/></div>
                </td>
            </tr>
        </table>

        <br/>

        <label>������� ����</label>

        <br/>

        <div style="width:100%;"><select id="timeZoneSelector" style="width:350px;" timeZoneId="{{html timeZone}}" onchange="this.timeZoneId=this.value;"></select></div>

        <br/>

        <label>����� (��������)</label>

        <br/>

        <div style="width: 500px;"><input id="addr1" value="{{html address1}}"/></div>

        <br/>

        <label>����� (��������������)</label>

        <br/>

        <div style="width: 500px;"><input id="addr2" value="{{html address2}}"/></div>

        <br/>

        <table style="" cellpadding="0" cellspacing="0">
        <tr><td><label>������� </label></td><td><label>���� </label></td><td><label>E-mail </label></td></tr>
        <tr>
            <td>
                <div style="width: 300px;"><input id="phone" value="{{html phone}}"/></div>
            </td>
            <td>
                <div style="width: 300px;"><input id="fax" value="{{html fax}}"/></div>
            </td>
            <td>
                <div style="width: 300px;"><input id="mail" value="{{html mail}}"/></div>
            </td>
        </tr>
        </table>

    </script>

    <script id="tmplGeneralOrgDetailedData1" type="text/x-jquery-tmpl">
        
        <table style="" cellpadding="0" cellspacing="0">
        <tr>
        <td><label>�����������</label></td><td></td>
        </tr>
        <tr>
        <td style="width: 300px;"><input id="orgName" value="{{html orgName}}"/></td>
        <td style="width: 300px;"><input style="width:20px;float:left;margin-top: -3px;" id="boxOnOff" type="checkbox" checked="true" disabled="true" class="unused"/>
        <label class="unused">�������</label></td>
        </tr>
        </br>

        </tr>
        </table>
        <!--<div style="margin:10px 0 10px 0; border-top:1px dashed #ccc;"></div>-->

    </script>

    <script id="tmplGeneralOrgDetailedData2" type="text/x-jquery-tmpl">

    <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>������ </label></td><td><label>����� </label></td><td><label>�������� ������ </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><select id="country" style="width:170px;" countryId="{{html country}}" onchange="this.countryId=this.value;"></select></div>
                </td>
                <td>
                    <div style="width: 300px;"><input id="city" value="{{html city}}"/></div>
                </td>
                <td>
                    <div style="width: 300px;"><input id="index" value="{{html index}}"/></div>
                </td>
            </tr>
        </table>

        <br/>

        <label>������� ����</label>

        <br/>

        <div style="width:100%;"><select id="timeZoneSelector" timeZoneId="{{html timeZone}}" style="width:350px;" onchange="this.timeZoneId=this.value;"></select></div>

        <br/>

        <label>����� (��������)</label>

        <br/>

        <div style="width: 500px;"><input id="addr1" value="{{html address1}}"/></div>

        <br/>

        <label>����� (��������������)</label>

        <br/><br/>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>���� (�����) </label></td><td><label>���� (������) </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><select id="lang_screen" style="width:170px;" langId="0" onchange="this.langId=this.value;">
                        <option value="0">�������</option>
                        <option value="1">English</option>
                    </select></div>
                </td>
                <td>
                    <div style="width: 300px;"><select id="lang_report" style="width:170px;" langId="0" onchange="this.langId=this.value;">
                        <option value="0">�������</option>
                        <option value="1">English</option>
                    </select></div>
                </td>
            </tr>
        </table>


    </script>


    <script id="GeneralData" type="text/x-jquery-tmpl">
     <div class="add-info-block">
     </div>
     <div id="tabs">
            <ul>
                <li><a href="#tabs-1">����� ��������</a></li>
                <li><a href="#tabs-2">������� ��������</a></li>
                <li><a href="#tabs-3">��������� ��������</a></li>
	        </ul>
            <div id="tabs-1">
                <div id="commonData" style="overflow: auto;">
                    <table style="width:100%;">
                        <tr>
                            <td id="firstGeneralRow">
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <div class="title-section">����������</div>
                            </td>
                            <td>
                                <div class="title-section">���������</div>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">
                                <div id="statisticTableWrapper">
                                    <table id="statisticTable"  style="width:100%;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                                        border="0" cellpadding="0" cellspacing="0">
                                        <thead id="statisticTableHeader"></thead>
                                        <tbody id="statisticTableBody" class="ui-widget-content wijmo-wijgrid-data">
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div id="messageTableWrapper">
                                    <table id="messageTable"  style="width:100%;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                                        border="0" cellpadding="0" cellspacing="0">
                                        <thead id="messageTableHeader"></thead>
                                        <tbody id="messageTableBody" class="ui-widget-content wijmo-wijgrid-data">
                                        </tbody>
                                    </table>
                                </div>
                                <button id="remove">�������</button>
                                <div id="deletedialog" title="�������� ���������" style="display: none;">
	                                <p>�� ������������� ������ ������� ���������� ���������?</p>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="tabs-2">
                <div id="detailedData1">
                </div>
            </div>
            <div id="tabs-3">
                <div id="detailedData2">
                </div>
            </div>
    </div>           
    </script>


    <script id="DealersData" type="text/x-jquery-tmpl">
        <table id="dealersTable"  style="width:100%;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
            border="0" cellpadding="0" cellspacing="0">
            <thead id="dealersTableHeader"></thead>
            <tbody id="dealersTableBody" class="ui-widget-content wijmo-wijgrid-data">
            </tbody>
        </table>
    </script>

    <script id="UsersData" type="text/x-jquery-tmpl">
     <div id="tabs">
            <ul>
                <li><a href="#tabs-2">������� ��������</a></li>
		        <li><a href="#tabs-3">��������� ��������</a></li>
	        </ul>
            <!--<div id="tabs-1">
                <div id="commonData" style="overflow: auto;">
                    <div id="userTableWrapper">
                      <table id="usersTable"  style="width:100%;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                          border="0" cellpadding="0" cellspacing="0">
                          <thead id="usersTableHeader"></thead>
                          <tbody id="usersTableBody" class="ui-widget-content wijmo-wijgrid-data">
                          </tbody>
                      </table>
                    </div>
                </div>
            </div>-->
            <div id="tabs-2">
                <div id="detailedData1" style="overflow: auto;">
                </div>
            </div>
            <div id="tabs-3">
                <div id="detailedData2" style="overflow: auto;">
                </div>
            </div>
    </div>           
    </script>

    <script id="tmplUsersDetailedData" type="text/x-jquery-tmpl">
        <label>�����������</label></br><div style="width: 300px;"><input id="orgName" value="{{html orgName}}"/></div></br>
        <label>������������</label></br><div style="width: 300px;"><input id="orgLogin" value="{{html login}}"/></div></br>

        <label>�����</label><br/>
        <div style="width:100%;"><select id="dealerSelector" style="width:100px;" dealerId="{{html dealerId}}" onchange="this.dealerId=this.value;"></select></div><br>

        <label>����</label><br/>
        <div style="width: 100%;"><select id="role" style="width:100px;" roleId="{{html roleId}}" onchange="this.roleId=this.value;"></select></div></br>

        <table style="" cellpadding="0" cellspacing="0">
        <tr><td><label>��� </label></td><td><label>�������� </label></td><td><label>������� </label></td></tr>
        <tr>
            <td>
                <div style="width: 300px;"><input id="name" value="{{html name}}"/></div>
            </td>
            <td>
                <div style="width: 300px;"><input id="patronimic" value="{{html patronimic}}"/></div>
            </td>
            <td>
                <div style="width: 300px;"><input id="surname" value="{{html surname}}"/></div>
            </td>
        </tr>
        </table>

        <br/>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>������ </label></td><td><label>������ (�������������) </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><input id="pass1" value="{{html password}}"/></div>
                </td>
                <td>
                    <div style="width: 300px;"><input id="pass2" value="{{html password}}"/></div>
                </td>
            </tr>
        </table>

        <div style="margin:10px 0 10px 0; border-top:1px dashed #ccc;"></div>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>������ </label></td><td><label>����� </label></td><td><label>�������� ������ </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><select id="country" style="width:170px;" countryId="{{html country}}" onchange="this.countryId=this.value;"></select>
                </td>
                <td>
                    <div style="width: 300px;"><input id="city" value="{{html city}}"/></div>
                </td>
                <td>
                    <div style="width: 300px;"><input id="index" value="{{html index}}"/></div>
                </td>
            </tr>
        </table>
        
        <br>

        <label>������� ����</label></br>
        <div style="width:100%;"><select id="timeZoneSelector" style="width:350px;" timeZoneId="{{html timeZone}}" onchange="this.timeZoneId=this.value;"></select></div><br>

        <label>����� (��������)</label><br><div style="width: 500px;"><input id="addr1" value="{{html address1}}"/></div><br>
        <label>����� (�������������)</label><br><div style="width: 500px;"><input id="addr2" value="{{html address2}}"/></div><br>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>������� </label></td><td><label>���� </label></td><td><label>E-mail </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;">
                        <input id="phone" value="{{html phone}}"/>
                    </div>
                </td>
                <td>
                    <div style="width: 300px;">
                        <input id="fax" value="{{html fax}}"/>
                    </div>
                </td>
                <td>
                    <div style="width: 300px;">
                        <input id="mail" value="{{html mail}}"/>
                    </div>
                </td>
            </tr>
        </table>

    </script>

    <script id="tmplUsersDetailedData1" type="text/x-jquery-tmpl">
        <label>�����������</label></br><div style="width: 300px;"><input id="orgName" value="{{html orgName}}"/></div></br>

        <table style="" cellpadding="0" cellspacing="0">
        <tr>
        <td><label>������������</label></td><td></td>
        </tr>
        <tr>
        <td><div style="width: 300px;"><input id="orgLogin" value="{{html login}}"/></div></td>
        <td><div  style="width: 300px;"><input style="width:20px;float:left;margin-top: -3px;" id="boxOnOff" type="checkbox" checked="true" disabled="true" class="unused"/>
        <label class="unused">�������</label></div></td>
        </tr>
        </tr>
        </table>

        </br>
        
        <table style="" cellpadding="0" cellspacing="0">
        <tr><td><label>����� </label></td><td><label>���� </label></td></tr>
        <tr>
            <td>
                <div style="width: 300px;"><select id="dealerSelector" style="width:100px;" dealerId="{{html dealerId}}" onchange="this.dealerId=this.value;"></select></div>
            </td>
            <td>
                <div style="width: 300px;"><select id="role" style="width:100px;" roleId="{{html roleId}}" onchange="this.roleId=this.value;"></select></div>
            </td>
        </tr>
        </table><br>
        
        <table style="" cellpadding="0" cellspacing="0">
        <tr><td><label>��� </label></td><td><label>�������� </label></td><td><label>������� </label></td></tr>
        <tr>
            <td>
                <div style="width: 300px;"><input id="name" value="{{html name}}"/></div>
            </td>
            <td>
                <div style="width: 300px;"><input id="patronimic" value="{{html patronimic}}"/></div>
            </td>
            <td>
                <div style="width: 300px;"><input id="surname" value="{{html surname}}"/></div>
            </td>
        </tr>
        </table>

        <br/>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>������ </label></td><td><label>������ (�������������) </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><input id="pass1" value="{{html password}}"/></div>
                </td>
                <td>
                    <div style="width: 300px;"><input id="pass2" value="{{html password}}"/></div>
                </td>
            </tr>
        </table>

        <!--<div style="margin:10px 0 10px 0; border-top:1px dashed #ccc;"></div>-->

    </script>

    <script id="tmplUsersDetailedData2" type="text/x-jquery-tmpl">
     <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>������ </label></td><td><label>����� </label></td><td><label>�������� ������ </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><select id="country" style="width:170px;" countryId="{{html country}}" onchange="this.countryId=this.value;"></select>
                </td>
                <td>
                    <div style="width: 300px;"><input id="city" value="{{html city}}"/></div>
                </td>
                <td>
                    <div style="width: 300px;"><input id="index" value="{{html index}}"/></div>
                </td>
            </tr>
        </table>
        
        <br>

        <label>������� ����</label></br>
        <div style="width:100%;"><select id="timeZoneSelector" style="width:350px;" timeZoneId="{{html timeZone}}" onchange="this.timeZoneId=this.value;"></select></div><br>

        <label>����� (��������)</label><br><div style="width: 500px;"><input id="addr1" value="{{html address1}}"/></div><br>
        <label>����� (�������������)</label><br><div style="width: 500px;"><input id="addr2" value="{{html address2}}"/></div><br>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>������� </label></td><td><label>���� </label></td><td><label>E-mail </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;">
                        <input id="phone" value="{{html phone}}"/>
                    </div>
                </td>
                <td>
                    <div style="width: 300px;">
                        <input id="fax" value="{{html fax}}"/>
                    </div>
                </td>
                <td>
                    <div style="width: 300px;">
                        <input id="mail" value="{{html mail}}"/>
                    </div>
                </td>
            </tr>
        </table>
        </br>
        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>���� (�����) </label></td><td><label>���� (������) </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><select id="lang_screen" style="width:170px;" langId="0" onchange="this.langId=this.value;">
                        <option value="0">�������</option>
                        <option value="1">English</option>
                    </select></div>
                </td>
                <td>
                    <div style="width: 300px;"><select id="lang_report" style="width:170px;" langId="0" onchange="this.langId=this.value;">
                        <option value="0">�������</option>
                        <option value="1">English</option>
                    </select></div>
                </td>
            </tr>
        </table>

    </script>

    <!--<script id="tmplDealerDetailedData" type="text/x-jquery-tmpl">
        <table style="width: 100%;">
        <label>�������</label></br><div style="width: 80%;"><input id="orgName" value="{{html orgName}}"/></div></br>
        <label>������������</label></br><div style="width: 40%;"><input id="orgLogin" value="{{html orgLogin}}"/></div></br>

        <table style="width:81%;">
        <tr><td><label>������ </label></td><td><label>������ (�������������) </label></td></tr>
        <tr><td><input id="pass1" value="{{html password}}"/></td><td><input id="pass2" value="{{html password}}"/></td></tr>
        </table>

        <hr>

        <table style="width:100%;">
        <tr><td><label>������ </label></td><td><label>����� </label></td><td><label>�������� ������ </label></td></tr>
        <tr><td><select id="country" countryId="{{html country}}" onchange="this.countryId=this.value;"></select></td><td>
        <input id="city" value="{{html city}}"/></td><td><input id="index" value="{{html index}}"/></td></tr>
        </table><br>

        <label>������� ����</label></br>
        <div style="width:50%;"><select id="timeZoneSelector" timeZoneId="{{html timeZone}}" onchange="this.timeZoneId=this.value;"></select></div><br>

        <label>�����1</label><br><div style="width: 80%;"><input id="addr1" value="{{html address1}}"/></div><br>
        <label>�����2</label><br><div style="width: 80%;"><input id="addr2" value="{{html address2}}"/></div><br>

        <table style="width:100%;">
        <tr><td><label>������� </label></td><td><label>���� </label></td><td><label>e-mail </label></td></tr>
        <tr><td><input id="phone" value="{{html phone}}"/></td><td><input id="fax" value="{{html fax}}"/></td><td><input id="mail" value="{{html mail}}"/></td></tr>
        </table>

        <hr>
    </script>-->

    <script id="tmplDealersTableContent" type="text/x-jquery-tmpl">
       <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                        <input type="checkbox" dealerId="{{html id}}" name="dealerCheckbox" onclick="radioIndex=$(this).attr('dealerId');"/>
                    </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input id="nameinput{{html id}}" value="{{html name}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <center>
                        {{html date}}
                   </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <center>
                        <input id="endDateInput{{html id}}" name="endDateInput" value="{{html endDate}}" class="inputField-readonly input" readonly="readonly"/>
                   </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <select id="country{{html id}}" dealerId="{{html id}}" countryId="{{html country}}" name="countrySelector" style="width:180px;" onchange="this.countryId=this.value;changeCountry(this);"></select>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <select id="city{{html id}}" cityDealerId="{{html id}}" countryId="{{html country}}" cityId="{{html city}}" name="citySelector" style="width:230px;" onchange="this.cityId=this.value;"></select>
                </div>
            </td>
        </tr>
    </script>

    <script id="tmplNewDealer" type="text/x-jquery-tmpl">
       <tr id="newRow" class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input id="newnameinput" class="inputField input"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <center>
                        <input id="startDatePicker" class="inputField-readonly input" readonly="readonly"/>
                   </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <center>
                        <input id="endDatePicker" class="inputField-readonly input" readonly="readonly"/>
                   </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <select id="newcountry" countryId="1" dealerId="-1" name="countrySelector" onchange="this.countryId=this.value;changeCountry(this);"></select>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <select id="newcity" countryId="1" cityDealerId="-1" cityId="0" name="citySelector" onchange="this.cityId=this.value;"></select>
                </div>
            </td>
        </tr>
    </script>

    <script id="tmplUsersTableContent" type="text/x-jquery-tmpl">
       <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                        <input type="radio" userId="{{html id}}" onclick="radioIndex=$(this).attr('userId');"/>
                    </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   {{html dealer}}
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   {{html name}}
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   {{html surname}}
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   {{html login}}
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                <select id="role{{html id}}" roleId="{{html roleId}}" name="roleSelector" style="width:100px;" onchange="this.roleId=this.value;"></select>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   {{html state}}
                </div>
            </td>
        </tr>
    </script>

    <script id="InvoiceData" type="text/x-jquery-tmpl">
            <div class="title-section" style="margin:10px;">������</div>
            <div id="filter" style="margin-left:10px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td><label>��������� ���� </label><input id="startDatePicker" type="text"/></td>
                        <td><label>�������� ���� </label><input id="endDatePicker" type="text"/></td>
                        <td>
                            <div id="dateErrorBlock" class="error-block-admin">
                                <label class="error-admin" id="dateErrorLabel"> ������: ������� ��������� � �������� ����!</label>
                            </div>
                        </td>
                    </tr>
                </table>

                <div style="margin-top:5px;margin-bottom:5px;height:30px;">
                    <div style="float:left;margin-right:10px;margin-top: 6px;">��� </div>
                    <div style="float:left;margin-right:10px;margin-top: 3px;">
                        <select id="invoiceStatusSelector" statusType="0" onchange="this.statusType=this.value;" style="width:100px;"></select>
                    </div>
                    <div style="float:left;">
                        <button id="buildButton">���������</button>
                    </div>
                </div>

            </div>

            <div id="invoiceTableWrapper">
                <table id="invoiceTable"  style="border-collapse: separate;width:100%;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                    border="0" cellpadding="0" cellspacing="0">
                    <thead id="invoiceTableHeader"></thead>
                    <tbody id="invoiceTableBody" class="ui-widget-content wijmo-wijgrid-data">
                    </tbody>
                </table>
            </div>
    </script>
    
    <script id="JournalData" type="text/x-jquery-tmpl">
            <div class="title-section" style="margin:10px;">������</div>
            <div id="filter" style="margin-left:10px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td><label>��������� ���� </label><input id="startDatePicker" type="text"/></td>
                        <td><label>�������� ���� </label><input id="endDatePicker" type="text"/><td>
                        <td>
                            <div id="dateErrorBlock" class="error-block-admin">
                                <label class="error-admin" id="dateErrorLabel"> ������: ������� ��������� � �������� ����!</label>
                            </div>
                        </td>
                    </tr>

                </table>

                <div style="margin-top:5px;margin-bottom:5px;height:30px;">
                    <div style="float:left;margin-right:10px;margin-top: 6px;">������� </div>
                    <div style="float:left;margin-right:10px;margin-top: 3px;">
                        <select id="eventSelector" event="-1" onchange="this.event=this.value;" style="width:250px;"></select>
                    </div>
                    <div style="float:left;margin-right:10px;margin-top: 6px;">
                        ����� � �������� 
                    </div>
                    <div style="float:left;margin-right:10px;margin-top: 4px;">
                        <input id="textInput" value=""/>
                    </div>
                    <div style="float:left;">
                        <button id="buildButton">���������</button>
                    </div>
                </div>

            </div>

            <div id="journalTableWrapper">
                <table id="journalTable"  style="border-collapse: separate;width:100%;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                    border="0" cellpadding="0" cellspacing="0">
                    <thead id="journalTableHeader"></thead>
                    <tbody id="journalTableBody" class="ui-widget-content wijmo-wijgrid-data">
                    </tbody>
                </table>
            </div>
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
                <div name="noteInput">
                    {{html note}}
                </div>
                </div>
            </td>
        </tr>
    </script>

    <script id="tmplMessageTableContent" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                        <input type="checkbox" messageId="{{html id}}" name="messageCheckbox"/>
                    </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   {{html sender}}
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   {{html topic}}
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   {{html date}}
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   {{html endDate}}
                </div>
            </td>
        </tr>
    </script>

    <script id="tmplStatisticTableContent" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   ���������� �������������:
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <center>
                        {{html usersCount}}
                   </center>
                </div>
            </td>
        </tr>
         <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   ���������� ���������:
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <center>
                        {{html driversCount}}
                   </center>
                </div>
            </td>
        </tr>
         <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   ���������� �����:
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <center>
                        {{html vehiclesCount}}
                   </center>
                </div>
            </td>
        </tr>
         <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   ���������� �������:
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                   <center>
                        {{html invoicesCount}}
                   </center>
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
    <script id="tmplDealersTree" type="text/x-jquery-tmpl">
        <li class="folder" likey="${Key}"><a><span key="${Key}" level="0">${DealerName}</span></a>
        <ul>
            {{each dealers}}
            <li class="file" likey="${Key}"><a><span key="${Key}" level="1">${DealerName}</span></a>
                <ul>
                    {{each dealers}}
                    <li class="file" likey="${Key}"><a><span key="${Key}" level="2">${DealerName}</span></a>
                        <ul>
                            {{each dealers}}
                            <li class="file" likey="${Key}"><a><span key="${Key}" level="3">${DealerName}</span></a>
                            </li>
                            {{/each}}
                        </ul>
                    </li>
                    {{/each}}
                </ul>
            </li>
            {{/each}}
        </ul>
        </li>
    </script>

    <script id="tmplUsersTree" type="text/x-jquery-tmpl">
    {{each orgs}}
        <li class="folder"><a><span level="1" key="-1" crtype="0" orgId="${OrgID}">${OrgName}</span></a>
        <ul>
            <li class="file"><a><span level="2" key="-1" crtype="1" orgId="${OrgID}">��������������</span></a>
                <ul>
                    {{each admins}}
                    <li class="file" likey="${Key}"><a><span key="${Key}" level="3" crtype="1" orgId="${OrgID}">${Value}</span></a>
                    </li>
                    {{/each}}
                </ul>
            </li>
            <li class="file"><a><span level="2" key="-1" crtype="2" orgId="${OrgID}">���������</span></a>
                <ul>
                    {{each managers}}
                    <li class="file" likey="${Key}"><a><span key="${Key}" level="3" crtype="2" orgId="${OrgID}">${Value}</span></a>
                    </li>
                    {{/each}}
                </ul>
            </li>
        </ul>
        </li>
    {{/each}}
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

        <div id="accordion" style="width: 5">
            <h3><asp:LinkButton ID="GeneralDataAccordionPane1" runat="server" CausesValidation="false" PostBackUrl="#" Text="�������" /></h3>
                <div id="firstAccordionPanel">                   
                    <!--<center>
                        ���������� � ������� ������������.
                        <br/>
                        <br/>
                        ������ ������ ��������� ������������� � ������������� ���������� � ������� ������������.
                    </center>-->
                   <ul id="dealersTree">
                    
                   </ul>
                    <div style="display: none">
                        <div id="wrongOrgMessage" title="SmartFIS">
                            <div style="margin-top: 10px;">
                                <h4>
                                    �������� ������
                                </h4>
                            </div>
                            <div style="margin-top: 20px;">
                                ������� �������� �����������!
                            </div>
                        </div>
                    </div>
                </div>
            <!--<h3 id="AccountsAccordionPane2_Header" runat="server"><asp:LinkButton ID="AccountsAccordionPane2" runat="server" CausesValidation="false" PostBackUrl="#" Text="������" /></h3>
              <div id="secondAccordionPanel">                   
                    <center>
                        ���������� � ������� ������� �����������.
                        <br/>
                        <br/>
                        ������ ������ ��������� ������������� � ������������� ���������� � ������� � ������ ������� �����������.
                    </center>

                </div>-->
            <h3><asp:LinkButton ID="UsersAccordionPane3" runat="server" CausesValidation="false" PostBackUrl="#" Text="������������" /></h3>
                <div id="thirdAccordionPanel">                   
                    <!--<center>
                        ���������� � ������������� ������� �����������.
                        <br/>
                        <br/>
                        ������ ������ ��������� ������������� � ������������� ���������� � ������������� � ������ ������� �����������.
                    </center>-->
                    <ul id="usersTree">
                    
                    </ul>
                    <div style="display: none">
                        <div id="wrongUserNameMessage" title="SmartFIS">
                            <div style="margin-top: 10px;">
                                <h4>
                                    �������� ������
                                </h4>
                            </div>
                            <div style="margin-top: 20px;">
                                ������� ����� ������������!
                            </div>
                        </div>
                        <div id="wrongUserRoleMessage" title="SmartFIS">
                            <div style="margin-top: 10px;">
                                <h4>
                                    �������� ������
                                </h4>
                            </div>
                            <div style="margin-top: 20px;">
                                ������� ���� ������������!
                            </div>
                        </div>
                        <div id="wrongUserPassMessage" title="SmartFIS">
                            <div style="margin-top: 10px;">
                                <h4>
                                    �������� ������
                                </h4>
                            </div>
                            <div style="margin-top: 20px;">
                                ������ �� ������ ��� ��������� ������ �� ���������!
                            </div>
                        </div>
                    </div>
                </div>
            <h3><asp:LinkButton ID="BillsAccordionPane5" runat="server" CausesValidation="false" PostBackUrl="#" Text="�����" /></h3>
                <div id="fourthAccordionPanel">                   
                    <!--<center>
                        ���������� � ������ ������� �����������.
                        <br/>
                        <br/>
                        ������ ������ ��������� ������������� ���������� � ������ � ������ ������� �����������.
                    </center>-->
                   <ul id="dealersTree2">
                    
                   </ul>
                </div>
            <h3><asp:LinkButton ID="LogAccordionPane6" runat="server" CausesValidation="false" PostBackUrl="#" Text="������" /></h3>
                <div id="fifthAccordionPanel">                   
                    <!--<center>
                        ���������� � �������� ������� ������� �����������.
                        <br/>
                        <br/>
                        ������ ������ ��������� ������������� ���������� � ��������� ������������� � ������ ������� �����������.
                    </center>-->
                    <ul id="dealersTree3">
                    
                   </ul>
                </div>
        </div>
            
</asp:Content>

<asp:Content ID="ChoisesContent" ContentPlaceHolderID="MainConditions_PlaceHolder"
    runat="server">
    <div id="statusPanel">
    </div>
</asp:Content>

<asp:Content ID="DataContent" ContentPlaceHolderID="Reports_PlaceHolder" runat="server">
    <div id="ContentContainer">
    </div>
</asp:Content>
<asp:Content ID="DecisionContent1" ContentPlaceHolderID="Decision_PlaceHolder" runat="server">

    <script id="�ontrolsGeneralDetailed" type="text/x-jquery-tmpl">
        <button id="edit">�������������</button>
        <button id="delete">�������</button>
        <button id="create">�������</button>
        <div style="float:right">
            <button id="save">���������</button>
            <button id="cancel">������</button>
        </div>
        <div id="deletedealerdialog" title="��������" style="display: none;">
	        <p>�� ������������� ������ ������� ���������� �����������?</p>
        </div>
    </script>

    <script id="�ontrolsDealers" type="text/x-jquery-tmpl">
        <button id="edit">�������������</button>
        <button id="delete">�������</button>
        <button id="create">�������</button>
        <div style="float:right">
            <button id="save">���������</button>
            <button id="cancel">������</button>
        </div>
        <div id="deletedialog" title="��������" style="display: none;">
	        <p>�� ������������� ������ ������� ���������� ��������?</p>
        </div>
    </script>

    <script id="controlsUsers" type="text/x-jquery-tmpl">
        <button id="edit">�������������</button>
        <button id="delete">�������</button>
        <button id="create">�������</button>
        <div style="float:right">
            <button id="save">���������</button>
            <button id="cancel">������</button>
        </div>
        <div id="deletedialog" title="��������" style="display: none;">
	        <p>�� ������������� ������ ������� ���������� �������?</p>
        </div>
    </script>

    <div id="userControls">
    </div>
</asp:Content>