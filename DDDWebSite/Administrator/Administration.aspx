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
                  if ($("a", ui.newHeader).text() == "Аккаунт") {
                      $("#userControls").empty();
                      mode = "";
                      tabIndex = 0;
                      radioIndex = -1;
                      buildOrgTree(0);
                      loadGeneralData();
                      createPeriodControls();
                  };
                  /*if ($("a", ui.newHeader).text() == "Дилеры") {
                  $("#userControls").empty();
                  mode = "";
                  tabIndex = 0;
                  radioIndex = -1;
                  loadDealersData();
                  resizeAdmin();
                  };*/
                  if ($("a", ui.newHeader).text() == "Пользователи") {
                      $("#userControls").empty();
                      mode = "";
                      tabIndex = 0;
                      radioIndex = -1;
                      buildUserTree(0);
                      createPeriodControls();
                      //loadUsersData();
                  };
                  if ($("a", ui.newHeader).text() == "Счета") {
                      $("#userControls").empty();
                      mode = "";
                      $("#dateErrorBlock").hide();
                      buildOrgTreeInvoices(0);
                      //loadInvoiceData();
                      resizeAdmin();
                  };
                  if ($("a", ui.newHeader).text() == "Журнал") {
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
            <label>Начальная дата </label><input id="startDatePicker" type="text"/>
            <label>Конечная дата </label><input id="endDatePicker" type="text"/>
            <button id="buildButton">Построить</button>
            <div id="dateErrorBlock" class="error-block">
                <label class="error" id="dateErrorLabel"> Ошибка: Укажите начальную и конечную дату!</label>
            </div>
            <br/><br/>
        </div>
    </script>

    <script id="tmplGeneralData" type="text/x-jquery-tmpl">
    <div id="generalDataLabels">
        <div style="margin-top:2px;">
            <div style="float:left;margin-right:5px;">Текущее подключение с:</div>
            <div style="font-weight:bold;"> {{html connectDate}}</div>
        </div>
        <div style="margin-top:2px;">
            <div style="float:left;margin-right:5px;">Тип лицензии:</div>
            <div style="font-weight:bold;"> {{html licenseType}}</div>
        </div>
        <!--<div style="margin-top:2px;">
            <div style="float:left;margin-right:5px;">Дата регистрации в системе:</div>
            <div style="font-weight:bold;"> {{html registerDate}}</div>
        </div>
        <div style="margin-top:2px;">
            <div style="float:left;margin-right:5px;">Срок окончания регистрации:</div>
            <div style="font-weight:bold;"> {{html endDate}}</div>
        </div>-->
    </div>
    </script>

    <script id="tmplGeneralDetailedData" type="text/x-jquery-tmpl">
        <table style="width: 100%;" cellpadding="0" cellspacing="0">
        <label>Организация</label></br><div style="width: 300px;"><input id="orgName" value="{{html orgName}}"/></div></br>
        <label>Пользователь</label></br><div style="width: 300px;"><input id="orgLogin" value="{{html orgLogin}}"/></div></br>

        <label>Дилер</label></br>
        <div style="width:50%;"><select id="dealerSelector" style="width:100px;" dealerId="{{html dealerId}}" onchange="this.dealerId=this.value;"></select></div><br>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>Пароль </label></td><td><label>Пароль (Подтверждение) </label></td></tr>
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
            <tr><td><label>Страна </label></td><td><label>Город </label></td><td><label>Почтовый индекс </label></td></tr>
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

        <label>Часовая зона</label>

        <br/>

        <div style="width:100%;"><select id="timeZoneSelector" style="width:350px;" timeZoneId="{{html timeZone}}" onchange="this.timeZoneId=this.value;"></select></div>

        <br/>

        <label>Адрес (Основной)</label>

        <br/>

        <div style="width: 500px;"><input id="addr1" value="{{html address1}}"/></div>

        <br/>

        <label>Адрес (Дополнительный)</label>

        <br/>

        <div style="width: 500px;"><input id="addr2" value="{{html address2}}"/></div>

        <br/>

        <table style="" cellpadding="0" cellspacing="0">
        <tr><td><label>Телефон </label></td><td><label>Факс </label></td><td><label>E-mail </label></td></tr>
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
        <td><label>Организация</label></td><td></td>
        </tr>
        <tr>
        <td style="width: 300px;"><input id="orgName" value="{{html orgName}}"/></td>
        <td style="width: 300px;"><input style="width:20px;float:left;margin-top: -3px;" id="boxOnOff" type="checkbox" checked="true" disabled="true" class="unused"/>
        <label class="unused">Включен</label></td>
        </tr>
        </br>

        </tr>
        </table>
        <!--<div style="margin:10px 0 10px 0; border-top:1px dashed #ccc;"></div>-->

    </script>

    <script id="tmplGeneralOrgDetailedData2" type="text/x-jquery-tmpl">

    <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>Страна </label></td><td><label>Город </label></td><td><label>Почтовый индекс </label></td></tr>
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

        <label>Часовая зона</label>

        <br/>

        <div style="width:100%;"><select id="timeZoneSelector" timeZoneId="{{html timeZone}}" style="width:350px;" onchange="this.timeZoneId=this.value;"></select></div>

        <br/>

        <label>Адрес (Основной)</label>

        <br/>

        <div style="width: 500px;"><input id="addr1" value="{{html address1}}"/></div>

        <br/>

        <label>Адрес (Дополнительный)</label>

        <br/><br/>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>Язык (экран) </label></td><td><label>Язык (отчеты) </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><select id="lang_screen" style="width:170px;" langId="0" onchange="this.langId=this.value;">
                        <option value="0">Русский</option>
                        <option value="1">English</option>
                    </select></div>
                </td>
                <td>
                    <div style="width: 300px;"><select id="lang_report" style="width:170px;" langId="0" onchange="this.langId=this.value;">
                        <option value="0">Русский</option>
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
                <li><a href="#tabs-1">Общие сведения</a></li>
                <li><a href="#tabs-2">Краткие сведения</a></li>
                <li><a href="#tabs-3">Детальные сведения</a></li>
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
                                <div class="title-section">Статистика</div>
                            </td>
                            <td>
                                <div class="title-section">Сообщения</div>
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
                                <button id="remove">Удалить</button>
                                <div id="deletedialog" title="Удаление сообщений" style="display: none;">
	                                <p>Вы действительно хотите удалить выделенные сообщения?</p>
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
                <li><a href="#tabs-2">Краткие сведения</a></li>
		        <li><a href="#tabs-3">Детальные сведения</a></li>
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
        <label>Организация</label></br><div style="width: 300px;"><input id="orgName" value="{{html orgName}}"/></div></br>
        <label>Пользователь</label></br><div style="width: 300px;"><input id="orgLogin" value="{{html login}}"/></div></br>

        <label>Дилер</label><br/>
        <div style="width:100%;"><select id="dealerSelector" style="width:100px;" dealerId="{{html dealerId}}" onchange="this.dealerId=this.value;"></select></div><br>

        <label>Роль</label><br/>
        <div style="width: 100%;"><select id="role" style="width:100px;" roleId="{{html roleId}}" onchange="this.roleId=this.value;"></select></div></br>

        <table style="" cellpadding="0" cellspacing="0">
        <tr><td><label>Имя </label></td><td><label>Отчество </label></td><td><label>Фамилия </label></td></tr>
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
            <tr><td><label>Пароль </label></td><td><label>Пароль (Подтверждение) </label></td></tr>
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
            <tr><td><label>Страна </label></td><td><label>Город </label></td><td><label>Почтовый индекс </label></td></tr>
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

        <label>Часовая зона</label></br>
        <div style="width:100%;"><select id="timeZoneSelector" style="width:350px;" timeZoneId="{{html timeZone}}" onchange="this.timeZoneId=this.value;"></select></div><br>

        <label>Адрес (Основной)</label><br><div style="width: 500px;"><input id="addr1" value="{{html address1}}"/></div><br>
        <label>Адрес (Дополнительно)</label><br><div style="width: 500px;"><input id="addr2" value="{{html address2}}"/></div><br>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>Телефон </label></td><td><label>Факс </label></td><td><label>E-mail </label></td></tr>
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
        <label>Организация</label></br><div style="width: 300px;"><input id="orgName" value="{{html orgName}}"/></div></br>

        <table style="" cellpadding="0" cellspacing="0">
        <tr>
        <td><label>Пользователь</label></td><td></td>
        </tr>
        <tr>
        <td><div style="width: 300px;"><input id="orgLogin" value="{{html login}}"/></div></td>
        <td><div  style="width: 300px;"><input style="width:20px;float:left;margin-top: -3px;" id="boxOnOff" type="checkbox" checked="true" disabled="true" class="unused"/>
        <label class="unused">Включен</label></div></td>
        </tr>
        </tr>
        </table>

        </br>
        
        <table style="" cellpadding="0" cellspacing="0">
        <tr><td><label>Дилер </label></td><td><label>Роль </label></td></tr>
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
        <tr><td><label>Имя </label></td><td><label>Отчество </label></td><td><label>Фамилия </label></td></tr>
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
            <tr><td><label>Пароль </label></td><td><label>Пароль (Подтверждение) </label></td></tr>
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
            <tr><td><label>Страна </label></td><td><label>Город </label></td><td><label>Почтовый индекс </label></td></tr>
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

        <label>Часовая зона</label></br>
        <div style="width:100%;"><select id="timeZoneSelector" style="width:350px;" timeZoneId="{{html timeZone}}" onchange="this.timeZoneId=this.value;"></select></div><br>

        <label>Адрес (Основной)</label><br><div style="width: 500px;"><input id="addr1" value="{{html address1}}"/></div><br>
        <label>Адрес (Дополнительно)</label><br><div style="width: 500px;"><input id="addr2" value="{{html address2}}"/></div><br>

        <table style="" cellpadding="0" cellspacing="0">
            <tr><td><label>Телефон </label></td><td><label>Факс </label></td><td><label>E-mail </label></td></tr>
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
            <tr><td><label>Язык (экран) </label></td><td><label>Язык (отчеты) </label></td></tr>
            <tr>
                <td>
                    <div style="width: 300px;"><select id="lang_screen" style="width:170px;" langId="0" onchange="this.langId=this.value;">
                        <option value="0">Русский</option>
                        <option value="1">English</option>
                    </select></div>
                </td>
                <td>
                    <div style="width: 300px;"><select id="lang_report" style="width:170px;" langId="0" onchange="this.langId=this.value;">
                        <option value="0">Русский</option>
                        <option value="1">English</option>
                    </select></div>
                </td>
            </tr>
        </table>

    </script>

    <!--<script id="tmplDealerDetailedData" type="text/x-jquery-tmpl">
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
            <div class="title-section" style="margin:10px;">Фильтр</div>
            <div id="filter" style="margin-left:10px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td><label>Начальная дата </label><input id="startDatePicker" type="text"/></td>
                        <td><label>Конечная дата </label><input id="endDatePicker" type="text"/></td>
                        <td>
                            <div id="dateErrorBlock" class="error-block-admin">
                                <label class="error-admin" id="dateErrorLabel"> Ошибка: Укажите начальную и конечную дату!</label>
                            </div>
                        </td>
                    </tr>
                </table>

                <div style="margin-top:5px;margin-bottom:5px;height:30px;">
                    <div style="float:left;margin-right:10px;margin-top: 6px;">Тип </div>
                    <div style="float:left;margin-right:10px;margin-top: 3px;">
                        <select id="invoiceStatusSelector" statusType="0" onchange="this.statusType=this.value;" style="width:100px;"></select>
                    </div>
                    <div style="float:left;">
                        <button id="buildButton">Применить</button>
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
            <div class="title-section" style="margin:10px;">Фильтр</div>
            <div id="filter" style="margin-left:10px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td><label>Начальная дата </label><input id="startDatePicker" type="text"/></td>
                        <td><label>Конечная дата </label><input id="endDatePicker" type="text"/><td>
                        <td>
                            <div id="dateErrorBlock" class="error-block-admin">
                                <label class="error-admin" id="dateErrorLabel"> Ошибка: Укажите начальную и конечную дату!</label>
                            </div>
                        </td>
                    </tr>

                </table>

                <div style="margin-top:5px;margin-bottom:5px;height:30px;">
                    <div style="float:left;margin-right:10px;margin-top: 6px;">Событие </div>
                    <div style="float:left;margin-right:10px;margin-top: 3px;">
                        <select id="eventSelector" event="-1" onchange="this.event=this.value;" style="width:250px;"></select>
                    </div>
                    <div style="float:left;margin-right:10px;margin-top: 6px;">
                        Текст в описании 
                    </div>
                    <div style="float:left;margin-right:10px;margin-top: 4px;">
                        <input id="textInput" value=""/>
                    </div>
                    <div style="float:left;">
                        <button id="buildButton">Применить</button>
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
                   Количество пользователей:
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
                   Количество водителей:
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
                   Количество машин:
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
                   Количество отчетов:
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
            <li class="file"><a><span level="2" key="-1" crtype="1" orgId="${OrgID}">Администраторы</span></a>
                <ul>
                    {{each admins}}
                    <li class="file" likey="${Key}"><a><span key="${Key}" level="3" crtype="1" orgId="${OrgID}">${Value}</span></a>
                    </li>
                    {{/each}}
                </ul>
            </li>
            <li class="file"><a><span level="2" key="-1" crtype="2" orgId="${OrgID}">Менеджеры</span></a>
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
            <h3><asp:LinkButton ID="GeneralDataAccordionPane1" runat="server" CausesValidation="false" PostBackUrl="#" Text="Аккаунт" /></h3>
                <div id="firstAccordionPanel">                   
                    <!--<center>
                        Информация о текущем пользователе.
                        <br/>
                        <br/>
                        Данный раздел позволяет просматривать и редактировать информацию о текущем пользователе.
                    </center>-->
                   <ul id="dealersTree">
                    
                   </ul>
                    <div style="display: none">
                        <div id="wrongOrgMessage" title="SmartFIS">
                            <div style="margin-top: 10px;">
                                <h4>
                                    Неверные данные
                                </h4>
                            </div>
                            <div style="margin-top: 20px;">
                                Введите название организации!
                            </div>
                        </div>
                    </div>
                </div>
            <!--<h3 id="AccountsAccordionPane2_Header" runat="server"><asp:LinkButton ID="AccountsAccordionPane2" runat="server" CausesValidation="false" PostBackUrl="#" Text="Дилеры" /></h3>
              <div id="secondAccordionPanel">                   
                    <center>
                        Информация о дилерах текущей организации.
                        <br/>
                        <br/>
                        Данный раздел позволяет просматривать и редактировать информацию о дилерах в рамках текущей организации.
                    </center>

                </div>-->
            <h3><asp:LinkButton ID="UsersAccordionPane3" runat="server" CausesValidation="false" PostBackUrl="#" Text="Пользователи" /></h3>
                <div id="thirdAccordionPanel">                   
                    <!--<center>
                        Информация о пользователях текущей организации.
                        <br/>
                        <br/>
                        Данный раздел позволяет просматривать и редактировать информацию о пользователях в рамках текущей организации.
                    </center>-->
                    <ul id="usersTree">
                    
                    </ul>
                    <div style="display: none">
                        <div id="wrongUserNameMessage" title="SmartFIS">
                            <div style="margin-top: 10px;">
                                <h4>
                                    Неверные данные
                                </h4>
                            </div>
                            <div style="margin-top: 20px;">
                                Введите логин пользователя!
                            </div>
                        </div>
                        <div id="wrongUserRoleMessage" title="SmartFIS">
                            <div style="margin-top: 10px;">
                                <h4>
                                    Неверные данные
                                </h4>
                            </div>
                            <div style="margin-top: 20px;">
                                Укажите роль пользователя!
                            </div>
                        </div>
                        <div id="wrongUserPassMessage" title="SmartFIS">
                            <div style="margin-top: 10px;">
                                <h4>
                                    Неверные данные
                                </h4>
                            </div>
                            <div style="margin-top: 20px;">
                                Пароль не указан или введенные пароли не совпадают!
                            </div>
                        </div>
                    </div>
                </div>
            <h3><asp:LinkButton ID="BillsAccordionPane5" runat="server" CausesValidation="false" PostBackUrl="#" Text="Счета" /></h3>
                <div id="fourthAccordionPanel">                   
                    <!--<center>
                        Информация о счетах текущей организации.
                        <br/>
                        <br/>
                        Данный раздел позволяет просматривать информацию о счетах в рамках текущей организации.
                    </center>-->
                   <ul id="dealersTree2">
                    
                   </ul>
                </div>
            <h3><asp:LinkButton ID="LogAccordionPane6" runat="server" CausesValidation="false" PostBackUrl="#" Text="Журнал" /></h3>
                <div id="fifthAccordionPanel">                   
                    <!--<center>
                        Информация о событиях журнала текущей организации.
                        <br/>
                        <br/>
                        Данный раздел позволяет просматривать информацию о действиях пользователей в рамках текущей организации.
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

    <script id="сontrolsGeneralDetailed" type="text/x-jquery-tmpl">
        <button id="edit">Редактировать</button>
        <button id="delete">Удалить</button>
        <button id="create">Создать</button>
        <div style="float:right">
            <button id="save">Сохранить</button>
            <button id="cancel">Отмена</button>
        </div>
        <div id="deletedealerdialog" title="Удаление" style="display: none;">
	        <p>Вы действительно хотите удалить выделенную организацию?</p>
        </div>
    </script>

    <script id="сontrolsDealers" type="text/x-jquery-tmpl">
        <button id="edit">Редактировать</button>
        <button id="delete">Удалить</button>
        <button id="create">Создать</button>
        <div style="float:right">
            <button id="save">Сохранить</button>
            <button id="cancel">Отмена</button>
        </div>
        <div id="deletedialog" title="Удаление" style="display: none;">
	        <p>Вы действительно хотите удалить выделенные элементы?</p>
        </div>
    </script>

    <script id="controlsUsers" type="text/x-jquery-tmpl">
        <button id="edit">Редактировать</button>
        <button id="delete">Удалить</button>
        <button id="create">Создать</button>
        <div style="float:right">
            <button id="save">Сохранить</button>
            <button id="cancel">Отмена</button>
        </div>
        <div id="deletedialog" title="Удаление" style="display: none;">
	        <p>Вы действительно хотите удалить выделенный элемент?</p>
        </div>
    </script>

    <div id="userControls">
    </div>
</asp:Content>