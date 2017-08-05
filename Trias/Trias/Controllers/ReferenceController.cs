using System;
using System.Linq;
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
        public ActionResult QueryList()
        {
            return View();
        }


        /// <summary>
        /// 获取文献列表，带分页
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="page">页码</param>
        /// <param name="rows">每页多少条数据</param>
        /// <param name="doiANDauthor">doi或者作者名</param>
        /// <returns></returns>
        public ActionResult GetList(string doiANDauthor, int year = 0, int page = 1, int rows = int.MaxValue)
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
            list = list.OrderByDescending(x => x.Year).Skip(rows * (page - 1)).Take(rows);
            var result = list.ToList().Select(x => new
            {
                x.R_ID,
                x.ReferenceType,
                x.FirstAuthor,
                x.SecondAuthor,
                x.OtherAuthors,
                x.Year,
                x.Title,
                x.BookTitle,
                x.Journal,
                x.Editor1,
                x.Editor2,
                x.Editor3,
                x.Editor4,
                x.Language,
                x.Publisher,
                x.Volume,
                x.No,
                x.PageBegin,
                x.PageEnd,
                x.DOI,
                x.URL1,
                x.URL2,
                x.Comments,
                ShowTitle = x.Title ?? x.BookTitle ?? x.Journal
            }).ToList();
            return Json(new
            {
                total,
                rows = result
            });
        }

        /// <summary>
        /// 添加文献
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add(ReferenceView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            if (!string.IsNullOrWhiteSpace(model.PageBegin) && !string.IsNullOrWhiteSpace(model.PageEnd))
            {
                try
                {
                    var start = int.Parse(model.PageBegin);
                    var end = int.Parse(model.PageEnd);
                    if (start > end)
                    {
                        return WriteError("起始页不能大于终止页！");
                    }
                }
                catch (Exception e)
                {
                    return WriteError(e.Message);
                }
            }
            if (referenceSer.Any(x => x.ReferenceType == model.ReferenceType && x.Year == model.Year && x.FirstAuthor == model.FirstAuthor && (x.Title == model.Title || x.Title == null) && (x.BookTitle == model.BookTitle || x.BookTitle
             == null) && (x.Journal == model.Journal || x.Journal == null)))
            {
                return WriteError("文献重复，请确认！");
            }
            var rModel = new Reference();
            rModel.CopyFrom(model);
            referenceSer.Add(rModel);
            rModel.R_ID = Guid.NewGuid().ToString();
            referenceSer.SaveChanges();
            return WriteSuccess(new
            {
                rModel.R_ID,
                ShowTitle = rModel.Title ?? rModel.BookTitle ?? rModel.Journal,
                msg = "操作成功！"
            });
        }

        /// <summary>
        /// 修改文献
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Edit(ReferenceView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            if (!string.IsNullOrWhiteSpace(model.PageBegin) && !string.IsNullOrWhiteSpace(model.PageEnd))
            {
                try
                {
                    var start = int.Parse(model.PageBegin);
                    var end = int.Parse(model.PageEnd);
                    if (start > end)
                    {
                        return WriteError("起始页不能大于终止页！");
                    }
                }
                catch (Exception e)
                {
                    return WriteError(e.Message);
                }
            }
            if (referenceSer.Any(x => x.R_ID != model.R_ID && x.ReferenceType == model.ReferenceType && x.Year == model.Year && x.FirstAuthor == model.FirstAuthor && x.Title == model.Title && x.BookTitle == model.BookTitle && x.Journal == model.Journal))
            {
                return WriteError("文献重复，请确认！");
            }
            referenceSer.EditWhere(x => x.R_ID == model.R_ID, model);
            referenceSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

        /// <summary>
        /// 删除文献
        /// </summary>
        /// <param name="id">文献的Id</param>
        /// <returns></returns>
        public ActionResult Remove(string id)
        {
            referenceSer.RemoveWhere(x => x.R_ID == id);
            referenceSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

    }
}
