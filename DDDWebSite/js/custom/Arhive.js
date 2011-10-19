function loadRecoverUserData() {
    destroyTree($("#tree"));
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
        loadRecoverUserNodeData()
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
    '[{"text": "", "style": "width: 50px"},' +
    '{"text": "Имя файла", "style": ""},' +
    '{"text": "Тип файла", "style": "width: 100px"},' +
    '{"text": "Начальная дата", "style": "width: 100px"},' +
    '{"text": "Конечная дата", "style": "width: 100px"},' +
    '{"text": "Количество записей", "style": "width: 100px"},' +
    '{"text": "Дата разбора файла", "style": "width: 150px"},' +
    '{"text": "", "style": "width: 50px"}]');

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
    //$("#OverlookDriverTree").wijtree("destroy");
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
                onOverlookDriverSelected(e, data);
            }
            });
        }
    });
}

//Событие при выделении узла дерева
function onOverlookDriverSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");
    //cardID = $("a span", data.element).attr("key");

    if (isSelected == "true") {
        loadOverlookDriverNodeData()
    } /*else {
        $("#contentTableBody").empty();
        $("#contentTable").hide();
    }*/
}

//Загрузить данные для выбранного элемента дерева в разделе "Просмотреть(Водитель)"
function loadOverlookDriverNodeData() {
    /*$("#contentTable").show();
    //create table header
    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px"},' +
    '{"text": "Имя файла", "style": ""},' +
    '{"text": "Тип файла", "style": "width: 100px"},' +
    '{"text": "Начальная дата", "style": "width: 100px"},' +
    '{"text": "Конечная дата", "style": "width: 100px"},' +
    '{"text": "Количество записей", "style": "width: 100px"},' +
    '{"text": "Дата разбора файла", "style": "width: 150px"},' +
    '{"text": "", "style": "width: 50px"}]');

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
    });*/
}

//FROM HERE OVERLOOK(VEHICLE) STARTS

function loadOverlookVehicle() {
    destroyTree($("#OverlookVehicleTree"));
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
                onOverlookVehicleSelected(e, data);
            }
            });
        }
    });
}

//Событие при выделении узла дерева
function onOverlookVehicleSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");
    //cardID = $("a span", data.element).attr("key");

    if (isSelected == "true") {
        loadOverlookVehicleNodeData()
    } /*else {
        $("#contentTableBody").empty();
        $("#contentTable").hide();
    }*/
}

//Загрузить данные для выбранного элемента дерева в разделе "Просмотреть(Водитель)"
function loadOverlookVehicleNodeData() {
    /*$("#contentTable").show();
    //create table header
    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px"},' +
    '{"text": "Имя файла", "style": ""},' +
    '{"text": "Тип файла", "style": "width: 100px"},' +
    '{"text": "Начальная дата", "style": "width: 100px"},' +
    '{"text": "Конечная дата", "style": "width: 100px"},' +
    '{"text": "Количество записей", "style": "width: 100px"},' +
    '{"text": "Дата разбора файла", "style": "width: 150px"},' +
    '{"text": "", "style": "width: 50px"}]');

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
    });*/
}