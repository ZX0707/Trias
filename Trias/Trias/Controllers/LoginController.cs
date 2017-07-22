using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Models.ViewModel;

namespace Trias.Controllers
{
    public class LoginController : Controller
    {
        TriasEntities db = new TriasEntities();
        //
        // GET: /Login/
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserView model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    status = "error",
                    msg = ModelState.Values.FirstOrDefault(x=>x.Errors.Count() > 0).Errors.FirstOrDefault()
                });
            }

            User user = db.User.Where(x => x.UserName == model.UserName && x.UserPwd == model.UserPwd).FirstOrDefault();
            if (user == null)
            {
                return Json(new
                {
                    status = "error",
                    msg = "用户名密码错误"
                });
            }
            else
            {
                return Json(new
                {
                    status = "success",
                    msg = "登陆成功"
                });
            }
        }

    }
}
