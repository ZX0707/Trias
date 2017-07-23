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
        }
    }, "json");
}



//function initMenu(){
//	$("#leftAnimite").click(function()
//	{
//		if ($(".right").css("left")=="300px") {
//			 layerToLeft();

//		}
//		else
//		{
//        	layerToRight();
//		}

//	})
//}
//	// 隐藏左侧
//	function layerToLeft(){
//	$(".left").animate({ "left": "-300px" }, "fast");//系统左侧
//    $(".right").animate({ "left": "0px" }, "fast");//二维地图容器
//    $(".Animite").css({ "background-image": "url('/Content/images/right.png')" });//右侧隐藏图标换成向左显示图标

//	}
//	// 显示左侧
//	function layerToRight(){
//	$(".right").animate({ "left": "300px" }, "fast");//系统左侧
//    $(".left").animate({ "left": "0px" }, "fast");//二维地图容器
//    $(".Animite").css({ "background-image": "url('/Content/images/left.png')" });//右侧隐藏图标换成向左显示图标

//	}