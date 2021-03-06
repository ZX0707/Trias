﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
        public ActionResult Details(string id)
        {
            var model = unitSer.Find(id);
            var viewModel = new UnitView();
            viewModel.CopyFrom(model);
            return View(viewModel);
        }
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
            var sort = unitSer.Where().Select(x => x.sort).OrderByDescending(x => x).FirstOrDefault() ?? 0;
            sort++;
            unitModel.sort = sort;
            #region 实体验证

            if (unitModel.Thickness == null)
            {
                return WriteError("厚度必填！");
            }

            #endregion
            unitModel.U_ID = Guid.NewGuid().ToString();
            var rockList = JsonConvert.DeserializeObject<List<Rock>>(rocks);
            #region 实体验证

            if (!rockList.Any())
            {
                return WriteError("必填项不能为空！");
            }
            for (var i = 0; i < rockList.Count; ++i)
            {
                var item = rockList.ElementAt(i);
                if (string.IsNullOrWhiteSpace(item.Color1))
                {
                    return WriteError("颜色一必填！");
                }
                if (string.IsNullOrWhiteSpace(item.Lithology1))
                {
                    return WriteError("岩性一必填！");
                }
            }

            #endregion

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

        //添加层信息
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
        //修改层信息
        public ActionResult EditUnit(string id)
        {
            var model = new UnitView();
            var m = unitSer.FirstOrDefault(x => x.U_ID == id);
            model.CopyFrom(m);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditUnit(string unit, string rocks)
        {
            var unitmodel = JsonConvert.DeserializeObject<Unit>(unit);//接受字符串转换成对象
            #region 
            if(unitmodel.Thickness==null)
            {
                return WriteError("层厚度必填");
            }
            #endregion
            unitSer.EditWhere(x => x.U_ID == unitmodel.U_ID,unitmodel);
            var rockList = JsonConvert.DeserializeObject<List<Rock>>(rocks);
            #region 
            if (!rockList.Any())
            {
                return WriteError("必填项不能为空！");
            }
            for (var i = 0; i < rockList.Count; ++i)
            {
                var item = rockList.ElementAt(i);
                if (string.IsNullOrWhiteSpace(item.Color1))
                {
                    return WriteError("颜色一必填！");
                }
                if (string.IsNullOrWhiteSpace(item.Lithology1))
                {
                    return WriteError("岩性一必填！");
                }
            }
            #endregion
            rockSer.RemoveWhere(x => x.Type_ID == unitmodel.U_ID);

            rockList.ForEach(x =>
            {
                x.Rock_ID = Guid.NewGuid().ToString();
                x.Type_ID = unitmodel.U_ID;
            });
            rockSer.AddList(rockList);
            rockSer.SaveChanges();
            formationSer.SaveChanges();
            return WriteSuccess("修改成功！");
        }
        //根据 岩石组id 查询层信息
        public ActionResult GetUnit(string id)
        {
            if (id == null)
            {
                WriteError("该岩石组不存在");
            }
            var unitlist = unitSer.Where(x => x.F_ID.Contains(id)).ToList();
            return Json(unitlist);
        }
        //删除层信息
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
