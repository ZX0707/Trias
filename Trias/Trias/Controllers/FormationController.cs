﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Trias.Models;
using Trias.Tool;
using WebGrease.Css.Extensions;

namespace Trias.Controllers
{
    public class FormationController : BaseController
    {
        //
        // GET: /Formation/
        public ActionResult Details(string id)
        {
            var model = formationSer.Find(id);
            var viewModel = new FormationView();
            viewModel.CopyFrom(model);
            return View(viewModel);
        }
        public ActionResult Index(string id)
        {
            var model = sectionSer.Find(id) ?? new Section();
            var viewModel = new SectionView();
            viewModel.CopyFrom(model);
            return View(viewModel);
        }

        public ActionResult Add(string id)
        {
            ViewBag.SId = id;
            return View();
        }

        [HttpPost]
        public ActionResult Add(string formation, string rocks)
        {
            var formtionModel = JsonConvert.DeserializeObject<Formation>(formation);
            var sort = formationSer.Where().Select(x => x.sort).OrderByDescending(x => x).FirstOrDefault() ?? 0;
            sort++;
            formtionModel.sort = sort;
            #region 实体验证

            if (string.IsNullOrWhiteSpace(formtionModel.FormationName))
            {
                return WriteError("岩石组名必填！");
            }
            if (formtionModel.Thickness == null)
            {
                return WriteError("厚度必填！");
            }

            #endregion

            formtionModel.F_ID = Guid.NewGuid().ToString();
            var rockList = JsonConvert.DeserializeObject<List<Rock>>(rocks);

            #region 实体验证

            if (!rockList.Any())
            {
                return WriteError("必填项不能为空！");
            }
            for (var i = 0; i < rockList.Count; ++i)
            {
                var item = rockList.ElementAt(i);
                if (string.IsNullOrWhiteSpace(item.Color1))
                {
                    return WriteError("颜色一必填！");
                }
                if (string.IsNullOrWhiteSpace(item.Lithology1))
                {
                    return WriteError("岩性一必填！");
                }
            }

            #endregion
            rockList.ForEach(x =>
            {
                x.Rock_ID = Guid.NewGuid().ToString();
                x.Type_ID = formtionModel.F_ID;
            });
            formationSer.Add(formtionModel);
            rockSer.AddList(rockList);
            rockSer.SaveChanges();
            formationSer.SaveChanges();
            return WriteSuccess("添加成功！");
        }

        /// <summary>
        /// 通过剖面Id获取岩石组相关信息
        /// </summary>
        /// <param name="sid">剖面Id</param>
        /// <returns></returns>
        public ActionResult GetInfomationBySId(string sid)
        {
            var formationList = formationSer.Where(x => x.S_ID == sid).OrderBy(x => x.sort).ToList();
            var formationIds = formationList.Select(x => x.F_ID).ToList();
            var unitList = unitSer.Where(x => formationIds.Contains(x.F_ID)).OrderBy(x => x.sort).ToList();
            var unitIds = unitList.Select(x => x.U_ID).ToList();
            var collectionList = collectionSer.Where(x => unitIds.Contains(x.U_ID)).OrderBy(x => x.sort).ToList();
            var collectionIds = collectionList.Select(x => x.C_ID).ToList();
            var fossilList = fossilSer.Where(x => collectionIds.Contains(x.C_ID)).OrderBy(x => x.sort).ToList();
            var geochemicalList = geochemicalSer.Where(x => collectionIds.Contains(x.C_ID)).OrderBy(x => x.sort).ToList();
            var list = formationList.Select(f => new
            {
                formation = f,
                units = unitList.Where(u => u.F_ID == f.F_ID).Select(u => new
                {
                    unit = u,
                    collections = collectionList.Where(c => c.U_ID == u.U_ID).Select(c => new
                    {
                        collection = c,
                        fossils = fossilList.Where(fo => fo.C_ID == c.C_ID).ToList(),
                        geochemicals = geochemicalList.Where(g => g.C_ID == c.C_ID).ToList()
                    })
                }).ToList()
            }).ToList();
            return Json(list);
        }

        public ActionResult EditFormation(string id)
        {
            var model = new FormationView();
            var m = formationSer.FirstOrDefault(x => x.F_ID == id);
            model.CopyFrom(m);
            return View(model);
        }
        //修改岩石组信息
        [HttpPost]
        public ActionResult EditFormation(string formation, string rocks)
        {
            var formtionModel = JsonConvert.DeserializeObject<Formation>(formation);

            #region 实体验证

            if (string.IsNullOrWhiteSpace(formtionModel.FormationName))
            {
                return WriteError("岩石组名必填！");
            }
            if (formtionModel.Thickness == null)
            {
                return WriteError("厚度必填！");
            }

            #endregion

            formationSer.EditWhere(x => x.F_ID == formtionModel.F_ID, formtionModel);
            var rockList = JsonConvert.DeserializeObject<List<Rock>>(rocks);

            #region 实体验证

            if (!rockList.Any())
            {
                return WriteError("必填项不能为空！");
            }
            for (var i = 0; i < rockList.Count; ++i)
            {
                var item = rockList.ElementAt(i);
                if (string.IsNullOrWhiteSpace(item.Color1))
                {
                    return WriteError("颜色一必填！");
                }
                if (string.IsNullOrWhiteSpace(item.Lithology1))
                {
                    return WriteError("岩性一必填！");
                }
            }

            #endregion

            rockSer.RemoveWhere(x => x.Type_ID == formtionModel.F_ID);

            rockList.ForEach(x =>
            {
                x.Rock_ID = Guid.NewGuid().ToString();
                x.Type_ID = formtionModel.F_ID;
            });
            rockSer.AddList(rockList);
            rockSer.SaveChanges();
            formationSer.SaveChanges();
            return WriteSuccess("修改成功！");
        }
        //根据 剖面id 查询岩石组信息
        public ActionResult GetFormation(string id)
        {
            if (id == null)
            {
                return WriteError("该剖面不存在");
            }
            var formationlist = formationSer.Where(x => x.S_ID.Contains(id)).ToList();
            if (formationlist.Count == 0)
            {
                return WriteSuccess("暂无岩石组信息");
            }
            return Json(formationlist);
        }
        //删除岩石组信息
        public ActionResult GetList()
        {
            var list = formationSer.Where();
            return Json(new
            {
                total = list.Count()
            });
        }
        public ActionResult RemoveFormation(string id)
        {
            formationSer.RemoveWhere(x => x.F_ID == id);
            var unitList = unitSer.Where(x => x.F_ID == id).ToList();
            unitList.ForEach(x =>
            {
                unitSer.Remove(x);
                var collectionList = collectionSer.Where(y => y.U_ID == x.U_ID).ToList();
                collectionList.ForEach(y =>
                {
                    collectionSer.Remove(y);
                    collectionSer.RemoveWhere(z => z.C_ID == y.C_ID);
                    fossilSer.RemoveWhere(z => z.C_ID == y.C_ID);
                    geochemicalSer.RemoveWhere(z => z.C_ID == y.C_ID);
                });

            });
            formationSer.SaveChanges();
            unitSer.SaveChanges();
            collectionSer.SaveChanges();
            fossilSer.SaveChanges();
            geochemicalSer.SaveChanges();
            return WriteSuccess("删除成功");
        }

        public ActionResult ChangeSort(string id1, string id2, string level)
        {
            int? sort;
            switch (level)
            {
                case "level1":
                    var f1 = formationSer.FirstOrDefault(x => x.F_ID == id1);
                    var f2 = formationSer.FirstOrDefault(x => x.F_ID == id2);
                    sort = f1.sort;
                    f1.sort = f2.sort;
                    f2.sort = sort;
                    formationSer.SaveChanges();
                    break;
                case "level2":
                    var u1 = unitSer.FirstOrDefault(x => x.U_ID == id1);
                    var u2 = unitSer.FirstOrDefault(x => x.U_ID == id2);
                    sort = u1.sort;
                    u1.sort = u2.sort;
                    u2.sort = sort;
                    unitSer.SaveChanges();
                    break;
                case "level3":
                    var c1 = collectionSer.FirstOrDefault(x => x.C_ID == id1);
                    var c2 = collectionSer.FirstOrDefault(x => x.C_ID == id2);
                    sort = c1.sort;
                    c1.sort = c2.sort;
                    c2.sort = sort;
                    collectionSer.SaveChanges();
                    break;
                case "level4":
                    var fo1 = fossilSer.FirstOrDefault(x => x.H_ID == id1);
                    var fo2 = fossilSer.FirstOrDefault(x => x.H_ID == id2);
                    if (fo1 != null && fo2 != null)
                    {
                        sort = fo1.sort;
                        fo1.sort = fo2.sort;
                        fo2.sort = sort;
                        fossilSer.SaveChanges();
                    }
                    else
                    {
                        var g1 = geochemicalSer.FirstOrDefault(x => x.G_ID == id1);
                        var g2 = geochemicalSer.FirstOrDefault(x => x.G_ID == id2);
                        if (g1 != null && g2 != null)
                        {
                            sort = g1.sort;
                            g1.sort = g2.sort;
                            g2.sort = sort;
                            geochemicalSer.SaveChanges();
                        }
                    }
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
