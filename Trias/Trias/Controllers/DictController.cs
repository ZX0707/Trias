using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Unit;

namespace Trias.Controllers
{
    /// <summary>
    /// 字典管理控制器
    /// </summary>
    public class DictController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            return Json(dictSer.Where().ToList());
        }

        public ActionResult Save(DictView model)
        {
            var Model = new Dict();
            Model.CopyFrom(model);
            dictSer.Add(Model);
            Model.Id = Guid.NewGuid().ToString();
            dictSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

        public ActionResult Edit(DictView model)
        {
            dictSer.EditWhere(x => x.Id == model.Id, model);
            dictSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

        public ActionResult Remove(string id)
        {
            dictSer.RemoveWhere(x => x.Id == id);
            dictSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

    }
}
