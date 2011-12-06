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
        var cells = $(".wijgridtd", rows[i]);
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
    /*$(".wijmo-wijgrid-datarow", tableBody).hover(function () {
        $(this).addClass("ui-state-hover");
    }, function () {
        $(this).removeClass("ui-state-hover");
    });

    //add ability to select rows
    $(".wijmo-wijgrid-datarow", tableBody).click(function () {
        $(".wijmo-wijgrid-datarow .ui-state-highlight", tableBody).removeClass("ui-state-highlight");
        $(this).find("td").addClass("ui-state-highlight");
    });*/
}

//create table header
//tableHeader - thead tag, template - template to generate header, columns - table's columns in JSON format (column name : column style)
function createTableHeader(tableHeader, template, columns) {
    $(tableHeader).empty();
    $(tableHeader).append("<tr class='wijmo-wijgrid-headerrow'></tr>");
    $(template).tmpl(jQuery.parseJSON(columns)).appendTo($(".wijmo-wijgrid-headerrow", tableHeader));
}

function buildTree() {
    //builds a tree
    $("#tree").wijtree();
    $("#tree").wijtree({ selectedNodeChanged: function (e, data) {
            onRecoverUserNodeSelected(e, data);
        }
    });
    //and select General Settings
    $("#general").wijtreenode({ selected: true });
    //and load general settings
    loadGeneralSettings();
}

//Событие при выделении узла дерева
function onRecoverUserNodeSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");

    if (isSelected == "true") {
        
        key = $("a span", data.element).attr("key");
        if (key == "General") {
            loadGeneralSettings();
        } else if (key == "Groups") {
            loadGroupsSettings();
        } else if (key == "Drivers") {
            loadDriversSettings();
        } else if (key == "Transport") {
            loadTransportsSettings();
        } else if (key == "Default") {
            loadGeneralSettings();
        }
    } else {
        $("#headerSettings").empty();
        $("#contentSettings").empty();
    }
}

function loadGeneralSettings() {
    //alert("bla");
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetGeneralSettings",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //очищаем от предыдущих данных
            $("#contentSettings").empty();
            //очищаем от предыдущих данных
            $("#contentTable").empty();
            //вставляем данные в шаблон и добавляем его к tbody
            $("#tmplGeneralSettings").tmpl(response.d).appendTo("#contentSettings");

            $("#headerSettings").empty();
            $("#headerSettings").text("Общие настройки");

            createUserControlsGeneral();
        }
    });
}

function createUserControlsGeneral() {
    $("#userControls").empty();
    $("#userControls").append($("#userControlsGeneral").text());

    $("#userControls button").button();
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    $("#edit").click(function () {
        var inputs = $("#contentSettings input");
        for (var i = 3; i < inputs.length; i++) {
            $(inputs[i]).removeClass("inputField-readonly");
            $(inputs[i]).addClass("inputField");
            $(inputs[i]).removeAttr("readonly");
        }

        $("#edit").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        return false;
    });

    $("#save").click(function () {
        var settings = [];
        var rows = $("#contentSettings tr");
        for (var i = 3; i < rows.length; i++) {
            settings.push({ Key: $(".key", rows[i]).attr("key"), Value: $(".value input", rows[i]).attr("value") });
        }

        var order = { OrgID: $.cookie("CURRENT_ORG_ID"), GeneralSettings: settings };

        $.ajax({
            type: "POST",
            //Page Name (in which the method should be called) and method name
            url: "Settings.aspx/SaveGeneralSettings",
            data: JSON.stringify(order),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.d == true) {
                    loadGeneralSettings();
                    return false;
                }
            },
            error: function () {
                alert("Ошибка Settings.aspx/SaveGeneralSettings");
            }
        });

        return false;
    });

    $("#cancel").click(function () {
        loadGeneralSettings();
        return false;
    });
}


function loadGroupsSettings() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetGroupsSettings",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            createUserControlsGroups();
            createContentTableGroups(response);
            $("#contentTable").show();
        }
    });
}

function loadDriversSettings() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetDriversSettings",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            createUserControlsDrivers();
            createContentTableDrivers(response);
            $("#contentTable").show();
        }
    });
}

function loadTransportsSettings() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetTransportsSettings",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            createUserControlsTransports();
            createContentTableTransports(response);
            $("#contentTable").show();
        }
    });
}

function createUserControlsGroups() {
    $("#headerSettings").empty();
    $("#headerSettings").text("Настройки групп");

    $("#userControls").empty();
    $("#userControls").append($("#userControlsGroups").text());

    $("#userControls button").button();
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    $("#delete").click(function () {
        var inputs = $("#contentTable input:checkbox");
        var c = 0;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                c++;
            }
        }
        if (c > 0) {
            $("#deletedialog").dialog({ buttons: { "OK": function () {
                $(this).dialog("close");
                var keys = [];
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        key = $(inputs[i]).attr("key");
                        keys.push({ Key: "", Value: key });
                    }
                }
                deleteGroup(keys);
            },
                "Отмена": function () {
                    $(this).dialog("close");
                }
            }

            });
            $("#deletedialog").dialog("option", "closeText", '');
            $("#deletedialog").dialog("option", "resizable", false);
            $("#deletedialog").dialog("option", "modal", true);
        }

        return false;
    });

    $("#edit").click(function () {
        var inputs = $("#contentTable input:checkbox");
        var c = 0;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                c++;
            }
        }
        if (c > 0) {
            for (var i = 0; i < inputs.length; i++) {
                $(inputs[i]).hide();
                if (inputs[i].checked) {
                    key = $(inputs[i]).attr("key");
                    $("#nameinput" + key).removeClass("inputField-readonly");
                    $("#nameinput" + key).addClass("inputField");
                    $("#nameinput" + key).removeAttr("readonly");
                    $("#commentinput" + key).removeClass("inputField-readonly");
                    $("#commentinput" + key).addClass("inputField");
                    $("#commentinput" + key).removeAttr("readonly");
                }
            }

            $("#edit").button({ disabled: true });
            $("#delete").button({ disabled: true });
            $("#save").button({ disabled: false });
            $("#cancel").button({ disabled: false });
        }

        return false;
    });

    $("#save").click(function () {
        var settings = [];

        var inputs = $("#contentTable input:checkbox");
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                key = $(inputs[i]).attr("key");
                name = $("#nameinput" + key).attr("value");
                comment = $("#commentinput" + key).attr("value");
                settings.push({ Name: name, Comment: comment, grID: key, Number: 0 });
            }
        }

        var order = { OrgID: $.cookie("CURRENT_ORG_ID"), GroupSettings: settings };

        $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/SaveGroupSettings",
        data: JSON.stringify(order),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadGroupsSettings();    
        }
        });

        return false;
    });

    $("#cancel").click(function () {
        loadGroupsSettings();
        return false;
    });
}

function createUserControlsDrivers() {
    $("#headerSettings").empty();
    $("#headerSettings").text("Настройки водителей");

    $("#userControls").empty();
    $("#userControls").append($("#userControlsGroups").text());

    $("#userControls button").button();
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    $("#delete").click(function () {
        var inputs = $("#contentTable input:checkbox");
        var c = 0;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                c++;
            }
        }
        if (c > 0) {
            $("#deletedialog").dialog({ buttons: { "OK": function () {
                $(this).dialog("close");
                var keys = [];
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        key = $(inputs[i]).attr("key");
                        keys.push({ Key: "", Value: key });
                    }
                }
                deleteDrivers(keys);
            },
                "Отмена": function () {
                    $(this).dialog("close");
                }
            }

            });
            $("#deletedialog").dialog("option", "closeText", '');
            $("#deletedialog").dialog("option", "resizable", false);
            $("#deletedialog").dialog("option", "modal", true);
        }

        return false;
    });

    $("#edit").click(function () {
        var inputs = $("#contentTable input:checkbox");
        var c = 0;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                c++;
            }
        }
        if (c > 0) {
            for (var i = 0; i < inputs.length; i++) {
                $(inputs[i]).hide();
                if (inputs[i].checked) {
                    key = $(inputs[i]).attr("key");
                    $("#numberinput" + key).removeClass("inputField-readonly");
                    $("#numberinput" + key).addClass("inputField");
                    $("#numberinput" + key).removeAttr("readonly");
                    $("#nameinput" + key).removeClass("inputField-readonly");
                    $("#nameinput" + key).addClass("inputField");
                    $("#nameinput" + key).removeAttr("readonly");
                    $("#commentinput" + key).removeClass("inputField-readonly");
                    $("#commentinput" + key).addClass("inputField");
                    $("#commentinput" + key).removeAttr("readonly");
                }
            }

            $("#edit").button({ disabled: true });
            $("#delete").button({ disabled: true });
            $("#save").button({ disabled: false });
            $("#cancel").button({ disabled: false });
        }

        return false;
    });

    $("#save").click(function () {
        var settings = [];

        var inputs = $("#contentTable input:checkbox");
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                key = $(inputs[i]).attr("key");
                name = $("#nameinput" + key).attr("value");
                comment = $("#commentinput" + key).attr("value");
                number = $("#numberinput" + key).attr("value");
                settings.push({ Name: name, Comment: comment, grID: key, Number: number });
            }
        }

        var order = { OrgID: $.cookie("CURRENT_ORG_ID"), DriverSettings: settings };

        $.ajax({
            type: "POST",
            //Page Name (in which the method should be called) and method name
            url: "Settings.aspx/SaveDriverSettings",
            data: JSON.stringify(order),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                loadDriversSettings();
            }
        });

        return false;
    });

    $("#cancel").click(function () {
        loadDriversSettings();
        return false;
    });
}

function createUserControlsTransports() {
    $("#headerSettings").empty();
    $("#headerSettings").text("Настройки ТС");

    $("#userControls").empty();
    $("#userControls").append($("#userControlsGroups").text());

    $("#userControls button").button();
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    $("#delete").click(function () {
        var inputs = $("#contentTable input:checkbox");
        var c = 0;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                c++;
            }
        }
        if (c > 0) {
            $("#deletedialog").dialog({ buttons: { "OK": function () {
                $(this).dialog("close");
                var keys = [];
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        key = $(inputs[i]).attr("key");
                        keys.push({ Key: "", Value: key });
                    }
                }
                deleteTransports(keys);
            },
                "Отмена": function () {
                    $(this).dialog("close");
                }
            }

            });
            $("#deletedialog").dialog("option", "closeText", '');
            $("#deletedialog").dialog("option", "resizable", false);
            $("#deletedialog").dialog("option", "modal", true);
        }

        return false;
    });

    $("#edit").click(function () {
        var inputs = $("#contentTable input:checkbox");
        var c = 0;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                c++;
            }
        }
        if (c > 0) {
            for (var i = 0; i < inputs.length; i++) {
                $(inputs[i]).hide();
                if (inputs[i].checked) {
                    key = $(inputs[i]).attr("key");
                    $("#numberinput" + key).removeClass("inputField-readonly");
                    $("#numberinput" + key).addClass("inputField");
                    $("#numberinput" + key).removeAttr("readonly");
                    $("#nameinput" + key).removeClass("inputField-readonly");
                    $("#nameinput" + key).addClass("inputField");
                    $("#nameinput" + key).removeAttr("readonly");
                    $("#commentinput" + key).removeClass("inputField-readonly");
                    $("#commentinput" + key).addClass("inputField");
                    $("#commentinput" + key).removeAttr("readonly");
                }
            }

            $("#edit").button({ disabled: true });
            $("#delete").button({ disabled: true });
            $("#save").button({ disabled: false });
            $("#cancel").button({ disabled: false });
        }

        return false;
    });

    $("#save").click(function () {
        var settings = [];

        var inputs = $("#contentTable input:checkbox");
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                key = $(inputs[i]).attr("key");
                name = $("#nameinput" + key).attr("value");
                comment = $("#commentinput" + key).attr("value");
                number = $("#numberinput" + key).attr("value");
                settings.push({ Name: name, Comment: comment, grID: key, Number: number });
            }
        }

        var order = { OrgID: $.cookie("CURRENT_ORG_ID"), TransportSettings: settings };

        $.ajax({
            type: "POST",
            //Page Name (in which the method should be called) and method name
            url: "Settings.aspx/SaveTransportSettings",
            data: JSON.stringify(order),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                loadTransportsSettings();
            }
        });

        return false;
    });

    $("#cancel").click(function () {
        loadTransportsSettings();
        return false;
    });
}

function deleteGroup(list) {
    var order = { OrgID: $.cookie("CURRENT_ORG_ID"), GroupIDs: list };
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/DeleteGroups",
        data: JSON.stringify(order),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadGroupsSettings();
        }
    });
}

function deleteDrivers(list) {
    var order = { OrgID: $.cookie("CURRENT_ORG_ID"), DriverIDs: list };
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/DeleteDrivers",
        data: JSON.stringify(order),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadDriversSettings();
        }
    });
}

function deleteTransports(list) {
    var order = { OrgID: $.cookie("CURRENT_ORG_ID"), TransportIDs: list };
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/DeleteTransports",
        data: JSON.stringify(order),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadTransportsSettings();
        }
    });
}

function createContentTableGroups(response) {
    //очищаем tbody от предыдущих данных
    $("#contentSettings").empty();
    $("#contentSettingsPlace").empty();
    $("#contentSettingsPlace").append($("#tmplContentTable").text());

    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Номер п/п", "style": "width: 80px;"},' +
    '{"text": "Название группы", "style": "width: 150px;"},' +
    '{"text": "Комментарий", "style": ""}]');

    updateTable($("#contentTableBody"), $("#tmplGroupTableContent"), response.d);
    $("#checkbox0").hide();
}

function createContentTableDrivers(response) {
    //очищаем tbody от предыдущих данных
    $("#contentSettings").empty();
    $("#contentSettingsPlace").empty();
    $("#contentSettingsPlace").append($("#tmplContentTable").text());

    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Номер", "style": "width: 120px;"},' +
    '{"text": "ФИО", "style": "width: 150px;"},' +
    '{"text": "Комментарий", "style": ""}]');

    updateTable($("#contentTableBody"), $("#tmplGroupTableContent"), response.d);
    //$("#checkbox0").hide();
}

function createContentTableTransports(response) {
    //очищаем tbody от предыдущих данных
    $("#contentSettings").empty();
    $("#contentSettingsPlace").empty();
    $("#contentSettingsPlace").append($("#tmplContentTable").text());

    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Номер", "style": "width: 120px;"},' +
    '{"text": "Гос. номер", "style": "width: 150px;"},' +
    '{"text": "Комментарий", "style": ""}]');

    updateTable($("#contentTableBody"), $("#tmplGroupTableContent"), response.d);
    //$("#checkbox0").hide();
}