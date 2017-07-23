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
        public ActionResult Index(string id)
        {
            try
            {
                return Json(BaiduApiHelper.GetLocationByName(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return WriteError(e.Message);
            }
        }

    }
}
