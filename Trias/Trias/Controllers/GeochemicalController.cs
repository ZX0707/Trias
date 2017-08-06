using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
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
        public ActionResult Add(string id)
        {
            ViewBag.C_ID = id;
            return View();
        }
        [HttpPost]
        public ActionResult Add(string geochemical, string nothing)
        {
            var geochemicalModel = JsonConvert.DeserializeObject<Geochemical>(geochemical);
            var sort = geochemicalSer.Where().Select(x => x.sort).OrderByDescending(x => x).FirstOrDefault() ?? 0;
            sort++;
            geochemicalModel.sort = sort;
            geochemicalModel.G_ID = Guid.NewGuid().ToString();
            geochemicalSer.Add(geochemicalModel);
            geochemicalSer.SaveChanges();
            return WriteSuccess("添加成功！");

        }
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
        public ActionResult EditGeochemical(string id)
        {
            var model = new GeochemicalView();
            var m = geochemicalSer.FirstOrDefault(x => x.G_ID == id);
            model.CopyFrom(m);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditGeochemical(string geochemical,string id)
        {
            var geochemicalmodel = JsonConvert.DeserializeObject<Geochemical>(geochemical);
            #region 
            if(geochemicalmodel.Position==null)
            {
                return WriteError("距离底部位置不能为空");
            }
            #endregion
            geochemicalSer.EditWhere(x => x.G_ID == geochemicalmodel.G_ID, geochemicalmodel);
            geochemicalSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        //删除地球化学信息
        public ActionResult RemoveGeochemical(string id)
        {
            geochemicalSer.RemoveWhere(x => x.G_ID == id);
            geochemicalSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }
}
