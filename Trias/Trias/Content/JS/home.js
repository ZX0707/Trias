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


