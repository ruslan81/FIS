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
            if (i < rows.length) {
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
                        },
                            closeText: '',
                            resizable: false,
                            modal: true,
                            height: 350
                        });
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
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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

function loadDriversTree(key, type) {
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
            //if (!(key == "" || type == "")) {
            $('#DriversTree [key="' + key + '"][li_type="' + type + '"]').wijtreenode({ selected: true });
            $('span .ui-icon').addClass("ui-icon-triangle-1-se");
            $('span .ui-icon').removeClass("ui-icon-triangle-1-e");
            $('.wijmo-wijtree-child').css("display", "block");
            //}
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadDriversTreeSingle(key, type) {
    $.ajax({
        type: "POST",
        url: "Data.aspx/GetOverlookDriversTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#DriversTreeSingle").wijtree("destroy");
            $("#DriversTreeSingle").empty();
            $("#tmplDriverTree").tmpl(response.d).appendTo("#DriversTreeSingle");
            $("#DriversTreeSingle").wijtree();
            $("#DriversTreeSingle").wijtree({ selectedNodeChanged: function (e, data) {
                onSingleDriverNodeSelected(e, data);
            }
            });
            if (!(key == "" || type == "")) {
                $('#DriversTreeSingle [key="' + key + '"][li_type="' + type + '"]').wijtreenode({ selected: true });
                $('span .ui-icon').addClass("ui-icon-triangle-1-se");
                $('span .ui-icon').removeClass("ui-icon-triangle-1-e");
                $('.wijmo-wijtree-child').css("display", "block");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadVehiclesTreeSingle(key, type) {
    $.ajax({
        type: "POST",
        url: "Data.aspx/GetOverlookVehiclesTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#VehiclesTreeSingle").wijtree("destroy");
            $("#VehiclesTreeSingle").empty();
            $("#tmplDriverTree").tmpl(response.d).appendTo("#VehiclesTreeSingle");
            $("#VehiclesTreeSingle").wijtree();
            $("#VehiclesTreeSingle").wijtree({ selectedNodeChanged: function (e, data) {
                onSingleVehicleNodeSelected(e, data);
            }
            });
            if (!(key == "" || type == "")) {
                $('#VehiclesTreeSingle [key="' + key + '"][li_type="' + type + '"]').wijtreenode({ selected: true });
                $('span .ui-icon').addClass("ui-icon-triangle-1-se");
                $('span .ui-icon').removeClass("ui-icon-triangle-1-e");
                $('.wijmo-wijtree-child').css("display", "block");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadGroupsTreeSingle(key, type) {
    $.ajax({
        type: "POST",
        url: "Data.aspx/GetGroupsTree",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#GroupsTreeSingle").wijtree("destroy");
            $("#GroupsTreeSingle").empty();
            $("#tmplDriverTree").tmpl(response.d).appendTo("#GroupsTreeSingle");
            $("#GroupsTreeSingle").wijtree();
            $("#GroupsTreeSingle").wijtree({ selectedNodeChanged: function (e, data) {
                onSingleGroupNodeSelected(e, data);
            }
            });
            if (!(key == "" || type == "")) {
                $('#GroupsTreeSingle [key="' + key + '"][li_type="' + type + '"]').wijtreenode({ selected: true });
                $('span .ui-icon').addClass("ui-icon-triangle-1-se");
                $('span .ui-icon').removeClass("ui-icon-triangle-1-e");
                $('.wijmo-wijtree-child').css("display", "block");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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

//Событие при выделении узла дерева
function onSingleDriverNodeSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");
    type = $("a span", data.element).attr("type");
    $("#userControls").empty();
    if (isSelected == "true") {
        if (type == "0") {
            currentCardId = $("a span", data.element).attr("key");
            selectedNodeType = "0";
            loadSingleDriverSettings();
        }
        if (type == "1") {
            currentCardId = "-1";
            selectedNodeType = $("a span", data.element).attr("key");
            loadSingleDriverSettings();
        }
    } else {
        //$("#headerSettings").empty();
        selectedNodeType = "-1";
        currentCardId = "-1";
        $("#contentSettings").empty();
    }
}

//Событие при выделении узла дерева
function onSingleVehicleNodeSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");
    type = $("a span", data.element).attr("type");
    $("#userControls").empty();
    if (isSelected == "true") {
        if (type == "0") {
            currentCardId = $("a span", data.element).attr("key");
            selectedNodeType = "0";
            loadSingleVehicleSettings();
        }
        if (type == "1") {
            currentCardId = "-1";
            selectedNodeType = $("a span", data.element).attr("key");
            loadSingleVehicleSettings();
        }
    } else {
        //$("#headerSettings").empty();
        $("#contentSettings").empty();
        selectedNodeType = "-1";
        currentCardId = "-1";
    }
}


//Событие при выделении узла дерева
function onSingleGroupNodeSelected(e, data) {
    isSelected = $("div", data.element).attr("aria-selected");
    type = $("a span", data.element).attr("type");
    $("#userControls").empty();
    if (isSelected == "true") {
        if (type == "0") {
            currentCardId = $("a span", data.element).attr("key");
            selectedNodeType = "0";
            loadSingleGroupSettings();
        }
        if (type == "1") {
            currentCardId = "-1";
            selectedNodeType = $("a span", data.element).attr("key");
            loadSingleGroupSettings();
        }
    } else {
        //$("#headerSettings").empty();
        $("#contentSettings").empty();
        selectedNodeType = "-1";
        currentCardId = "-1";
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

            resizeAllMaster();
            resizeSettings();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function createUserControlsGeneral() {
    if (mode != "cancel") {
        $("#userControls").empty();
        $("#userControls").append($("#userControlsGeneral").text());
        $("#userControls button").button();
    } else {
        mode = "";
    }
    $("#userControls button").button({ disabled: false });
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
            error: function (jqXHR, textStatus, errorThrown) {
                showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
            }
        });

        return false;
    });

    $("#cancel").click(function () {
        mode = "cancel";
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

            resizeAllMaster();
            resizeSettings();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadSingleDriverSettings() {
    $("#headerSettings").empty();
    $("#headerSettings").text("Настройки водителей");
    $("#tabs").tabs("destroy");
    $("#contentSettings").empty();
    $("#contentSettingsPlace").empty();
    //$("#userControls").empty();

    if (currentCardId == "-1" && selectedNodeType == "0") {
        return;
    }
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetDriverSettings",
        data: "{'CardID':'" + currentCardId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (selectedNodeType == "0") {
                $("#tmplSingleDriverData").tmpl(response.d).appendTo("#contentSettings");
            } else {
                var param = { Name: "", Number: "", Comment: "", groupID: selectedNodeType };
                $("#tmplSingleDriverData").tmpl(param).appendTo("#contentSettings");
            }
            $("#tabs").tabs();
            $("#tabs").tabs({ show: function (e, ui) {
                if (ui.index == 0) {
                }
                if (ui.index == 1) {
                    $("#lang").wijcombobox("destroy");
                    $("#lang").wijcombobox({
                        showingAnimation: { effect: "blind" },
                        hidingAnimation: { effect: "blind" },
                        disabled: true
                    });
                    if (mode == "edit" || mode == "create") {
                        $("#lang").wijcombobox({
                            disabled: false
                        });
                        if ($("#lang").attr("langId") == 0) {
                            $("#lang").wijcombobox({
                                selectedIndex: 0
                            });
                        }
                    }
                }
                return false;
            }
            });
            //loadLangList();
            $("#contentTable").show();
            createGroupSelectorDrivers();
            createUserControlsSingleDriver();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadSingleVehicleSettings() {
    $("#tabs").tabs("destroy");
    $("#headerSettings").empty();
    $("#headerSettings").text("Настройки транспортных средств");
    $("#contentSettings").empty();
    $("#contentSettingsPlace").empty();
    //$("#userControls").empty();

    if (currentCardId == "-1" && selectedNodeType == "0") {
        return;
    }
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetTransportSettings",
        data: "{'CardID':'" + currentCardId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (selectedNodeType == "0") {
                $("#tmplSingleVehicleData").tmpl(response.d).appendTo("#contentSettings");
            } else {
                var param = { Name: "", Number: "", Comment: "", groupID: selectedNodeType };
                $("#tmplSingleVehicleData").tmpl(param).appendTo("#contentSettings");
            }
            $("#tabs").tabs();
            $("#tabs").tabs('select', 0);
            createGroupSelectorTransports();
            $("#contentTable").show();
            createUserControlsSingleTransport();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadSingleGroupSettings() {
    $("#headerSettings").empty();
    $("#headerSettings").text("Настройки групп");
    $("#contentSettings").empty();
    $("#contentSettingsPlace").empty();
    //$("#userControls").empty();

    if (currentCardId == "-1" && selectedNodeType == "0") {
        return;
    }
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetGroupSettings",
        data: "{'CardID':'" + currentCardId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (selectedNodeType == "0") {
                $("#tmplSingleGroupData").tmpl(response.d).appendTo("#contentSettings");
            } else {
                var param = { Name: "", Number: "", Comment: "", cardType: selectedNodeType };
                $("#tmplSingleGroupData").tmpl(param).appendTo("#contentSettings");
            }
            createGroupSelectorGroups();
            $("#contentTable").show();
            createUserControlsSingleGroup();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function createUserControlsGroups() {
    $("#headerSettings").empty();
    $("#headerSettings").text("Настройки групп");
    if (mode != "cancel") {
        $("#userControls").empty();
        $("#userControls").append($("#userControlsGroups").text());
        $("#userControls button").button();
    }
    $("#userControls button").button({ disabled: false });
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    if (mode = "cancel") { mode = ""; return; }

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
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        }

        mode = "";
        return false;
    });

    $("#cancel").click(function () {
        mode = "cancel";
        loadGroupsSettings();
        return false;
    });
}

function createUserControlsDrivers() {
    $("#headerSettings").empty();
    $("#headerSettings").text("Настройки водителей");

    if (mode != "cancel") {
        $("#userControls").empty();
        $("#userControls").append($("#userControlsGroups").text());
        $("#userControls button").button();
    }
    $("#userControls button").button({ disabled: false });

    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    if (mode == "cancel") { mode = ""; return; }

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
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        }
        if (mode == "create") {
            name = $("#newCardName").attr("value");
            comment = $("#newCardComment").attr("value");
            number = $("#newCardNumber").attr("value");
            card = $("#newCardGroupSelector").attr("group");

            var data = { Name: name, Comment: comment, Number: number, groupID: card };
            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), data: data, UserID: $.cookie("CURRENT_USERNAME") };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/CreateCardDriver",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    loadDriversSettings();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        }
        mode = "";
        return false;
    });

    $("#cancel").click(function () {
        mode = "cancel";
        loadDriversSettings();
        return false;
    });
}

function createUserControlsSingleDriver() {
    if (mode != "cancel") {
        $("#userControls").empty();
        $("#userControls").append($("#userControlsGroups").text());
        $("#userControls button").button();
    }
    $("#userControls button").button({ disabled: false });
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });
    if (selectedNodeType != "0") {
        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
    }

    var today = new Date();
    var todaystr = "" + convert(today);

    $("#tabs .datepicker").datepicker();
    $("#tabs .datepicker").datepicker("option", "dateFormat", "dd.mm.yy");
    $("#tabs .datepicker").datepicker("setDate", todaystr);
    $("#tabs .datepicker").datepicker($.datepicker.regional['ru']);
    $("#tabs .datepicker").datepicker('disable');

    if (mode == "cancel") { mode = ""; return; }

    $("#create").click(function () {
        mode = "create";
        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        $("#tabs .input").removeClass("inputField-readonly");
        $("#tabs .input").addClass("inputField");
        $("#tabs .input").removeAttr("readonly");
        $("#tabs .input").attr("value", "");

        createGroupSelectorDriversSingle($("#groupSelectorSingle"));
        $("#groupSelectorSingle").wijcombobox(
                {
                    disabled: false
                });

        var today = new Date();
        var todaystr = "" + convert(today);
        $("#tabs .datepicker").datepicker("setDate", todaystr);
        $("#tabs .datepicker").datepicker('enable');

        $("#lang").wijcombobox({
            disabled: false
        });
        if ($("#lang").attr("langId") == 0) {
            $("#lang").wijcombobox({
                selectedIndex: 0
            });
        }
        return false;
    });

    $("#delete").click(function () {
        if (currentCardId == "-1") {
            return;
        }
        $("#deletedialog").dialog({ buttons: { "OK": function () {
            $(this).dialog("close");
            var keys = [];
            keys.push({ Key: "", Value: currentCardId });
            deleteDriversSingle(keys);
        },
            "Отмена": function () {
                $(this).dialog("close");
            }
        }

        });
        $("#deletedialog").dialog("option", "closeText", '');
        $("#deletedialog").dialog("option", "resizable", false);
        $("#deletedialog").dialog("option", "modal", true);

        return false;
    });

    $("#edit").click(function () {
        mode = "edit";

        $("#tabs .input").removeClass("inputField-readonly");
        $("#tabs .input").addClass("inputField");
        $("#tabs .input").removeAttr("readonly");

        $("#groupSelectorSingle").wijcombobox(
                    {
                        disabled: false
                    });

        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        $("#tabs .datepicker").datepicker('enable');
        $("#lang").wijcombobox({
            disabled: false
        });
        if ($("#lang").attr("langId") == 0) {
            $("#lang").wijcombobox({
                selectedIndex: 0
            });
        }

        return false;
    });

    $("#save").click(function () {
        if (mode == "edit") {
            var settings = [];
            name = $("#nameinputSingle").attr("value");
            comment = $("#commentinputSingle").attr("value");
            number = $("#numberinputSingle").attr("value");
            group = $("#groupSelectorSingle").attr("group");
            settings.push({ Name: name, Comment: comment, grID: currentCardId, Number: number, groupID: group });

            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), DriverSettings: settings };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/SaveDriverSettings",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    loadSingleDriverSettings();
                    loadDriversTreeSingle(currentCardId, "0");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        }
        if (mode == "create") {
            name = $("#nameinputSingle").attr("value");
            comment = $("#commentinputSingle").attr("value");
            number = $("#numberinputSingle").attr("value");
            group = $("#groupSelectorSingle").attr("group");

            var data = { Name: name, Comment: comment, Number: number, groupID: group };
            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), data: data, UserID: $.cookie("CURRENT_USERNAME") };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/CreateCardDriver",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    currentCardId = "-1";
                    selectedNodeType = "-1";
                    loadSingleDriverSettings();
                    loadDriversTreeSingle("", "");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        }
        mode = "";
        return false;
    });

    $("#cancel").click(function () {
        mode = "cancel";
        loadSingleDriverSettings();
        return false;
    });
}

function createUserControlsSingleTransport() {
    if (mode != "cancel") {
        $("#userControls").empty();
        $("#userControls").append($("#userControlsGroups").text());
        $("#userControls button").button();
    }
    $("#userControls button").button({ disabled: false });
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });
    if (selectedNodeType != "0") {
        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
    }

    if (mode == "cancel") { mode = ""; return; }

    $("#create").click(function () {
        mode = "create";
        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        $("#numberinputSingle").removeClass("inputField-readonly");
        $("#numberinputSingle").addClass("inputField");
        $("#numberinputSingle").removeAttr("readonly");
        $("#numberinputSingle").attr("value", "");
        $("#nameinputSingle").removeClass("inputField-readonly");
        $("#nameinputSingle").addClass("inputField");
        $("#nameinputSingle").removeAttr("readonly");
        $("#nameinputSingle").attr("value", "");
        $("#commentinputSingle").removeClass("inputField-readonly");
        $("#commentinputSingle").addClass("inputField");
        $("#commentinputSingle").removeAttr("readonly");
        $("#commentinputSingle").attr("value", "");
        createGroupSelectorDriversSingle($("#groupSelectorSingle"));
        $("#groupSelectorSingle").wijcombobox(
                {
                    disabled: false
                });
        return false;
    });

    $("#delete").click(function () {
        if (currentCardId == "-1") {
            return;
        }
        $("#deletedialog").dialog({ buttons: { "OK": function () {
            $(this).dialog("close");
            var keys = [];
            keys.push({ Key: "", Value: currentCardId });
            deleteTransportsSingle(keys);
        },
            "Отмена": function () {
                $(this).dialog("close");
            }
        }

        });
        $("#deletedialog").dialog("option", "closeText", '');
        $("#deletedialog").dialog("option", "resizable", false);
        $("#deletedialog").dialog("option", "modal", true);

        return false;
    });

    $("#edit").click(function () {
        mode = "edit";

        $("#numberinputSingle").removeClass("inputField-readonly");
        $("#numberinputSingle").addClass("inputField");
        $("#numberinputSingle").removeAttr("readonly");
        $("#nameinputSingle").removeClass("inputField-readonly");
        $("#nameinputSingle").addClass("inputField");
        $("#nameinputSingle").removeAttr("readonly");
        $("#commentinputSingle").removeClass("inputField-readonly");
        $("#commentinputSingle").addClass("inputField");
        $("#commentinputSingle").removeAttr("readonly");
        $("#groupSelectorSingle").wijcombobox(
                    {
                        disabled: false
                    });

        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        return false;
    });

    $("#save").click(function () {
        if (mode == "edit") {
            var settings = [];
            name = $("#nameinputSingle").attr("value");
            comment = $("#commentinputSingle").attr("value");
            number = $("#numberinputSingle").attr("value");
            group = $("#groupSelectorSingle").attr("group");
            settings.push({ Name: name, Comment: comment, grID: currentCardId, Number: number, groupID: group });

            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), TransportSettings: settings };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/SaveTransportSettings",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    loadSingleVehicleSettings();
                    loadVehiclesTreeSingle(currentCardId, "0");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        }
        if (mode == "create") {
            name = $("#nameinputSingle").attr("value");
            comment = $("#commentinputSingle").attr("value");
            number = $("#numberinputSingle").attr("value");
            group = $("#groupSelectorSingle").attr("group");

            var data = { Name: name, Comment: comment, Number: number, groupID: group };
            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), data: data, UserID: $.cookie("CURRENT_USERNAME") };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/CreateCardTransport",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    currentCardId = "-1";
                    selectedNodeType = "-1";
                    loadSingleVehicleSettings();
                    loadVehiclesTreeSingle("", "");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        }
        mode = "";
        return false;
    });

    $("#cancel").click(function () {
        mode = "cancel";
        loadSingleVehicleSettings();
        return false;
    });
}

/*function recreateUserControls() {
if (mode != "cancel") {
$("#userControls").empty();
$("#userControls").append($("#userControlsGroups").text());
$("#userControls button").button();
} else {
mode = "";
}
$("#userControls button").button({ disabled: false });
}*/

function createUserControlsSingleGroup() {
    if (mode != "cancel") {
        $("#userControls").empty();
        $("#userControls").append($("#userControlsGroups").text());
        $("#userControls button").button();
    }
    $("#userControls button").button({ disabled: false });
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });
    if (selectedNodeType != "0") {
        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
    }

    if (mode == "cancel") { mode = ""; return; }

    $("#create").click(function () {
        mode = "create";
        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        $("#numberinputSingle").removeClass("inputField-readonly");
        $("#numberinputSingle").addClass("inputField");
        $("#numberinputSingle").removeAttr("readonly");
        $("#numberinputSingle").attr("value", "");
        $("#nameinputSingle").removeClass("inputField-readonly");
        $("#nameinputSingle").addClass("inputField");
        $("#nameinputSingle").removeAttr("readonly");
        $("#nameinputSingle").attr("value", "");
        $("#commentinputSingle").removeClass("inputField-readonly");
        $("#commentinputSingle").addClass("inputField");
        $("#commentinputSingle").removeAttr("readonly");
        $("#commentinputSingle").attr("value", "");
        createGroupSelectorDriversSingle($("#groupSelectorSingle"));
        $("#groupSelectorSingle").wijcombobox(
                {
                    disabled: false
                });
        return false;
    });

    $("#delete").click(function () {
        if (currentCardId == "-1") {
            return;
        }
        $("#deletedialog").dialog({ buttons: { "OK": function () {
            $(this).dialog("close");
            var keys = [];
            keys.push({ Key: "", Value: currentCardId });
            deleteGroupSingle(keys);
        },
            "Отмена": function () {
                $(this).dialog("close");
            }
        }

        });
        $("#deletedialog").dialog("option", "closeText", '');
        $("#deletedialog").dialog("option", "resizable", false);
        $("#deletedialog").dialog("option", "modal", true);

        return false;
    });

    $("#edit").click(function () {
        mode = "edit";

        $("#numberinputSingle").removeClass("inputField-readonly");
        $("#numberinputSingle").addClass("inputField");
        $("#numberinputSingle").removeAttr("readonly");
        $("#nameinputSingle").removeClass("inputField-readonly");
        $("#nameinputSingle").addClass("inputField");
        $("#nameinputSingle").removeAttr("readonly");
        $("#commentinputSingle").removeClass("inputField-readonly");
        $("#commentinputSingle").addClass("inputField");
        $("#commentinputSingle").removeAttr("readonly");
        $("#groupSelectorSingle").wijcombobox(
                    {
                        disabled: false
                    });

        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });

        return false;
    });

    $("#save").click(function () {
        if (mode == "edit") {
            var settings = [];
            name = $("#nameinputSingle").attr("value");
            comment = $("#commentinputSingle").attr("value");
            group = $("#groupSelectorSingle").attr("card");
            settings.push({ Name: name, Comment: comment, grID: currentCardId, cardType: group });

            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), GroupSettings: settings };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/SaveGroupSettings",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    loadSingleGroupSettings();
                    loadGroupsTreeSingle(currentCardId, "0");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        }
        if (mode == "create") {
            name = $("#nameinputSingle").attr("value");
            comment = $("#commentinputSingle").attr("value");
            group = $("#groupSelectorSingle").attr("card");

            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), Name: name, Comment: comment, CardType: group };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/CreateGroup",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    currentCardId = "-1";
                    selectedNodeType = "-1";
                    loadSingleGroupSettings();
                    loadGroupsTreeSingle("", "");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        }
        mode = "";
        return false;
    });

    $("#cancel").click(function () {
        mode = "cancel";
        loadSingleGroupSettings();
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
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                }
            });
        }
        if (mode == "create") {
            name = $("#newCardName").attr("value");
            comment = $("#newCardComment").attr("value");
            number = $("#newCardNumber").attr("value");
            card = $("#newCardGroupSelector").attr("group");

            var data = { Name: name, Comment: comment, Number: number, groupID: card };
            var order = { OrgID: $.cookie("CURRENT_ORG_ID"), data: data, UserID: $.cookie("CURRENT_USERNAME") };

            $.ajax({
                type: "POST",
                //Page Name (in which the method should be called) and method name
                url: "Settings.aspx/CreateCardTransport",
                data: JSON.stringify(order),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    loadTransportsSettings();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
            },
            error: function (jqXHR, textStatus, errorThrown) {
                showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function deleteGroupSingle(list) {
    var order = { OrgID: $.cookie("CURRENT_ORG_ID"), GroupIDs: list };
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/DeleteGroups",
        data: JSON.stringify(order),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            currentCardId = "-1";
            selectedNodeType = "-1";
            loadSingleGroupSettings();
            loadGroupsTreeSingle("", "");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function deleteDriversSingle(list) {
    var order = { OrgID: $.cookie("CURRENT_ORG_ID"), DriverIDs: list };
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/DeleteDrivers",
        data: JSON.stringify(order),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            currentCardId = "-1";
            selectedNodeType = "-1";
            loadSingleDriverSettings();
            loadDriversTreeSingle("", "");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function deleteTransportsSingle(list) {
    var order = { OrgID: $.cookie("CURRENT_ORG_ID"), TransportIDs: list };
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/DeleteTransports",
        data: JSON.stringify(order),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            currentCardId = "-1";
            selectedNodeType = "-1";
            loadSingleVehicleSettings();
            loadVehiclesTreeSingle("", "");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
    '{"text": "Тип", "style": ""}]');

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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function createGroupSelectorGroups() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetGroupListGroups",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            createSelectors(response, "groupSelector", "card");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function createGroupCardTypeSelectors() {
    $("[cardType='0']").remove();

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
        $(selectors[i]).wijcombobox("destroy");
        $(selectors[i]).empty();
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
            $("#userSelectorNew").attr("user", value);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadLangList() {
    //AJAX HERE
    $("#lang").wijcombobox({
        showingAnimation: { effect: "blind" },
        hidingAnimation: { effect: "blind" },
        disabled: true
    });
    if (mode == "create") {
        $("#lang").wijcombobox({
            disabled: false,
            selectedIndex: 0
        });
        $("#lang").attr("langId", "0");
    }
}

function convert(date) {
    res = date.getDate() + ".";
    if (date.getMonth() < 9) res = res + "0";
    return res + (date.getMonth() + 1) + "." + date.getFullYear();
}