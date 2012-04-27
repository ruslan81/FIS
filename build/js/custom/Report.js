//Создать закладку PLP Файлы
function createPLFTab() {
    $("#report-tabs").empty();
    $("#tmplPLFFilesTab").tmpl("").appendTo("#report-tabs");
    chart = null;
    plfData = null;
    currentTab = 0;
    map = null;

    loadPLFFilesTree();
    $("#report-tabs").tabs({ show: function (e, ui) {
        if (ui.index == 0) {
            currentTab = 0;
        }

        if (ui.index == 1) {
            currentTab = 1;
            if (chart == null) {
                if (plfData != null) {
                    createCharts(plfData);
                }
            }
        }

        if (ui.index == 2) {
            currentTab = 2;
            if (map == null) {
                if (plfData != null) {
                    createMap(plfData);
                }
            }
        }
    }
    });

    resizeReports();
}

function destroyPLFTab() {
    $("#report-tabs").empty();

    if (chart != null) {
        chart.destroy();
    }
    chart = null;

    if (map != null) {
        delete (map);
    }
    map = null;

    plfData = null;
    currentTab = 0;
}

//Загрузить элементы дерева Водителей
function loadPLFFilesTree() {
    $("#statusPanel").empty();
    $("#tmplNoPLFFile").tmpl("").appendTo("#statusPanel");

    //destroy a tree
    $("#PLFFilesTree").wijtree("destroy");
    $("#PLFFilesTree").empty();

    if (chart != null) {
        chart.destroy();
    }
    chart = null;

    $("#report").empty();
    $("#chart").empty();

    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Reports.aspx/GetPLFFilesTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#tmplPLFFilesTree").tmpl(response.d).appendTo("#PLFFilesTree");

            //builds a tree
            $("#PLFFilesTree").wijtree();

            //sets a listener to node selection
            $("#PLFFilesTree").wijtree({ selectedNodeChanged: function (e, data) {
                onPLFFilesNodeSelected(e, data);
            }
            });
            
        }
    });
}

function onPLFFilesNodeSelected(e, data) {
    var isSelected = $("div", data.element).attr("aria-selected");
    var plf = $("a span", data.element).attr("key");
    var cardID = $("a span", data.element).attr("cardid");
    var driver = $("a span", data.element).attr("driver");
    var driverNumber = $("a span", data.element).attr("drivernumber");
    var device = $("a span", data.element).attr("device");
    var period = $("a span", data.element).text();

    if (isSelected == "true") {
        if (driver != null) {
            $("#statusPanel").empty();
            $("#tmplChoosePLFFile").tmpl({ 'Driver': driver+" / "+driverNumber, 'Device': device, 'Period': period,
                'type': 'GetReport','CardID': cardID, 'PLFID': plf, 'UserName': $.cookie("CURRENT_USERNAME")}).appendTo("#statusPanel");

            $("#getReport").button();
            $("#formatChooser").wijcombobox({ changed: function (e, item) {
                var format = $("#formatChooser").attr("selectedIndex");
                if (format == "0") {
                    $("#format").attr("value","pdf");
                }
                if (format == "1") {
                    $("#format").attr("value", "html");
                }
                if (format == "2") {
                    $("#format").attr("value", "rtf");
                }
                if (format == "3") {
                    $("#format").attr("value", "png");
                }
            },
                isEditable: false
            });

            reportTypes = null;

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Reports.aspx/GetReportTypes",
                data: [],
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    reportTypes = res;
                    $("#tmplSelect").tmpl({ 'filenames': reportTypes.d }).appendTo("#reportChooser");

                    $("#reportChooser").wijcombobox({ changed: function (e, item) {
                        var reportType = $("#reportChooser").attr("selectedIndex");

                        $("#reportType").attr("value", reportTypes.d[reportType]);

                        $("#report").empty();
                        $("#tmplLoadingPLFFile").tmpl({}).appendTo("#report");

                        $.ajax({
                            type: "POST",
                            //Page Name (in which the method should be called) and method name
                            url: "Reports.aspx/GetReport",
                            data: "{'CardID':'" + cardID + "', 'PLFID':'" + plf + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "', 'ReportType':'" + ((reportTypes == null) ? "" : reportTypes.d[$("#reportChooser").attr("selectedIndex")]) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                $("#report").empty();

                                $('#report').html(result.d.report);
                            }
                        });
                    },
                        isEditable: false
                    });

                    //default select general report
                    var defTypeReport = "FullReport";
                    for (var i = 0; i < reportTypes.d.length; i++) {
                        if (reportTypes.d[i] == defTypeReport) {
                            $("#reportChooser").wijcombobox({ selectedIndex: i });
                            $("#reportChooser").attr("selectedIndex", i);
                            break;
                        }
                    }
                }
            });

            $("#report").empty();
            $("#tmplLoadingPLFFile").tmpl({}).appendTo("#report");

            if (chart != null) {
                chart.destroy();
            }
            chart = null;
            $("#chart").empty();
            $("#tmplLoadingPLFFile").tmpl({}).appendTo("#chart");

            if (map != null) {
                delete (map);
            }
            map = null;
            $("#map").empty();
            $("#tmplLoadingPLFFile").tmpl({}).appendTo("#map");
            $("#slider").slider("disable");
            $("#slider").slider({ value: 0 });
            $("#playPath").button("disable");
            isPlay = false;

            plfData = null;

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Reports.aspx/GetReport",
                data: "{'CardID':'" + cardID + "', 'PLFID':'" + plf + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "', 'ReportType':'" + ((reportTypes == null) ? "" : reportTypes.d[$("#reportChooser").attr("selectedIndex")]) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $("#report").empty();
                    $("#chart").empty();
                    $("#map").empty();

                    $('#report').html(result.d.report);

                    plfData = result;

                    if (currentTab == 1) {
                        createCharts(plfData);
                    }

                    if (currentTab == 2) {
                        createMap(plfData);
                    }
                }
            });
        } else {
            $("#statusPanel").empty();
            $("#tmplNoPLFFile").tmpl("").appendTo("#statusPanel");
            $("#report").empty();
            $("#chart").empty();
            $("#map").empty();
            $("#slider").slider("disable");
            $("#playPath").button("disable");
            isPlay = false;
        }
    } else {
        $("#statusPanel").empty();
        $("#tmplNoPLFFile").tmpl("").appendTo("#statusPanel");
        $("#report").empty();
        $("#chart").empty();
        $("#map").empty();
        $("#slider").slider("disable");
        $("#playPath").button("disable");
        isPlay = false;
    }
}

function createCharts(result) {
    chart = new Highcharts.Chart({
        chart: {
            renderTo: 'chart',
            zoomType: 'x',
            marginLeft: 35
        },
        title: {
            text: result.d.period
        },
        subtitle: {
            text: ' '
        },
        xAxis: {
            type: 'datetime',
            maxZoom: 1000 * 60 * 10,
            gridLineWidth: 1,
            title: {
                text: null
            }
        },
        yAxis: [
                    {
                        title: {
                            text: null
                        },
                        opposite: true
                    },
                    {
                        title: {
                            text: null
                        }
                    },
                    {
                        title: {
                            text: null
                        },
                        opposite: true
                    },
                    {
                        title: {
                            text: null
                        },
                        opposite: true
                    }
                ],
        tooltip: {

            shared: true,
            crosshairs: true
        },
        legend: {
            align: 'left',
            verticalAlign: 'top',
            y: 20,
            x: 20,
            floating: true,
            borderWidth: 0
        },
        plotOptions: {
            area: {
                fillOpacity: 0.01,
                lineWidth: 1,
                marker: {
                    enabled: false,
                    states: {
                        hover: {
                            enabled: true,
                            radius: 5
                        }
                    }
                },
                shadow: false,
                states: {
                    hover: {
                        lineWidth: 1
                    }
                }
            },
            marker: {
                lineWidth: 2
            }
        },
        credits: {
            enabled: true,
            align: 'right',
            verticalAlign: 'bottom'
        },
        exporting: {
            enabled: false
        },
        series: []
    });

    var data = [];
    for (var i = 0; i < result.d.time.length; i++) {
        var x = result.d.time[i];
        var y = result.d.speed[i];
        data.push({
            x: x,
            y: y
        });
    }
    chart.addSeries({
        name: 'Скорость',
        type: 'area',
        yAxis: 0,
        data: data
    });

    data = [];
    for (var i = 0; i < result.d.time.length; i++) {
        var x = result.d.time[i];
        var y = result.d.voltage[i];
        data.push({
            x: x,
            y: y
        });
    }
    chart.addSeries({
        name: 'Напряжение',
        type: 'area',
        yAxis: 1,
        data: data
    });

    data = [];
    for (var i = 0; i < result.d.time.length; i++) {
        var x = result.d.time[i];
        var y = result.d.rpm[i];
        data.push({
            x: x,
            y: y
        });
    }
    chart.addSeries({
        name: 'RPM',
        type: 'area',
        yAxis: 2,
        data: data
    });

    data = [];
    for (var i = 0; i < result.d.time.length; i++) {
        var x = result.d.time[i];
        var y = result.d.fuel[i];
        data.push({
            x: x,
            y: y
        });
    }
    chart.addSeries({
        name: 'Уровень топлива',
        type: 'area',
        yAxis: 3,
        data: data
    });

    resizeReports();
}

function createMap(result) {
    $("#sliderWrapper").show();
    $("#playPath").show();
    resizeReports();

    //create slider and set event listener
    $("#slider").slider({ min: 0, value: 0, max: result.d.lat.length - 1 });
    if (!isSliderExist) {
        $("#slider").bind("slidechange", function (event, ui) {
            //return if no data
            if (plfData == null || map == null) {
                return;
            }

            var diff = currentLastPoint - ui.value;

            var path = flightPath.getPath();
            for (var i = 1; i <= Math.abs(diff); i++) {
                if (diff > 0) {
                    path.pop();
                } else {
                    path.push(new google.maps.LatLng(plfData.d.lat[currentLastPoint + i],
                    plfData.d.lng[currentLastPoint + i]));
                }
            }
            currentLastPoint = ui.value;

            //передвигаем карту в конечную точку
            map.panTo(new google.maps.LatLng(plfData.d.lat[currentLastPoint],
                    plfData.d.lng[currentLastPoint]));
            //передвигаем маркер в конечную точку
            markers[1].setPosition(new google.maps.LatLng(plfData.d.lat[currentLastPoint],
                    plfData.d.lng[currentLastPoint]));

            infoWindowFinish.setContent(
            "<div class='infowindow'>" +
            "<span style='font-size:13px'><b>Конечная точка</b></span><br/>" +
            "Широта: " + plfData.d.lat[currentLastPoint] + "<br/>" +
            "Долгота: " + plfData.d.lng[currentLastPoint] + "<br/>" +
            "Время: <b>" + date2String(new Date(plfData.d.time[currentLastPoint])) + "</b><br/>" +
            "</div>"
        );

            map.setZoom(13);
        });
    }

    currentLastPoint = result.d.lat.length - 1;

    //play path
    $("#slider").slider("enable");
    $("#playPath").button("enable");

    isPlay = false;

    $("#playPath").button({ icons: 
        {
            primary: "ui-icon-play"
        }
    });
    if (!isSliderExist) {
        $("#playPath").click(function () {
            if (!isPlay) {
                isPlay = true;
                playPath();
                //change button icon
                $("#playPath").button({ icons:
                {
                    primary: "ui-icon-pause"
                }
                });

            } else {
                isPlay = false;
                //change button icon
                $("#playPath").button({ icons:
                {
                    primary: "ui-icon-play"
                }
                });
            }
            return false;
        });
    }

    isSliderExist = true;

    //create path
    var bounds = new google.maps.LatLngBounds();
    var flightPlanCoordinates = [];
    for (var i = 0; i < result.d.lat.length; i++) {
        if (result.d.lat[i] != 0 && result.d.lng[i] != 0) {
            flightPlanCoordinates.push(new google.maps.LatLng(result.d.lat[i], result.d.lng[i]));
            bounds.extend(new google.maps.LatLng(result.d.lat[i], result.d.lng[i]));
        }
    }

    //no data
    if (!flightPlanCoordinates.length >0) {
        $("#slider").slider("disable");
        $("#playPath").button("disable");
        $("#map").html("<center><br/>Нет данных.</center>");

        return;
    }

    //create map
    var myOptions = {
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map"), myOptions);
    map.fitBounds(bounds);
    
    //set path to map
    flightPath = new google.maps.Polyline({
        path: flightPlanCoordinates,
        strokeColor: "#2D479A",
        strokeOpacity: 0.6,
        strokeWeight: 2
    });
    flightPath.setMap(map);

    //create markers
    neighborhoods = [
        new google.maps.LatLng(result.d.lat[0], result.d.lng[0]),
        new google.maps.LatLng(result.d.lat[result.d.lat.length-1], result.d.lng[result.d.lng.length-1])
    ];
    //marker icons
    markerImages = [
        new google.maps.MarkerImage('../css/icons/Green Flag-32x32.png',
            new google.maps.Size(32, 32),
            new google.maps.Point(0, 0),
            new google.maps.Point(6, 32)),
        new google.maps.MarkerImage('../css/icons/Red-Flag-32x32.png',
            new google.maps.Size(32, 32),
            new google.maps.Point(0, 0),
            new google.maps.Point(6, 32)),
        ]

    markers = [];
    iterator = 0;

    drop();

    infoWindowBegin = new InfoBubble({
        content: "<div class='infowindow'>" +
            "<span style='font-size:13px'><b>Начальная точка</b></span><br/>" +
            "Широта: " + result.d.lat[0] + "<br/>" +
            "Долгота: " + result.d.lng[0] + "<br/>" +
            "Время: <b>" + date2String(new Date(result.d.time[0])) + "</b><br/>" +
            "</div>"
    });

    infoWindowFinish = new InfoBubble({
        content: "<div class='infowindow'>" +
            "<span style='font-size:13px'><b>Конечная точка</b></span><br/>" +
            "Широта: " + result.d.lat[result.d.lat.length - 1] + "<br/>" +
            "Долгота: " + result.d.lng[result.d.lng.length - 1] + "<br/>" +
            "Время: <b>" + date2String(new Date(result.d.time[result.d.lng.length - 1])) + "</b><br/>" +
            "</div>"
    });
}

function drop() {
    for (var i = 0; i < neighborhoods.length; i++) {
      setTimeout(function() {
        addMarker();
      }, i * 500);
    }
}

function addMarker() {
    markers.push(new google.maps.Marker({
      position: neighborhoods[iterator],
      map: map,
      icon: markerImages[iterator],
      draggable: false,
      animation: google.maps.Animation.DROP
    }));

    if (iterator == 0) {
        google.maps.event.addListener(markers[0], 'click', function () {
            infoWindowBegin.open(map, markers[0]);
        });
    } else {
        google.maps.event.addListener(markers[1], 'click', function () {
            infoWindowFinish.open(map, markers[1]);
        });
    }

    iterator++;
}

function playPath() {
    //no data
    if (plfData == null)
        return;
    var currentSliderValue = $("#slider").slider("value");
    var maxSliderValue = plfData.d.lat.length - 1;
    if (isPlay) {
        if (currentSliderValue < maxSliderValue) {
            $("#slider").slider("value", currentSliderValue + 1);
            setTimeout(function () {
                playPath();
            }, 500);
        } else {
            //stop play path
            $("#playPath").click();
        }
    }
}