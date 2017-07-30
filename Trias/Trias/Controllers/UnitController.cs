using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class UnitController : BaseController
    {
        //
        // GET: /Unit/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        //添加单元信息
        public ActionResult AddUnit(UnitView model)
        {
            if(!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            Unit modelUnit = new Unit();
            model.CopyFrom(model);
            unitSer.Add(modelUnit);
            modelUnit.U_ID = Guid.NewGuid().ToString();
            unitSer.SaveChanges();
            return WriteSuccess("添加成功");
        }
        //修改单元信息
        public ActionResult EditUnit(UnitView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            unitSer.EditWhere(x => x.U_ID == model.U_ID, model);
            unitSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        //根据 岩石组id 查询单元信息
        public ActionResult GetUnit(string id)
        {
            if(id==null)
            {
                WriteError("该岩石组不存在");
            }
            var unitlist = unitSer.Where(x => x.F_ID.Contains(id)).ToList();
            return Json(unitlist);
        }
        //删除单元信息
        public ActionResult RemoveUnit(string id)
        {
            if(id==null)
            {
                WriteError("该单元信息不存在");
            }
            unitSer.RemoveWhere(x=>x.U_ID==id);
            unitSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }
}
