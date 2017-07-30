using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class RockController : BaseController
    {
        //
        // GET: /Rock/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        //添加岩石信息
        public ActionResult AddRock(RockView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            var modelRock = new Rock();
            modelRock.CopyFrom(model);
            rockSer.Add(modelRock);
            modelRock.Rock_ID = Guid.NewGuid().ToString();
            rockSer.SaveChanges();
            return WriteSuccess("添加成功");
        }
        //根据剖面或单元id查询添加的岩石信息
        public ActionResult GetRock(string id)
        {
            if (id == null)
            {
                return WriteError("该剖面不存在");
            }
            var rocklist = rockSer.Where(x => x.Type_ID.Contains(id)).ToList();
            if (rocklist.Count == 0)
            {
                return WriteSuccess("暂未添加岩石");
            }
            return Json(rocklist);
        }
        public ActionResult EditRock(RockView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            rockSer.EditWhere(x => x.Rock_ID == model.Rock_ID, model);
            rockSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        public ActionResult RemoveRock(string id)
        {
            if (id == null)
            {
                return WriteError("岩石不存在");
            }
            rockSer.RemoveWhere(x => x.Rock_ID == id);
            rockSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }
}
