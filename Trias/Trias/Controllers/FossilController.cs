﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        //添加化石信息
        public ActionResult AddFossil(FossilView model)
        {
            if(!ModelState.IsValid)
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
            if(id==null)
            {
                return WriteError("采样位置不存在");
            }
            var fossillist = fossilSer.Where(x => x.H_ID.Contains(id)).ToList();
            if(fossillist.Count==0)
            {
                return WriteSuccess("暂无化石信息");
            }
            return Json(fossillist);
        }
        //修改化石信息
        public ActionResult EditFossil(FossilView model)
        {
            if(!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            fossilSer.EditWhere(x => x.H_ID == model.H_ID, model);
            fossilSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        //删除化石信息
        public ActionResult RemoveFossil(string id)
        {
            if(id==null)
            {
                return WriteError("该化石信息不存在");
            }
            fossilSer.RemoveWhere(x => x.H_ID == id);
            fossilSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }
}