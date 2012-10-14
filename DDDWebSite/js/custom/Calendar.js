(function( $ ){

    var settings = {
        'height-ratio-percent' : '33',
        'http-request-timeout' : '60000',
        'card-id' : '0',
        'org-id' : '0',
        'url': 'Data.aspx/GetOverlookDriverNodeData',
        
	'background-color-CenterOn' : "rgba(196, 219, 154, 1)",
	'background-color-CenterOff' : "transparent",
	'background-color-Year' : '#FFFBDB',	// rgba(255, 251, 219, 1)
	'background-color-MonthDigit' : 'rgba(205, 222, 243, 1)',
	'background-color-MonthSymbol' : 'rgba(174, 201, 231, 1)',
	'background-color-MonthWithData' : 'rgba(164, 191, 221, 1)',
	'background-color-Day' : "rgba(229, 216, 234, 1)",
	'background-color-DayWithData' : "rgba(215, 190, 220, 1)",
	'background-color-Selected' : 'rgba(255, 243, 131, 1)',
	'background-color-MouseTracking' : 'rgba(80, 180, 240, 1)',

	'color-CenterOn' : "rgba(94, 123, 42, 1)",
	'color-CenterOff' : "black",
	'color-Year' : 'rgba(222, 211, 145, 1)',
	'color-Month' : 'rgba(98, 135, 187, 1)',
	'color-MonthWithData' : 'rgba(98, 135, 187, 1)',
	'color-Day' : "rgba(146, 126, 163, 1)",
	'color-DayWithData' : "rgba(146, 126, 163, 1)",
	'color-Selected' : 'rgba(213, 101, 19, 1)',
	'color-MouseTracking' : 'rgba(10, 100, 60, 1)',

	'text-color-CenterOn' : "rgba(94, 123, 42, 1)",
	'text-color-CenterOff' : "black",
	'text-color-Arrow' : 'rgba(222, 211, 145, 1)',
	'text-color-Other' : '#000000'
    };

    var methods = {
        init : function(options) {
            m_HeightRatioPercent = settings["height-ratio-percent"];
            m_HttpRequestTimeout = settings["http-request-timeout"];
            m_CardID = settings["card-id"];
            m_OrgID = settings["org-id"];
            m_Url = settings["url"];

	    m_BackgroundColorCenterOn = settings["background-color-CenterOn"];
	    m_BackgroundColorCenterOff = settings["background-color-CenterOff"];
	    m_BackgroundColorYear = settings['background-color-Year'];
	    m_BackgroundColorMonthDigit = settings['background-color-MonthDigit'];
	    m_BackgroundColorMonthSymbol = settings['background-color-MonthSymbol'];
	    m_BackgroundColorMonthWithData = settings['background-color-MonthWithData'];
	    m_BackgroundColorDay = settings["background-color-Day"];
	    m_BackgroundColorDayWithData = settings["background-color-DayWithData"];
	    m_BackgroundColorSelected = settings['background-color-Selected'];
	    m_BackgroundColorMouseTracking = settings['background-color-MouseTracking'];

	    m_ColorCenterOn = settings["color-CenterOn"];
	    m_ColorCenterOff = settings["color-CenterOff"];
	    m_ColorYear = settings['color-Year'];
	    m_ColorMonth = settings['color-Month'];
	    m_ColorMonthWithData = settings['color-MonthWithData'];
	    m_ColorDay = settings["color-Day"];
	    m_ColorDayWithData = settings["color-DayWithData"];
	    m_ColorSelected = settings['color-Selected'];
	    m_ColorMouseTracking = settings['color-MouseTracking'];

            m_TextColorCenterOn = settings["text-color-CenterOn"];
            m_TextColorCenterOff = settings["text-color-CenterOff"];
            m_TextColorArrow = settings["text-color-Arrow"];
            m_TextColorOther = settings["text-color-Other"];

            init(this.attr('id'));
	    m_This = this;

            return this.each(function() {
              if ( options ) { 
                $.extend( settings, options );
              }

            });
        },
        show : function() { this.show(); },
        hide : function() { this.hide(); },
        destroy : function() {
           destroy(this.attr('id'));
           return this;
        }
    };

    $.fn.Calendar = function(method) {
        if ( methods[method] ) {
          return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ));
        }
        else if ( typeof method === 'object' || ! method ) {
          return methods.init.apply( this, arguments );
        }
        else {
          $.error( 'Метод ' +  method + ' не существует в jQuery.Calendar' );
        }    
    };

})(jQuery);

function ajax_PostData() {
    if (!document.getElementById) {
         alert("JavaScript:ajax_PostData() - getElementById failed.");
         return;
    }

    var PageRequest = false;

    params = "{'CardID':'" + m_CardID + "', 'OrgID':'" + m_OrgID + "', 'StartDate':'01.01." + m_nYear + "', 'EndDate':'31.12." + m_nYear + "'}";

    if (window.XMLHttpRequest) { // mozilla, safari, etc
        PageRequest = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) { // if IE
        try { PageRequest = new ActiveXObject("Msxml2.XMLHTTP"); }
        catch(e) {
            try { PageRequest = new ActiveXObject("Microsoft.XMLHTTP"); }
            catch(e){}
        }
    }
    else {
        alert("JavaScript:ajax_PostData() - Could not create the XMLHttpRequest Object.");
        return false;
    } // end create XMLHttp PageRequest Object

    if (PageRequest) {
        m_bProcessingRequest = true;
        PageRequest.open('POST', m_Url, true);
        PageRequest.setRequestHeader("Content-type", "application/json; charset=UTF-8");
        PageRequest.send(params);
        var timeout = setTimeout( function(){ PageRequest.abort(); handleError("Time over") }, m_HttpRequestTimeout);
    }

    PageRequest.onreadystatechange = function() {
        if (4 != PageRequest.readyState)
            return;

        m_bProcessingRequest = false;
        clearTimeout(timeout);

        if (200 == PageRequest.status) {
            setDataJSON(PageRequest);
        }
        else {
            handleError(PageRequest.statusText);
        }
    }

    function handleError(message) {
      alert("Ошибка: " + message)
    }

    function setDataJSON(req) {
        m_Results.length = 0;
	    var data = JSON.parse(req.responseText);

        var nCurYear = -1, nCurMonth = -1, curyear, curmon;
	        
	    for (var i = 0; i < data.d.length; i++) {

            m_Results[i] = {'__type': data.d[i].__type, 'YearName': data.d[i].YearName, 'MonthName': data.d[i].MonthName,
                'DayName': data.d[i].DayName, 'Percent': parseFloat(data.d[i].Percent.replace(",", ".")), 'key': data.d[i].key};

            if (4 == m_Results[i].YearName.length && (curyear = parseInt(m_Results[i].YearName)) > -1)
                nCurYear = curyear;

            if (m_nYear == nCurYear) {
                if (m_Results[i].MonthName.length > 1) {
                    if ((curmon = GetMonthNum(m_Results[i].MonthName)) > -1)
                        nCurMonth = curmon;
                }
                else if ("" == m_Results[i].DayName || " " == m_Results[i].DayName || 0 == parseInt(m_Results[i].DayName))
                    m_dCurPercent = m_Results[i].Percent;

                if (nCurMonth > -1 && m_Results[i].Percent > 0) {
                    m_bMonths[nCurMonth] = true;
                }
            }
	    }
        FillSector();
    }

} // end ajax_PostData()

function GetMonthNum(sName) {
    var sMonths = [ "ЯНВАРЬ", "ФЕВРАЛЬ", "МАРТ", "АПРЕЛЬ", "МАЙ", "ИЮНЬ", "ИЮЛЬ", "АВГУСТ", "СЕНТЯБРЬ", "ОКТЯБРЬ", "НОЯБРЬ", "ДЕКАБРЬ" ];
    
    for (var i = 0; i < sMonths.length; i++) {
        if (-1 != sMonths[i].indexOf(sName.toUpperCase()))
            return i;
    }
    return -1;
}

var m_This;
var m_bProcessingRequest;

var m_nRadiusCenter;
var m_nRadiusYear;
var m_nRadiusMonth;
var m_nRadiusMonthDigit;
var m_nRadiusDay;
var m_nCenterX, m_nCenterY;
var nYear, nMonth, nDay;
var m_nYear, m_nMonth, m_nDay;
var m_nYearTrack, m_nMonthTrack, m_nDayTrack;

var m_HeightRatioPercent;
var m_HttpRequestTimeout;
var m_CardID;
var m_OrgID;
var m_Url;

var m_nNominalCalendarDiameter;
var m_nActualCalendarDiameter;
var m_dFactor;

var m_CanvasBounds;
var m_nMinYear;
var m_nMaxYear;
var m_nYearBase;

var m_bSetData;
var m_bSvcButton;

var m_bDays = [];
var m_bMonths = [];
var m_Results = [];
var m_dCurPercent;

var DAYS_COUNT;
var MONTHS_COUNT;

var m_nRecordCount;

var m_CanvasId = '';

var IE = document.all ? true : false;

var m_BackgroundColorCenterOn;
var m_BackgroundColorCenterOff;
var m_BackgroundColorYear;
var m_BackgroundColorMonthDigit;
var m_BackgroundColorMonthSymbol;
var m_BackgroundColorMonthWithData;
var m_BackgroundColorDayWithData;
var m_BackgroundColorSelected;
var m_BackgroundColorMouseTracking;

var m_ColorCenterOn;
var m_ColorCenterOff;
var m_ColorYear;
var m_ColorMonth;
var m_ColorMonthWithData;
var m_ColorDay;
var m_ColorDayWithData;
var m_ColorSelected;
var m_ColorMouseTracking;

var m_TextColorCenterOn;
var m_TextColorCenterOff;
var m_TextColorArrow;
var m_TextColorOther;

function init(idCanvas) {
    m_nDay = 0;
    m_nYear = 0;
    m_nMonth = 0;
   
    DAYS_COUNT = 31;
    MONTHS_COUNT = 12;

    for (var i = 0; i < DAYS_COUNT; ++i)
    	m_bDays[i] = false;
    for (var i = 0; i < MONTHS_COUNT; ++i)
            m_bMonths[i] = false;
    
    m_nMinYear = 1981;
    m_nMaxYear = 2099;
    m_nYearBase = 2007;
    
    m_bSetData = false;
    m_bSvcButton = false;

    m_dCurPercent = 0;
    m_nRecordCount = 0;

    m_CanvasId = idCanvas;
    var canvas = document.getElementById(m_CanvasId);
	canvas.width = m_nActualCalendarDiameter;
	canvas.height = m_nActualCalendarDiameter;
    m_CanvasBounds = getBounds(canvas);
    var context = canvas.getContext('2d');

    canvas.onclick = function(e) { onClick(e); }
    canvas.onmousemove = function(e) { onMouseMove(e); }

    m_nCenterX = canvas.width / 2;
    m_nCenterY = canvas.height / 2;

    m_nNominalCalendarDiameter = 446;
    m_nActualCalendarDiameter = screen.height * m_HeightRatioPercent / 100;
    m_dFactor = 1.0 * m_nActualCalendarDiameter / m_nNominalCalendarDiameter;

    m_nRadiusCenter = 73.0 / m_nNominalCalendarDiameter * canvas.width;
    m_nRadiusYear = 115.0 / m_nNominalCalendarDiameter * canvas.width;
    m_nRadiusMonthDigit = 172.0 / m_nNominalCalendarDiameter * canvas.width;
    m_nRadiusMonth = 190.0 / m_nNominalCalendarDiameter * canvas.width;
    m_nRadiusDay = Math.max(0, canvas.width / 2.0 - 2);

    m_bProcessingRequest = false;

    DrawCalendar(context);
    DrawText(context);
}

function destroy(idCanvas) {
    if (undefined === m_This)
	return;
    m_CanvasId = idCanvas;
    var canvas = document.getElementById(idCanvas);
	canvas.width = m_nActualCalendarDiameter;
	canvas.height = m_nActualCalendarDiameter;
    m_CanvasBounds = getBounds(canvas);
    var context = canvas.getContext('2d');

    canvas.onclick = 0;
    canvas.onmousemove = 0;

    m_nCenterX = 0;
    m_nCenterY = 0;

    m_nRadiusCenter = 0;
    m_nRadiusYear = 0;
    m_nRadiusMonthDigit = 0;
    m_nRadiusMonth = 0;
    m_nRadiusDay = 0;

    m_nYear = 0;
    m_nMonth = 0;
    m_nDay = 0;
    
    m_bSetData = false;
    m_bSvcButton = false;
    
    m_nRecordCount = 0;
    
    DrawCalendar(context);
    //DrawText(context);
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
    var canvas = document.getElementById(m_CanvasId);
    var canvasBounds = getBounds(canvas);
    if (m_CanvasBounds.left != canvasBounds.left) {
        var context = canvas.getContext('2d');
        m_CanvasBounds = canvasBounds;
        DrawCalendar(context);
        DrawText(context);
    }

    var nPosX = (IE) ? event.clientX + document.body.scrollLeft : e.pageX;
    var nPosY = (IE) ? event.clientY + document.body.scrollTop : e.pageY;

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

function onClick(e)
{
    var nPosX = (IE) ? event.clientX + document.body.scrollLeft : e.pageX;
    var nPosY = (IE) ? event.clientY + document.body.scrollTop : e.pageY;

    var nMousePosX = nPosX - m_CanvasBounds.left;
    var nMousePosY = nPosY - m_CanvasBounds.top;

    if (GetDate(nMousePosX - m_nCenterX, m_nCenterY - nMousePosY)) {
        if (m_bSetData) {
            if (m_nYear && m_nMonth && m_nDay) {
                //window.opener.changeDate(m_nYear, m_nMonth, m_nDay);
	    }
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
                for (var i = 0; i < DAYS_COUNT; ++i)
                    m_bDays[i] = false;
                for (var i = 0; i < MONTHS_COUNT; ++i)
                    m_bMonths[i] = false;
		m_This.trigger({ type : 'selectyear', year : m_nYear });
                ajax_PostData();
            }
        }
        else if (nMonth && m_bMonths[nMonth - 1]) {
            if (m_nMonth != nMonth) {
 	       m_This.trigger({ type : 'selectmonth', month : nMonth });
               for (var i = 0; i < DAYS_COUNT; ++i)
                    m_bDays[i] = false;
                m_nMonth = nMonth;
                m_nDay = 0;

                var nCurYear = -1, nCurMonth = -1, nCurDay = 0, curyear, curmon, curday;

	            for (var i = 0; i < m_Results.length; i++) {
                    if (4 == m_Results[i].YearName.length && (curyear = parseInt(m_Results[i].YearName)) > -1)
                        nCurYear = curyear;

                    if ((curmon = GetMonthNum(m_Results[i].MonthName)) > -1)
                        nCurMonth = curmon;

                    if (m_nYear == nCurYear && m_nMonth == nCurMonth + 1) {

                        if (!isNaN(m_Results[i].DayName) && (curday = parseInt(m_Results[i].DayName)) > 0)
                            nCurDay = curday - 1;
                        else m_dCurPercent = m_Results[i].Percent;

                        if (nCurDay > 0 && m_Results[i].Percent > 0) {
                            m_bDays[nCurDay] = true;
                        }
                    }
	            }
            }
        }
        else if (nDay && m_bDays[nDay - 1]) {
            if (m_nDay != nDay) {
                m_nDay = nDay;

  	       m_This.trigger({ type : 'selectday', day : nDay });
               var nCurYear = -1, nCurMonth = -1, nCurDay = 0, curyear, curmon, curday;

	            for (var i = 0; i < m_Results.length; i++) {
                    if (4 == m_Results[i].YearName.length && (curyear = parseInt(m_Results[i].YearName)) > -1)
                        nCurYear = curyear;

                    if ((curmon = GetMonthNum(m_Results[i].MonthName)) > -1)
                        nCurMonth = curmon;

                    if (!isNaN(m_Results[i].DayName) && (curday = parseInt(m_Results[i].DayName)) > 0)
                        nCurDay = curday;

                    if (m_nYear == nCurYear && m_nMonth == nCurMonth + 1 && m_nDay == nCurDay)
                        m_dCurPercent = m_Results[i].Percent;
	            }
            }
        }
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
    
    if (nSquareRadius <= m_nRadiusCenter * m_nRadiusCenter) {       // Центральная зона
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
            nMonth = MONTHS_COUNT;
    }
    else if (nSquareRadius <= m_nRadiusDay * m_nRadiusDay) {        // Day
        nDay = Math.ceil(32 * nAngle / 360);
        if (nDay > DAYS_COUNT)
            nDay = 1;
    }
    else return false;

    return true;
}

function FillSector()
{
    var canvas = document.getElementById(m_CanvasId);
    var context = canvas.getContext('2d');

    context.clearRect(0, 0, canvas.width, canvas.height);

    DrawCalendar(context);

    for (var i = 0; i < MONTHS_COUNT; ++i) {
        if (true == m_bMonths[i])
            PaintSelection(context, 0, i + 1, 0, m_ColorMonthWithData, m_BackgroundColorMonthWithData);//"rgba(164, 191, 221, 1)"); // "rgba(174, 201, 231, 1)"
    }

    for (var i = 0; i < DAYS_COUNT; ++i) {
        if (true == m_bDays[i])
            PaintSelection(context, 0, 0, i + 1, m_ColorDayWithData, m_BackgroundColorDayWithData);//"rgba(215, 190, 220, 1)"); // "rgba(174, 201, 231, 1)"
    }

    PaintSelection(context, m_nYear, m_nMonth, m_nDay, m_ColorSelected, m_BackgroundColorSelected);//"rgba(255, 243, 131, 1)");
    PaintSelection(context, m_nYearTrack, m_nMonthTrack, m_nDayTrack, m_ColorMouseTracking, m_BackgroundColorMouseTracking);//"rgba(80, 180, 240, 1)"); // "rgba(100, 240, 180, 1)"

    DrawText(context);
}

function DrawCalendar(context)
{
    var i;
    var nAngle;

    context.save();

    // Day
    context.lineWidth = 2;
    context.fillStyle = m_BackgroundColorDay;	// "rgba(229, 216, 234, 1)";
    context.strokeStyle = m_ColorDay;
    context.beginPath();
    context.arc(m_nCenterX, m_nCenterY, m_nRadiusDay, 23 * Math.PI / 16, 25 * Math.PI / 16);
    context.arc(m_nCenterX, m_nCenterY, m_nRadiusMonth, 25 * Math.PI / 16, 23 * Math.PI / 16, true);
    context.fill();
    context.stroke();
    for (i = 0; i < 30; ++i) {
        context.beginPath();
        nAngle = - 7 * Math.PI / 16 + i * Math.PI / 16;
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusDay, nAngle, nAngle + Math.PI / 16);
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusMonth, nAngle + Math.PI / 16, nAngle, true);
        context.fill();
        context.stroke();
    }

    // Month
    context.strokeStyle = m_ColorMonth;
    for (i = 0; i < MONTHS_COUNT; ++i) {
        context.beginPath();
        nAngle = - Math.PI + i * Math.PI / 6;
        context.fillStyle = m_BackgroundColorMonthDigit;	// "rgba(205, 222, 243, 1)";
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusYear, nAngle, nAngle + Math.PI / 6);
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusMonthDigit, nAngle + Math.PI / 6, nAngle, true);
        context.fill();
        context.stroke();
        context.beginPath();
        context.fillStyle = m_BackgroundColorMonthSymbol;	// "rgba(174, 201, 231, 1)";
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusMonthDigit, nAngle + Math.PI / 6, nAngle, true);
        context.arc(m_nCenterX, m_nCenterY, m_nRadiusMonth, nAngle, nAngle + Math.PI / 6);
        context.fill();
        context.stroke();
    }

    // Year
    context.fillStyle = m_BackgroundColorYear;	// "rgba(255, 251, 219, 1)";
    context.strokeStyle = m_ColorYear;
    for (i = 0; i < 6; ++i) {
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
        context.lineWidth = 6 * m_dFactor;
        context.fillStyle = m_BackgroundColorCenterOn;	// "rgba(196, 219, 154, 1)";
        context.strokeStyle = m_ColorCenterOn;
    }
    else {
        context.fillStyle = m_BackgroundColorCenterOff;
        context.strokeStyle = m_ColorCenterOff;
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

    context.font = 'bold ' + 26 * m_dFactor + 'px Arial';
    if (m_nYear && m_nMonth && m_nDay)
        context.fillStyle = m_TextColorCenterOn;
    else context.fillStyle = m_TextColorCenterOff;
    context.fillText(sDate, m_nCenterX - 6 * m_dFactor * sDate.length, m_nCenterY + 10 * m_dFactor);

    if (sDate.length) {
        context.font = 'bold ' + 14 * m_dFactor + 'px Arial';
        //sDate = m_nRecordCount + " записей";
	if (false == m_bProcessingRequest)
        	sDate = m_dCurPercent.toFixed(2) + " %";
	else sDate = "Запрос";
        context.fillText(sDate, m_nCenterX - 4 * m_dFactor * sDate.length + 5, m_nCenterY + 30 * m_dFactor);
    }

    // Browse Year
    context.fillStyle = m_TextColorArrow;
    context.font = 'bold ' + 30 * m_dFactor + 'px Arial';
    context.fillText("<<", m_nCenterX - 45 * m_dFactor, m_nCenterY - 82 * m_dFactor);
    context.fillText(">>", m_nCenterX + 11 * m_dFactor, m_nCenterY - 82 * m_dFactor);

    // Year
    context.fillStyle = m_TextColorOther;
    context.font = 'bold ' + 30 * m_dFactor + 'px Arial';
    context.translate(m_nCenterX, m_nCenterY);
    for (i = 0; i < 5; ++i) {
        context.rotate(Math.PI / 3);
        context.fillText("" + (m_nYearBase + i), - 30 * m_dFactor, 140 * m_dFactor - m_nCenterY);
    }
    context.rotate(Math.PI / 3);

    // Symbol Month
    var sMonths = [ "ЯНВАРЬ  ", "ФЕВРАЛЬ  ", " МАРТ   ", "АПРЕЛЬ  ", " МАЙ    ", " ИЮНЬ  ", " ИЮЛЬ  ", " АВГУСТ   ", "СЕНТЯБРЬ  ", "ОКТЯБРЬ  ", " НОЯБРЬ  ", "ДЕКАБРЬ  " ];
    context.font = 14 * m_dFactor + 'px Verdana';
    for (i = 0; i < MONTHS_COUNT; ++i) {
        for (j = 0; j < sMonths[i].length; ++j) {
            context.rotate(Math.PI / 6 / sMonths[i].length);
            context.fillText(sMonths[i][j], 0, 48 * m_dFactor - m_nCenterY);
        }
    }

    var dAngle;
    // Digit Month
    var sMonthsD = [ " 1", " 2", " 3", " 4", " 5", " 6", " 7", " 8", " 9", "10", "11", "12" ];
    context.font = 'bold ' + 40 * m_dFactor + 'px Arial';
    context.translate(- m_nCenterX, - m_nCenterY);
    for (i = 0; i < MONTHS_COUNT; ++i) {
        dAngle = Math.PI * (2 * i - 5) / MONTHS_COUNT;
        context.fillText(sMonthsD[i], m_nCenterX + (m_nRadiusYear + m_nRadiusMonthDigit) / 2 * Math.cos(dAngle) - 20 * m_dFactor,
        15 * m_dFactor + m_nCenterY + (m_nRadiusYear + m_nRadiusMonthDigit) / 2 * Math.sin(dAngle));
    }

    // Day
    context.font = 'bold ' + 20 * m_dFactor + 'px Arial';
    context.fillText("1", m_nCenterX - 5 * m_dFactor, m_nCenterY - (m_nRadiusMonth + m_nRadiusDay) / 2 + 8 * m_dFactor);
    for (i = 0; i < 30; ++i) {
        dAngle = - Math.PI / 2 + 3 * Math.PI / 32 + i * Math.PI / 16;
        context.fillText((i < 8 ? " " : "") + (i + 2), m_nCenterX + (m_nRadiusMonth + m_nRadiusDay) / 2 * Math.cos(dAngle) - 10 * m_dFactor,
        8 * m_dFactor + m_nCenterY + (m_nRadiusMonth + m_nRadiusDay) / 2 * Math.sin(dAngle));
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

if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
