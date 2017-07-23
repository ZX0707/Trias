using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Tool;

namespace Trias.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexWithLogin()
        {
            var user = UserMgr.CurrUserInfo();
            ViewBag.UserName = user.UserName;
            return View();
        }
    }
}
