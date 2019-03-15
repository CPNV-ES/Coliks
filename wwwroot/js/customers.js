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
        })
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

