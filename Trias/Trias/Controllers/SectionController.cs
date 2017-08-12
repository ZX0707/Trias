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

        /// <summary>
        ///    
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="rows">每页多少条数据</param>
        /// <param name="keyWord">搜索关键字</param>
        /// <returns></returns>
        public ActionResult GetList(int page=1, int rows=int.MaxValue, string keyWord="")
        {
            var list = sectionSer.Where();
            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                int year = 0;
                if (int.TryParse(keyWord, out year))
                {
                    var start = DateTime.Parse(year + "-01-01 00:00:00");
                    var end = DateTime.Parse(year + "-12-31 23:59:59");
                    list = list.Where(s => s.EnterTime >= start && s.EnterTime <= end);
                }
                else
                {
                    list = list.Where(s => s.SectionName.Contains(keyWord) || s.Time.Contains(keyWord) || s.SubTime.Contains(keyWord) || s.Authorizer.Contains(keyWord));
                }
            }
            var total = list.Count();
            list = list.OrderByDescending(s => s.SectionName).Skip(rows * (page - 1)).Take(rows);
            var result = list.ToList().Select(s => new
            {
                s.S_ID,//必须传一个唯一值过去
                s.SectionName,
                s.Time,
                s.SubTime,
                s.Authorizer,
                s.EnterTime

            }).ToList();
            return Json(new
            {
                total = total,
                rows = result
            });
        }
        /// <summary>
        /// 删除剖面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(string id)
        {
            sectionSer.RemoveWhere(x => x.S_ID == id);
            sectionSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }
        //
        // GET: /Section/

        public ActionResult Index(string rid, string showTitle)
        {
            ViewBag.R_ID = rid;
            ViewBag.ShowTitle = showTitle;
            return View();
        }
        //查询剖面
        public ActionResult QueryList()
        {
            return View();
        }
        //添加剖面
        public ActionResult AddSection(SectionView model, string RID11, string RID22, string RID33, double? Altitude1)
        {
            model.Altitude = Altitude1;
            model.RID1 = RID11;
            model.RID2 = RID22;
            model.RID3 = RID33;
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
        public ActionResult Edit(string id)
        {
            var model = sectionSer.FirstOrDefault(s => s.S_ID == id);
            var viewModel = new SectionView();
            viewModel.CopyFrom(model);
            return View(viewModel);
        }
        public ActionResult EditSection(SectionView model, string RID11, string RID22, string RID33, double? Altitude1)
        {
            model.Altitude = Altitude1;
            model.RID1 = RID11;
            model.RID2 = RID22;
            model.RID3 = RID33;
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
            var model = sectionSer.FirstOrDefault(x => x.S_ID == id);
            if (model == null)
            {
                return WriteError("该剖面不存在！");
            }
            sectionSer.Remove(model);
            sectionSer.SaveChanges();
            return WriteSuccess("删除成功");
        }
    }

}
