﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
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

        public ActionResult Add(string id)
        {
            ViewBag.F_ID = id;
            return View();
        }

        [HttpPost]
        public ActionResult Add(string unit, string rocks)
        {
            var unitModel = JsonConvert.DeserializeObject<Unit>(unit);
            unitModel.U_ID = Guid.NewGuid().ToString();
            var rockList = JsonConvert.DeserializeObject<List<Rock>>(rocks);
            rockList.ForEach(x =>
            {
                x.Rock_ID = Guid.NewGuid().ToString();
                x.Type_ID = unitModel.U_ID;
            });
            unitSer.Add(unitModel);
            rockSer.AddList(rockList);
            unitSer.SaveChanges();
            rockSer.SaveChanges();
            return WriteSuccess("添加成功！");

        }

        //添加单元信息
        public ActionResult AddUnit(UnitView model)
        {
            if (!ModelState.IsValid)
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
            if (id == null)
            {
                WriteError("该岩石组不存在");
            }
            var unitlist = unitSer.Where(x => x.F_ID.Contains(id)).ToList();
            return Json(unitlist);
        }
        //删除单元信息
        public ActionResult RemoveUnit(string id)
        {
            unitSer.RemoveWhere(x => x.U_ID == id);
            var collectionList = collectionSer.Where(x => x.U_ID == id).ToList();
            collectionList.ForEach(x =>
            {
                collectionSer.Remove(x);
                collectionSer.RemoveWhere(y => y.C_ID == x.C_ID);
                fossilSer.RemoveWhere(y => y.C_ID == x.C_ID);
                geochemicalSer.RemoveWhere(y => y.C_ID == x.C_ID);
            });
            unitSer.SaveChanges();
            collectionSer.SaveChanges();
            fossilSer.SaveChanges();
            geochemicalSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }
}
