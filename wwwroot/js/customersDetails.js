/**
 Author: Carboni Davide
 Description: Manage the customer details page
 version: 1.0
 */

// to check the purchase form status
var formStatus = {};
var isPrinted = false;

/**
 * Entry point
 */
$(document).ready(function () {
    customerDetailsPage.init();
});

/**
 * Base Module
 * */
var customerDetailsPage = {
    init: function () {
        customerDetailsPage.loadEvents();
    },
    loadEvents: function () {

        // print vaucher
        $(".modal").on("click", "#vaucher-printable-button", function (e) {
            e.stopPropagation();
            e.preventDefault();
            vaucher.print();
        });

         /* Popovers enable */
        $('[data-toggle="popover"]').popover({ delay: { show: 1000, hide: 100 }, trigger: "hover" });

        /* Vaucher button */
        $("#vaucher-customer-add-button").on("click", function(e) {
            e.preventDefault();
            e.stopPropagation();
            purchase.addVaucher();
        });

        /* Add new Purchase to a customer */
        $("#purchase-customer-add-button").on("click", function(e) {
            e.preventDefault();
            e.stopPropagation();
            purchase.addPurchase();
        });

        // add events in form purchase
        purchase.getInputFieldsList().forEach(function (field) {
            // set false the state of each field (not valide)
            formStatus[field] = false;
            $('#purchase-customer-add').on("change paste keyup click", field, function () {
                // change the state of field
                formStatus[field] = purchase.validateFormFields(field);
                // check if button is available
                purchase.enableButtonSubmit();
            });
        });      

        // Stop propagation
        // All event in the click button inside the modal are executed three times. PreventDefault or stopPropagation don't work in modal popup
        // It means that the print button inside the modal popup send three request to print the document
        // To resolve the problem we can use a manual stop propagation using the isPrinted variable
        $('#vaucher-printable-button').on('shown.bs.modal',  function () {
            isPrinted = false;
        });
    },
}

/**
 * Purchases Module
 * */
var purchase = {
    // add  a new purchase into table
    addPurchase: function () {
        var fieldsData = purchase.getFieldsData("purchase");
        purchase.postData(fieldsData['date'], fieldsData['customerId'], fieldsData['amount'], fieldsData['description'], function (output) {
            if (output) {
                purchase.update(fieldsData['customerId']);
            }
        });
    },
    // add  a new vaucher into table
    addVaucher: function() {
        var fieldsData = purchase.getFieldsData("vaucher");
        purchase.postData(fieldsData['date'], fieldsData['customerId'], -fieldsData['amount'], fieldsData['description'], function (output) {
            if (output) {
                purchase.update(fieldsData['customerId']);
                $("#vaucher-modal-value").html(fieldsData['vaucher'] + " CHF.-");
                vaucher.show();
            }
        });
    },
    // get fields data from purchase or vaucher
    getFieldsData: function (node) {
        var fieldsData = {}
        // generate a new data
        var current_datetime = new Date()
        fieldsData['customerId'] = $("#" + node + "-customer-add #customerId").val();
        fieldsData['amount'] = $("#" + node + "-customer-add #amount").val();
        fieldsData['description'] = $("#" + node + "-customer-add #description").val();
        fieldsData['vaucher'] = $("#" + node + "-customer-add #vaucher").val();
        fieldsData['date'] = current_datetime.getFullYear() + "/" + (current_datetime.getMonth() + 1) + "-" + current_datetime.getDate() + " " + current_datetime.getHours() + ":" + current_datetime.getMinutes() + ":" + current_datetime.getSeconds();
        return fieldsData;
    },
    // ajax request to store a new purchase into database using API
    postData: function (date, customerId, amount, description, handleData) {
        // use tha api to store a new purchase for the current customer and return a partial view without reload the page
        $.ajax({
            accepts: "application/json",
            contentType: "application/json",
            type: "POST",
            url: '/api/PurchasesApi',
            data: JSON.stringify({ CustomerId: customerId, Date: date, Description: description, Amount: amount }),
        })
            .done(function () {
                handleData(true);
            })
            .fail(function () {
                handleData(false);
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
    // validate the amount value
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
    //validate the description
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
    // enable/disable submit button if form is valid
    enableButtonSubmit() {
        // read each field status and check if button can be enabled
        var res = true;
        for (var key in formStatus) {                  // check
            res =  res && formStatus[key];
        }

        // enable button if forms is valide
        if (res) {
            // enable
            $("#purchase-customer-add-button").removeClass("disabled");
            $("#purchase-customer-add-button").removeAttr("disabled");
        } else {
            // disable
            $("#purchase-customer-add-button").addClass("disabled");
            $("#purchase-customer-add-button").attr("disabled","disabled");
        }
    },
    // array of all field in form
    getInputFieldsList() {
        return ["#amount", "#description"];
    }
}

/**
 * Message Module
 * 
 * hide show a message using bootstrap classes
 * @paramters:
 * msg: the message to show
 * node: the node in html dom
 * */
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

/**
 * Vaucher Module
 * 
 * show vaucher details and print content
 * */
var vaucher = {
    show: function () {
        $("#vaucherModal").modal({
            fadeDuration: 100
        });
    },
    print: function () {
        if (!isPrinted) {
             isPrinted = true;
            $("#vaucher-printable").print({
                globalStyles: true,
                mediaPrint: false,
                stylesheet: null,
                noPrintSelector: ".no-print",
                iframe: true,
                append: null,
                prepend: null,
                manuallyCopyFormValues: true,
                deferred: $.Deferred(),
                timeout: 750,
                title: null,
                doctype: '<!doctype html>'
            });
        }
    }
}