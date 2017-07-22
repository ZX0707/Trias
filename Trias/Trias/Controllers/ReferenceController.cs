using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Unit;

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
        public ActionResult GetList(int page = 1, int rows = int.MaxValue)
        {
            var list = referenceSer.Where();
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
            var Model = new Reference();
            Model.CopyFrom(model);
            referenceSer.Add(Model);
            referenceSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

        public ActionResult Edit(ReferenceView model)
        {
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
