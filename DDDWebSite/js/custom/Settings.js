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
            loadDefaultSettings();
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
            //$("#contentTable").show();
            createUserControlsGroups();
            createContentTableGroups(response);
            createGroupCardTypeSelectors();
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
            createGroupSelectorDrivers();
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
            createGroupSelectorTransports();
            $("#contentTable").show();
        }
    });
}

function loadDefaultSettings() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetDefaultSettings",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //$("#contentTable").show();
            createUserControlsDefault();
            createContentTableDefault(response);
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

    $("#create").click(function () {
        mode = "create";
        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        var trs = $("#contentTable tr");
        $(trs[0]).after($("#NewGroup").text());

        return false;
    });

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
        mode = "edit";
        var inputs = $("#contentTable input:checkbox");
        var selInputs = $("#contentTable select");
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
                    $(selInputs[i-1]).wijcombobox(
                    {
                        disabled: false
                    });
                }
            }

            $("#edit").button({ disabled: true });
            $("#delete").button({ disabled: true });
            $("#create").button({ disabled: true });
            $("#save").button({ disabled: false });
            $("#cancel").button({ disabled: false });
        }

        return false;
    });

    $("#save").click(function () {
        if (mode == "edit") {
            var settings = [];

            var inputs = $("#contentTable input:checkbox");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    key = $(inputs[i]).attr("key");
                    name = $("#nameinput" + key).attr("value");
                    comment = $("#commentinput" + key).attr("value");
                    card = $("#groupSelector" + key).attr("card");
                    settings.push({ Name: name, Comment: comment, grID: key, Number: 0, cardType: card });
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
        }

        if (mode == "create") {
            name = $("#newNameinputGroup").attr("value");
            comment = $("#newCommentinputGroup").attr("value");
            card = $("#newGroupSelector").attr("card");
            
            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), Name: name, Comment: comment, CardType: card};

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/CreateGroup",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    loadGroupsSettings();
                }
            });
        }

        mode = "";
        return false;
    });

    $("#cancel").click(function () {
        mode = "";
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

    $("#create").click(function () {
        mode = "create";
        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        var trs = $("#contentTable tr");
        $(trs[0]).after($("#newCard").text());
        createGroupSelectorDriversSingle($("#newCardGroupSelector"));

        return false;
    });

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
        mode = "edit";
        var inputs = $("#contentTable input:checkbox");
        var selInputs = $("#contentTable select");
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
                    $(selInputs[i]).wijcombobox(
                    {
                        disabled: false
                    });
                }
            }

            $("#edit").button({ disabled: true });
            $("#delete").button({ disabled: true });
            $("#create").button({ disabled: true });
            $("#save").button({ disabled: false });
            $("#cancel").button({ disabled: false });
        }

        return false;
    });

    $("#save").click(function () {
        if (mode == "edit") {
            var settings = [];

            var inputs = $("#contentTable input:checkbox");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    key = $(inputs[i]).attr("key");
                    name = $("#nameinput" + key).attr("value");
                    comment = $("#commentinput" + key).attr("value");
                    number = $("#numberinput" + key).attr("value");
                    group = $("#groupSelector" + key).attr("group");
                    settings.push({ Name: name, Comment: comment, grID: key, Number: number, groupID: group });
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
        }
        if (mode == "create") {
            name = $("#newCardName").attr("value");
            comment = $("#newCardComment").attr("value");
            number = $("#newCardNumber").attr("value");
            card = $("#newCardGroupSelector").attr("group");
            
            var data={Name: name, Comment: comment, Number: number, groupID: card};
            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), data: data, UserID: 0 };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/CreateCardDriver",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    loadDriversSettings();
                }
            });
        }
        mode = "";
        return false;
    });

    $("#cancel").click(function () {
        mode = "";
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


    $("#create").click(function () {
        mode = "create";
        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        var trs = $("#contentTable tr");
        $(trs[0]).after($("#newCard").text());
        createGroupSelectorTransportsSingle($("#newCardGroupSelector"));

        return false;
    });

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
        mode = "edit";
        var inputs = $("#contentTable input:checkbox");
        var selInputs = $("#contentTable select");
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
                    $(selInputs[i]).wijcombobox(
                    {
                        disabled: false
                    });
                }
            }

            $("#edit").button({ disabled: true });
            $("#delete").button({ disabled: true });
            $("#create").button({ disabled: true });
            $("#save").button({ disabled: false });
            $("#cancel").button({ disabled: false });
        }

        return false;
    });

    $("#save").click(function () {
        if (mode == "edit") {
            var settings = [];

            var inputs = $("#contentTable input:checkbox");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    key = $(inputs[i]).attr("key");
                    name = $("#nameinput" + key).attr("value");
                    comment = $("#commentinput" + key).attr("value");
                    number = $("#numberinput" + key).attr("value");
                    group = $("#groupSelector" + key).attr("group");
                    settings.push({ Name: name, Comment: comment, grID: key, Number: number, groupID: group });
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
        }
        if (mode == "create") {
            name = $("#newCardName").attr("value");
            comment = $("#newCardComment").attr("value");
            number = $("#newCardNumber").attr("value");
            card = $("#newCardGroupSelector").attr("group");

            var data = { Name: name, Comment: comment, Number: number, groupID: card };
            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), data: data, UserID: 0 };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/CreateCardTransport",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    loadTransportsSettings();
                }
            });
        }
        mode = "";
        return false;
    });

    $("#cancel").click(function () {
        loadTransportsSettings();
        return false;
    });
}

function createUserControlsDefault() {
    $("#headerSettings").empty();
    $("#headerSettings").text("Настройки по умолчанию");

    $("#userControls").empty();
    $("#userControls").append($("#userControlsDefault").text());

    $("#userControls button").button();
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    $("#edit").click(function () {
        var inputs = $("#contentTable input:checkbox");
        var selInputs = $("#contentTable select");
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
                    $('#MinValue' + key).removeClass("inputField-readonly");
                    $('#MinValue' + key).addClass("inputField");
                    $('#MinValue' + key).removeAttr("readonly");

                    $('#MaxValue' + key).removeClass("inputField-readonly");
                    $('#MaxValue' + key).addClass("inputField");
                    $('#MaxValue' + key).removeAttr("readonly");

                    $('#CriteriaNote' + key).removeClass("inputField-readonly");
                    $('#CriteriaNote' + key).addClass("inputField");
                    $('#CriteriaNote' + key).removeAttr("readonly");
                }
            }

            $("#edit").button({ disabled: true });
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
                min = $('#MinValue' + key).attr("value");
                max = $('#MaxValue' + key).attr("value");
                note = $('#CriteriaNote' + key).attr("value");
                settings.push({ keyID: key, MinValue: min, MaxValue: max, CriteriaNote: note });
            }
        }

        var order = { DefaultSettings: settings };

        $.ajax({
            type: "POST",
            //Page Name (in which the method should be called) and method name
            url: "Settings.aspx/SaveDefaultSettings",
            data: JSON.stringify(order),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                loadDefaultSettings();
            }
        });

        return false;
    });

    $("#cancel").click(function () {
        loadDefaultSettings();
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
    $("#contentTable").show();
    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Номер п/п", "style": "width: 80px;"},' +
    '{"text": "Название группы", "style": "width: 150px;"},' +
    '{"text": "Комментарий", "style": "width: 250px;"},' +
    '{"text": "Тип карты", "style": ""}]');

    updateTable($("#contentTableBody"), $("#tmplGroupTableContent"), response.d);
    $("#checkbox1").hide();
    
}

function createContentTableDrivers(response) {
    //очищаем tbody от предыдущих данных
    $("#contentSettings").empty();
    $("#contentSettingsPlace").empty();
    $("#contentSettingsPlace").append($("#tmplContentTable").text());

    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Номер", "style": "width: 170px;"},' +
    '{"text": "ФИО", "style": "width: 200px;"},' +
    '{"text": "Комментарий", "style": "width: 250px;"},' +
    '{"text": "Группа", "style": ""}]');

    updateTable($("#contentTableBody"), $("#tmplCardTableContent"), response.d);
    //$("#checkbox0").hide();
}

function createContentTableTransports(response) {
    //очищаем tbody от предыдущих данных
    $("#contentSettings").empty();
    $("#contentSettingsPlace").empty();
    $("#contentSettingsPlace").append($("#tmplContentTable").text());

    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Номер", "style": "width: 170px;"},' +
    '{"text": "Гос. номер", "style": "width: 200px;"},' +
    '{"text": "Комментарий", "style": "width: 250px;"},' +
    '{"text": "Группа", "style": ""}]');

    updateTable($("#contentTableBody"), $("#tmplCardTableContent"), response.d);
    //$("#checkbox0").hide();
}

function createContentTableDefault(response) {
    //очищаем tbody от предыдущих данных
    $("#contentSettings").empty();
    $("#contentSettingsPlace").empty();
    $("#contentSettingsPlace").append($("#tmplContentTable").text());

    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Название критерия", "style": "width: 150px;"},' +
    '{"text": "Единица измерения", "style": "width: 150px;"},' +
    '{"text": "Мин. Значение", "style": "width: 100px;"},' +
    '{"text": "Макс. Значение", "style": "width: 100px;"},' +
    '{"text": "Комментарий", "style": "width: 200px;"}]');

    updateTable($("#contentTableBody"), $("#tmplDefaultSettingsTable"), response.d);
}

function createGroupSelectorDrivers() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetGroupListDrivers",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var selectors = [];
            selectors = $('select[name="groupSelector"]');
            for (var i = 0; i < selectors.length; i++) {
                $("#tmplOption").tmpl(response.d).appendTo(selectors[i]);
                var group = $(selectors[i]).attr("group");
                var options = $("#" + selectors[i].id + " option");
                for (var j = 0; j < options.length; j++) {
                    var option = $(options[j]).attr("value")
                    if (option == group) {
                        $(options[j]).attr("selected", true);
                    }
                }
                $(selectors[i]).wijcombobox(
                {
                    showingAnimation: { effect: "blind" },
                    hidingAnimation: { effect: "blind" },
                    isEditable: false,
                    disabled: true
                });
            }
        }
    });
}

function createGroupSelectorTransports() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetGroupListTransports",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var selectors = [];
            selectors = $('select[name="groupSelector"]');
            for (var i = 0; i < selectors.length; i++) {
                $("#tmplOption").tmpl(response.d).appendTo(selectors[i]);
                var group = $(selectors[i]).attr("group");
                var options = $("#" + selectors[i].id + " option");
                for (var j = 0; j < options.length; j++) {
                    var option = $(options[j]).attr("value")
                    if (option == group) {
                        $(options[j]).attr("selected", true);
                        break;
                    }
                }
                $(selectors[i]).wijcombobox(
                {
                    showingAnimation: { effect: "blind" },
                    hidingAnimation: { effect: "blind" },
                    isEditable: false,
                    disabled: true
                });
            }
        }
    });
}

function createGroupSelectorDriversSingle(selector) {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetGroupListDrivers",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#tmplOption").tmpl(response.d).appendTo(selector);
            var group = $(selector).attr("group");
            var options = $("#" + selector.id + " option");
            for (var j = 0; j < options.length; j++) {
                var option = $(options[j]).attr("value")
                if (option == group) {
                    $(options[j]).attr("selected", true);
                    break;
                }
            }
            $(selector).wijcombobox(
                {
                    showingAnimation: { effect: "blind" },
                    hidingAnimation: { effect: "blind" },
                    isEditable: false
                    //disabled: true
                });
        }
        });
   }

   function createGroupSelectorTransportsSingle(selector) {
        $.ajax({
            type: "POST",
            //Page Name (in which the method should be called) and method name
            url: "Settings.aspx/GetGroupListTransports",
            data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#tmplOption").tmpl(response.d).appendTo(selector);
                var group = $(selector).attr("group");
                var options = $("#" + selector.id + " option");
                for (var j = 0; j < options.length; j++) {
                    var option = $(options[j]).attr("value")
                    if (option == group) {
                        $(options[j]).attr("selected", true);
                        break;
                    }
                }
                $(selector).wijcombobox(
                {
                    showingAnimation: { effect: "blind" },
                    hidingAnimation: { effect: "blind" },
                    isEditable: false
                    //                   disabled: true
                });
            }
        });
    }

function createGroupCardTypeSelectors() {
    $("#groupSelector1").remove();

    var selectors = [];
    selectors = $('select[name="groupSelector"]');
    for (var i = 0; i < selectors.length; i++) {
        var group = $(selectors[i]).attr("card");
        var options = $("#" + selectors[i].id + " option");
        for (var j = 0; j < options.length; j++) {
            var option = $(options[j]).attr("value");
            if (option == group) {
                $(options[j]).attr("selected", true);
                break;
            }
        }
        $(selectors[i]).wijcombobox(
        {
            showingAnimation: { effect: "blind" },
            hidingAnimation: { effect: "blind" },
            isEditable: false,
            disabled: true
        });
    }
}