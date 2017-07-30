$(function () {
    //initMenu();
    $("#UserName").change(function () {
        $("#remain").html();
    }); $("#Password").change(function () {
        $("#remain").html();
    });
});
function login() {
    var user = {
        UserName: $("#UserName").val(),
        UserPwd: $("#Password").val()
        //UserName,UserPwd传到后台的值，保持一致
    };
    $.post("/Login/Login", user, function (result) {
        if (result.status == "error") {
            $("#remain").html(result.msg);
        } else {
            location.href = "/Home/IndexWithLogin";
        }
    }, "json");
}

var commHelper = {
    //1.0格式：yyyy-MM-dd HH:mm:ss将json格式的日期转换为正确格式：/Date(1330866873400)/
    ChangeDateFormatMS: function (cellval) {
        var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minute = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        return date.getFullYear() + "-" + month + "-" + currentDate + " " + hours + ":" + minute;
    },
    //1.1格式：yyyy-MM-dd将json格式的日期转换为正确格式：/Date(1330866873400)/
    ChangeDateFormatYMD: function (cellval) {
        var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }
}

