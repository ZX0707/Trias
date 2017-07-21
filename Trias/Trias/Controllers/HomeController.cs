using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;

namespace Trias.Controllers
{
    public class HomeController : Controller
    {
        TriasEntities db = new TriasEntities();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
