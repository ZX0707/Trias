using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class ReferenceController : BaseController
    {
        /// <summary>
        /// 主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取文献列表，带分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="rows">每页多少条数据</param>
        /// <returns></returns>
        public ActionResult GetList(string doiANDauthor,int year=0,int page = 1, int rows = int.MaxValue)
        {
            var list = referenceSer.Where();
            if (year != 0)
            {
                list = list.Where(x => x.Year == year);
            }
            if (!string.IsNullOrWhiteSpace(doiANDauthor))
            {
                list = list.Where(x => x.DOI == doiANDauthor || x.FirstAuthor.Contains(doiANDauthor) || x.SecondAuthor.Contains(doiANDauthor) || x.OtherAuthors.Contains(doiANDauthor));
            }
            var total = list.Count();
            list = list.Skip(rows * (page - 1)).Take(rows);
            return Json(new
            {
                total = total,
                rows = list.ToList()
            });
        }

        public ActionResult Add(ReferenceView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            if (referenceSer.Any(x => x.DOI == model.DOI))
            {
                return WriteError("已经存在文献的DOI为" + model.DOI + "，请确认！");
            }
            var Model = new Reference();
            Model.CopyFrom(model);
            referenceSer.Add(Model);
            Model.R_ID = Guid.NewGuid().ToString();
            referenceSer.SaveChanges();
            return WriteSuccess(new
            {
                R_ID = Model.R_ID,
                msg = "操作成功！"
            });
        }

        public ActionResult Edit(ReferenceView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            if (referenceSer.Any(x => x.DOI == model.DOI && x.R_ID != model.R_ID))
            {
                return WriteError("已经存在文献的DOI为" + model.DOI + "，请确认！");
            }
            referenceSer.EditWhere(x => x.R_ID == model.R_ID, model);
            referenceSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

        public ActionResult Remove(string id)
        {
            referenceSer.RemoveWhere(x => x.R_ID == id);
            referenceSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

    }
}
