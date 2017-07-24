using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Tool;

namespace Trias.Controllers
{
    public class BaiduController : BaseController
    {
        public ActionResult Index(string city, string place)
        {
            return Content(BaiduApiHelper.GetLocationsByName(city, place));
        }

    }
}
