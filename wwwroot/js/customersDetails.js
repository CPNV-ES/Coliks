/**
 Author: Carboni Davide
 Description: init all necessary events into Customers details page with purchases elements
 version: 1.0
 */

/**
 * Entry point
 */

// to check the purchase form status
var formStatus = {};

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

        // add events in form purchase
        purchase.getInputFieldsList().forEach(function (field) {
            // set false the state of each field (not valide)
            formStatus[field] = false;
            $('#customer-purchase-add ').on("change paste keyup click", field, function () {
                // change the state of field
                formStatus[field] = purchase.validateFormFields(field);
                // check if button is available
                purchase.enableButtonSubmit();
            });
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
    },
    // validate form fields values
    validateFormFields(fieldId) {
        // return if the current field is valide or not
        switch (fieldId) 
        {
            case '#amount':                       
                return purchase.validateAmount(fieldId);
                break;
            case '#description':                   
                return purchase.validateDescription(fieldId);
                break;
        }
        return false;
    },
    validateAmount: function (fieldId) {
        value = $(fieldId).val().replace(/\s/g, '');

        if (value.length == 0)                 // Amount must be at at least 1 chars long
        {
            errorMessage.show("Pas de montant", fieldId);
            return false;
        }
        if (value == 0)                        // Amount must be different to 0
        {
            errorMessage.show("Montant trop petit", fieldId);
            return false;
        }
        if (value > 1000000)                   // Amount must be < 1000000
        {
            errorMessage.show("Montant trop grand", fieldId);
            return false
        }
        if (value < 0)                          // Amount must be > 0
        {
            errorMessage.show("Montant negatif", fieldId);
            return false
        }

        errorMessage.hide(fieldId);
        return true;
    },
    validateDescription: function (fieldId) {
        value = $(fieldId).val().replace(/\s/g, '');

        if (value.length == 0)                        // Description must be at least 1 chars long
        { 
            errorMessage.show("Pas de description", fieldId);
            return false;
        }
        if (value.length > 50)                       // Description must be < 50 chars
        {
            errorMessage.show("La description doit contenir au maximum 50 caracters", fieldId);
            return false
        }

        errorMessage.hide(fieldId);
        return true;
    },
    enableButtonSubmit() {
        // read each field status and check if button can be enabled
        var res = true;
        for (var key in formStatus) {                  // check
            res =  res && formStatus[key];
        }

        // enable button if forms is valide
        if (res) {
            // enable
            $("#add-purchase-customer-button").removeClass("disabled");
            $("#add-purchase-customer-button").removeAttr("disabled");
        } else {
            // disable
            $("#add-purchase-customer-button").addClass("disabled");
            $("#add-purchase-customer-button").attr("disabled","disabled");
        }
    },
    // array of all field in form
    getInputFieldsList() {
        return ["#amount", "#description"];
    }
}

// module message
// hide show a message using bootstrap classes
// par: 
//  msg: the message to show
//  node: the node in html dom

var errorMessage = {
    show: function (msg, node) {
        // change style into input field with a red border color
        $(node).removeClass("is-valid");
        $(node).addClass("is-invalid");

        // show the message
        $("#error-" + node.substring(1)).removeClass("d-none");
        $("#error-" + node.substring(1)).html(msg);
    },
    hide: function (node) {
        // change style into input field into green border color
        $(node).removeClass("is-invalid");
        $(node).addClass("is-valid");

        // hide the message
        $("#error-" + node.substring(1)).addClass("d-none");

    }
}