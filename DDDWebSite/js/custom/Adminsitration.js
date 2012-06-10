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
            //if (i < rows.length - 1) {
            $(cells[j]).addClass("wijmo-wijgrid-cell-border-bottom");
            //}
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

function loadGeneralData() {
    $("#ContentContainer").empty();
    $("#ContentContainer").append($("#GeneralData").text());

    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetGeneralData",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $($("#tmplGeneralData")).tmpl(response.d).appendTo($("#firstGeneralRow"));
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });

    createStatisticTable();
    createMessageTable();

    $("#tabs").tabs();

    $("#tabs").tabs({ show: function (e, ui) {
        if (ui.index == 0) {
            tabIndex = 0;
            $("#userControls").hide();
            resizeAdmin();
        }
        if (ui.index == 1) {
            tabIndex = 1;
            loadGeneralDetailedData();
            $("#userControls").show();
            resizeAdmin();
        }
        return false;
    }
    });

    //loadGeneralDetailedData();
    $("#userControls").hide();

    resizeAdmin();
}

function createStatisticTable() {
    createTableHeader($("#statisticTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 40%;"},' +
    '{"text": "Текущее", "style": "width: 30%;"}]');
    $("#statisticTable").show();

    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetStatistic",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            updateTable($("#statisticTableBody"), $("#tmplStatisticTableContent"), response.d);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function createMessageTable() {
    createTableHeader($("#messageTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Отправитель", "style": "width: 100px;"},' +
    '{"text": "Тема", "style": ";"},' +
    '{"text": "Дата", "style": "width: 100px;"},' +
    '{"text": "Срок окончания", "style": "width: 100px;"}]');

    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetMessages",
        data: "{'UserName':'" + $.cookie("CURRENT_USERNAME") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            updateTable($("#messageTableBody"), $("#tmplMessageTableContent"), response.d);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
    $("#messageTable").show();

    $("#remove").button();
    $("#remove").click(function () {
        var boxes = $("#messageTable [type=checkbox]");
        var c = 0;
        for (var i = 0; i < boxes.length; i++) {
            if (boxes[i].checked)
                c++;
        }
        if (c == 0)
            return false;
        $("#deletedialog").dialog({ buttons: {
            "OK": function () {
                removeMessages();
                $(this).dialog("close");
            },
            "Отмена": function () {
                $(this).dialog("close");
            }
        },
            closeText: '',
            resizable: false,
            modal: true
        });
        return false;
    });
    return false;
}

function loadDealersData() {
    $("#ContentContainer").empty();
    $("#ContentContainer").append($("#DealersData").text());

    createTableHeader($("#dealersTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Наименование", "style": ";"},' +
    '{"text": "Дата регистрации", "style": "width: 100px;"},' +
    '{"text": "Окончание регистрации", "style": "width: 100px;"},' +
    '{"text": "Страна", "style": "width: 100px;"},' +
    '{"text": "Город", "style": "width: 100px;"}]');
    $("#dealersTable").show();

    loadGeneralDealersData();

    $("#userControls").empty();
    $("#userControls").append($("#сontrolsDealers").text());
    $("#userControls button").button();

    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    $("#edit").click(function () {

        var boxes = $("#commonData [type=checkbox]");
        var c = 0;
        for (var i = 0; i < boxes.length; i++) {
            if (boxes[i].checked)
                c++;
        }
        if (c == 0)
            return false;

        mode = "edit";

        $("[name=dealerCheckbox]").hide();

        var checkboxes = $("#dealersTable [type='checkbox']");
        for (var i = 0; i < checkboxes.length; i++) {
            if (!checkboxes[i].checked)
                continue;
            var id = $(checkboxes[i]).attr("dealerId");
            $("#nameinput" + id).removeClass("inputField-readonly");
            $("#nameinput" + id).removeAttr("readonly");
            $("#nameinput" + id).addClass("inputField");
            $("#country" + id).wijcombobox({
                disabled: false
            });
            $("#city" + id).wijcombobox({
                disabled: false
            });
            $("#endDateInput" + id).datepicker("enable");
            $("#endDateInput" + id).removeClass("inputField-readonly");
            //$("#endDateInput" + id).removeAttr("readonly");
            $("#endDateInput" + id).addClass("inputField");
        }

        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });
        return false;
    });

    $("#create").click(function () {
        mode = "create";

        //$("#dealersTableBody").append($("#tmplNewDealer").text());
        var trs = $("#dealersTableBody tr");
        $(trs[0]).before($("#tmplNewDealer").text());

        $("[name=dealerCheckbox]").hide();

        var cells = $("#newRow .wijgridtd");
        for (var j = 0; j < cells.length; j++) {
            $(cells[j]).addClass("wijmo-wijgrid-cell-border-right");
            $(cells[j]).addClass("wijmo-wijgrid-cell-border-bottom");
            $(cells[j]).addClass("wijmo-wijgrid-cell");
        }

        var today = new Date();
        var todaystr = "" + convert(today);
        today.setMonth(today.getMonth() + 6);
        var thenstr = "" + convert(today);

        $("#startDatePicker").datepicker();
        $("#startDatePicker").datepicker("option", "dateFormat", "dd.mm.yy");
        $("#startDatePicker").datepicker("setDate", todaystr);

        $("#endDatePicker").datepicker();
        $("#endDatePicker").datepicker("option", "dateFormat", "dd.mm.yy");
        $("#endDatePicker").datepicker("setDate", thenstr);

        $("#startDatePicker").datepicker($.datepicker.regional['ru']);
        $("#endDatePicker").datepicker($.datepicker.regional['ru']);


        loadNewCountryList();

        $("#edit").button({ disabled: true });
        $("#delete").button({ disabled: true });
        $("#create").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });
        return false;
    });

    $("#cancel").click(function () {
        mode = "";
        loadDealersData();
        return false;
    });

    $("#save").click(function () {
        if (mode == "edit") {
            saveDealersData();
        }
        if (mode == "create") {
            createNewDealer();
        }
        //loadDealersData();
        mode = "";
        return false;
    });

    $("#delete").click(function () {
        var inputs = $('[name="dealerCheckbox"]');
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
                        key = $(inputs[i]).attr("dealerId");
                        keys.push({ Key: "", Value: key });
                    }
                }
                deleteDealers(keys);
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


    $("#tabs").tabs();

    $("#tabs").tabs({ show: function (e, ui) {
        if (ui.index == 0) {
            tabIndex = 0;
            loadGeneralDealersData();
            $("#userControls").show();
        }
        if (ui.index == 1) {
            tabIndex = 1;
            loadDealersDetailedData();
            //$("#userControls").show();
        }
        return false;
    }
    });

    $("#userControls").show();
}

function convert(date) {
    res = date.getDate() + ".";
    if (date.getMonth() < 9) res = res + "0";
    return res + (date.getMonth() + 1) + "." + date.getFullYear();
}

function deleteDealers(list) {
    var order = { OrgID: $.cookie("CURRENT_ORG_ID"), ids: list };
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/DeleteDealers",
        data: JSON.stringify(order),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadDealersData();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function deleteUser(index) {
    //var order = { OrgID: $.cookie("CURRENT_ORG_ID"), ids: list };
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/DeleteUser",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'UserID':'" + index + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadUsersData();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadGeneralDealersData() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetDealers",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            updateTable($("#dealersTableBody"), $("#tmplDealersTableContent"), response.d);
            radioIndex = -1;

            var pickers = $("[name=endDateInput]");
            for (var i = 0; i < pickers.length; i++) {
                var date = $(pickers[i]).attr("value");
                $(pickers[i]).datepicker();
                $(pickers[i]).datepicker("option", "dateFormat", "dd.mm.yy");
                $(pickers[i]).datepicker("setDate", date);
                $(pickers[i]).datepicker("disable");
            }

            loadCommonCountryList();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function createNewDealer() {
    var name = $("#newnameinput").attr("value");
    var date = $("#startDatePicker").attr("value");
    var endDate = $("#endDatePicker").attr("value");
    var country = $("#newcountry").attr("value");
    var city = $("#newcity").attr("value");
    var dealerData = "{'name':'" + name + "','date':'" + date + "','endDate':'" + endDate + "','country':'" + country + "','city':'" + city + "'}";
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/CreateNewDealer",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "','data': " + dealerData + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadDealersData();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadUsersData() {
    $("#ContentContainer").empty();
    $("#ContentContainer").append($("#UsersData").text());

    createTableHeader($("#usersTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 50px;"},' +
    '{"text": "Дилер", "style": ";"},' +
    '{"text": "Фамилия", "style": ";"},' +
    '{"text": "Имя", "style": ";"},' +
    '{"text": "Логин", "style": ";"},' +
    '{"text": "Роль", "style": ";"},' +
    '{"text": "Состояние", "style": ";"}]');
    $("#usersTable").show();

    $("#tabs").tabs();

    loadGeneralUsersData();

    $("#userControls").show();

    resizeAdmin();

    $("#tabs").tabs({ show: function (e, ui) {
        if (ui.index == 0) {
            tabIndex = 0;
            loadGeneralUsersData();
            $("#userControls").show();
            resizeAdmin();
        }
        if (ui.index == 1) {
            tabIndex = 1;
            loadUsersDetailedData();
            $("#userControls").show();
            resizeAdmin();
        }
        return false;
    }
    });
}

function loadGeneralUsersData() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetUsers",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            updateTable($("#usersTableBody"), $("#tmplUsersTableContent"), response.d);
            radioIndex = -1;
            loadUserTypesList();
            loadUsersControls();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

//INVOICE
function loadInvoiceData() {
    $("#ContentContainer").append($("#InvoiceData").text());
    createTableHeader($("#invoiceTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "Наименование", "style": ";"},' +
    '{"text": "Дата выставления", "style": "width: 150px;"},' +
    '{"text": "Срок оплаты", "style": "width: 150px;"},' +
    '{"text": "Статус", "style": "width: 100px;"},' +
    '{"text": "Дата оплаты", "style": "width: 150px;"}]');
    $("#invoiceTable").show();

    createFilter();

    $("#buildButton").click(function () {
        buildInvoiceTable();
        return false;
    });

    loadInvoiceStatusList();
    buildInvoiceTable();

    $("#userControls").hide();
}

function loadUsersControls() {
    $("#userControls").empty();
    $("#userControls").append($("#controlsUsers").text());
    $("#userControls button").button();

    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    $("#edit").click(function () {
        var boxes = $("#commonData [type=radio]");
        var c = 0;
        for (var i = 0; i < boxes.length; i++) {
            if (boxes[i].checked)
                c++;
        }
        if (c == 0)
            return false;

        mode = "edit";
        if (tabIndex == 0) {
            $("#tabs").tabs({ selected: 1 });
        } else {
            loadUsersDetailedData();
            $("#userControls").show();
            resizeAdmin();
        }

        return false;
    });

    $("#create").click(function () {
        mode = "create";
        $("#tabs").tabs({ selected: 1 });
        return false;
    });

    $("#cancel").click(function () {
        mode = "";
        loadUsersData();
        return false;
    });

    $("#save").click(function () {
        if (mode == "edit") {
            saveUsersData();
        }
        if (mode == "create") {
            createNewUser();
        }
        mode = "";
        return false;
    });

    $("#delete").click(function () {
        if (radioIndex > 0) {
            $("#deletedialog").dialog({ buttons: { "OK": function () {
                $(this).dialog("close");
                deleteUser(radioIndex);
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

    $("#userControls").show();
}

//JOURNAL
function loadJournalData() {
    $("#ContentContainer").append($("#JournalData").text());
    createTableHeader($("#journalTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "Дата и время", "style": "width: 150px;"},' +
    '{"text": "Пользователь", "style": "width: 100px;"},' +
    '{"text": "Описание", "style": ""}]');
    $("#journalTable").show();

    createFilter();

    $("#buildButton").click(function () {
        buildJournalTable();
        return false;
    });
    $("#dateErrorBlock").hide();
    loadEventList();
    //buildJournalTable();
}

function buildJournalTable() {
    $("#dateErrorBlock").hide();
    var startDate = $("#startDatePicker").datepicker("getDate");
    var endDate = $("#endDatePicker").datepicker("getDate");
    var event = $("#eventSelector").attr("event");
    var text = $("#textInput").attr("value");
    if (endDate == null || startDate == null) {
        $("#dateErrorBlock").show();
        return;
    }
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetJournal",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'StartDate':'" + convert(startDate) + "', 'EndDate':'" + convert(endDate) + "', 'eventType':'" + event + "', 'searchString':'" + text + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "html json",
        success: function (response) {
            if (response.d == null) {
            $("#dateErrorBlock").show();
            $("#journalTable").hide();
            return;
            }
            updateTable($("#journalTableBody"), $("#tmplJournalTableContent"), response.d);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function buildInvoiceTable() {
    $("#dateErrorBlock").hide();
    var startDate = $("#startDatePicker").datepicker("getDate");
    var endDate = $("#endDatePicker").datepicker("getDate");
    var status = $("#invoiceStatusSelector").attr("statusType");

    if (endDate == null || startDate == null) {
        $("#dateErrorBlock").show();
        return;
    }
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetInvoices",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'StartDate':'" + convert(startDate) + "', 'EndDate':'" + convert(endDate) + "', 'statusType':'" + status + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.d == null) {
                $("#dateErrorBlock").show();
                $("#journalTable").hide();
                return;
            }
            updateTable($("#invoiceTableBody"), $("#tmplInvoiceTableContent"), response.d);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function convert(date) {
    res = date.getDate() + ".";
    if (date.getMonth() < 9) res = res + "0";
    return res + (date.getMonth() + 1) + "." + date.getFullYear();
}

function loadEventList() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetEvents",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#tmplOption").tmpl(response.d).appendTo("#eventSelector");
            $("#eventSelector").wijcombobox({
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

function createFilter() {
    var today = new Date();
    var todaystr = "" + convert(today);
    today.setMonth(today.getMonth() - 1);
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

}

function loadInvoiceStatusList() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetInvoiceStatuses",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#tmplOption").tmpl(response.d).appendTo("#invoiceStatusSelector");
            $("#invoiceStatusSelector").wijcombobox({
                showingAnimation: { effect: "blind" },
                hidingAnimation: { effect: "blind" },
                isEditable: false
            });
            $("#invoiceStatusSelector option [value='0']").attr("selected", "true");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadGeneralDetailedData() {
    $("#detailedData").empty();
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetGeneralDetailedData",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#tmplGeneralDetailedData").tmpl(response.d).appendTo("#detailedData");
            $("#detailedData input").addClass("inputField-readonly input");
            $("#detailedData input").attr("readonly", "readonly");
            loadCountryList();
            loadTimeZoneList();
            loadAllDealersList();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });

    $("#userControls").empty();
    $("#userControls").append($("#сontrolsGeneralDetailed").text());

    $("#userControls button").button();
    $("#save").button({ disabled: true });
    $("#cancel").button({ disabled: true });

    $("#edit").click(function () {
        mode = "edit";


        $("#timeZoneSelector").wijcombobox({
            disabled: false
        });
        $("#country").wijcombobox({
            disabled: false
        });
        $("#dealerSelector").wijcombobox({
            disabled: false
        });

        $("#detailedData .input").removeClass("inputField-readonly");
        $("#detailedData .input").removeAttr("readonly");
        $("#detailedData .input").addClass("inputField");

        $("#orgName").removeClass("inputField");
        $("#orgName").addClass("inputField-readonly");
        $("#orgName").attr("readonly", "readonly");

        $("#orgLogin").removeClass("inputField");
        $("#orgLogin").addClass("inputField-readonly");
        $("#orgLogin").attr("readonly", "readonly");

        $("#edit").button({ disabled: true });
        $("#save").button({ disabled: false });
        $("#cancel").button({ disabled: false });
        return false;
    });

    $("#cancel").click(function () {
        mode = "";
        loadGeneralDetailedData();
        return false;
    });

    $("#save").click(function () {
        mode = "";
        var res = saveGeneralDetailedData();
        if (res == "error") {
            return false;
        }

        loadGeneralDetailedData();
        return false;
    });

}

function loadDealersDetailedData() {
    $("#detailedData").empty();
    if (radioIndex == -1) {
        $("#detailedData").append("<font color='#FF0000' size='5'>Выберите объект для отображения детальных сведений!<font>");
        return;
    }
}

function loadUsersDetailedData() {
    $("#detailedData").empty();
    if (radioIndex == -1 && mode != "create") {
        $("#detailedData").append("<font color='#FF0000' size='5'>Выберите объект для отображения детальных сведений!<font>");
        return;
    }
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetUserDetailedData",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'UserID':'" + radioIndex + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#tmplUsersDetailedData").tmpl(response.d).appendTo("#detailedData");
            //$("#detailedData input").removeClass("inputField");
            $("#detailedData input").addClass("inputField-readonly input");
            $("#detailedData input").attr("readonly", "readonly");
            //$("#detailedData input").attr("style", "border: 2px solid red;");
            loadCountryList();
            loadTimeZoneList();
            loadRoleList();
            loadAllDealersList();

            if (mode == "edit") {
                $("#detailedData .input").removeClass("inputField-readonly");
                $("#detailedData .input").removeAttr("readonly");
                $("#detailedData .input").addClass("inputField");

                $("#orgName").removeClass("inputField");
                $("#orgName").addClass("inputField-readonly");
                $("#orgName").attr("readonly", "readonly");

                $("#edit").button({ disabled: true });
                $("#delete").button({ disabled: true });
                $("#create").button({ disabled: true });
                $("#save").button({ disabled: false });
                $("#cancel").button({ disabled: false });
            }
            if (mode == "create") {
                $("#detailedData .input").removeClass("inputField-readonly");
                $("#detailedData .input").removeAttr("readonly");
                $("#detailedData .input").addClass("inputField");

                $("#orgName").removeClass("inputField");
                $("#orgName").addClass("inputField-readonly");
                $("#orgName").attr("readonly", "readonly");

                $("#orgName").attr("value", "Текущая организация");

                $("#edit").button({ disabled: true });
                $("#delete").button({ disabled: true });
                $("#create").button({ disabled: true });
                $("#save").button({ disabled: false });
                $("#cancel").button({ disabled: false });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function saveGeneralDetailedData() {
    var orgName = $("#orgName").attr("value");
    var orgLogin = $("#orgLogin").attr("value");
    var pass1 = $("#pass1").attr("value");
    var pass2 = $("#pass2").attr("value");

    if (pass1 != pass2) {
        alert("Введенные пароли не совпадают!");
        return "error";
    }

    var country = $("#country").attr("countryId");
    var dealerId = $("#dealerSelector").attr("dealerId");
    var city = $("#city").attr("value");
    var index = $("#index").attr("value");
    var timeZone = $("#timeZoneSelector").attr("timeZoneId");
    var address1 = $("#addr1").attr("value");
    var address2 = $("#addr2").attr("value");
    var phone = $("#phone").attr("value");
    var fax = $("#fax").attr("value");
    var mail = $("#mail").attr("value");

    var ud =
    //+ "', 'orgName':'" + orgName
           "{'orgLogin':'" + orgLogin
           + "', 'country':'" + country
           + "', 'password':'" + pass1
           + "', 'city':'" + city
           + "', 'index':'" + index
           + "', 'address1':'" + address1
           + "', 'address2':'" + address2
           + "', 'timeZone':'" + timeZone
           + "', 'dealerId':'" + dealerId
           + "', 'phone':'" + phone
           + "', 'fax':'" + fax
           + "', 'mail':'" + mail + "'}";
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/SaveGeneralDetailedData",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "', 'ud':" + ud + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            /*$("#detailedData input").addClass("inputField-readonly input");
            $("#detailedData input").attr("readonly", "readonly");
            $("#detailedData input").attr("style", "border: 2px solid red;");*/
            return "OK";
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function saveDealersData() {

    var checkboxes = $("#dealersTable [type='checkbox']");
    var data = "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'list':[";
    var k = 0;
    for (var i = 0; i < checkboxes.length; i++) {
        if (!checkboxes[i].checked)
            continue;
        var id = $(checkboxes[i]).attr("dealerId");
        var name = $("#nameinput" + id).attr("value");
        var endDate = $("#endDateInput" + id).attr("value");
        var country = $("#country" + id).attr("countryId");
        var city = $("#city" + id).attr("cityId");
        if (k == 0) {
            data = data + "{ 'id':'" + id + "', 'name':'" + name + "', 'country':'" + country + "', 'endDate':'" + endDate + "', 'city':'" + city + "'}";
        } else {
            data = data + ",{ 'id':'" + id + "', 'name':'" + name + "', 'country':'" + country + "', 'endDate':'" + endDate + "', 'city':'" + city + "'}";
        }
        k++;
    }
    data = data + "]}";

    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/SaveDealersData",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadDealersData();
            return "OK";
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function saveUsersData() {

    //var orgName = $("#orgName").attr("value");
    var login = $("#orgLogin").attr("value");
    var role = $("#role").attr("roleId");
    var pass1 = $("#pass1").attr("value");
    var pass2 = $("#pass2").attr("value");

    if (pass1 != pass2) {
        alert("Введенные пароли не совпадают!");
        return "error";
    }

    var country = $("#country").attr("countryId");
    var city = $("#city").attr("value");
    var index = $("#index").attr("value");
    var timeZone = $("#timeZoneSelector").attr("timeZoneId");
    var dealerId = $("#dealerSelector").attr("dealerId");
    var address1 = $("#addr1").attr("value");
    var address2 = $("#addr2").attr("value");
    var phone = $("#phone").attr("value");
    var fax = $("#fax").attr("value");
    var mail = $("#mail").attr("value");
    var name = $("#name").attr("value");
    var patronimic = $("#patronimic").attr("value");
    var surname = $("#surname").attr("value");

    var ud =
           "{'login':'" + login
           + "', 'country':'" + country
           + "', 'password':'" + pass1
           + "', 'city':'" + city
           + "', 'index':'" + index
           + "', 'address1':'" + address1
           + "', 'address2':'" + address2
           + "', 'timeZone':'" + timeZone
           + "', 'dealerId':'" + dealerId
           + "', 'phone':'" + phone
           + "', 'fax':'" + fax
           + "', 'roleId':'" + role
           + "', 'name':'" + name
           + "', 'surname':'" + surname
           + "', 'patronimic':'" + patronimic
           + "', 'mail':'" + mail + "'}";

    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/SaveUsersData",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'UserID':'" + radioIndex + "', 'ud':" + ud + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadUsersData();
            return "OK";
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function createNewUser() {
    //var orgName = $("#orgName").attr("value");
    var login = $("#orgLogin").attr("value");
    var role = $("#role").attr("roleId");
    var pass1 = $("#pass1").attr("value");
    var pass2 = $("#pass2").attr("value");

    if (pass1 != pass2) {
        alert("Введенные пароли не совпадают!");
        return "error";
    }

    var country = $("#country").attr("countryId");
    var city = $("#city").attr("value");
    var index = $("#index").attr("value");
    var timeZone = $("#timeZoneSelector").attr("timeZoneId");
    var dealerId = $("#dealerSelector").attr("dealerId");
    var address1 = $("#addr1").attr("value");
    var address2 = $("#addr2").attr("value");
    var phone = $("#phone").attr("value");
    var fax = $("#fax").attr("value");
    var mail = $("#mail").attr("value");
    var name = $("#name").attr("value");
    var patronimic = $("#patronimic").attr("value");
    var surname = $("#surname").attr("value");

    var ud =
           "{'login':'" + login
           + "', 'country':'" + country
           + "', 'password':'" + pass1
           + "', 'city':'" + city
           + "', 'index':'" + index
           + "', 'address1':'" + address1
           + "', 'address2':'" + address2
           + "', 'timeZone':'" + timeZone
           + "', 'dealerId':'" + dealerId
           + "', 'phone':'" + phone
           + "', 'fax':'" + fax
           + "', 'roleId':'" + role
           + "', 'name':'" + name
           + "', 'surname':'" + surname
           + "', 'patronimic':'" + patronimic
           + "', 'mail':'" + mail + "'}";

    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/CreateNewUser",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'UserName':'" + $.cookie("CURRENT_USERNAME") + "', 'ud':" + ud + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadUsersData();
            return "OK";
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadCountryList() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetCountries",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#country").append("<option value='0'>Не указано</option>");
            $("#tmplOption").tmpl(response.d).appendTo("#country");
            var country = $("#country").attr("countryId");
            if (country == "") { country = "0"; }
            $('#country [value="' + country + '"]').attr("selected", "true");
            $("#country").wijcombobox({
                showingAnimation: { effect: "blind" },
                hidingAnimation: { effect: "blind" },
                isEditable: false,
                disabled: true
            });
            if (mode == "edit" || mode == "create") {
                $("#country").wijcombobox({
                    disabled: false
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadAllDealersList() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetAllDealers",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#dealerSelector").append("<option value='0'>Не указано</option>");
            $("#tmplOption").tmpl(response.d).appendTo("#dealerSelector");
            var country = $("#dealerSelector").attr("dealerId");
            if (country == "") { country = "0"; }
            $('#dealerSelector [value="' + country + '"]').attr("selected", "true");
            $("#dealerSelector").wijcombobox({
                showingAnimation: { effect: "blind" },
                hidingAnimation: { effect: "blind" },
                isEditable: false,
                disabled: true
            });
            if (mode == "edit" || mode == "create") {
                $("#dealerSelector").wijcombobox({
                    disabled: false
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadRoleList() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetUserTypes",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#role").append("<option value='0'>Не указано</option>");
            $("#tmplOption").tmpl(response.d).appendTo("#role");
            var role = $("#role").attr("roleId");
            if (role == "") { role = "0"; }
            $('#role [value="' + role + '"]').attr("selected", "true");
            $("#role").wijcombobox({
                showingAnimation: { effect: "blind" },
                hidingAnimation: { effect: "blind" },
                isEditable: false,
                disabled: true
            });
            if (mode == "edit" || mode == "create") {
                $("#role").wijcombobox({
                    disabled: false
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadNewCountryList() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetCountries",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#newcountry").append("<option value='0'>Не указано</option>");
            $("#tmplOption").tmpl(response.d).appendTo("#newcountry");
            var country = $("#newcountry").attr("countryId");
            if (country == "") { country = "0"; }
            $('#newcountry [value="' + country + '"]').attr("selected", "true");
            $("#newcountry").wijcombobox({
                showingAnimation: { effect: "blind" },
                hidingAnimation: { effect: "blind" },
                isEditable: false
            });
            loadNewCityList();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadNewCityList() {
    var country = $("#newcountry").attr("countryId");
    if (country == "") { country = "0"; }
    loadCityList(country, $("#newcity"), false);
}

function loadCommonCountryList() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetCountries",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var selectors = $("#outputId [name='countrySelector']");
            //alert(selectors.length);
            for (var i = 0; i < selectors.length; i++) {
                $(selectors[i]).append("<option value='0'>Не указано</option>");
                $("#tmplOption").tmpl(response.d).appendTo(selectors[i]);
                var country = $(selectors[i]).attr("countryId");
                if (country == "") { country = "0"; }
                $('#' + selectors[i].id + ' [value="' + country + '"]').attr("selected", "true");
                $(selectors[i]).wijcombobox({
                    showingAnimation: { effect: "blind" },
                    hidingAnimation: { effect: "blind" },
                    isEditable: false,
                    disabled: true
                });
            }
            loadCommonCityList();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadUserTypesList() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetUserTypes",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var selectors = $("#outputId [name='roleSelector']");
            //alert(selectors.length);
            for (var i = 0; i < selectors.length; i++) {
                $(selectors[i]).append("<option value='0'>Не указано</option>");
                $("#tmplOption").tmpl(response.d).appendTo(selectors[i]);
                var role = $(selectors[i]).attr("roleId");
                if (role == "") { role = "0"; }
                $('#' + selectors[i].id + ' [value="' + role + '"]').attr("selected", "true");
                $(selectors[i]).wijcombobox({
                    showingAnimation: { effect: "blind" },
                    hidingAnimation: { effect: "blind" },
                    isEditable: false,
                    disabled: true
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadCommonCityList() {
    var selectors = $("[name='citySelector']");
    for (var i = 0; i < selectors.length; i++) {
        var country = $(selectors[i]).attr("countryId");
        if (country == "") { country = "0"; }
        loadCityList(country, selectors[i], true);
    }
}

function loadCityList(country, selector, disabled) {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetCities",
        data: "{'CountryID' : '" + country + "' }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $(selector).append("<option value='0'>Не указано</option>");
            $("#tmplOption").tmpl(response.d).appendTo(selector);
            var city = $(selector).attr("cityId");
            if (city == "") { city = "0"; }
            $('#' + selector.id + ' [value="' + city + '"]').attr("selected", "true");
            $(selector).wijcombobox("destroy");
            $(selector).wijcombobox({
                showingAnimation: { effect: "blind" },
                hidingAnimation: { effect: "blind" },
                isEditable: false,
                disabled: disabled
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function loadTimeZoneList() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetTimeZones",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#timeZoneSelector").append("<option value='0'>Не указано</option>");
            $("#tmplOption").tmpl(response.d).appendTo("#timeZoneSelector");
            var timeZone = $("#timeZoneSelector").attr("timeZoneId");
            if (timeZone == "") { timeZone = "0"; }
            $('#timeZoneSelector [value="' + timeZone + '"]').attr("selected", "true");
            $("#timeZoneSelector").wijcombobox({
                showingAnimation: { effect: "blind" },
                hidingAnimation: { effect: "blind" },
                isEditable: false,
                disabled: true
            });
            if (mode == "edit" || mode == "create") {
                $("#timeZoneSelector").wijcombobox({
                    disabled: false
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function removeMessages() {

    var inputs = $('#messageTableBody input');
    var list = "[";
    var count = 0;
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].checked) {
            var id = $(inputs[i]).attr("messageId");
            if (count > 0) {
                list = list + ", {'Value':'" + id + "', 'Key':' '}";
            } else {
                list = list + " {'Value':'" + id + "', 'Key':' '}";
            }
            count++;
        }
    }
    list = list + "]";
    if (count == 0)
        return false;
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/DeleteMessages",
        data: "{'messageIds': " + list + " }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadGeneralData();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
        }
    });
}

function changeCountry(selector) {
    var dealer = $(selector).attr("dealerId");
    var country = $(selector).attr("countryId");
    var citySel = $("[cityDealerId=" + dealer + "]");
    $(citySel).attr("countryId", country);
    $(citySel).attr("cityId", "0");
    $(citySel).empty();
    loadCityList(country, citySel, false);
}