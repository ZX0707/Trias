using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var user = userSer.FirstOrDefault(x => x.UserName == model.UserName && x.UserPwd == model.UserPwd && x.isPass != false);
            if (user == null)
            {
                return WriteError("用户名或密码错误");
            }
            Session[Keys.Login_UserInfo] = user;
            return WriteSuccess("登录成功");
        }
        public ActionResult SignOut()
        {
            Session[Keys.Login_UserInfo] = null;//清除Session
            return Content("/");
        }
        public ActionResult GetList()
        {
            var list = userSer.Where();
            return Json(new
            {
                total = list.Count()
            });
        }
        public ActionResult GetCurrentUser()
        {
            return WriteSuccess(UserMgr.CurrUserInfo());
        }
        public ActionResult Regist(User user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                return WriteError("用户名不能为空！");
            }
            if (string.IsNullOrWhiteSpace(user.UserPwd))
            {
                return WriteError("密码不能为空！");
            }
            if (user.UserPwd.Length < 6)
            {
                return WriteError("密码");
            }
            if (user.UserPwd.Length > 14)
            {
                return WriteError("密码最大长度为14！");
            }
            if (string.IsNullOrWhiteSpace(user.UserUnit))
            {
                return WriteError("单位不能为空！");
            }
            if (string.IsNullOrWhiteSpace(user.UserEmail))
            {
                return WriteError("邮箱不能为空！");
            }
            var r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
            if (!r.IsMatch(user.UserEmail))
            {
                return WriteError("邮箱不合法！");
            }
            if (userSer.Any(x => x.UserName == user.UserName))
            {
                return WriteError("该用户名已经存在！");
            }

            user.User_ID = Guid.NewGuid().ToString();
            user.isPass = false;
            userSer.Add(user);

            try
            {
                userSer.SaveChanges();
                return WriteSuccess("操作成功！请耐心等待管理员验证！");
            }
            catch (Exception e)
            {
                return WriteError(e.Message);
            }
        }
        public ActionResult PassRegeist(string id)
        {
            var model = userSer.Find(id);
            if (model == null)
            {
                return WriteError("未找到该用户！");
            }
            model.isPass = true;
            userSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }
    }
}
