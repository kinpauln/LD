
function updateLotteryResultState(id, state, message) {
    var loadi;
    $.ajax({
        type: "post",
        data: { id: id, state: state },
        url: "/Vip/UpdateLotteryResult",
        beforeSend: function () {
            loadi = layer.load(message);
        },
        dataType: "json",
        success: function (data) {
            var result = data.OK;
            var message = data.Message;
            if (!result) {
                layer.alert(message, 8);
                return;
            }
            layer.alert(message, 9, function () {
                location.href = location.href;
            });
        },
        error: function () {

        },
        complete: function () {
            layer.close(loadi);
        }
    });
}