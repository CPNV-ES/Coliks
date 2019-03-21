$(document).ready(function () {
    $('#tmpSearchBar').keyup(function (e) {
        $('#searchBar').val($('#tmpSearchBar').val())
    })

    $('#btnSearch').click(function () {
        $('#formSearch').submit()
    })
})