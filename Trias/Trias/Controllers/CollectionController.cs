﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class CollectionController : BaseController
    {
        //
        // GET: /Collection/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Details(string id)
        {
            var model = collectionSer.Find(id);
            var viewModel = new CollectionView();
            viewModel.CopyFrom(model);
            return View(viewModel);
        }
        public ActionResult Add(string id)
        {
            ViewBag.F_ID = unitSer.FirstOrDefault(x => x.U_ID == id).F_ID;
            ViewBag.U_ID = id;
            return View();
        }
        [HttpPost]
        public ActionResult Add(string collection, string rocks)
        {
            var collectionModel = JsonConvert.DeserializeObject<Collection>(collection);
            var sort = collectionSer.Where().Select(x => x.sort).OrderByDescending(x => x).FirstOrDefault() ?? 0;
            sort++;
            collectionModel.sort = sort;
            #region 是体验证

            if (collectionModel.Depth1 == null || collectionModel.Depth2 == null)
            {
                return WriteError("距离底部位置必填！");
            }

            #endregion
            collectionModel.C_ID = Guid.NewGuid().ToString();
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
                x.Type_ID = collectionModel.C_ID;
            });
            collectionSer.Add(collectionModel);
            rockSer.AddList(rockList);
            collectionSer.SaveChanges();
            rockSer.SaveChanges();
            return WriteSuccess("添加成功！");

        }
        public ActionResult GetList()
        {
            var list = collectionSer.Where();
            return Json(new{
                total=list.Count()
            });
        }


        //根据 层id 查询采样位置
        public ActionResult GetCollection(string id)
        {
            if (id == null)
            {
                return WriteError("采样位置不存在");
            }
            var collectionlist = collectionSer.Where(x => x.U_ID.Contains(id)).ToList();
            if (collectionlist.Count == 0)
            {
                return WriteSuccess("暂无采样");
            }
            return Json(collectionlist);
        }
        //修改采样位置
        public ActionResult EditCollection(string id)
        {
            var model = new CollectionView();
            var m = collectionSer.FirstOrDefault(x => x.C_ID == id);
            model.CopyFrom(m);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditCollection(string collection,string rocks)
        {
            var collectionmodel = JsonConvert.DeserializeObject<Collection>(collection);
            #region 
            if(collectionmodel.Depth1==null)
            {
                return WriteError("起始深度不能为空");
            }
            if (collectionmodel.Depth2 == null)
            {
                return WriteError("终止深度不能为空");
            }
            #endregion
            collectionSer.EditWhere(x => x.C_ID == collectionmodel.C_ID, collectionmodel);
            var rocklist = JsonConvert.DeserializeObject<List<Rock>>(rocks);
            #region 
            if(!rocklist.Any())
            {
                return WriteError("必填项不能为空！");
            }
            for(var i=0;i<rocklist.Count;i++)
            {
                var item = rocklist.ElementAt(i);
                if(string.IsNullOrWhiteSpace(item.Color1))
                {
                    return WriteError("颜色一不能为空！");
                }
                if(string.IsNullOrWhiteSpace(item.Lithology1))
                {
                    return WriteError("岩性一不能为空！");
                }
            }
            #endregion
            rockSer.RemoveWhere(x => x.Type_ID == collectionmodel.C_ID);
            rocklist.ForEach(x =>
            {
                x.Rock_ID = Guid.NewGuid().ToString();
                x.Type_ID = collectionmodel.C_ID;
            });
            rockSer.AddList(rocklist);
            rockSer.SaveChanges();
            collectionSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        //删除采样位置
        public ActionResult RemoveCollection(string id)
        {
            collectionSer.RemoveWhere(x => x.C_ID == id);
            fossilSer.RemoveWhere(x => x.C_ID == id);
            geochemicalSer.RemoveWhere(x => x.C_ID == id);
            collectionSer.SaveChanges();
            fossilSer.SaveChanges();
            geochemicalSer.SaveChanges();
            return WriteSuccess("删除成功");
        }

    }
}
