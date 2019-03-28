
function cancelIt(evt) {
    var e = (typeof evt != 'undefined') ? evt : event;
    e.cancelBubble = true;
}