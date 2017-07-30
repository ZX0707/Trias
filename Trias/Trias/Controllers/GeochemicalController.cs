using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class GeochemicalController : BaseController
    {
        //
        // GET: /Geochemical/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        //添加地球化学信息
        public ActionResult AddGeochemiacal(GeochemicalView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            Geochemical geochemicalmodel = new Geochemical();
            geochemicalmodel.CopyFrom(model);
            geochemicalSer.Add(geochemicalmodel);
            geochemicalmodel.G_ID = Guid.NewGuid().ToString();
            geochemicalSer.SaveChanges();
            return WriteSuccess("添加成功");
        }
        //根据 cid 获取地球化学信息
        public ActionResult GetGeochemical(string id)
        {
            if (id == null)
            {
                return WriteError("该剖面不存在");
            }
            var geochemicallist = geochemicalSer.Where(x => x.C_ID.Contains(id)).ToList();
            if (geochemicallist.Count == 0)
            {
                WriteSuccess("暂无地球化学信息");
            }
            return Json(geochemicallist);
        }
        //修改地球化学信息
        public ActionResult EditGeochemical(GeochemicalView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            geochemicalSer.EditWhere(x => x.G_ID == model.G_ID, model);
            geochemicalSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        //删除地球化学信息
        public ActionResult RemoveGeochemical(string id)
        {
            if (id == null)
            {
                return WriteError("地球化学信息不存在");
            }
            geochemicalSer.RemoveWhere(x => x.G_ID == id);
            geochemicalSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }
}
