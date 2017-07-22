using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class LoginController : BaseController
    {
        [HttpPost]
        public ActionResult Login(UserView model)
        {
            var user = userSer.FirstOrDefault(x => x.UserName == model.UserName && x.UserPwd == model.UserPwd);
            if (user == null)
            {
                return WriteError("用户名或密码错误");
            }
            else
            {
                Session[Keys.Login_UserInfo] = user;
                return WriteSuccess("登录成功");
            }
        }

    }
}
