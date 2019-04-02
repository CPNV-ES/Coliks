/**
 Author: Carboni Davide
 Description: init all necessary events into Edit Customer Page
 version: 1.0
 */

/**
 * Entry point
 */
$(document).ready(function () {
    customersModifyPage.init();
});

/**
 *Customers home page events
 * */
var customersModifyPage = {
    init: function () {
        customersModifyPage.loadEvents();
        
    },
    loadEvents: function () {
        // validate the edit form
        $(".edit-customer-validation").on("change paste keyup click blur", function (e) {
            e.preventDefault();
            e.stopPropagation();
            $("#edit-form-customer").validate();
            customer.enableButtonSubmit($("#edit-form-customer").valid());
        });
    },
}

/**
 * Customers base functions
 * */
var customer = {    
    enableButtonSubmit(res) {
        // enable button if forms is valide
        if (res) {
            console.log("valide");
            // enable
            $("#edit-form-customer button").removeClass("disabled");
            $("#edit-form-customer button").removeAttr("disabled");
        } else {
            console.log("invalide");
            // disable
            $("#edit-form-customer button").addClass("disabled");
            $("#edit-form-customer button").attr("disabled", "disabled");
        }
    }
}