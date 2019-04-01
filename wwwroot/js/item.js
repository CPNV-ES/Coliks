function deleteConfirm() {
    if (confirm("Voulez vous vraiment supprimer cet article ?")) {
        $('#deleteItem').submit() 
    }
}


// Have to add an extra validation on the button for the form-inline.valid cause of unique rule on Itemnb
function sendFormCreate() {
    if ($('#form').valid()) {
        $('#form').submit()
    }
}

function cancelIt(evt) {
    var e = (typeof evt != 'undefined') ? evt : event;
    e.cancelBubble = true;
}



$(document).ready(function () {
    $(document).on('keyup keydown change', '#form', function () {
        if ($('#form').valid()) { // The itemnb is not valid and the $('.form-inline').valid() return false (if we do it on the console). But enter the true part of the condition anyway.
            $('.btn-success').removeAttr('disabled')
        }
        else {
            $('.btn-success').attr('disabled', true)
        }
    });

    $('#save').click(function () { 
        if ($('#form').valid()) {
            $('#form').submit()
        }
    })
})