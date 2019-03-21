/**
 Author: Carboni Davide
 Description: init all necessary events into Customers home page
 version: 1.0
 */

/**
 * Entry point
 */
$(document).ready(function () {
    customersPage.init();
});

/**
 *Customers home page events
 * */
var customersPage = {
    init: function () {
        customersPage.loadEvents();
        
    },
    loadEvents: function () {
        // load customer details when row is selected
        $(".customer-clickable-row .clickable").on("click", function (e) {
            e.preventDefault();
            e.stopPropagation();
            customer.details($(this));
        });

        // start search when click in search icon
        $("#search-values").on("click", function (e) {
            e.preventDefault();
            e.stopPropagation();
            if ($("#customer-search-box input").val() == "")
                return;
            $('#customer-search-box').submit();
        });

        // delete string in box search and restore initial search
        $("#delete-search").on("click", function (e) {
            e.preventDefault();
            e.stopPropagation();
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