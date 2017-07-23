using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class FormationController : BaseController
    {
        //
        // GET: /Formation/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        //添加岩石组信息
        public ActionResult AddFormation(FormationView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            Formation modelformation = new Formation();
            modelformation.CopyFrom(model);
            formationSer.Add(modelformation);
            modelformation.F_ID = new Guid().ToString();
            formationSer.SaveChanges();
            return WriteSuccess("添加成功");
        }
        //修改岩石组信息
        public ActionResult EditFormation(FormationView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            formationSer.EditWhere(x => x.F_ID == model.F_ID, model);
            formationSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        //根据 剖面id 查询岩石组信息
        public ActionResult GetFormation(string id)
        {
            if(id==null)
            {
                return WriteError("该剖面不存在");
            }
            var formationlist = formationSer.Where(x => x.S_ID.Contains(id)).ToList();
            if(formationlist.Count==0)
            {
                return WriteSuccess("暂无岩石组信息");
            }
            return Json(formationlist);
        }
        //删除岩石组信息
        public ActionResult RemoveFormation(string id)
        {
            if(id==null)
            {
                return WriteError("岩石组不存在")
            }
            formationSer.RemoveWhere(x=>x.F_ID==id);
            formationSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }
}
