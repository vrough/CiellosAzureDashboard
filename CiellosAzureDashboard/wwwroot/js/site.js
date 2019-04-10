// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function copyToClipboard(url) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(location.origin + "/AnonymousDashboard?accessCode=" + url).select();
    document.execCommand("copy");
    $temp.remove();
}
