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

function createRemindControls() {
    $("#headerSettings").empty();
    $("#headerSettings").text("Напоминания");
    //$("#headerSettings").append($("#RemindMainLabels").text());
    buildRemindTree();
    //$("#contentSettings").append($("#tmplTabsContent").text());
    //$('#tabs').tabs();
    //$("#tabs-1").append($("#tmplContentTable").text());
    //loadReminds();
    
    $("#userControls").empty();
    $("#userControls").append($("#userControlsGroups").text());

    $("#userControls button").button();
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    $("#edit").click(function () {
        mode = "edit";
        var inputs = $('#contentTable [name="selectCheckbox"]');
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
                    var key = $(inputs[i]).attr("key");
                    
                    $("#userSelector" + key).wijcombobox({
                        disabled: false
                    });
                    $("#periodSelector" + key).wijcombobox({
                        disabled: false
                    });
                    $("#typeSelector" + key).wijcombobox({
                        disabled: false
                    });
                    $("#active" + key).removeClass("inputField-readonly");
                    $("#active" + key).removeAttr("readonly");
                    $("#active" + key).removeAttr("disabled");
                    $("#preview" + key).show();
                    $('#preview' + key).click(function () {
                        var key = $(this).attr("key");
                        var driverKey = $('#source' + key).attr("key");
                        var type = $('#source' + key).attr("sourceType");
                        var text = $('#source' + key).attr("value");
                        currentDriverId = '#source' + key;
                        $("#choosedialog").dialog({ buttons: {
                            "OK": function () {
                                $(this).dialog("close");
                            },
                            "Отмена": function () {
                                $(currentDriverId).attr("key", driverKey);
                                $(currentDriverId).attr("value", text);
                                $(currentDriverId).attr("sourceType", type);
                                $(this).dialog("close");
                            }
                        }
                        }
                        );
                        $("#choosedialog").dialog("option", "closeText", '');
                        $("#choosedialog").dialog("option", "resizable", false);
                        $("#choosedialog").dialog("option", "modal", true);
                        $("#choosedialog").dialog("option", "height", 350);
                        loadDriversTree(driverKey, type);
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

    $("#delete").click(function () {
        var inputs = $('#contentTable [name="selectCheckbox"]');
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
                deleteRemind(keys);
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

    $("#save").click(function () {
        if (mode == "edit") {
            var settings = [];

            var inputs = $("#contentTable input:checkbox");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    key = $(inputs[i]).attr("key");
                    user = $("#userSelector" + key).attr("user");
                    source = $("#source" + key).attr("key");
                    sourceType = $("#source" + key).attr("sourceType");
                    period = $("#periodSelector" + key).attr("period");
                    type = $("#typeSelector" + key).attr("typeSel");
                    activeBool = $("#active" + key).attr("checked");
                    var active = 0;
                    if (activeBool) {
                        active = 1;
                    }
                    settings.push({ id: key, userId: user, sourceType: sourceType, sourceId: source, type: type, periodType: period, active: active });
                }
            }

            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), RemindSettings: settings };
            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/SaveRemindSettings",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    loadReminds();
                }
            });
        }

        if (mode == "create") {
            user = $("#userSelectorNew").attr("user");
            source = $("#sourceNew").attr("key");
            sourceType = $("#sourceNew").attr("sourceType");
            period = $("#periodSelectorNew").attr("period");
            type = $("#typeSelectorNew").attr("typeSel");
            activeBool = $("#activeNew").attr("checked");
            var active = 0;
            if (activeBool) {
                active = 1;
            }

            var data = { userId: user, sourceType: sourceType, sourceId: source, periodType: period, type: type, active: active };
            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), data: data };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/CreateRemind",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    loadReminds();
                }
            });
        }

        mode = "";
        return false;
    });

    $("#create").click(function () {
        mode = "create";
        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        var trs = $("#contentTable tr");
        $(trs[0]).after($("#NewRemind").text());

        $('#previewNew').click(function () {
            var driverKey = $('#sourceNew').attr("key");
            var type = $('#sourceNew').attr("sourceType");
            var text = $('#sourceNew').attr("value");
            currentDriverId = '#sourceNew';
            loadDriversTree(driverKey, type);
            $("#choosedialog").dialog({ buttons: {
                "OK": function () {
                    $(this).dialog("close");
                },
                "Отмена": function () {
                    $(currentDriverId).attr("key", driverKey);
                    $(currentDriverId).attr("value", text);
                    $(currentDriverId).attr("sourceType", type);
                    $(this).dialog("close");
                }
            }
            }
            );
            $("#choosedialog").dialog("option", "closeText", '');
            $("#choosedialog").dialog("option", "resizable", false);
            $("#choosedialog").dialog("option", "modal", true);
            $("#choosedialog").dialog("option", "height", 350);
        });

        createRemindSelectorsSingle();

        
        return false;
    });

    $("#cancel").click(function () {
        mode = "";
        loadReminds();
        return false;
    });
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

function buildRemindTree() {
    //builds a tree
    /*$("#RemindTree").wijtree();
    $("#RemindTree").wijtree({ selectedNodeChanged: function (e, data) {
        onRemindNodeSelected(e, data);
    }
    });
    $("#SpeedRemind").wijtreenode({ selected: true });
    $("#RemindLabel1").text($("#SpeedRemind").text());*/
    //loadDriversTree();
}

function loadDriversTree(key,type) {
    $.ajax({
        type: "POST",
        url: "Data.aspx/GetOverlookDriversTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#DriversTree").wijtree("destroy");
            $("#DriversTree").empty();
            $("#tmplDriverTree").tmpl(response.d).appendTo("#DriversTree");
            $("#DriversTree").wijtree();
            $("#DriversTree").wijtree({ selectedNodeChanged: function (e, data) {
                onRemindDriverNodeSelected(e, data);
            }
            });
            $('#DriversTree [key="' + key + '"][li_type="' + type + '"]').wijtreenode({ selected: true });
            $('span .ui-icon').addClass("ui-icon-triangle-1-se");
            $('span .ui-icon').removeClass("ui-icon-triangle-1-e");
            $('.wijmo-wijtree-child').css("display", "block");
            //$("#RemindLabel2").text($("#OrgLI").attr("name"));
            //$("#DriversTree").hide();
        }
    });
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

function onRemindNodeSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");

    if (isSelected == "true") {
        key = $("a span", data.element).attr("key");
        text = $("a span", data.element).text();
        $("#RemindLabel1").text(text);
    } else {
        //$("#headerSettings").empty();
        $("#RemindLabel1").empty();
        //$("#RemindLabel2").empty();
        //$("#contentSettings").empty();
    }
}

function onRemindDriverNodeSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");

    if (isSelected == "true") {
        var key = $("a span", data.element).attr("key");
        var type = $("a span", data.element).attr("type");
        var text = $(":first a span", data.element).text();
        
        $(currentDriverId).attr("key", key);
        $(currentDriverId).attr("value", text);
        $(currentDriverId).attr("sourceType", type);
        //$("#RemindLabel2").text(text);
    } else {
        //$("#headerSettings").empty();
        //$("#RemindLabel1").empty();
        //$("#RemindLabel2").empty();
        //$("#contentSettings").empty();
    }
}

function loadGeneralSettings() {
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

function loadReminds() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetRemindList",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //$("#contentTable").show();
            //createUserControlsGroups();
            $("#DriversTree").wijtree("destroy");
            $("#DriversTree").empty();
            createContentTableRemind(response);
            createRemindSelectors();
            createRemindControls();
            $("#contentTable").show();
        }
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

        $("#newGroupSelector").wijcombobox(
        {
            showingAnimation: { effect: "blind" },
            hidingAnimation: { effect: "blind" }
        });

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
                    $(selInputs[i - 1]).wijcombobox(
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

            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), Name: name, Comment: comment, CardType: card };

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

            var data = { Name: name, Comment: comment, Number: number, groupID: card };
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

function deleteRemind(list) {
    var order = { OrgID: $.cookie("CURRENT_ORG_ID"), RemindIDs: list };
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/DeleteReminds",
        data: JSON.stringify(order),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadReminds();
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

function createContentTableRemind(response) {
    //очищаем tbody от предыдущих данных
    $("#contentSettings").empty();
    $("#contentSettingsPlace").empty();
    $("#contentSettingsPlace").append($("#tmplContentTable").text());
    $("#contentTable").show();
    createTableHeader($("#contentTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Кому", "style": "width: 120px;"},' +
    '{"text": "Водитель", "style": "width: 120px;"},' +
    '{"text": "Периодичность", "style": "width: 120px;"},' +
    '{"text": "Последняя отправка", "style": "width: 120px;"},' +
    '{"text": "Тип напоминания", "style": "width: 120px;"},' +
    '{"text": "Активно", "style": "width: 50px;"}]');
    updateTable($("#contentTableBody"), $("#tmplRemindTable"), response.d);
    //$("#checkbox1").hide();

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
            createSelectors(response, "groupSelector", "group");            
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
            createSelectors(response, "groupSelector", "group");
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
            createSelector(selector, response, "group");
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
            createSelector(selector, response, "group");
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

function createSelectors(response, name, attr) {
    var selectors = [];
    selectors = $('select[name="' + name + '"]');
    for (var i = 0; i < selectors.length; i++) {
        $("#tmplOption").tmpl(response.d).appendTo(selectors[i]);
        var group = $(selectors[i]).attr(attr);
        //alert(group);
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

function createSelector(selector, response, attr) {
    $("#tmplOption").tmpl(response.d).appendTo(selector);
    var group = $(selector).attr(attr);
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

function createRemindSelectors() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetRemindTypeList",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            createSelectors(response, "typeSelector", "typeSel");
        }
    });
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetRemindPeriodTypeList",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            createSelectors(response, "periodSelector", "period");
        }
    });
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetUserList",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            createSelectors(response, "userSelector", "user");
        }
    });
}

function createRemindSelectorsSingle() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetRemindTypeList",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            createSelector($("#typeSelectorNew"), response, "typeSel");
        }
    });
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetRemindPeriodTypeList",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            createSelector($("#periodSelectorNew"), response, "period");
        }
    });
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetUserList",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            createSelector($("#userSelectorNew"), response, "user");
            value = $("#userSelectorNew option:first").attr("value");
            $("#userSelectorNew option:first").attr("selected", "true");
            $("#userSelectorNew").attr("user",value);
        }
    });
}

function loadUserList() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetUserList",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#tmplOption").tmpl(response.d).appendTo("#whomSelector");
            $("#whomSelector").wijcombobox({
                showingAnimation: { effect: "blind" },
                hidingAnimation: { effect: "blind" },
                isEditable: false
            });
        }
    });   
}