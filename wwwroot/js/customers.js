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

        /* Popovers init */

        $('#client-new-btn').popover({ content: "Créer a client", delay: { show: 1000, hide: 100 }, trigger: "hover", placement: "bottom" }); 
        $('#search-values').popover({ content: "Rechercher a client", delay: { show: 1000, hide: 100 }, trigger: "hover", placement: "bottom" }); 
        $('#delete-search').popover({ content: "Supprimer la recherche", delay: { show: 1000, hide: 100 }, trigger: "hover", placement: "bottom" }); 
        $('#customer-home-actions #delete').popover({ content: "Supprimer le client", delay: { show: 1000, hide: 100 }, trigger: "hover", placement: "bottom" }); 
        $('#customer-home-actions #edit').popover({ content: " Modifier le client", delay: { show: 1000, hide: 100 }, trigger: "hover", placement: "bottom" });
        $('#search-new input').popover({ content: " Barre de recherche", delay: { show: 1000, hide: 100 }, trigger: "hover", placement: "bottom" }); 

        var v = $("#customer-search-box input").val();
        $("#customer-search-box input").val("").val(v);

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
