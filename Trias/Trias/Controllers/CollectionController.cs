using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Trias.Models;
using Trias.Tool;

namespace Trias.Controllers
{
    public class CollectionController : BaseController
    {
        //
        // GET: /Collection/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Add(string id)
        {
            ViewBag.U_ID = id;
            return View();
        }
        [HttpPost]
        public ActionResult Add(string collection, string rocks)
        {
            var collectionModel = JsonConvert.DeserializeObject<Collection>(collection);
            collectionModel.C_ID = Guid.NewGuid().ToString();
            var rockList = JsonConvert.DeserializeObject<List<Rock>>(rocks);
            rockList.ForEach(x =>
            {
                x.Rock_ID = Guid.NewGuid().ToString();
                x.Type_ID = collectionModel.C_ID;
            });
            collectionSer.Add(collectionModel);
            rockSer.AddList(rockList);
            collectionSer.SaveChanges();
            rockSer.SaveChanges();
            return WriteSuccess("添加成功！");

        }


        //添加采样位置
        public ActionResult AddCollection(ViewCollection model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            var modelCollection = new Collection();
            modelCollection.CopyFrom(model);
            collectionSer.Add(modelCollection);
            modelCollection.U_ID = Guid.NewGuid().ToString();
            collectionSer.SaveChanges();
            return WriteSuccess("添加成功");
        }

        //根据 层id 查询采样位置
        public ActionResult GetCollection(string id)
        {
            if (id == null)
            {
                return WriteError("采样位置不存在");
            }
            var collectionlist = collectionSer.Where(x => x.U_ID.Contains(id)).ToList();
            if (collectionlist.Count == 0)
            {
                return WriteSuccess("暂无采样");
            }
            return Json(collectionlist);
        }
        //修改采样位置
        public ActionResult EditCollection(CollectionView model)
        {
            if (!ModelState.IsValid)
            {
                return WriteStatusError(ModelState);
            }
            collectionSer.EditWhere(x => x.C_ID == model.C_ID, model);
            collectionSer.SaveChanges();
            return WriteSuccess("修改成功");
        }
        //删除采样位置
        public ActionResult RemoveCollection(string id)
        {
            collectionSer.RemoveWhere(x => x.C_ID == id);
            fossilSer.RemoveWhere(x => x.C_ID == id);
            geochemicalSer.RemoveWhere(x => x.C_ID == id);
            collectionSer.SaveChanges();
            fossilSer.SaveChanges();
            geochemicalSer.SaveChanges();
            return WriteSuccess("删除成功");
        }

    }
}
