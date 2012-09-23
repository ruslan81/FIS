jQuery.fn.searchTree = function (options) {

    var opts = jQuery.extend({}, jQuery.fn.searchTree.defaults, options);
    // ----------------------------------------------------------------------

    // basic code
    // ----------------------------------------------------------------------
    return this.each(function () {
        var $this = $(this).parent();

        //destroy previous if exist
        if ($(".search-outer", $this).length != 0 || $(".search-outer", $this.parent()).length != 0) {
            jQuery.fn.searchTree.destroy($this);
        }

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
            jQuery.fn.searchTree.search($this.children("ul"), searchRequest,false);
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
    param hasParentEntry determine if parent node has search string
*/
jQuery.fn.searchTree.search = function ($this, request, hasParentEntry) {
    var hasEntry = false;
    var children = $this.children('li');
    //console.log("length: " + children.length);
    for (var i = 0; i < children.length; i++) {
        var currentChildHasEntry = false;

        var item = $("div:first > span > a > span", $(children[i]));
        var rgxp = new RegExp(request, 'gi');
        var index = item.text().search(rgxp);
        if (index != -1) {
            //console.log(item.text());
            //console.log(item.length);

            item.highlight(request, "highlight");

            hasEntry = true;
            currentChildHasEntry = true;
        } else {
            /*if (!$(children[i]).hasClass("wijmo-wijtree-parent")) {
                //$(children[i]).css("display", "none");
            }*/
            if (!hasParentEntry) {
                $(children[i]).css("display", "none");
                //console.log("hide: " + item.text());
            }
        }

        if ($(children[i]).hasClass("wijmo-wijtree-parent")) {
            var ul = $(children[i]).children("ul");
            var newHasParentEntry = currentChildHasEntry ? currentChildHasEntry : hasParentEntry;

            if (jQuery.fn.searchTree.search(ul, request, newHasParentEntry)) {

                $(children[i]).wijtreenode({ expanded: true });

                $(children[i]).css("display", "block");

                hasEntry = true;
            } else {

                $(children[i]).wijtreenode({ expanded: false });

                //$(children[i]).css("display", "none");
            }
        }


    }
    return hasEntry
};

// define reset function
jQuery.fn.searchTree.reset = function ($this) {
    var items = $("div > span > a > span", $this);
    $(".highlight").replaceWith(function () { return $(this).contents(); });

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


// define destroy function
jQuery.fn.searchTree.destroy = function ($this) {
    $(".search-outer", $this).remove();
    $(".cancel-search", $this).remove();

    $(".search-outer", $this.parent()).remove();
    $(".cancel-search", $this.parent()).remove();
};