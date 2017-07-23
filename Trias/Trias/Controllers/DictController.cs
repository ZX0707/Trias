using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    /// <summary>
    /// 字典管理控制器
    /// </summary>
    public class DictController : BaseController
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
        /// 获取字典列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetList()
        {
            return Json(dictSer.Where().ToList());
        }

        /// <summary>
        /// 保存一个新的字典数据
        /// </summary>
        /// <param name="model">字典的ViewModel</param>
        /// <returns></returns>
        public ActionResult Save(DictView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            var Model = new Dict();
            Model.CopyFrom(model);
            dictSer.Add(Model);
            Model.Id = Guid.NewGuid().ToString();
            dictSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

        /// <summary>
        /// 修改一个字典数据
        /// </summary>
        /// <param name="model">字典的ViewModel</param>
        /// <returns></returns>
        public ActionResult Edit(DictView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            dictSer.EditWhere(x => x.Id == model.Id, model);
            dictSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

        /// <summary>
        /// 删除一个字典数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public ActionResult Remove(string id)
        {
            dictSer.RemoveWhere(x => x.Id == id);
            dictSer.SaveChanges();
            return WriteSuccess("操作成功！");
        }

        /// <summary>
        /// 获取下拉选项
        /// </summary>
        /// <param name="type">选项type</param>
        /// <returns></returns>
        public ActionResult GetSelectOptions(string type)
        {
            return Json(dictSer.Where(x => x.Type == type).OrderBy(x => x.Sort).ToList());
        }

    }
}
