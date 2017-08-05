using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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

        public ActionResult UpLoadRefecence()
        {
            #region 获取上传的文件 file

            if (Request.Files.Count == 0) return WriteError("文件上传失败!");
            var file = Request.Files[0];
            if (file == null || file.ContentLength <= 0) return WriteError("文件上传失败!");

            #endregion

            IWorkbook workbook = null;
            var ex = System.IO.Path.GetExtension(file.FileName);
            if (ex.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                workbook = new XSSFWorkbook(file.InputStream);
            }
            else if (ex.Equals(".xls", StringComparison.OrdinalIgnoreCase))
            {
                workbook = new HSSFWorkbook(file.InputStream);
            }
            else
            {
                return WriteError("文件格式错误，仅支持xlsx和xls格式的文件！");
            }

            if (workbook == null)
            {
                return WriteError("文件读取失败！");
            }
            var sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                return WriteError("文件结构错误！");
            }
            var rowCount = sheet.LastRowNum;
            if (rowCount <= 0)
            {
                return WriteError("文件内数据为空！");
            }
            var firstRow = sheet.GetRow(0);
            var cellCount = firstRow.LastCellNum;
            var cellNameArray = new[]
            {
                "文献类型", "第一作者", "第二作者", "其他作者", "年度", "文献名", "书名", "杂志名", "编辑1", "编辑2", "编辑3", "编辑4", "语言", "出版社", "卷",
                "期", "起始页", "终止页", "DOI", "链接1", "链接2", "评论"
            };
            if (cellCount < cellNameArray.Length)
            {
                return WriteError("上传的文件列数缺少，请确认！");
            }
            for (var i = 0; i < cellCount; i++)
            {
                var cell = firstRow.GetCell(i);
                if (!cell.ToString().Equals(cellNameArray[i]))
                {
                    return WriteError(string.Format(@"文件的第{0}列必须为{1}", i + 1, cellNameArray[i]));
                }
            }

            var referenceList = new List<Reference>();
            for (var rowIndex = 1; rowIndex < rowCount; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                var referenceModel = new Reference();
                referenceModel.R_ID = Guid.NewGuid().ToString();
                referenceModel.ReferenceType = row.GetCell(0).ToString();
                referenceModel.FirstAuthor = row.GetCell(1).ToString();
                referenceModel.SecondAuthor = row.GetCell(2).ToString();
                referenceModel.OtherAuthors = row.GetCell(3).ToString();
                referenceModel.Year = int.Parse(row.GetCell(4).ToString());
                referenceModel.Title = row.GetCell(5).ToString();
                referenceModel.BookTitle = row.GetCell(6).ToString();
                referenceModel.Journal = row.GetCell(7).ToString();
                referenceModel.Editor1 = row.GetCell(8).ToString();
                referenceModel.Editor2 = row.GetCell(9).ToString();
                referenceModel.Editor3 = row.GetCell(10).ToString();
                referenceModel.Editor4 = row.GetCell(11).ToString();
                referenceModel.Language = row.GetCell(12).ToString();
                referenceModel.Publisher = row.GetCell(13).ToString();
                referenceModel.Volume = row.GetCell(14).ToString();
                referenceModel.No = row.GetCell(15).ToString();
                referenceModel.PageBegin = row.GetCell(16).ToString();
                referenceModel.PageEnd = row.GetCell(17).ToString();
                referenceModel.DOI = row.GetCell(18).ToString();
                referenceModel.URL1 = row.GetCell(19).ToString();
                referenceModel.URL2 = row.GetCell(20).ToString();
                referenceModel.Comments = row.GetCell(21).ToString();
                referenceList.Add(referenceModel);
            }
            referenceSer.AddList(referenceList);
            referenceSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

    }
}
