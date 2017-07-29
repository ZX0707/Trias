using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trias.Tool;

namespace Trias.Controllers
{
    public class BaiduController : BaseController
    {
        /// <summary>
        /// 获取地理位置的经纬度
        /// </summary>
        /// <param name="city">城市名称</param>
        /// <param name="place">城市内地名，给空则返回城市的经纬度信息</param>
        /// <returns></returns>
        public ActionResult GetLocationByPlaceName(string city, string place)
        {
            try
            {
                return Content(BaiduApiHelper.GetLocationsByName(city, place));
            }
            catch (Exception e)
            {
                return WriteError(e.Message);
            }
        }

    }
}
