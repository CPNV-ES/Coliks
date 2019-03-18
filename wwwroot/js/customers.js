/**
 * Entry point
 */
$(document).ready(function () {
    htmlPage.init();
});


/**
 *Home page events
 * */
var htmlPage = {
    init: function () {
        this.loadEvents();
    },
    loadEvents: function () {
        $(".customer-clickable-row .clickable").on("click", function (e) {
            e.preventDefault();
            customer.details($(this));
        });

        $("#search-values").on("click", function () {
            if ($("#customer-search-box input").val() == "")
                return;
            $('#customer-search-box').submit();
        });

        $("#delete-search").on("click", function () {
            if ($("#customer-search-box input").val() == "")
                return;
            $("#customer-search-box input").val("");
            $('#customer-search-box').submit();
        });

        /* restore old string into search bar */
        var v = $("#customer-search-box input").val();
        $("#customer-search-box input").val("").val(v);

         /* Popovers enable */
        $('[data-toggle="popover"]').popover({ delay: { show: 1000, hide: 100 }, trigger: "hover" });

    },
}

/**
 * Customers base functions
 * */
var customer = {    
    details: function (elm) {
        window.location = this.getDetailsRoute(elm);
    },
    getDetailsRoute: function (elm) {
        return $(elm).parent().find('#customer-home-actions a:nth-child(2)').attr('href');
    }
}