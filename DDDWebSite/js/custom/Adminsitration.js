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

function loadGeneralData() {
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
        }
    });

    createStatisticTable();
    createMessageTable();

    $("#tabs").tabs();

    $("#tabs").tabs({ show: function (e, ui) {
        if (ui.index == 0) {
            tabIndex = 0;
            $("#userControls").hide();
        }
        if (ui.index == 1) {
            tabIndex = 1;
            loadGeneralDetailedData();
            $("#userControls").show();
        }
        return false;
    }
    });

    //loadGeneralDetailedData();
    $("#userControls").hide();
}

function createStatisticTable() {
    createTableHeader($("#statisticTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "", "style": "width: 40%;"},' +
    '{"text": "Текущее", "style": "width: 30%;"},' +
    '{"text": "Доступное", "style": "width: 30%;"}]');
    $("#statisticTable").show();
}

function createMessageTable() {
    createTableHeader($("#messageTableHeader"), $("#tmplHeadColumn"),
    '[{"text": "Отправитель", "style": "width: 100px;"},' +
    '{"text": "Тема", "style": ";"},' +
    '{"text": "Дата", "style": "width: 100px;"},' +
    '{"text": "Срок окончания", "style": "width: 100px;"}]');
    $("#messageTable").show();
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

    loadEventList();
    //buildJournalTable();
}

function buildJournalTable() {
    $("#dateErrorBlock").hide();
    var startDate = $("#startDatePicker").datepicker("getDate");
    var endDate = $("#endDatePicker").datepicker("getDate");
    var event=$("#eventSelector").attr("event");
    var text=$("#textInput").attr("value");
    if (endDate == null || startDate == null) {
        $("#dateErrorBlock").show();
        return;
    }
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Administration.aspx/GetJournal",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "', 'StartDate':'" + convert(startDate) + "', 'EndDate':'" + convert(endDate) + "', 'eventType':'" + eventType + "', 'searchString':'" + text + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            updateTable($("#journalTableBody"), $("#tmplJournalTableContent"), response.d);
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
            updateTable($("#invoiceTableBody"), $("#tmplInvoiceTableContent"), response.d);
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
            //$("#detailedData input").removeClass("inputField");
            $("#detailedData input").addClass("inputField-readonly input");
            $("#detailedData input").attr("readonly", "readonly");
            //$("#detailedData input").attr("style", "border: 2px solid red;");
            loadCountryList();
            loadTimeZoneList();
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

        $("#detailedData .input").removeClass("inputField-readonly");
        $("#detailedData .input").removeAttr("readonly");
        $("#detailedData .input").addClass("inputField");
        

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

function saveGeneralDetailedData() {
    var orgName=$("#orgName").attr("value");
    var orgLogin=$("#orgLogin").attr("value");
    var pass1=$("#pass1").attr("value");
    var pass2 = $("#pass2").attr("value");

    if (pass1 != pass2) {
        alert("Введенные пароли не совпадают!");
        return "error";
    }

    var country=$("#country").attr("countryId");
    var city=$("#city").attr("value");
    var index=$("#index").attr("value");
    var timeZone = $("#timeZoneSelector").attr("timeZoneId");
    var address1=$("#addr1").attr("value");
    var address2=$("#addr2").attr("value");
    var phone=$("#phone").attr("value");
    var fax=$("#fax").attr("value");
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
        }
    });
}