using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class SectionController : BaseController
    {
        //
        // GET: /Section/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        //添加剖面
        public ActionResult AddSection(SectionView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            var Model = new Section();
            Model.CopyFrom(model);
            sectionSer.Add(Model);
            Model.S_ID = Guid.NewGuid().ToString();
            sectionSer.SaveChanges();
            return WriteSuccess("添加成功");
        }
        //根据用户
        public ActionResult GetSection(string username)
        {
            if(username==null)
            {
                return WriteError("用户名为空");
            }
            var sectionlist = sectionSer.Where(x => x.Authorizer.Contains(username)).ToList();
            if(sectionlist.Count==0)
            {
                return WriteSuccess("查询成功，未添加剖面");
            }
            return Json(sectionlist);
        }
        public ActionResult EditSection(SectionView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            sectionSer.EditWhere(x => x.S_ID == model.S_ID, model);
            sectionSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        public ActionResult RemoveSection(string id)
        {
            if(id==null)
            {
                return WriteError("次文献不存在");
            }
            sectionSer.RemoveWhere(x => x.S_ID == id);
            sectionSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }
}
