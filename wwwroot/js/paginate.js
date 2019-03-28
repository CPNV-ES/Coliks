var paginate = function () {
    var that = {};
    var item = {};
    var PageNo = 1;


    // All the initial events will be placed here.  
    var InitEvents = function () {
        // every button has the common class as filter-page and common attribute data-page  
        // data-page is nothing but the page number on click if which the data will be filtered  
        $(document).on('click', '.filter-page', function () {
            //setting the page no.  
            PageNo = $(this).data('page');
            Pagination();
        });

        // every filters has the common class as filter-text  
        // on keyup or change of the filters, we will call the ajax function  
        $(document).on('keyup change', '.filter-text', function () {
            
            //setting the page no to 1 thus on any filter change matching data will be shown from page 1  
            PageNo = 1;
            Pagination();
        })
    }

    // Common ajax call function to bind the filter data and page number and pass to the Action result of our controller via ajax call  
    var Pagination = function () {

        // All filters controls
        item.itemnb = $('#item_nb').val();
        item.brand = $('#item_brand').val();
        item.model = $('#item_model').val();
        item.size = $('#item_size').val();
        item.stock = $('#item_stock').val();
        item.category = $('#item_gender').val();
        item.search = $('#search').val();

        $.ajax({
            type: "POST",
            url: "/Items/PaginateData",
            data: { pageNo: PageNo, filter: item },
            content: "application/json; charset=utf-8",
            dataType: "html", //here we set datatype as html becausing we are returning partial view  
            success: function (d) {
                // d will contain the html of partial view  
                var result = $.parseHTML(d) // Parse string returned
                $('#tableBody').replaceWith(result[2].childNodes[1].childNodes[3]) // Get the tbody from result and display it
                $('.panel-footer').replaceWith(result[4]) // Get the footer and display it
            },
            error: function (xhr, textStatus, errorThrown) {

            }
        });
    }

    that.init = function () {
        // to load the initial events  
        InitEvents();
    }

    return that;
}();
