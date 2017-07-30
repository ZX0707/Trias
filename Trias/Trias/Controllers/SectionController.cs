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

        public ActionResult Index(string id)
        {
            ViewBag.DOI = id;
            return View();
        }
        //添加剖面
        public ActionResult AddSection(SectionView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            if (sectionSer.Any(x => x.SectionName == model.SectionName))
            {
                return WriteError("剖面名称" + model.SectionName + "已经存在，请确认！");
            }
            var user = UserMgr.CurrUserInfo();
            model.User_ID = user.User_ID;
            var Model = new Section();
            Model.CopyFrom(model);
            sectionSer.Add(Model);
            Model.S_ID = Guid.NewGuid().ToString();
            sectionSer.SaveChanges();
            return WriteSuccess(new
            {
                Model.S_ID,
                msg = "添加成功！"
            });
        }
        //根据用户id查询剖面信息
        public ActionResult GetSection(string sectionName, string authorizer, int page = 1, int rows = int.MaxValue)
        {
            var list = sectionSer.Where();
            if (!string.IsNullOrWhiteSpace(sectionName))
            {
                list = list.Where(x => x.SectionName.Contains(sectionName));
            }
            if (!string.IsNullOrWhiteSpace(authorizer))
            {
                list = list.Where(x => x.Authorizer.Contains(authorizer));
            }
            var total = list.Count();
            list = list.OrderByDescending(x => x.EnterTime).Skip((page - 1) * rows).Take(rows);
            return Json(new
            {
                total,
                rows = list.ToList()
            });
        }
        public ActionResult EditSection(SectionView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            if (sectionSer.Any(x => x.SectionName == model.SectionName && x.S_ID != model.S_ID))
            {
                return WriteError("剖面名称" + model.SectionName + "已经存在，请确认！");
            }
            sectionSer.EditWhere(x => x.S_ID == model.S_ID, model);
            sectionSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        public ActionResult RemoveSection(string id)
        {
            if (id == null)
            {
                return WriteError("次文献不存在");
            }
            sectionSer.RemoveWhere(x => x.S_ID == id);
            sectionSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }
}
