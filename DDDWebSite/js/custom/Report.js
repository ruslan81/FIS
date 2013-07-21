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

    $("#report-tabs").tabs("destroy");

    $("#report-tabs").empty();
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

    /*$.ajax({
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
            $("#PLFFilesTree").searchTree();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });*/
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
            $("#tmplChoosePLFFile").tmpl({ 'Driver': driver + " / " + driverNumber, 'Device': device, 'Period': period,
                'type': 'GetPLFReport', 'CardID': cardID, 'PLFID': plf, 'UserName': $.cookie("CURRENT_USERNAME")
            }).appendTo("#statusPanel");

            $("#getReport").button();
            $("#formatChooser").wijcombobox({ changed: function (e, item) {
                var format = $("#formatChooser").attr("selectedIndex");
                if (format == "0") {
                    $("#format").attr("value", "pdf");
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
                url: "Reports.aspx/GetPLFReportTypes",
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
                        $("#tmplLoading").tmpl({}).appendTo("#report");

                        $.ajax({
                            type: "POST",
                            //Page Name (in which the method should be called) and method name
                            url: "Reports.aspx/GetPLFReport",
                            data: "{'CardID':'" + cardID + "', 'PLFID':'" + plf + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "', 'ReportType':'" + ((reportTypes == null) ? "" : reportTypes.d[$("#reportChooser").attr("selectedIndex")]) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                $("#report").empty();

                                $('#report').html(result.d.report);
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                            }
                        });
                    },
                        isEditable: false
                    });

                    //default select general report
                    var defTypeReport = "Полный отчет";
                    for (var i = 0; i < reportTypes.d.length; i++) {
                        if (reportTypes.d[i] == defTypeReport) {
                            $("#reportChooser").wijcombobox({ selectedIndex: i });
                            $("#reportChooser").attr("selectedIndex", i);
                            break;
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });

            $("#report").empty();
            $("#tmplLoading").tmpl({}).appendTo("#report");

            if (chart != null) {
                chart.destroy();
            }
            chart = null;
            $("#chart").empty();
            $("#tmplLoading").tmpl({}).appendTo("#chart");

            if (map != null) {
                delete (map);
            }
            map = null;
            $("#map").empty();
            $("#tmplLoading").tmpl({}).appendTo("#map");
            $("#slider").slider("disable");
            $("#slider").slider({ value: 0 });
            $("#playPath").button("disable");
            isPlay = false;

            plfData = null;

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Reports.aspx/GetPLFReport",
                data: "{'CardID':'" + cardID + "', 'PLFID':'" + plf + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") +
                        "', 'ReportType':'" + ((reportTypes == null) ? "" : reportTypes.d[$("#reportChooser").attr("selectedIndex")]) + "'}",
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
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        if (i < result.d.time.length - 1) {
            var xNext = result.d.time[i + 1];
            console.log(xNext - x);
            if (xNext - x > 60000) {
                data.push({
                    x: x + 60000,
                    y: null
                });
            }
        }
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
        if (i < result.d.time.length - 1) {
            var xNext = result.d.time[i + 1];
            console.log(xNext - x);
            if (xNext - x > 60000) {
                data.push({
                    x: x + 60000,
                    y: null
                });
            }
        }
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
        if (i < result.d.time.length - 1) {
            var xNext = result.d.time[i + 1];
            console.log(xNext - x);
            if (xNext - x > 60000) {
                data.push({
                    x: x + 60000,
                    y: null
                });
            }
        }
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
        if (i < result.d.time.length - 1) {
            var xNext = result.d.time[i + 1];
            console.log(xNext - x);
            if (xNext - x > 60000) {
                data.push({
                    x: x+60000,
                    y: null
                });
            }
        }
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
    if (!flightPlanCoordinates.length > 0) {
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
        new google.maps.LatLng(result.d.lat[result.d.lat.length - 1], result.d.lng[result.d.lng.length - 1])
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
        setTimeout(function () {
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

//////////////////////////////////////////////////////

//Создать закладку Транспортные средства
function createVehicles() {
    $("#report-tabs").empty();
    $("#tmplVehiclesTab").tmpl("").appendTo("#report-tabs");

    loadVehiclesTree();
}

//Создать закладку Транспортные средства
function createDDDTab() {
    $("#report-tabs").empty();
    $("#tmplVehiclesTab").tmpl("").appendTo("#report-tabs");
}

//Загрузить элементы дерева Транспортные средства
function loadVehiclesTree() {
    $("#statusPanel").empty();
    $("#tmplNoVehicles").tmpl("").appendTo("#statusPanel");

    //destroy a tree
    $("#vehiclesTree").wijtree("destroy");
    $("#vehiclesTree").empty();

    $("#report-tabs").empty();

    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Reports.aspx/GetVehiclesTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#tmplVehiclesTree").tmpl(response.d).appendTo("#vehiclesTree");

            //builds a tree
            $("#vehiclesTree").wijtree();

            //sets a listener to node selection
            $("#vehiclesTree").wijtree({ selectedNodeChanged: function (e, data) {
                onVehiclesNodeSelected(e, data);
            }
            });
            $("#vehiclesTree").searchTree();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function onVehiclesNodeSelected(e, data) {
    var isSelected = $("div", data.element).attr("aria-selected");
    var dataBlockID = $("a span", data.element).attr("dataBlockID");
    var file = $("a span", data.element).attr("file");
    var period = $("a span", data.element).text();
    var vehicleCode = $("a span", data.element).attr("code");
    var vehicleName = $("a span", data.element).attr("name");

    if (isSelected == "true") {
        if (dataBlockID != null) {
            $("#statusPanel").empty();
            $("#tmplChooseDDDFile").tmpl({ 'Name': vehicleName, 'VehicleVin': vehicleCode, 'Period': period,
                'type': 'GetDDDReport', 'UserName': $.cookie("CURRENT_USERNAME"), 'DataBlockID': dataBlockID
            }).appendTo("#statusPanel");

            $("#getReport").button();
            $("#formatChooser").wijcombobox({ changed: function (e, item) {
                var format = $("#formatChooser").attr("selectedIndex");
                if (format == "0") {
                    $("#format").attr("value", "pdf");
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
                url: "Reports.aspx/GetDDDReportTypes",
                data: [],
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    reportTypes = res;
                    $("#tmplSelect").tmpl({ 'filenames': reportTypes.d }).appendTo("#reportChooser");

                    $("#reportChooser").wijcombobox({ changed: function (e, item) {
                        var reportType = $("#reportChooser").attr("selectedIndex");

                        $("#reportType").attr("value", reportTypes.d[reportType]);

                        $("#report-tabs").empty();
                        $("#tmplLoading").tmpl({}).appendTo("#report-tabs");

                        $.ajax({
                            type: "POST",
                            //Page Name (in which the method should be called) and method name
                            url: "Reports.aspx/GetDDDReport",
                            data: "{'DataBlockID':'" + dataBlockID + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "', 'ReportType':'" + ((reportTypes == null) ? "" : reportTypes.d[$("#reportChooser").attr("selectedIndex")]) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                $("#report-tabs").empty();

                                $('#report-tabs').html("<center><div style='background:#fff;padding:20px 0 20px 0;'>" +
                                    result.d +
                                    "</div></center>");
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                            }
                        });
                    },
                        isEditable: false
                    });

                    //default select general report
                    var defTypeReport = "Полный отчет";
                    for (var i = 0; i < reportTypes.d.length; i++) {
                        if (reportTypes.d[i] == defTypeReport) {
                            $("#reportChooser").wijcombobox({ selectedIndex: i });
                            $("#reportChooser").attr("selectedIndex", i);
                            break;
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });

            $("#report-tabs").empty();
            $("#tmplLoading").tmpl({}).appendTo("#report-tabs");

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Reports.aspx/GetDDDReport",
                data: "{'DataBlockID':'" + dataBlockID + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "', 'ReportType':'" + ((reportTypes == null) ? "" : reportTypes.d[$("#reportChooser").attr("selectedIndex")]) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $("#report-tabs").empty();

                    $('#report-tabs').html("<center><div style='background:#fff;padding:20px 0 20px 0;'>" +
                        result.d +
                        "</div></center>");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });

        } else {
            $("#statusPanel").empty();
            $("#tmplNoVehicles").tmpl("").appendTo("#statusPanel");
            $("#report-tabs").empty();
        }
    } else {
        $("#statusPanel").empty();
        $("#tmplNoVehicles").tmpl("").appendTo("#statusPanel");
        $("#report-tabs").empty();
    }
}

function loadDriverTree() {
    $.ajax({
        type: "POST",
        url: "Data.aspx/GetOverlookDriversTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#DriversTree").wijtree("destroy");
            $("#DriversTree").empty();
            $("#tmplGroupTree").tmpl(response.d).appendTo("#DriversTree");
            $("#DriversTree").wijtree();
            $("#DriversTree").wijtree({ selectedNodeChanged: function (e, data) {
                onCardTypeNodeSelected(e, data);
            }
            });
            $("#DriversTree").searchTree();
            loadReportTypesTree("DriversTreePlace");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadVehicleTree() {
    $.ajax({
        type: "POST",
        url: "Data.aspx/GetOverlookVehiclesTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#VehiclesTree").wijtree("destroy");
            $("#VehiclesTree").empty();
            $("#tmplGroupTree").tmpl(response.d).appendTo("#VehiclesTree");
            $("#VehiclesTree").wijtree();
            $("#VehiclesTree").wijtree({ selectedNodeChanged: function (e, data) {
                onCardTypeNodeSelected(e, data);
            }
            });
            $("#VehiclesTree").searchTree();
            loadReportTypesTree("VehiclesTreePlace");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadReportTypesTree(placeId) {
    $("#ReportTree").wijtree("destroy");
    $("#ReportTree").remove();
    $("#" + placeId).empty();
    $("#" + placeId).append($("#tmplReportTree").text());

    /*if (cardType == "Driver") {
        $("#DDDSubtree").remove();
    }
    if (cardType == "Vehicle") {
        $("#PLFSubtree").remove();
    }*/

    $.ajax({
        type: "POST",
        url: "Reports.aspx/GetPLFReportTypes",
        data: [],
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            for (var i = 0; i < response.d.length; i++) {
                //$("#PLFSubtree").append("<li class='file'><a><span repform='PLF' key='" + response.d[i] + "'>" + response.d[i] + "</span></a></li>");
                $("#ReportInsideTree").append("<li class='file'><a><span repform='PLF' key='" + response.d[i] + "'>" + response.d[i] + "</span></a></li>");
            }

            $.ajax({
                type: "POST",
                url: "Reports.aspx/GetDDDReportTypes",
                data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    for (var i = 0; i < response.d.length; i++) {
                        //$("#DDDSubtree").append("<li class='file'><a><span repform='DDD' key='" + response.d[i] + "'>" + response.d[i] + "</span></a></li>");
                        $("#ReportInsideTree").append("<li class='file'><a><span repform='DDD' key='" + response.d[i] + "'>" + response.d[i] + "</span></a></li>");
                    }

                    $("#ReportTree").wijtree();
                    $("#ReportTree").wijtree({ selectedNodeChanged: function (e, data) {
                        onReportTypeNodeSelected(e, data);
                    }
                    });
                    //$("#ReportTree").searchTree();

                    //HIDING OF UNUSED LEAFS
                    var leafs = $('#ReportInsideTree .file');
                    for (var i = 0; i < leafs.length; i++) {
                        if (cardType == "Driver") {
                            if ($('#ReportInsideTree li:eq(' + i + ') span [repform="DDD"]').length > 0) {
                                $(leafs[i]).hide();
                            }
                            continue;
                        }
                        if (cardType == "Vehicle") {
                            if ($('#ReportInsideTree li:eq(' + i + ') span [repform="PLF"]').length > 0) {
                                $(leafs[i]).hide();
                            }
                            continue;
                        }
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function createPeriodControls() {
    $("#main-conditions").append($("#tmplPeriodSelection").text());
    //$("#main-conditions").append($("#tmplLoadReportControls").text());
    //$("#LoadReportControls").empty();

    var today = new Date();
    var todaystr = "" + convert(today);
    today.setMonth(today.getMonth() - 1);
    var thenstr = "" + convert(today);

    $("#startDatePicker").datepicker();
    $("#startDatePicker").datepicker("option", "dateFormat", "dd.mm.yy");
    $("#startDatePicker").datepicker("setDate", thenstr);
    $("#startDatePicker").change(function () { 
        $("#LoadReportControls").remove();
    });

    $("#endDatePicker").datepicker();
    $("#endDatePicker").datepicker("option", "dateFormat", "dd.mm.yy");
    $("#endDatePicker").datepicker("setDate", todaystr);
    $("#endDatePicker").change(function () {
        $("#LoadReportControls").remove();
    });

    $("#startDatePicker").datepicker($.datepicker.regional['ru']);
    $("#endDatePicker").datepicker($.datepicker.regional['ru']);

    $("#buildButton").button();
    $("#buildButton").click(function () {
        buildReport();
        return false;
    });

    $("#periodSelection").show();
    $("#dateErrorBlock").hide();

}

function createFormatSelector() {
    $("#getReport").button();
    $("#formatChooser").wijcombobox({ changed: function (e, item) {
        var format = $("#formatChooser").attr("selectedIndex");
        if (format == "0") {
            $("#format").attr("value", "pdf");
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
}

function destroyPeriodControls() {
    $("#buildButton").button("destroy");
    $("#statusPanel").hide();
    $("#periodSelection").remove();
    $("#LoadReportControls").remove();

    //!TODO comment if you want standart functional without diagram
    //$("#calendarWrapper").hide();

    resizeReports();
}

function convert(date) {
    res = date.getDate() + ".";
    if (date.getMonth() < 9) res = res + "0";
    return res + (date.getMonth() + 1) + "." + date.getFullYear();
}

function resizeReports() {
    var vertHeightSTR = document.getElementById('vertical-menu').style.height;
    vertHeightSTR = vertHeightSTR.substr(0, vertHeightSTR.length - 2);
    document.getElementById('outputId').style.height = (vertHeightSTR - 30) + "px";
    document.getElementById('outputId-content').style.height = (vertHeightSTR - 30) + "px";
    if ($('#main-conditions:visible').length > 0) {
        var h = $('#outputId').height() - $('#main-conditions').height() - 25;
        $('#outputId').height(h);
        $('#outputId-content').height(h);
    }
}

//Событие при выделении узла дерева
function onReportTypeNodeSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");
    selectedReportType = $("a span", data.element).attr("key");
    reportFormat = $("a span", data.element).attr("repform");
    if (isSelected != "true") {
        selectedReportType = "None";
        reportFormat = "None";
    }
    $("#LoadReportControls").remove();
}

//Событие при выделении узла дерева
function onCardTypeNodeSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");
    selectedCardID = $("a span", data.element).attr("key");
    if (isSelected != "true") {
        selectedCardID = "None";
    }
    $("#LoadReportControls").remove();
}

function checkDate() {
    $("#dateErrorBlock").hide();
    $("#dateErrorLabel").empty();

    var startDate = $("#startDatePicker").datepicker("getDate");
    var endDate = $("#endDatePicker").datepicker("getDate");

    if (endDate < startDate) {
        $("#LoadReportControls").remove();
        $("#dateErrorLabel").append(" Ошибка: Неверно заданы даты!");
        $("#dateErrorBlock").show();
        return "BAD";
    }

    if (endDate == null || startDate == null) {
        $("#LoadReportControls").remove();
        $("#dateErrorLabel").append(" Ошибка: Укажите начальную и конечную дату!");
        $("#dateErrorBlock").show();
        return "BAD";
    }
    return "OK";
}

function buildReport() {

    var startDate = $("#startDatePicker").datepicker("getDate");
    var endDate = $("#endDatePicker").datepicker("getDate");

    //remove report chooser and '#getReport' button
    $("#LoadReportControls").remove();

    if (checkDate() != "OK")
        return;

    if ((cardType == "Driver" && reportFormat=="DDD") || (cardType == "Vehicle" && reportFormat == "PLF")) {
        $("#dateErrorLabel").append(" Ошибка: Тип отчета не доступен для объекта!");
        $("#dateErrorBlock").show();
        return;
    }
    if (selectedCardID == "None") {
        $("#dateErrorLabel").append(" Ошибка: Выберите объект для отчета!");
        $("#dateErrorBlock").show();
        return;
    }
    if (reportFormat == "None" || selectedReportType == "None") {
        $("#dateErrorLabel").append(" Ошибка: Выберите тип отчета!");
        $("#dateErrorBlock").show();
        return;
    }

    if (reportFormat == "PLF") {
        $("#report").empty();
        $("#tmplLoading").tmpl({}).appendTo("#report");

        if (chart != null) {
            chart.destroy();
        }
        chart = null;
        $("#chart").empty();
        $("#tmplLoading").tmpl({}).appendTo("#chart");

        if (map != null) {
            delete (map);
        }
        map = null;
        $("#map").empty();
        $("#tmplLoading").tmpl({}).appendTo("#map");
        $("#slider").slider("disable");
        $("#slider").slider({ value: 0 });
        $("#playPath").button("disable");
        isPlay = false;

        plfData = null;

        $.ajax({
            type: "POST",
            //Page Name (in which the method should be called) and method name
            url: "Reports.aspx/GetPLFReportForPeriod",
            data: "{'CardID':'" + selectedCardID + "', 'StartDate':'" + convert(startDate) + "', 'EndDate':'" + convert(endDate) + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "', 'ReportType':'" + selectedReportType + "'}",
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

                $("#tmplLoadReportControls").tmpl({ 'CardID': selectedCardID, 'StartDate': convert(startDate), 'EndDate': convert(endDate),
                    'type': 'GetPLFReportForPeriod', 'UserName': $.cookie("CURRENT_USERNAME"), 'ReportType': selectedReportType
                }).appendTo("#main-conditions");
                createFormatSelector();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("#report").empty();
                $("#chart").empty();
                $("#map").empty();

                showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
            }
        });
    }

    if (reportFormat == "DDD") {
        $("#report-tabs").empty();
        $("#tmplLoading").tmpl({}).appendTo("#report-tabs");

        $.ajax({
            type: "POST",
            //Page Name (in which the method should be called) and method name
            url: "Reports.aspx/GetDDDReportForPeriod",
            data: "{'CardID':'" + selectedCardID + "', 'StartDate':'" + convert(startDate) + "', 'EndDate':'" + convert(endDate) + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "', 'ReportType':'" + selectedReportType + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $("#report-tabs").empty();
                $('#report-tabs').html(
                        "<center><div style='background:#fff;padding:20px 0 20px 0;'>" +
                            result.d +
                        "</div></center>");

                $("#tmplLoadReportControls").tmpl({ 'CardID': selectedCardID, 'StartDate': convert(startDate), 'EndDate': convert(endDate),
                    'type': 'GetDDDReportForPeriod', 'UserName': $.cookie("CURRENT_USERNAME"), 'ReportType': selectedReportType
                }).appendTo("#main-conditions");
                createFormatSelector();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("#report-tabs").empty();
                showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
            }
        });
    }
}
