jQuery.fn.searchTree = function (options) {

    var opts = jQuery.extend({}, jQuery.fn.searchTree.defaults, options);
    // ----------------------------------------------------------------------

    // basic code
    // ----------------------------------------------------------------------
    return this.each(function () {
        var $this = $(this).parent();

        // call our create function
        jQuery.fn.searchTree.create($this);

    });
    // ----------------------------------------------------------------------
};

// default settings
// ----------------------------------------------------------------------
jQuery.fn.searchTree.defaults = {
    foreground: 'red',
    background: 'yellow'
};

// define our create function
jQuery.fn.searchTree.create = function ($this) {
    var element =
        "<div class='search-outer'>" +
            "<input type='text' maxlength='60' size='15' value='' class='search-name' placeholder='Поиск...'/>" +
	        "<div class='search-button'></div>" +
		"</div>" +

		"<div class='cancel-search icon-cancel-search' title='Отменить результаты поиска'>" +
		"</div>";
    $this.prepend(element);

    $(".search-button", $this.parent()).click(function () {
        if ($(".search-name", $this.parent()).attr("value") != "") {
            var searchRequest = $(".search-name", $this.parent()).attr("value");
            $(".search-name", $this.parent()).attr("readonly", "true");
            $(".search-name", $this.parent()).addClass("search-name-readonly");
            $(".cancel-search", $this.parent()).show();
            jQuery.fn.searchTree.search($this.children("ul"), searchRequest);
        }
    });

    $(".search-name", $this.parent()).keypress(function (event) {
        if (event.keyCode == 13) {
            $(".search-button", $this.parent()).click();
            return false;
        }
    });

    $(".cancel-search", $this.parent()).click(function () {
        //$(".search-name").attr("value", "");
        var searchRequest = "";
        $(".search-name", $this.parent()).removeAttr("readonly");
        $(".search-name", $this.parent()).removeClass("search-name-readonly");
        $(".cancel-search", $this.parent()).hide();
        jQuery.fn.searchTree.reset($this);
    });
};

// define our search function
/**
    param $this <ul> element
    param request text for search
*/
jQuery.fn.searchTree.search = function ($this, request) {
    var hasEntry = false;
    var children = $this.children('li');
    console.log("length: " + children.length);
    for (var i = 0; i < children.length; i++) {

        if ($(children[i]).hasClass("wijmo-wijtree-parent")) {
            var ul = $(children[i]).children("ul");

            if (jQuery.fn.searchTree.search(ul, request)) {
                /*var triangle = $("div:first > span > span.ui-icon", $(children[i]));
                triangle.addClass("ui-icon-triangle-1-se");
                triangle.removeClass("ui-icon-triangle-1-e");
                ul.css("display", "block");*/
                $(children[i]).wijtreenode({ expanded: true });

                hasEntry = true;
            } else {
                /*var triangle = $("div:first > span > span.ui-icon", $(children[i]));
                triangle.removeClass("ui-icon-triangle-1-se");
                triangle.addClass("ui-icon-triangle-1-e");
                ul.css("display", "none");*/
                $(children[i]).wijtreenode({ expanded: false });

                $(children[i]).css("display", "none");
            }
        }

        var item = $("div:first > span > a > span", $(children[i]));
        var rgxp = new RegExp(request, 'gi');
        var index = item.text().search(rgxp);
        if (index != -1) {
            console.log(item.text());
            console.log(item.length);

            item.highlight(request, "highlight");

            $(children[i]).css("display", "block");

            hasEntry = true;
        } else {
            if (!$(children[i]).hasClass("wijmo-wijtree-parent")) {
                $(children[i]).css("display", "none");
            }
        }
    }
    return hasEntry
};

// define reset function
jQuery.fn.searchTree.reset = function ($this) {
    var items = $("div > span > a > span", $this);
    $(".highlight").replaceWith(function () { return $(this).contents(); });

    /*$('span .ui-icon', $this).removeClass("ui-icon-triangle-1-se");
    $('span .ui-icon', $this).addClass("ui-icon-triangle-1-e");
    $('.wijmo-wijtree-child', $this).css("display", "none");*/
    $("li", $this).wijtreenode({ expanded: false });

    $("li", $this).css("display", "block");
}


//highlight string in html of element
jQuery.fn.highlight = function (str, className) {
    var regex = new RegExp(str, "gi");
    return this.each(function () {
        this.innerHTML = this.innerHTML.replace(regex, function (matched) {
            return "<span class=\"" + className + "\">" + matched + "</span>";
        });
    });
};