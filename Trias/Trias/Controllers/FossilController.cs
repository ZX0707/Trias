using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class FossilController : BaseController
    {
        //
        // GET: /Fossil/

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
        public ActionResult Add(FossilView model, string fossil)
        {
            var filePath = Server.MapPath("~/UpLoad/Picture");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fossilModel = JsonConvert.DeserializeObject<Fossil>(fossil);
            fossilModel.Picture = "";
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file == null) continue;
                var fileName = MD5Helper.GetStreamMd5(file.InputStream);
                fileName += Path.GetExtension(file.FileName);
                fileName = Path.Combine(filePath, fileName);
                file.SaveAs(fileName);
                fossilModel.Picture += fileName + ";";
            }
            var sort = fossilSer.Where().Select(x => x.sort).OrderByDescending(x => x).FirstOrDefault() ?? 0;
            sort++;
            fossilModel.sort = sort;
            fossilModel.H_ID = Guid.NewGuid().ToString();
            fossilSer.Add(fossilModel);
            fossilSer.SaveChanges();
            return WriteSuccess("添加成功！");

        }

        //添加化石信息
        public ActionResult AddFossil(FossilView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            Fossil fossilmodel = new Fossil();
            fossilmodel.CopyFrom(model);
            fossilSer.Add(fossilmodel);
            fossilmodel.H_ID = Guid.NewGuid().ToString();
            fossilSer.SaveChanges();
            return WriteSuccess("添加成功");
        }
        //根据 采样位置id 查询化石信息
        public ActionResult GetFossil(string id)
        {
            if (id == null)
            {
                return WriteError("采样位置不存在");
            }
            var fossillist = fossilSer.Where(x => x.H_ID.Contains(id)).ToList();
            if (fossillist.Count == 0)
            {
                return WriteSuccess("暂无化石信息");
            }
            return Json(fossillist);
        }
        //修改化石信息
        public ActionResult EditFossil(string id)
        {
            var model = new FossilView();
            var m = fossilSer.FirstOrDefault(x => x.H_ID == id);
            model.CopyFrom(m);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditFossil(string fossil, string id)
        {
            var fossilmodel = JsonConvert.DeserializeObject<Fossil>(fossil);
            #region 
            if (string.IsNullOrWhiteSpace(fossilmodel.GenusName))
            {
                WriteError("属名不能为空");
            }
            if (string.IsNullOrWhiteSpace(fossilmodel.SpeciesName))
            {
                WriteError("种名不能为空");
            }
            #endregion
            fossilSer.EditWhere(x => x.H_ID == fossilmodel.H_ID, fossilmodel);
            fossilSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        //删除化石信息
        public ActionResult RemoveFossil(string id)
        {
            fossilSer.RemoveWhere(x => x.H_ID == id);
            fossilSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }
}
