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
        loadGeneralSettings();
    } else {
        $("#headerSettings").empty();
        $("#contentSettings").empty();
    }
}

//Загрузить элементы дерева Водителей в разделе "Восстановить у пользователя"
function loadGeneralSettings() {
    $.ajax({
        type: "POST",
        //Page Name (in which the method should be called) and method name
        url: "Settings.aspx/GetGeneralSettings",
        data: "{'OrgID':'" + $.cookie("CURRENT_ORG_ID") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //очищаем tbody от предыдущих данных
            $("#contentSettings").empty();
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
            }
        });

        return false;
    });

    $("#cancel").click(function () {
        loadGeneralSettings();
        return false;
    });
}