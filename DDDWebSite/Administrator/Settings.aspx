<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true"
    CodeFile="Settings.aspx.cs" Inherits="Administrator_Settings" %>

<%@ Register src="../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

    <link type="text/css" href="../css/custom-theme/jquery.wijmo.wijcombobox.css" rel="stylesheet" />

    <script src="../js/custom/Settings.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.datepicker-ru.js" type="text/javascript"></script>
    <script src="../js/jquery.wijmo.wijcombobox.js" type="text/javascript"></script>
    
</asp:Content>

<asp:Content ID="AccordionContent" ContentPlaceHolderID="VerticalOutlookMenu_PlaceHolder" runat="server">

    <asp:HiddenField ID="AccordionSelectedPane" Visible="true" runat="server" Value="0" />

    <script type="text/javascript" language="javascript">

        var mode = "";
        var currentCardId = "";
        var currentDriverId = "";
        var selectedNodeType = "-1";

        //run on page load
        $(function () {
            mode = "";
            buildTree();
            buildRemindTree();

            $("#accordion").accordion({
                change: function (event, ui) {
                    if ($("a", ui.newHeader).text() == "Организация") {
                        mode = "";
                        $("#contentSettingsPlace").empty();
                        $("#headerSettings").empty();
                        $("#general").wijtreenode({ selected: true });
                        loadGeneralSettings();
                    };
                    if ($("a", ui.newHeader).text() == "Напоминания") {
                        mode = "";
                        $("#contentSettingsPlace").empty();
                        //createRemindControls();
                        loadReminds();
                    }
                    /*if ($("a", ui.newHeader).text() == "Дополнительно") {
                    }*/

                    //Раздел Водители
                    if ($("a", ui.newHeader).attr("code") == 3) {
                        currentCardId = "-1";
                        mode = "";
                        $("#headerSettings").empty();
                        $("#headerSettings").text("Настройки водителей");
                        $("#contentSettings").empty();
                        $("#contentSettingsPlace").empty();
                        $("#userControls").empty();
                        loadDriversTreeSingle("", "");
                    }
                    //Раздел ТС
                    if ($("a", ui.newHeader).attr("code") == 4) {
                        currentCardId = "-1";
                        mode = "";
                        $("#headerSettings").empty();
                        $("#headerSettings").text("Настройки транспортных средств");
                        $("#contentSettings").empty();
                        $("#contentSettingsPlace").empty();
                        $("#userControls").empty();
                        loadVehiclesTreeSingle("", "");
                    }
                    //Раздел Группы
                    if ($("a", ui.newHeader).attr("code") == 5) {
                        currentCardId = "";
                        mode = "";
                        $("#headerSettings").empty();
                        $("#headerSettings").text("Настройки групп");
                        $("#contentSettings").empty();
                        $("#contentSettingsPlace").empty();
                        $("#userControls").empty();
                        loadGroupsTreeSingle("", "");
                    }
                    //Раздел Настройки по умолчанию
                    if ($("a", ui.newHeader).attr("code") == 6) {
                        mode = "";
                        loadDefaultSettings();
                    }
                }
            });

            resizeSettings();
        });

        $(window).resize(function () {
            resizeAllMaster();
            resizeSettings();
            $("#accordion").accordion("resize");
        });

        function resizeSettings() {
            if ($('#decision:visible').length > 0) {
                var h = $('#outputId').height() - $('#decision').height()-5;
                $('#outputId').height(h);
                $('#outputId-content').height(h);
            }
        }
    </script>  
    
    <script id="tmplGeneralSettings" type="text/x-jquery-tmpl">
        <tr>
            <td class="key" key="${Key}" style="font-size:12px;width:200px;">
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

    <script id="RemindMainLabels" type="text/x-jquery-tmpl">
        <table style="width:100%;">
        <tr>
            <td style="width:50%;"><label id="RemindLabel1"></label></td>
            <td style="width:50%;"><label id="RemindLabel2"></label></td>
        </tr>
        </table>
    </script>

    <script id="RemindContentControls" type="text/x-jquery-tmpl">
        <table id="RemindControlsTable">
        <tr>
            <td style="width:20%;">
            <div style="float:right"><label>Кому</label></div>
            </td>
            <td style="width:20%;">
                <select id="whomSelector">
                </select>
            </td>
            <td style="width:20%;">
            <div style="float:right"><label>Периодичность</label></div>
            </td>
            <td style="width:20%;">
                <select id="periodSelector">
                    <option value="1">По факту</option>
                    <option value="2">Каждый час</option>
                    <option value="3">Каждый день</option>
                    <option value="4">Каждый месяц</option>
                </select>
            </td>
            <td style="width:20%;">
                <div style="float:right"><input type="checkbox"/></div>
            </td>
            <td style="width:20%;">
                <label>Активно</label>
            </td>
        </tr>
        </table>
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
                    <input type="checkbox" id="checkbox{{html grID}}" cardType="{{html cardType}}" key="{{html grID}}"/>
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
                    <input id="nameinput{{html grID}}" value="{{html Name}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input id="commentinput{{html grID}}" value="{{html Comment}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <select id="groupSelector{{html grID}}" name="groupSelector" cardType="{{html cardType}}" card="{{html cardType}}" onchange="this.card=this.value;">
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

    <script id="tmplTabsContent" type="text/x-jquery-tmpl">
        <div id="tabs" style="background: transparent;">                
            <ul style="height:30px;">
    		    <li><asp:LinkButton ID="CurrentTab" runat="server" Text="Текущие напоминания" href="#tabs-1"/></li>
		        <li><asp:LinkButton ID="JournalTab" runat="server" Text="Журнал" href="#tabs-2"/></li>
	        </ul>
            <div id="tabs-1">
            </div>
            <div id="tabs-2">
                I am JournalTab
            </div>
        </div>
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
                   <input id="numberinput{{html grID}}" value="{{html Number}}" class="inputField-readonly input" readonly="readonly"/>
                    </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input id="nameinput{{html grID}}" value="{{html Name}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input id="commentinput{{html grID}}" value="{{html Comment}}" class="inputField-readonly input" readonly="readonly"/>
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

 <script id="tmplSingleDriverData" type="text/x-jquery-tmpl">
  <div id="tabs" style="background: transparent;">
    <ul style="height:30px;">
      <li>
        <asp:LinkButton ID="GeneralTab" runat="server" Text="Основные" href="#tabs-1"/>
      </li>
      <li>
        <asp:LinkButton ID="PrivateTab" runat="server" Text="Личные" href="#tabs-2"/>
      </li>
      <li>
        <asp:LinkButton ID="CardTab" runat="server" Text="Карта" href="#tabs-3"/>
      </li>
    </ul>
    <div id="tabs-1">
      <table id="contentTable1" style="border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
          border="0" cellpadding="0" cellspacing="0">
        <tbody id="" class="ui-widget-content wijmo-wijgrid-data">
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Фамилия</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="surnameinputSingle" value="{{html Surname}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Имя</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="nameinputSingle" value="{{html Name}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Отчество</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="patronymicinputSingle" value="{{html Patronymic}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Водительское удостоверение</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="licenseinputSingle" value="{{html License}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Кто выдал</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="licGiverinputSingle" value="{{html LicGiver}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">ТС</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="vehicleinputSingle" value="{{html Vehicle}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Комментарий</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="commentinputSingle" value="{{html Comment}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Группа</td>
            <td style="padding-left:50px;padding-top:10px;">
              <select id="groupSelectorSingle" name="groupSelector" group="{{html groupID}}" onchange="this.group=this.value;"></select>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div id="tabs-2">
      <table id="contentTable2" style="border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                     border="0" cellpadding="0" cellspacing="0">
        <tbody id="" class="ui-widget-content wijmo-wijgrid-data">
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Телефон моб.</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="phoneinputSingle" value="{{html Phone}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Дата рождения</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="birthdateinputSingle" value="{{html BirthDate}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Язык</td>
            <td style="padding-left:50px;padding-top:10px;">
              <select id="lang" style="width:170px;" langId="0" onchange="this.langId=this.value;">
                <option value="0">Русский</option>
                <option value="1">English</option>
              </select>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div id="tabs-3">
      <table id="contentTable3" style="border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                       border="0" cellpadding="0" cellspacing="0">
        <tbody id="" class="ui-widget-content wijmo-wijgrid-data">
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Номер карты</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="numberinputSingle" value="{{html Number}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Кто выдал</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="cardGiverinputSingle" value="{{html CardGiver}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Страна</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="countryinputSingle" value="{{html CardGiver}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Дата выдачи</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="givenDateinputSingle" value="{{html GivenDate}}" class="datepicker"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Действительна с/по</td>
            <td style="padding-left:50px;padding-top:10px;">
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Начало</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="fromDateinputSingle" value="{{html FromDate}}" class="datepicker"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Окончание</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="toDateinputSingle" value="{{html ToDate}}" class="datepicker"/>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</script>

    <script id="tmplSingleVehicleData" type="text/x-jquery-tmpl">
  <div id="tabs" style="background: transparent;">
    <ul style="height:30px;">
      <li>
        <asp:LinkButton ID="GeneralVehicleTab" runat="server" Text="Основные" href="#tabs-1"/>
      </li>
      <li>
        <asp:LinkButton ID="AdditionalTab" runat="server" Text="Дополнительные" href="#tabs-2"/>
      </li>
      <li>
        <asp:LinkButton ID="EquipmentTab" runat="server" Text="Оборудование" href="#tabs-3"/>
      </li>
      <li>
        <asp:LinkButton ID="CoefficientTab" runat="server" Text="Коэффициенты" href="#tabs-4"/>
      </li>
    </ul>
    <div id="tabs-1">
      <table id="contentTable1" style="border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
         border="0" cellpadding="0" cellspacing="0">
        <tbody id="" class="ui-widget-content wijmo-wijgrid-data">
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">VIN</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="numberinputSingle" value="{{html Number}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Гос. Номер</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="nameinputSingle" value="{{html Name}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Гаражный номер</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="garageinputSingle" value="{{html GarageNumber}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Год выпуска</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="makeYearinputSingle" value="{{html MakeYear}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
           <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Комментарий</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="commentinputSingle" value="{{html Comment}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;padding-top:10px;">Группа</td>
            <td style="padding-left:50px;padding-top:10px;">
              <select id="groupSelectorSingle" name="groupSelector" group="{{html groupID}}" onchange="this.group=this.value;"></select>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div id="tabs-2">
          <table id="contentTable2" style="border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
         border="0" cellpadding="0" cellspacing="0">
        <tbody id="" class="ui-widget-content wijmo-wijgrid-data">
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Бак 1</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="tank1inputSingle" value="{{html Tank1}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Бак 2</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="tank2inputSingle" value="{{html Tank2}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Грузоподъемность</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="capacityinputSingle" value="{{html Capacity}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Тип топлива</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="fuelTypeinputSingle" value="{{html FuelType}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">ТО 1</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="to1inputSingle" value="{{html TO1}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">ТО 2</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="to2inputSingle" value="{{html TO2}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div id="tabs-3">
     <table id="contentTable3" style="border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
         border="0" cellpadding="0" cellspacing="0">
        <tbody id="" class="ui-widget-content wijmo-wijgrid-data">
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Тип оборудования</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="eqtypeinputSingle" value="{{html EquipmentType}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Серийный номер</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="serialinputSingle" value="{{html Serial}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Последнее считывание</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="lastReadDateinputSingle" value="{{html LastReadDate}}" class="datepicker"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Причина последней калибровки</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="calibrReasoninputSingle" value="{{html CalibrReason}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Кто калибровал</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="calibratorinputSingle" value="{{html Calibrator}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Номер карты калибровщика</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="calibratorCardinputSingle" value="{{html CalibratorCard}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
         <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Следующая калибровка</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="nextCalibrinputSingle" value="{{html NextCalibrDate}}" class="datepicker"/>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div id="tabs-4">
    <table id="contentTable4" style="border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
         border="0" cellpadding="0" cellspacing="0">
        <tbody id="" class="ui-widget-content wijmo-wijgrid-data">
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Номинальные обороты</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="turninputSingle" value="{{html Turns}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Максимальная скорость</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="maxVelocityinputSingle" value="{{html MaxVelocity}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Маневрирование</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="manevringinputSingle" value="{{html Manevring}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Город</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="cityinputSingle" value="{{html City}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Магистраль</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="magistralinputSingle" value="{{html Magistral}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Номинальный расход</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="consumptioninputSingle" value="{{html Consumption}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Холодный старт</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="coldstartinputSingle" value="{{html ColdStart}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
          <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Горячий стоп</td>
            <td style="padding-left:50px;padding-top:10px;">
              <input id="hotstopinputSingle" value="{{html HotStop}}" class="inputField-readonly input" readonly="readonly"/>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</script>

    <script id="tmplSingleGroupData" type="text/x-jquery-tmpl">
        <table id="contentTable" style="border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
                border="0" cellpadding="0" cellspacing="0">
                <tbody id="" class="ui-widget-content wijmo-wijgrid-data">
            <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Название группы</td><td style="padding-left:50px;padding-top:10px;"><input id="nameinputSingle" value="{{html Name}}" class="inputField-readonly input" readonly="readonly"/></td>
            </tr>
            <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Комментарий</td><td style="padding-left:50px;padding-top:10px;"><input id="commentinputSingle" value="{{html Comment}}" class="inputField-readonly input" readonly="readonly"/></td>
            </tr>
            <tr style="background-color:#eee;">
            <td class="key" style="font-size:12px;width:200px;padding-top:10px;">Тип группы</td>
            <td style="padding-left:50px;padding-top:10px;">
            <select id="groupSelectorSingle" name="groupSelector" card="{{html cardType}}" onchange="this.card=this.value;">
            </select>
            </td>
            </tr>
            </tbody>
            </table>
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
                    <input value="{{html CriteriaName}}" id="CriteriaName{{html keyID}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html MeasureName}}" id="MeasureName{{html keyID}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html MinValue}}" id="MinValue{{html keyID}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html MaxValue}}" id="MaxValue{{html keyID}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html CriteriaNote}}" id="CriteriaNote{{html keyID}}" class="inputField-readonly input" readonly="readonly"/>
                </div>
            </td>
        </tr>
    </script>

    <script id="tmplRemindTable" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell" style="margin-left:5px;">
                    <input type="checkbox" key="{{html id}}" name="selectCheckbox"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <select id="userSelector{{html id}}" name="userSelector" user="{{html userId}}" onchange="this.user=this.value;">
                    </select>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html sourceName}}" id="source{{html id}}" sourceType="{{html sourceType}}" key="{{html sourceId}}" class="inputField-readonly" readonly="readonly" style="width:70%;"/>
                    <img id="preview{{html id}}" src="../css/icons/24x24/search.png" alt="Load Driver" width="24" height="24" key="{{html id}}" style="display:none;cursor:pointer;float:right;"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <select id="periodSelector{{html id}}" name="periodSelector" period="{{html periodType}}" onchange="this.period=this.value;">
                    </select>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <input value="{{html date}}" id="lastDate{{html id}}" class="inputField-readonly" readonly="readonly"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <select id="typeSelector{{html id}}" name="typeSelector" typeSel="{{html type}}" onchange="this.typeSel=this.value;">
                    </select>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                {{if active==1}}
                     <center>
                          <input type="checkbox" checked="true" disabled="true" id="active{{html id}}" class="inputField-readonly" readonly="readonly"/></center>
                {{else}}
                <center>
                    <input type="checkbox" disabled="true" id="active{{html id}}" class="inputField-readonly" readonly="readonly"/>
                </center>
                {{/if}}
                </div>
            </td>
        </tr>
    </script>

    <script id="NewRemind" type="text/x-jquery-tmpl">
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow" style="height:30px;">
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell" style="margin-left:5px;">
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <select id="userSelectorNew" name="userSelector" user="29" onchange="this.user=this.value;">
                    </select>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <input value="Init Organization" id="sourceNew" key="1" sourceType="2" class="inputField-readonly" readonly="readonly" style="width:70%;"/>
                    <img id="previewNew" style="cursor: pointer;float:right;" src="../css/icons/24x24/search.png" alt="Load Driver" width="24" height="24"/>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <select id="periodSelectorNew" name="periodSelector" period="1" onchange="this.period=this.value;">
                    </select>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <select id="typeSelectorNew" name="typeSelector" typeSel="1" onchange="this.typeSel=this.value;">
                    </select>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string wijmo-wijgrid-cell-border-bottom wijmo-wijgrid-cell-border-right wijmo-wijgrid-cell">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                        <input type="checkbox" checked="true" id="activeNew"/>
                    </center>
                </div>
            </td>
        </tr>
    </script>

    <script id="tmplDriverTree" type="text/x-jquery-tmpl">
        <li class="folder" id="OrgLI" name="${OrgName}" key=${OrgId} li_type="2"><a><span key=${OrgId} type="2">${OrgName}</span></a>
        <ul>
            {{each groups}}
            <li class="file" key=${GroupId} li_type="1"><a><span key=${GroupId} type="1">${GroupName}</span></a>
                <ul>
                    {{each values}}
                    <li class="file" key=${Key} li_type="0"><a><span key=${Key} type="0">${Value}</span></a></li>
                    {{/each}}
                </ul>
            </li>
            {{/each}}
        </ul>
        </li>
    </script>

    

    <div id="accordion" style="width: 5">
        <h3><asp:LinkButton CausesValidation="false" runat="server" PostBackUrl="#" Text="Организация"/></h3>
        <div>

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
        
        <div>
            <h3><asp:LinkButton ID="AccordionHeader5_Groups" CausesValidation="false" runat="server" PostBackUrl="#" Text="Группы" code="5"/></h3>
            <div>

            <div>
                <ul id="GroupsTreeSingle">
                </ul>
            </div>

            </div>

        </div>
        <div>
            <h3><asp:LinkButton ID="AccordionHeader3_Drivers" CausesValidation="false" runat="server" PostBackUrl="#" Text="Водители" code="3"/></h3>
            <div>

            <div>
                <ul id="DriversTreeSingle">
                </ul>
            </div>

            </div>

        </div>

        <div>
            <h3><asp:LinkButton ID="AccordionHeader4_Vehicles" CausesValidation="false" runat="server" PostBackUrl="#" Text="Транспортные средства" code="4"/></h3>
            <div>

            <div>
                <ul id="VehiclesTreeSingle">
                </ul>
            </div>

            </div>

        </div>
        <div id="reminders">
            <h3><asp:LinkButton ID="AccordionHeader2_Reminders" CausesValidation="false" runat="server" PostBackUrl="#" Text="Напоминания"/></h3>
            <div>

            <div style="margin-top:10px;">
                <center>
                    Данный раздел позволяет создавать и редактировать напоминания различных типов и периодичности для контроля за группами водителей или конкретными водителями.
                    <br/>
                    <br/>
                    Выбранные вами напоминания будут автоматически формироваться и отправляться на e-mail адресата с заданной периодичностью.
                </center>
            </div>

            <div id="choosedialog" title="Выбор источника" style="display: none;">
	            <p>Выберите водителя или группу водителей:</p>
                <div>
                    <ul id="DriversTree">
                    </ul>
                </div>
            </div>
            </div>

        </div>

        <div>
            <h3><asp:LinkButton ID="AccordionHeader6_Default" CausesValidation="false" runat="server" PostBackUrl="#" Text="Настройки по умолчанию" code="6"/></h3>
            <div>

            <div style="margin-top:10px;">
                <center>
                    Данный раздел позволяет вам редактировать настройки по умолчанию.
                </center>
            </div>
            </div>

        </div>
        

                
    </div>
</asp:Content>
<asp:Content ID="ChoisesContent" ContentPlaceHolderID="MainConditions_PlaceHolder" runat="server">
    <div id="headerSettings">
        Общие настройки
    </div>

</asp:Content>
<asp:Content ID="DataContent" ContentPlaceHolderID="Reports_PlaceHolder" runat="server">
    <div>
    
        <table cellpadding="2" style="margin-left:30px; width: 800px">
            <tbody id="contentSettings">
            </tbody>
        </table>
    </div>
    <div id="contentSettingsPlace">
    </div>
    <asp:UpdatePanel ID="DataContentUpdatePanel" runat="server" UpdateMode="Always">
    <ContentTemplate>

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

</asp:Content>