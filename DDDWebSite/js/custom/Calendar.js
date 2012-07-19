var m_nRadiusCenter;
var m_nRadiusYear;
var m_nRadiusMonth;
var m_nRadiusMonthDigit;
var m_nRadiusDay;
var m_nCenterX, m_nCenterY;
var nYear, nMonth, nDay;
var m_nYear = 0, m_nMonth = 0, m_nDay = 0;
var m_nYearTrack, m_nMonthTrack, m_nDayTrack;

var m_CanvasBounds;
var m_nYearBase = 2007;
var m_nMinYear = 1981;
var m_nMaxYear = 2099;

var m_bSetData = false;
var m_bSvcButton = false;

var m_bDays = [];
var m_bMonths = [];

var IE = document.all ? true : false;

function initCalendar() {
    var nActualCalendarDiameter = 446;
    var nNominalCalendarDiameter = 446;

    var canvas = document.getElementsByTagName('canvas')[0];
	canvas.width = nActualCalendarDiameter;
	canvas.height = nActualCalendarDiameter;
    m_CanvasBounds = getBounds(canvas);
    var context = canvas.getContext('2d');

    m_nCenterX = canvas.width / 2;
    m_nCenterY = canvas.height / 2;

    m_nRadiusCenter = 73.0 / nNominalCalendarDiameter * canvas.width;
    m_nRadiusYear = 115.0 / nNominalCalendarDiameter * canvas.width;
    m_nRadiusMonthDigit = 172.0 / nNominalCalendarDiameter * canvas.width;
    m_nRadiusMonth = 190.0 / nNominalCalendarDiameter * canvas.width;
    m_nRadiusDay = canvas.width / 2.0 - 2;

    DrawCalendar(context);
    DrawText(context);

    if (!IE) {
        document.captureEvents(Event.MOUSEMOVE);
        document.captureEvents(Event.MOUSEDOWN);
        document.captureEvents(Event.MOUSEUP);
        document.captureEvents(Event.CLICK);
    }

    document.onmousemove = onMouseMove;
    document.onmousedown = onMouseDown;
    document.onmouseup = onMouseUp;
    document.onclick = onClick;
}

function getBounds(element)
{
    var left = element.offsetLeft;
    var top = element.offsetTop;
    for (var parent = element.offsetParent; parent; parent = parent.offsetParent) {
        left += parent.offsetLeft - parent.scrollLeft;
        top += parent.offsetTop - parent.scrollTop
    }
    return {left: left, top: top, width: element.offsetWidth, height: element.offsetHeight};
}

function onMouseMove(e)
{
    nPosX = (IE) ? event.clientX + document.body.scrollLeft : e.pageX;
    nPosY = (IE) ? event.clientY + document.body.scrollTop : e.pageY;

    var nMousePosX = nPosX - m_CanvasBounds.left;
    var nMousePosY = nPosY - m_CanvasBounds.top;

    m_nDayTrack = m_nYearTrack = m_nMonthTrack = 0;
    if (GetDate(nMousePosX - m_nCenterX, m_nCenterY - nMousePosY)) {
        if (nYear)
            m_nYearTrack = nYear;
        else if (nMonth && m_bMonths[nMonth - 1])
            m_nMonthTrack = nMonth;
        else if (nDay && m_bDays[nDay - 1])
            m_nDayTrack = nDay;
    }
    FillSector();

    return true;
}

function onMouseDown(e)
{
    return true;
}

function onMouseUp(e)
{
    return true;
}

function onClick(e)
{
    var nMousePosX = e.pageX - m_CanvasBounds.left;
    var nMousePosY = e.pageY - m_CanvasBounds.top;

    if (GetDate(nMousePosX - m_nCenterX, m_nCenterY - nMousePosY)) {
        if (m_bSetData) {
            window.opener.changeDate(m_nYear, m_nMonth, m_nDay);
        }
        else if (m_bSvcButton) {
            if (-1 == nYear) {  // >>
                m_nYearBase -= 1;
                if (m_nMinYear - 1 == m_nYearBase)
                    m_nYearBase = m_nMaxYear - 4;
            }
            else if (10 == nYear) { // <<
                m_nYearBase += 1;
                if (m_nMaxYear - 3 == m_nYearBase)
                    m_nYearBase = m_nMinYear;
            }
            nYear = 0;
        }
        if (nYear) {
            if (m_nYear != nYear) {
	        m_nYear = nYear;
	        m_nMonth = m_nDay = 0;
	        for (var i = 0; i < 31; ++i)
	            m_bDays[i] = false;
	        for (var i = 0; i < 12; ++i)
                    m_bMonths[i] = false;

                m_bDays[0] = false;    // *!*
                m_bDays[1] = false;    // *!*
                m_bDays[5] = false;    // *!*
                m_bDays[7] = false;    // *!*
                m_bDays[11] = false;    // *!*
                m_bDays[12] = false;    // *!*
                m_bDays[13] = false;    // *!*
                m_bDays[18] = false;    // *!*
                m_bDays[25] = false;    // *!*
                m_bDays[26] = false;    // *!*

                m_bMonths[0] = true;    // *!*
                m_bMonths[2] = true;    // *!*
                m_bMonths[4] = true;    // *!*
                m_bMonths[6] = true;    // *!*
                m_bMonths[8] = true;    // *!*
                m_bMonths[10] = true;    // *!*
            }
        }
        else if (nMonth && m_bMonths[nMonth - 1]) {
            if (m_nMonth != nMonth) {
            	for (var i = 0; i < 31; ++i)
                    m_bDays[i] = false;
            	m_nMonth = nMonth;
            	m_nDay = 0;

                m_bDays[0] = true;    // *!*
                m_bDays[1] = true;    // *!*
                m_bDays[5] = true;    // *!*
                m_bDays[7] = true;    // *!*
                m_bDays[11] = true;    // *!*
                m_bDays[12] = true;    // *!*
                m_bDays[13] = true;    // *!*
                m_bDays[18] = true;    // *!*
                m_bDays[25] = true;    // *!*
                m_bDays[26] = true;    // *!*
            }
        }
        else if (nDay && m_bDays[nDay - 1])
            m_nDay = nDay;
        FillSector();
    }
    return true;
}

function GetDate(nPosX, nPosY)
{
    nDay = 0;
    nYear = 0;
    nMonth = 0;

    m_bSetData = false;
    m_bSvcButton = false;

    var nSquareRadius = nPosX * nPosX + nPosY * nPosY;
    var nAngle = 90 - 180 * Math.atan2(nPosY, nPosX) / Math.PI;
    if (nAngle < 0) nAngle += 360;
    
    if (nSquareRadius <= m_nRadiusCenter * m_nRadiusCenter) {       // Г–ГҐГ­ГІГ°Г Г«ГјГ­Г Гї Г§Г®Г­Г 
        m_bSetData = true;
    }
    else if (nSquareRadius <= m_nRadiusYear * m_nRadiusYear) {      // Year
        nYear = Math.floor((nAngle - 30) / 30);
        if (nYear < 0 || nYear > 9)  // << / >>
           m_bSvcButton = true;
        else {  // Year
            nYear >>= 1;
            nYear += m_nYearBase;
        }
    }
    else if (nSquareRadius <= m_nRadiusMonth * m_nRadiusMonth) {    // Month
        nMonth = Math.ceil(nAngle / 30);
        if (0 == nMonth)
            nMonth = 12;
    }
    else if (nSquareRadius <= m_nRadiusDay * m_nRadiusDay) {        // Day
        nDay = Math.ceil(32 * nAngle / 360);
        if (nDay > 31)
            nDay = 1;
    }
    else return false;

    return true;
}

function FillSector()
{
    var canvas = document.getElementsByTagName('canvas')[0];
    var context = canvas.getContext('2d');

    context.clearRect(0, 0, canvas.width, canvas.height);

    DrawCalendar(context);

    for (var i = 0; i < 12; ++i) {
        if (true == m_bMonths[i])
            PaintSelection(context, 0, i + 1, 0, "rgba(98, 135, 187, 1)", "rgba(164, 191, 221, 1)"); // "rgba(174, 201, 231, 1)"
    }

    for (var i = 0; i < 31; ++i) {
        if (true == m_bDays[i])
            PaintSelection(context, 0, 0, i + 1, "rgba(146, 126, 163, 1)", "rgba(215, 190, 220, 1)"); // "rgba(174, 201, 231, 1)"
    }

    PaintSelection(context, m_nYear, m_nMonth, m_nDay, "rgba(213, 101, 19, 1)", "rgba(255, 243, 131, 1)");
    PaintSelection(context, m_nYearTrack, m_nMonthTrack, m_nDayTrack, "rgba(10, 100, 60, 1)", "rgba(80, 180, 240, 1)"); // "rgba(100, 240, 180, 1)"

    DrawText(context);
}

function DrawCalendar(context)
{
    var i;
    var nAngle;

    context.save();

    // Day
    context.lineWidth = 2;
    context.fillStyle = "rgba(229, 216, 234, 1)";
    context.strokeStyle = "rgba(146, 126, 163, 1)";
    context.beginPath();
    context.arc(m_nCenterX, m_nCenterY, m_nRadiusDay, - 9 * Math.PI / 16, - 7 * Math.PI / 16);
    context.arc(m_nCenterX, m_nCenterY, m_nRadiusMonth, - 7 * Math.PI / 16, - 9 * Math.PI / 16, true);
    context.fill();
    context.stroke();
    for (i = 0; i < 30; i += 1) {
        context.beginPath();
        nAngle = - 7 * Math.PI / 16 + i * Math.PI / 16;
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusDay, nAngle, nAngle + Math.PI / 16);
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusMonth, nAngle + Math.PI / 16, nAngle, true);
        context.fill();
        context.stroke();
    }

    // Month
    context.strokeStyle = "rgba(98, 135, 187, 1)";
    for (i = 0; i < 12; i += 1) {
        context.beginPath();
        nAngle = - Math.PI + i * Math.PI / 6;
        context.fillStyle = "rgba(205, 222, 243, 1)";
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusYear, nAngle, nAngle + Math.PI / 6);
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusMonthDigit, nAngle + Math.PI / 6, nAngle, true);
        context.fill();
        context.stroke();
        context.beginPath();
        context.fillStyle = "rgba(174, 201, 231, 1)";
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusMonthDigit, nAngle + Math.PI / 6, nAngle, true);
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusMonth, nAngle, nAngle + Math.PI / 6);
        context.fill();
        context.stroke();
    }

    // Year
    context.fillStyle = "rgba(255, 251, 219, 1)";
    context.strokeStyle = "rgba(222, 211, 145, 1)";
    for (i = 0; i < 6; i += 1) {
        context.beginPath();
        nAngle = - Math.PI + i * Math.PI / 3;
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusYear, nAngle, nAngle + Math.PI / 3);
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusCenter, nAngle + Math.PI / 3, nAngle, true);
        context.fill();
        context.stroke();
    }
    context.beginPath();
    context.arc(m_nCenterX, m_nCenterY, m_nRadiusYear, - 2 * Math.PI / 3, - Math.PI / 2);
    context.arc(m_nCenterX, m_nCenterY, m_nRadiusCenter, - Math.PI / 2, - 2 * Math.PI / 3, true);
    context.fill();
    context.stroke();

    // Center
    context.beginPath();
    if (m_nYear && m_nMonth && m_nDay) {
        context.lineWidth = 6;
        context.fillStyle = "rgba(196, 219, 154, 1)";
        context.strokeStyle = "rgba(94, 123, 42, 1)";
    }
    else {
        context.fillStyle = 'white';
        context.strokeStyle = 'black';
    }
    context.arc(m_nCenterX, m_nCenterY, m_nRadiusCenter, 0, 2 * Math.PI);
    context.fill();
    context.stroke();

    context.restore();
}

function DrawText(context)
{
    var i, j;

    context.save();

    var sDate = "";
    if (m_nDay)
        sDate += ((m_nDay < 10 ? "0" + m_nDay : m_nDay) + ".");
    if (m_nMonth)
        sDate += ((m_nMonth < 10 ? "0" + m_nMonth : m_nMonth) + ".");
    if (m_nYear)
        sDate += m_nYear;

    context.font = 'bold 26px Arial';
    if (m_nYear && m_nMonth && m_nDay)
        context.fillStyle = "rgba(94, 123, 42, 1)";
    else context.fillStyle = 'black';
    context.fillText(sDate, m_nCenterX - 6 * sDate.length, m_nCenterY + 10);

    // Browse Year
    context.fillStyle = "rgba(222, 211, 145, 1)";
    context.font = 'bold 30px Arial';
    context.fillText("<<", m_nCenterX - 45, m_nCenterY - 82);
    context.fillText(">>", m_nCenterX + 11, m_nCenterY - 82);

    // Year
    context.fillStyle = "#000000";
    context.font = 'bold 30px Arial';
    context.translate(m_nCenterX, m_nCenterY);
    for (i = 0; i < 5; i += 1) {
        context.rotate(Math.PI / 3);
        context.fillText("" + (m_nYearBase + i), - 30, 140 - m_nCenterY);
    }
    context.rotate(Math.PI / 3);

    // Symbol Month
    var sMonths = [ "ЯНВАРЬ  ", "ФЕВРАЛЬ  ", " МАРТ   ", "АПРЕЛЬ  ", " МАЙ    ", " ИЮНЬ  ", " ИЮЛЬ  ", " АВГУСТ   ", "СЕНТЯБРЬ  ", "ОКТЯБРЬ  ", " НОЯБРЬ  ", "ДЕКАБРЬ  " ];
    context.font = '14px Verdana';
    for (i = 0; i < 12; i += 1) {
        for (j = 0; j < sMonths[i].length; j += 1) {
            context.rotate(Math.PI / 6 / sMonths[i].length);
            context.fillText(sMonths[i][j], 0, 48 - m_nCenterY);
        }
    }

    var dAngle;
    // Digit Month
    var sMonthsD = [ " 1", " 2", " 3", " 4", " 5", " 6", " 7", " 8", " 9", "10", "11", "12" ];
    context.font = 'bold 40px Arial';
    context.translate(- m_nCenterX, - m_nCenterY);
    for (i = 0; i < 12; i += 1) {
        dAngle = Math.PI * (2 * i - 5) / 12;
        context.fillText(sMonthsD[i], m_nCenterX + (m_nRadiusYear + m_nRadiusMonthDigit) / 2 * Math.cos(dAngle) - 20,
        15 + m_nCenterY + (m_nRadiusYear + m_nRadiusMonthDigit) / 2 * Math.sin(dAngle));
    }

    // Day
    context.font = 'bold 20px Arial';
    context.fillText("1", m_nCenterX - 5, m_nCenterY - (m_nRadiusMonth + m_nRadiusDay) / 2 + 8);
    for (i = 0; i < 30; i += 1) {
        dAngle = - Math.PI / 2 + 3 * Math.PI / 32 + i * Math.PI / 16;
        context.fillText((i < 8 ? " " : "") + (i + 2), m_nCenterX + (m_nRadiusMonth + m_nRadiusDay) / 2 * Math.cos(dAngle) - 10,
        8 + m_nCenterY + (m_nRadiusMonth + m_nRadiusDay) / 2 * Math.sin(dAngle));
    }

    context.restore();
}

function PaintSelection(context, nYear, nMonth, nDay, strokeColor, fillColor)
{
    var dAngle = 0.0;

    if (nYear) {
        if (nYear - m_nYearBase < 5 && nYear - m_nYearBase > - 1) {
            dAngle = Math.PI * (nYear - m_nYearBase - 1) / 3;
            DrawPie(context, m_nRadiusCenter, m_nRadiusYear, dAngle, dAngle + Math.PI / 3, strokeColor, fillColor);
        }
    }
    if (nMonth) {
        dAngle = Math.PI * (nMonth - 4) / 6;
        DrawPie(context, m_nRadiusYear, m_nRadiusMonthDigit, dAngle, dAngle + Math.PI / 6, strokeColor, fillColor);
        DrawPie(context, m_nRadiusMonthDigit, m_nRadiusMonth, dAngle, dAngle + Math.PI / 6, strokeColor, fillColor);
    }
    if (nDay) {
        if (1 == nDay)
            DrawPie(context, m_nRadiusMonth, m_nRadiusDay, - 9 * Math.PI / 16, - 7 * Math.PI / 16, strokeColor, fillColor);
        else {
            dAngle = Math.PI * (nDay - 9) / 16;
            DrawPie(context, m_nRadiusMonth, m_nRadiusDay, dAngle, dAngle + Math.PI / 16, strokeColor, fillColor);
        }
    }
}

function DrawPie(context, nRadius1, nRadius2, dAngle1, dAngle2, strokeColor, fillColor)
{
    context.save();
    context.beginPath();
    context.lineWidth = 4;
    context.arc(m_nCenterX, m_nCenterY, nRadius1 + 1, dAngle1, dAngle2);
    context.arc(m_nCenterX, m_nCenterY, nRadius2 - 1, dAngle2, dAngle1, true);
    context.arc(m_nCenterX, m_nCenterY, nRadius1 + 1, dAngle1, dAngle2);
    context.arc(m_nCenterX, m_nCenterY, nRadius2 - 1, dAngle2, dAngle1, true);
    context.strokeStyle = strokeColor;
    context.fillStyle = fillColor;
    context.stroke();
    context.fill();
    context.restore();
}
