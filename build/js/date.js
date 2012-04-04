function date2String(now) {
    var month = now.getMonth() + 1;
    if (month < 10) {
        month = "0" + month;
    }

    var h;
    if (now.getHours() < 10) {
        h = "0" + now.getHours();
    } else {
        h = now.getHours();
    }

    var m;
    if (now.getMinutes() < 10) {
        m = "0" + now.getMinutes();
    } else {
        m = now.getMinutes();
    }

    var s;
    if (now.getSeconds() < 10) {
        s = "0" + now.getSeconds();
    } else {
        s = now.getSeconds();
    }

    var d;
    if (now.getDate() < 10) {
        d = "0" + now.getDate();
    } else {
        d = now.getDate();
    }

    return d + "." + month + "." + now.getFullYear() + " " + h + ":" + m + ":" + s;

}