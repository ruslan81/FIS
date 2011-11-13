function loadRecoverUserData() {
    destroyTree($("#tree"));
    destroyPeriodControls();
    $("#DriversTree").empty();
    $("#TransportTree").empty();
    destroyTable($("#contentTable"), $("#contentTableBody"), $("#contentTableHeader"));
    //цепочка вызывов:  loadRecoverUserDriversTree()->loadRecoverUserTransportTree()->build tree->loadRecoverUserNodeData
    loadRecoverUserDriversTree();
}

//Загрузить элементы дерева Водителей в разделе "Восстановить у пользователя"
function loadRecoverUserDriversTree() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Data.aspx/GetRecoverUserDriversTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            $("#DriversTree").empty();
            $("#tmplTreeItem").tmpl(response.d).appendTo("#DriversTree");

            loadRecoverUserTransportTree();
        }
    });
}

//Загрузить элементы дерева Транспортные средства в разделе "Восстановить у пользователя"
function loadRecoverUserTransportTree() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Data.aspx/GetRecoverUserTransportTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            $("#TransportTree").empty();
            $("#tmplTreeItem").tmpl(response.d).appendTo("#TransportTree");

            //builds a tree
            $("#tree").wijtree();

            //sets a listener to node selection
            $("#tree").wijtree({ selectedNodeChanged: function (e, data) {

                onRecoverUserNodeSelected(e, data);
            }
            });
        }
    });
}

//Событие при выделении узла дерева
function onRecoverUserNodeSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");
    cardID = $("a span", data.element).attr("key");

    if (isSelected == "true") {
        loadRecoverUserNodeData();
    } else {
        $("#contentTableBody").empty();
        $("#contentTable").hide();
    }
}

//Загрузить данные для выбранного элемента дерева в разделе "Восстановить у пользователя"
function loadRecoverUserNodeData() {
    $("#contentTable").show();
    //create table header
    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Имя файла", "style": ""},' +
    '{"text": "Тип файла", "style": "width: 100px;"},' +
    '{"text": "Начальная дата", "style": "width: 100px;"},' +
    '{"text": "Конечная дата", "style": "width: 100px;"},' +
    '{"text": "Количество", "style": "width: 100px;"},' +
    '{"text": "Дата разбора файла", "style": "width: 150px;"},' +
    '{"text": "", "style": "width: 50px;"}]');

    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Data.aspx/GetRecoverUserNodeData",
        data: "{'CardID':'" + cardID + "', 'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            updateTable($("#contentTableBody"), $("#tmplDriversTable"), result.d);
        }
    });


}

//create table header
//tableHeader - thead tag, template - template to generate header, columns - table's columns in JSON format (column name : column style)
function createTableHeader(tableHeader, template, columns) {
    $(tableHeader).empty();
    $(tableHeader).append("<tr class='wijmo-wijgrid-headerrow'></tr>");
    $(template).tmpl(jQuery.parseJSON(columns)).appendTo($(".wijmo-wijgrid-headerrow", tableHeader));
}


//Update/create table
//tableBody - tbody tag, template - template to generate table, data - table data
function updateTable(tableBody, template, data) {
    //очищаем tbody от предыдущих данных
    $(tableBody).empty();
    //вставляем данные в шаблон и добавляем его к tbody
    $(template).tmpl(data).appendTo(tableBody);

    //get table rows
    var rows = $(".wijmo-wijgrid-datarow", tableBody);
    //делаем таблицу "полосатой"
    for (var i = 0; i < rows.length; i++) {
        if (i % 2 != 0)
            $(rows[i]).addClass("wijmo-wijgrid-alternatingrow");
    }

    //отделяем ячейки границами
    for (var i = 0; i < rows.length; i++) {
        var cells=$(".wijgridtd",rows[i]);
        for (var j = 0; j < cells.length; j++) {
            if (j < cells.length - 1) {
                $(cells[j]).addClass("wijmo-wijgrid-cell-border-right");
            }
            if (i < rows.length - 1) {
                $(cells[j]).addClass("wijmo-wijgrid-cell-border-bottom");
            }
            $(cells[j]).addClass("wijmo-wijgrid-cell");
        }
    }

    //add hover effect
    $(".wijmo-wijgrid-datarow", tableBody).hover(function () {
        $(this).addClass("ui-state-hover");
    }, function () {
        $(this).removeClass("ui-state-hover");
    });

    //add ability to select rows
    $(".wijmo-wijgrid-datarow", tableBody).click(function () {
        $(".wijmo-wijgrid-datarow .ui-state-highlight", tableBody).removeClass("ui-state-highlight");
        $(this).find("td").addClass("ui-state-highlight");
    });
}

function destroyTree(tree) {
    $(tree).wijtree("destroy");
}

function destroyTable(table, tableBody, tableHeader) {
    $(table).hide();
    $(tableBody).empty();
    $(tableHeader).empty();
}


//FROM HERE OVERLOOK(DRIVER) STARTS

function loadOverlookDriver() {
    destroyTree($("#OverlookDriverTree"));
    destroyPeriodControls();
    loadOverlookDriverTree();
}

//Загрузить элементы дерева в разделе "Просмотреть(Водитель)"
function loadOverlookDriverTree() {
    $.ajax({
        type: "POST",
        url: "Data.aspx/GetOverlookDriversTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#OverlookDriverTree").empty();
            $("#tmplGroupTree").tmpl(response.d).appendTo("#OverlookDriverTree");
            $("#OverlookDriverTree").wijtree();
            $("#OverlookDriverTree").wijtree({ selectedNodeChanged: function (e, data) {
                onOverlookNodeSelected(e, data);
            }
            });
        }
    });
}

//Событие при выделении узла дерева
/*function onOverlookDriverSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");
    cardID = $("a span", data.element).attr("key");
    if (cardID == "None")
        return;
    cardID = "136";
    if (isSelected == "true") {
        loadOverlookDriverNodeData()
    } else {
        $("#contentTableBody").empty();
        $("#contentTable").hide();
    }
}

//Загрузить данные для выбранного элемента дерева в разделе "Просмотреть(Водитель)"
function loadOverlookDriverNodeData() {
    $("#contentTable").show();
    //create table header
    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "Год", "style": "width: 60px;"},' +
    '{"text": "Месяц", "style": "width: 60px;"},' +
    '{"text": "Число", "style": "width: 60px;"},' +
    '{"text": "Процент данных", "style": "width: 60px;"},' +
    '{"text": "Прогресс", "style": ""}]');

    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Data.aspx/GetOverlookDriverNodeData",
        data: "{'CardID':'" + cardID + "', 'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            updateTable($("#contentTableBody"), $("#tmplOverlookTable"), result.d);
            refreshProgressBars();
        }
    });
}*/

//FROM HERE OVERLOOK(VEHICLE) STARTS

function loadOverlookVehicle() {
    destroyTree($("#OverlookVehicleTree"));
    destroyPeriodControls();
    //$("#OverlookVehicleTree").wijtree("destroy");
    loadOverlookVehicleTree();
}

//Загрузить элементы дерева в разделе "Просмотреть(ТС)"
function loadOverlookVehicleTree() {
    $.ajax({
        type: "POST",
        url: "Data.aspx/GetOverlookVehiclesTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#OverlookVehicleTree").empty();
            $("#tmplGroupTree").tmpl(response.d).appendTo("#OverlookVehicleTree");
            $("#OverlookVehicleTree").wijtree();
            $("#OverlookVehicleTree").wijtree({ selectedNodeChanged: function (e, data) {
                onOverlookNodeSelected(e, data);
            }
            });
        }
    });
}

//Событие при выделении узла дерева
function onOverlookNodeSelected(e, data) {

    $("#contentTableBody").empty();
    $("#contentTable").hide();
    $("#periodSelection").hide();
        
    isSelected = $("div", data.element).attr("aria-selected");
    cardID = $("a span", data.element).attr("key");
    if (cardID == "None"){
        cardID = null;
        return;
    }
    //cardID = "135";
    if (isSelected == "true") {
        $("#periodSelection").show();
        $("#dateErrorBlock").hide();
    } else {
        $("#contentTableBody").empty();
        $("#contentTable").hide();
        $("#periodSelection").hide();
        //destroyPeriodControls();
    }
}

function onClickBuildReport() {
    $("#dateErrorBlock").hide();
    if (cardID != null) {
        $("#contentTable").show();
        createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
        '[{"text": "Год", "style": "width: 60px;"},' +
        '{"text": "Месяц", "style": "width: 60px;"},' +
        '{"text": "Число", "style": "width: 60px;"},' +
        '{"text": "Процент данных", "style": "width: 60px;"},' +
        '{"text": "Прогресс", "style": ""}]');
    }
    startDate=$("#startDatePicker").datepicker("getDate");
    endDate = $("#endDatePicker").datepicker("getDate");

    if (endDate == null || startDate == null){// || !checkDate(startDate) || !checkDate(endDate)) {
        $("#dateErrorBlock").show();
        return;
    }
    if ($("#accordion").accordion("option", "active") == 3) {
        $.ajax({
            type: "POST",
            url: "Data.aspx/GetOverlookVehicleNodeData",
            data: "{'CardID':'" + cardID + "', 'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'StartDate':'" + convert(startDate) + "', 'EndDate':'" + convert(endDate) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d == null) {
                    $("#dateErrorBlock").show();
                    $("#contentTable").hide();
                    return;
                }
                updateTable($("#contentTableBody"), $("#tmplOverlookTable"), result.d);
                refreshProgressBars();
            }
        });
    }
    if ($("#accordion").accordion("option", "active") == 2) {
        $.ajax({
            type: "POST",
            url: "Data.aspx/GetOverlookDriverNodeData",
            data: "{'CardID':'" + cardID + "', 'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'StartDate':'" + convert(startDate) + "', 'EndDate':'" + convert(endDate) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d == null) {
                    $("#dateErrorBlock").show();
                    $("#contentTable").hide();
                    return;
                }
                updateTable($("#contentTableBody"), $("#tmplOverlookTable"), result.d);
                refreshProgressBars();
            }
        });
    }
}

/*function checkDate(date) {
    y = date.getFullYear();
    m = date.getMonth();
    d = date.getDate();
    alert(y + " " + m + " " + d);
    if (d < 1 || d > 31) {
        return false;
    }
    if (m < 1 || d > 12) {
        return false;
    }
    if (y < 2000 || y > 2100) {
        return false;
    }
    return true;
}*/

function convert(date) {
    res=date.getDate() + ".";
    if (date.getMonth() < 9) res = res + "0";
    return res + (date.getMonth() + 1) + "." + date.getFullYear();
}

function createPeriodControls() {
    //$("#periodSelection").show();

    var today = new Date();
    var todaystr = "" + convert(today);
    today.setMonth(today.getMonth()-1);
    var thenstr = "" + convert(today);

    $("#startDatePicker").datepicker();
    $("#startDatePicker").datepicker("option", "dateFormat", "dd.mm.yy");
    $("#startDatePicker").datepicker("setDate", thenstr);

    $("#endDatePicker").datepicker();
    $("#endDatePicker").datepicker("option", "dateFormat", "dd.mm.yy");
    $("#endDatePicker").datepicker("setDate", todaystr);

    $("#startDatePicker").datepicker($.datepicker.regional['ru']);
    $("#endDatePicker").datepicker($.datepicker.regional['ru']);

    $("#buildButton").button();
    $("#buildButton").click(function () {
        onClickBuildReport();
        return false;
    });
}

function destroyPeriodControls() {
    //$("#buildButton").button("destroy");
    $("#periodSelection").hide();
    $("#contentTable").hide();
}