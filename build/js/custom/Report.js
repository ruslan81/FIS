//Загрузить элементы дерева Водителей в разделе "Восстановить у пользователя"
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

            $("#report").empty();
            $("#tmplLoadingPLFFile").tmpl({}).appendTo("#report");

            if (chart != null) {
                chart.destroy();
            }
            chart = null;
            $("#chart").empty();
            $("#tmplLoadingPLFFile").tmpl({}).appendTo("#chart");

            plfData = null;

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Reports.aspx/GetReport",
                data: "{'CardID':'" + cardID + "', 'PLFID':'" + plf + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $("#report").empty();
                    $("#chart").empty();

                    $('#report').html(result.d.report);

                    plfData = result;

                    if (currentTab == 1) {
                        createCharts(plfData);
                    }
                }
            });
        } else {
            $("#statusPanel").empty();
            $("#tmplNoPLFFile").tmpl("").appendTo("#statusPanel");
            $("#report").empty();
            $("#chart").empty();
        }
    } else {
        $("#statusPanel").empty();
        $("#tmplNoPLFFile").tmpl("").appendTo("#statusPanel");
        $("#report").empty();
        $("#chart").empty();
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