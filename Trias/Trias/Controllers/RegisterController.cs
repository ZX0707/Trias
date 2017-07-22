﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class RegisterController : BaseController
    {
        //
        // GET: /Register/
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserView model2)
        {
            var Model = new User();
            Model.CopyFrom(model2);
            userSer.Add(Model);
            Model.User_ID = new Guid().ToString();
            userSer.SaveChanges();
            return WriteSuccess("添加成功");
        }

    }
}
