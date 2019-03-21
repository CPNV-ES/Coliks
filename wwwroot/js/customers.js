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

        /* Vaucher button */

        $("#purchase-add-vaucher").on("click", function(){
            //get input data
            var customerId = $("#customerId").val();
            var amount = - $("#amount").val();
            var description = $("#description").val();
            //send data to api to store new vaucher
            purchase.add(customerId, amount, description, false);
        });

        /* Add new Purchase to a customer */

        $("#add-purchase-customer-button").on("click", function () {
            //get input data
            var customerId = $("#add-purchase-customer #customerId").val();
            var amount = $("#add-purchase-customer #amount").val();
            var description = $("#add-purchase-customer #description").val();
            //send data to api to store new vaucher
            purchase.add(customerId, amount, description, true);
        });

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

/**
 * Purchases functions
 * */

var purchase = {
    add: function (customerId, amount, description, noMsg) {
        let current_datetime = new Date()
        let formatted_date = current_datetime.getFullYear() + "/" + (current_datetime.getMonth() + 1) + "-" + current_datetime.getDate() + " " + current_datetime.getHours() + ":" + current_datetime.getMinutes() + ":" + current_datetime.getSeconds()

        $.ajax({
            accepts: "application/json",
            contentType: "application/json",
            type: "POST",
            url: '/api/PurchasesApi',
            data: JSON.stringify({CustomerId: customerId, Date: formatted_date, Description: description, Amount: amount}),
        })
            .done(function () {
                if (!noMsg) { alert("Le bon d'achat à été rajouter"); }
                purchase.update(customerId);
            })
            .fail(function () {
                if (!noMsg) alert("Le bon d'achat n'a été pas crée");
            });
    },
    update: function (customerId) {
        $("#partialView").load("/Customers/DetailsPartial/"+customerId);
    }
}