document.addEventListener("DOMContentLoaded", function () {
    document.getElementById('searchBar').onkeyup = function () {
        document.getElementById('formSearch').submit()
    }
});