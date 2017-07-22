using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Models.ViewModel;

namespace Trias.Controllers
{
    public class RegisterController : Controller
    {
        TriasEntities db = new TriasEntities();
        //
        // GET: /Register/

        public ActionResult Register(UserView model2)
        {
            db.User.Add(new User
            {
                User_ID=Guid.NewGuid().ToString(),
                UserName = model2.UserName,
                UserPwd = model2.UserPwd,
                UserEmail = model2.UserEmail,
                UserUnit = model2.UserUnit,
                ResearchField = model2.ResearchField
            });
            db.SaveChanges();
            return View();
        }

    }
}
