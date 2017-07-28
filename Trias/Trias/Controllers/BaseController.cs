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
        public ReferenceService referenceSer = new ReferenceService();
        public SectionService sectionSer = new SectionService();
        public UserService userSer = new UserService();
        public CollectionService collectionSer = new CollectionService();
        public RockService rockSer = new RockService();
        public UnitService unitSer = new UnitService();
        public FormationService formationSer = new FormationService();

        public ActionResult WriteStatusError(ModelStateDictionary modelState)
        {
            return WriteError(modelState.Values.Where(x => x.Errors.Any()).FirstOrDefault().Errors.FirstOrDefault().ErrorMessage);
        }

        public ActionResult WriteError(object obj)
        {
            return Json(new
            {
                status = "error",
                msg = obj
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WriteSuccess(object obj)
        {
            return Json(new
            {
                status = "success",
                msg = obj
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
