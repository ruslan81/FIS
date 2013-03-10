function showErrorMessage(title, jqXHR, errorThrown) {
    //if unauthorized
    if (jqXHR.status == 401) {
        window.location.href = "http://smartfis.ru";
        return false;
    }

    $("#error-dialog-message").attr("title", title);
    if (jqXHR.responseText != null) {
        if (jqXHR.responseText.search("<html>") == -1) {
            if (jqXHR.responseText.search("<HTML>") == -1) {
                $("#errorThrown").text($.parseJSON(jqXHR.responseText).Message);
            } else {
                var begin = jqXHR.responseText.search("<TITLE>");
                begin += "<TITLE>".length;
                var end = jqXHR.responseText.search("</TITLE>");
                var text = jqXHR.responseText.substring(begin, end);

                begin = jqXHR.responseText.search("<H3>");
                begin += "<H3>".length;
                end = jqXHR.responseText.search("</H3>");
                text += "<br/>" + jqXHR.responseText.substring(begin, end);

                $("#errorThrown").html(text);
            }
        } else {
            var begin = jqXHR.responseText.search("<title>");
            begin += "<title>".length;
            var end = jqXHR.responseText.search("</title>");
            var text = jqXHR.responseText.substring(begin, end);

            begin = jqXHR.responseText.search("<h3>");
            begin += "<h3>".length;
            end = jqXHR.responseText.search("</h3>");
            text += "<br/>" + jqXHR.responseText.substring(begin, end);

            $("#errorThrown").html(text);
        }
    } else {
        $("#errorThrown").text(jqXHR.errorThrown);
    }

    $("#errorDetail").hide();
    $("#showErrorDetail").text("подробнее");

    $("#error-dialog-message").dialog({
        autoOpen: true,
        maxHeight: 500,
        width: 420,
        modal: true,
        closeText: '',
        resizable: false,
        draggable: false,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        },
        captionButtons: {
            pin: { visible: false },
            refresh: { visible: false },
            toggle: { visible: false },
            minimize: { visible: false },
            maximize: { visible: false }
        }
    });
}