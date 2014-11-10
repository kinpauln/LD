
//function showLoading(isShow) {
//    if (isShow) {
//        $("#loadingOverlay")
//            .width($(document).width())
//            .height($(document).height())
//            .show();

//        $("#loading")
//            .css({
//                "left": $(window).width() / 2.0 - $("#loading").width() / 2.0,
//                "top": $(window).height() / 2.0 - $("#loading").height() / 2.0
//            })
//            .show();
//    }
//    else {
//        $("#loadingOverlay").hide();
//        $("#loading").hide();
//    }
//}
function showLoading(show) {
    if (show) {
        $("#loading span").css({
            //top: ($("#loading").height() / 2 - $("#loading span").height() / 2) + "px",
            top: (document.documentElement.clientHeight / 2 - $("#loading span").height() / 2) + "px",
            //top: ($(document).height() / 2 - $("#loading span").height() / 2) + "px",
            left: ($("#loading").width() / 2 - $("#loading span").width() / 2) + "px"
        });

        $("#loading").fadeIn();
    }
    else {
        $("#loading").fadeOut();
    }
}
window.onresize = function () {
    if ($("#loading").is(":visible")) {
        $("#loading span").css({
            top: (document.documentElement.clientHeight / 2 - $("#loading span").height() / 2) + "px",
            left: ($("#loading").width() / 2 - $("#loading span").width() / 2) + "px"
        });
    }
}

$(document).ready(function () {

    //$.ajaxSetup({
    //    /*beforeSend: function(xhr) {
    //    showLoading(true);
    //    },
    //    complete: function(xhr, textStatus) {
    //    showLoading(false);
    //    },*/
    //    error: function (xhr, textStatus, errorThrown) {
    //        showError(xhr.status + ": " + xhr.statusText, "", true);
    //    }
    //});
});
