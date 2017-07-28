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
        public ActionResult Login(UserView model)
        {
            var user = userSer.FirstOrDefault(x => x.UserName == model.UserName && x.UserPwd == model.UserPwd);
            if (user == null)
            {
                return WriteError("用户名或密码错误");
            }
            Session[Keys.Login_UserInfo] = user;
            return WriteSuccess("登录成功");
        }

        public ActionResult SignOut()
        {
            Session[Keys.Login_UserInfo] = null;
            return Content("/");
        }

        public ActionResult GetCurrentUser()
        {
            return WriteSuccess(UserMgr.CurrUserInfo());
        }
    }
}
