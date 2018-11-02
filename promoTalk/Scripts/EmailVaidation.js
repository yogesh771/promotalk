
var emailvalidation = function (email) {
    var mailformat = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (!email.match(mailformat)) {
        return false;
    }
    else { return true;}
}