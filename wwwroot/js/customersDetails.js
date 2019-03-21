/**
 Author: Carboni Davide
 Description: init all necessary events into Customers details page with purchases elements
 version: 1.0
 */

/**
 * Entry point
 */
$(document).ready(function () {
    customerDetailsPage.init();
});

/**
 *Customer-Details page events
 * */
var customerDetailsPage = {
    init: function () {
        customerDetailsPage.loadEvents();
    },
    loadEvents: function () {
         /* Popovers enable */
        $('[data-toggle="popover"]').popover({ delay: { show: 1000, hide: 100 }, trigger: "hover" });

        /* Vaucher button */
        $("#purchase-add-vaucher").on("click", function (e) {
            e.preventDefault();
            e.stopPropagation();
            //get input data
            var customerId = $("#add-vaucher-customer #customerId").val();
            var amount = - $("#add-vaucher-customer #amount").val();
            var description = $("#add-vaucher-customer #description").val();
            //send data to api to store new vaucher
            purchase.add(customerId, amount, description, false);
        });

        /* Add new Purchase to a customer */
        $("#add-purchase-customer-button").on("click", function (e) {
            e.preventDefault();
            //get input data
            var customerId = $("#customer-purchase-add #customerId").val();
            var amount = $("#customer-purchase-add #amount").val();
            var description = $("#customer-purchase-add #description").val();
            //send data to api to store new vaucher
            purchase.add(customerId, amount, description, true);
        });

    },
}

/**
 * Purchases functions
 * */

var purchase = {
    // add  a new purchase into table
    add: function (customerId, amount, description, noMsg) {
        let current_datetime = new Date()
        let formatted_date = current_datetime.getFullYear() + "/" + (current_datetime.getMonth() + 1) + "-" + current_datetime.getDate() + " " + current_datetime.getHours() + ":" + current_datetime.getMinutes() + ":" + current_datetime.getSeconds()

        // use tha api to store a new purchase for the current customer and return a partial view without reload the page
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
    // update the purchases details in view
    update: function (customerId) {
        // return the partial view
        $("#partialView").load("/Customers/DetailsPartial/"+customerId);
    }
}