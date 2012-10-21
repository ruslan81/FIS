<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true"
    CodeFile="Reports.aspx.cs" Inherits="Administrator_Report" %>

<%@ Register Src="../UserControlsForAll/BlueButton.ascx" TagName="BlueButton" TagPrefix="uc2" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <link type="text/css" href="../css/custom-theme/jquery.wijmo.wijcombobox.css" rel="stylesheet" />
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>
    <script src="../js/custom/Report.js" type="text/javascript"></script>
    <script src="../js/jquery.wijmo.wijcombobox.js" type="text/javascript"></script>
    <script src="../js/infobubble.js" type="text/javascript"></script>
    <script src="../js/date.js" type="text/javascript"></script>
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="AccordionContent" ContentPlaceHolderID="VerticalOutlookMenu_PlaceHolder"
    runat="server">
    <link type="text/css" href="../js/custom/jquery.search-tree.1.0.1.css" rel="stylesheet" />
    <script src="../js/custom/jquery.search-tree.1.0.1.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.datepicker-ru.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/custom/Calendar.js"></script>
    
    <script type="text/javascript">
        //график
        var chart = null;
        //данные
        var plfData = null;
        //текущая закладка
        var currentTab = 0;
        //виды отчетов
        var reportTypes = null;
        //карта
        var map = null;
        //путь на карте
        var flightPath = null;
        //текущая последняя точка пути на карте
        var currentLastPoint = 0;
        //маркеры на карте
        var markers = [];
        //координаты маркеров
        var neighborhoods = [];
        //итератор для маркеров
        var iterator = 0;
        //иконки для маркеров на карте
        var markerImages = [];
        //сообщение для маркера начала
        var infoWindowBegin = "";
        //сообщение для маркера конца
        var infoWindowFinish = "";
        //play or pause flag
        var isPlay = false;
        //!important: slider "play-path" control created flag, set to false if controls destroyed
        var isSliderExist = false;
        //play or pause flag
        var selectedReportType = "None";
        var selectedCardID = "None";
        var reportFormat = "None";
        var cardType = "";

        //run on page load
        $(function () {

            $("#accordion").accordion({
                change: function (event, ui) {
                    cardType = "";
                    //Раздел PLF Файлы (Датчики)
                    if ($("a", ui.newHeader).text() == "PLF Файлы (Датчики)") {
                        destroyPeriodControls();
                        destroyPLFTab();
                        $("#statusPanel").show();
                        createPLFTab();
                    }

                    //Раздел Транспортные средства старый
                    if ($("a", ui.newHeader).attr("code") == 1) {
                        destroyPeriodControls();
                        $("#statusPanel").show();
                        destroyPLFTab();
                        createVehicles();
                    }

                    //Раздел Водители
                    if ($("a", ui.newHeader).attr("code") == 3) {
                        cardType = "Driver";
                        destroyPLFTab();
                        loadDriverTree();
                        destroyPeriodControls();
                        createPeriodControls();
                        createPLFTab();
                        //destroyPLFTab();
                        //createVehicles();
                    }

                    //Раздел транспортные средства
                    if ($("a", ui.newHeader).attr("code") == 4) {
                        cardType = "Vehicle";
                        destroyPLFTab();
                        loadVehicleTree();
                        destroyPeriodControls();
                        createPeriodControls();
                        createDDDTab();

                        //destroyPLFTab();
                        createVehicles();
                    }
                }
            });

            createVehicles();

            $("#dialog").dialog();
            $("#dialog2").dialog({ autoOpen: false, draggable: true, resizable: true });
        });

        $(window).resize(function () {
            resizeAllMaster();
            $("#accordion").accordion("resize");
            resizeReports();
        });

        function resizeReports() {
            var outHeight = $("#main-content").height() - 155 + 37;
            $("#outputId").height(outHeight);
            $("#outputId-content").height(outHeight);
            try {
                document.getElementById('report-tabs').style.height = outHeight - 6 + "px";
                document.getElementById('report').style.height = outHeight - 7 - 50 + "px";
                document.getElementById('chart').style.height = outHeight - 7 - 50 + "px";
                document.getElementById('chart').style.width = $('#outputId').width() - 35 + "px";
                document.getElementById('map').style.height = outHeight - 7 - 50 - 20 + "px";
                document.getElementById('map').style.width = $('#outputId').width() - 35 + "px";
            } catch (e) {

            }
        }

        function showModal() {
            $find('ShowModal').show();
        }

        function acordionIndexSwitch(accIndex) {
            document.getElementById("<% =AccordionSelectedPane.ClientID %>").value = accIndex;
        }
    </script>
    <script id="tmplPLFFilesTab" type="text/x-jquery-tmpl">
        <ul>
            <li><a href="#tabs-1">Отчеты</a></li>
		    <li><a href="#tabs-2">Графики</a></li>
            <li><a href="#tabs-3">Карта</a></li>
	    </ul>
        <div id="tabs-1">
            <center>
            <div id="report" style="overflow: auto;">
            </div>
            </center>
        </div>
        <div id="tabs-2">
            <div id="chart" style="overflow: hidden;">
            </div>
        </div>
        <div id="tabs-3">
            <div style="float:right;">
                <button id="playPath" title="Проиграть маршрут" style="display:none;"></button>
            </div>
            <div id="sliderWrapper" style="display:none;">
                <div id="slider"></div>
            </div>
            <div id="map" style="overflow: hidden;">
            </div>
        </div>
    </script>
    <script id="tmplPLFFilesTree" type="text/x-jquery-tmpl">
        <li class="folder"><a><span key="${ID}">${Name}</span></a>
            <ul>
                {{each PLFItems}}
                    <li class="file"><a><span key="None">${Vehicle} / ${DeviceID}</span></a>
                        <ul>
                            {{each PLFs}}
                                <li class="file"><a><span cardID="${ID}" driver="${Name}" driverNumber="${Number}" device="${Vehicle} / ${DeviceID}" key=${Key}>${Value}</span></a></li>
                            {{/each}}
                        </ul>
                    </li>
                {{/each}}
            </ul>
        </li>
    </script>
    <script id="tmplNoPLFFile" type="text/x-jquery-tmpl">
        <div style="color:#a60000;font-weight:bold;text-align:center;">
            Выберите интересующий вас PLF файл
        </div>
    </script>
    <script id="tmplLoading" type="text/x-jquery-tmpl">
        <center>
            <div style="padding-top:20px">
                <div class="loading-icon">
                </div>
            </div>
        </center>
    </script>
    <script id="tmplChoosePLFFile" type="text/x-jquery-tmpl">
        <div style="float:left">
            <div class="item-detail">
                - водитель: <b>${Driver}</b>
                <br/>
                - транспортное средство: <b>${Device}</b>
                <br/>
                - период: <b>${Period}</b>
            </div>
        </div>

        <div style="float:right;">
            <form method="post" action="Download.aspx">
                <div style="float:right;">
                    <input type="hidden" name="type" value="${type}"/>
                    <input type="hidden" name="CardID" value="${CardID}"/>
                    <input type="hidden" name="PLFID" value="${PLFID}"/>
                    <input type="hidden" name="UserName" value="${UserName}"/>
                    <input type="hidden" id="format" name="Format" value="pdf"/>
                    <input type="hidden" id="reportType" name="reportType" value=""/>
                    <input id="getReport" value="Получить отчет" type="submit" title="Скачать pdf-файл"/>
                </div>
            </form>
        </div>

        <div style="float:right;margin-right:15px;margin-left:10px;">
            <select id="formatChooser"> 
                <option value="pdf">pdf</option>
                <option value="html">html</option>
                <option value="rtf">rtf</option>
                <option value="png">png</option>
            </select>
        </div>

        <div style="float:right;margin-top:2px;">
            выберите формат отчета:
        </div>

        <div style="float:right;margin-right:15px;margin-left:10px;">
            <select id="reportChooser"> 
            </select>
        </div>

        <div style="float:right;margin-top:2px;">
            выберите вид отчета:
        </div>
        
    </script>
    <script id="tmplLoadReportControls" type="text/x-jquery-tmpl">        
    <div id="LoadReportControls" style="margin-top:-40px;">
        <div style="float:right;">
            <form method="post" action="Download.aspx">
                <div style="float:right;">
                    <input type="hidden" name="type" value="${type}"/>
                    <input type="hidden" name="CardID" value="${CardID}"/>
                    <input type="hidden" name="StartDate" value="${StartDate}"/>
                    <input type="hidden" name="EndDate" value="${EndDate}"/>
                    <input type="hidden" name="UserName" value="${UserName}"/>
                    <input type="hidden" id="format" name="Format" value="pdf"/>
                    <input type="hidden" id="reportType" name="reportType" value="${ReportType}"/>
                    <input id="getReport" value="Получить отчет" type="submit" title="Скачать pdf-файл"/>
                </div>
            </form>
        </div>

        <div style="float:right;margin-right:15px;margin-left:10px;">
            <select id="formatChooser"> 
                <option value="pdf">pdf</option>
                <option value="html">html</option>
                <option value="rtf">rtf</option>
                <option value="png">png</option>
            </select>
        </div>

        <div style="float:right;margin-top:2px;">
            выберите формат отчета:
        </div>
    </div>
    </script>
    <script id="tmplSelect" type="text/x-jquery-tmpl">
        {{each filenames}}
            <option value="${$value}">${$value}</option>
        {{/each}}
    </script>
    <!------------------------------------------------>
    <script id="tmplVehiclesTab" type="text/x-jquery-tmpl">
        <center>
            <div id="report" style="overflow: auto;">
            </div>
        </center>
    </script>
    <script id="tmplVehiclesTree" type="text/x-jquery-tmpl">
        <li class="folder"><a><span key="${VehicleID}">${Name}</span></a>
            <ul>
                {{each Files}}
                    <li class="file">
                        <a>
                            <span dataBlockID="${DataBlockID}" name="${Name}" file="${FileName}" code="${VehicleVin}">
                                (${VehicleCardPeriodBegin} - ${VehicleCardPeriodEnd})
                            </span>
                        </a>
                    </li>
                {{/each}}
            </ul>
        </li>
    </script>
    <script id="tmplNoVehicles" type="text/x-jquery-tmpl">
        <div style="color:#a60000;font-weight:bold;text-align:center;">
            Выберите интересующий вас файл
        </div>
    </script>
    <script id="tmplChooseDDDFile" type="text/x-jquery-tmpl">
        <div style="float:left">
            <div class="item-detail">
                - код ТС: <b>${VehicleVin}</b>
                <br/>
                - регистрационный номер: <b>${Name}</b>
                <br/>
                - период: <b>${Period}</b>
            </div>
        </div>

        <div style="float:right;">
            <form method="post" action="Download.aspx">
                <div style="float:right;">
                    <input type="hidden" name="type" value="${type}"/>
                    <input type="hidden" name="DataBlockID" value="${DataBlockID}"/>
                    <input type="hidden" name="UserName" value="${UserName}"/>
                    <input type="hidden" id="format" name="Format" value="pdf"/>
                    <input type="hidden" id="reportType" name="reportType" value=""/>
                    <input id="getReport" value="Получить отчет" type="submit" title="Скачать pdf-файл"/>
                </div>
            </form>
        </div>

        <div style="float:right;margin-right:15px;margin-left:10px;">
            <select id="formatChooser"> 
                <option value="pdf">pdf</option>
                <option value="html">html</option>
                <option value="rtf">rtf</option>
                <option value="png">png</option>
            </select>
        </div>

        <div style="float:right;margin-top:2px;">
            выберите формат отчета:
        </div>

        <div style="float:right;margin-right:15px;margin-left:10px;">
            <select id="reportChooser"> 
            </select>
        </div>

        <div style="float:right;margin-top:2px;">
            выберите вид отчета:
        </div>
        
    </script>
    <asp:HiddenField ID="AccordionSelectedPane" Visible="true" runat="server" Value="0" />
    <!--Боковая панель-->
    <div id="accordion">
        <!--Транспортные средства-->
        <h3>
            <asp:LinkButton ID="AccordionHeader4_VehiclesGroup" runat="server" PostBackUrl="#"
                Text="Транспортные средства (по старому стилю)" code="1" />
        </h3>
        <div>
            <!--Дерево-->
            <div>
                <ul id="vehiclesTree">
                </ul>
            </div>
        </div>
        <!--Раздел PLF Файлы (Датчики)-->
        <h3>
            <asp:LinkButton ID="AccordionHeader5_PLF" runat="server" PostBackUrl="#" Text="PLF Файлы (Датчики)"
                code="2" />
        </h3>
        <div>
            <!--Дерево-->
            <div>
                <ul id="PLFFilesTree">
                </ul>
            </div>
        </div>
        <!--Водители-->
        <h3>
            <asp:LinkButton ID="AccordionDrivers" runat="server" PostBackUrl="#" Text="Водители"
                code="3" />
        </h3>
        <div>
            <!--Дерево-->
            <div>
                <ul id="DriversTree">
                </ul>
            </div>
            <div id="DriversTreePlace">
                
            </div>
            
        </div>
        <!--Транспортные средства-->
        <h3>
            <asp:LinkButton ID="AccordionVehicles" runat="server" PostBackUrl="#" Text="Транспортные средства"
                code="4" />
        </h3>
        <div>
            <!--Дерево-->
            <div>
                <ul id="VehiclesTree">
                </ul>
            </div>
            <div id="VehiclesTreePlace">
                
            </div>
        </div>
    </div>
    <!--Конец боковой панели-->
    <script id="tmplGroupTree" type="text/x-jquery-tmpl">
        <li class="folder"><a><span key="None">${OrgName}</span></a>
        <ul>
            {{each groups}}
            <li class="file"><a><span key="None">${GroupName}</span></a>
                <ul>
                    {{each values}}
                    <li class="file"><a><span key="${Key}">${Value}</span></a></li>
                    {{/each}}
                </ul>
                </li>
            {{/each}}
        </ul>
        </li>
    </script>
    <script id="tmplTreeItem" type="text/x-jquery-tmpl">
        <li class="file"><a><span key="${Key}">${Value}</span></a></li>
    </script>
    <script id="tmplReportTree" type="text/x-jquery-tmpl">
        
                <ul id="ReportTree">
                    <li class="folder"><a><span key="None">Отчеты</span></a>
                        <ul>
                            <li class="file"><a><span key="None">PLF</span></a>
                                <ul id="PLFSubtree">
                                </ul>
                            </li>
                            <li class="file"><a><span key="None">DDD</span></a>
                                <ul id="DDDSubtree">
                                </ul>
                            </li>
                        </ul>
                    </li>
                </ul>
    </script>
</asp:Content>
<asp:Content ID="ChoisesContent" ContentPlaceHolderID="MainConditions_PlaceHolder"
    runat="server">
    <div id="statusPanel">
    </div>
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
</asp:Content>
<asp:Content ID="DataContent" ContentPlaceHolderID="Reports_PlaceHolder" runat="server">
    <!--Центральная панель-->
    <div id="report-tabs">
    </div>
</asp:Content>
<asp:Content ID="DecisionContent1" ContentPlaceHolderID="Decision_PlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Bottom_PlaceHolder" runat="server">
</asp:Content>
