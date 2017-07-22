using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Service;

namespace Trias.Controllers
{
    public class BaseController : Controller
    {
        public DictService dictSer = new DictService();
        public FossilService fossilSer = new FossilService();
        public GeochemicalService geochemicalSer = new GeochemicalService();
        public ReferenceService ReferenceSer = new ReferenceService();
        public SectionService SectionSer = new SectionService();
        public UserService userSer = new UserService();

        public ActionResult WriteError(object obj)
        {
            return Json(new
            {
                status = "error",
                msg = obj
            });
        }

        public ActionResult WriteSuccess(object obj)
        {
            return Json(new
            {
                status = "success",
                msg = obj
            });
        }
    }
}
