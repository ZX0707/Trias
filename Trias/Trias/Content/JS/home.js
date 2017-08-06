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
        if (result.status === "error") {
            $("#remain").html(result.msg);
        } else {
            location.href = "/Home/IndexWithLogin";
        }
    }, "json");
}

// 初始化下拉框
// obj下拉框的Jquery对象
// url请求的url地址
// prarm请求附带的参数
// callback回调函数
function initSelect(obj, url, param, callback) {
    $.post(url, param, function (result) {
        obj.html("");
        for (var i = 0; i < result.length; i++) {
            var value = result[i].KeyName;
            var text = result[i].ValueName;
            var id = result[i].Id;
            obj.append("<option id='" + id + "' value='" + value + "'>" + text + "</option>");
        }
        if (typeof callback == "function") {
            callback();
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
    },

    //2.1打开一个模态框
    OpenDialog: function (title, url, width, height) {
        if (width == undefined) {
            width = $(window).width() * 0.8;
        }
        if (height == undefined) {
            height = $(window).height() * 0.8;
        }
        var dialog = $("<div id='dialog_" + title + "'/>");
        //var dialog = $("<div id='dialog_" + title + "' style='overflow:scroll;'/>");
        $(window).find("body").append(dialog);
        dialog.dialog({
            title: title,
            width: width,
            height: height,
            modal: true,
            content: '<iframe scrolling="yes" frameborder="0"  style="width:100%;height:99%;" src="' + url + '"/>'
        });
    },
    //2.1关闭一个模态框
    CloseDialog: function (title) {
        window.parent.$("#dialog_" + title).dialog("close");
    }
}

