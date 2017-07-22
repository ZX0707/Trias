using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trias.Tool
{
    public class CacheFacory
    {
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <returns></returns>
        public static CacheHelper Cache()
        {
            return new CacheHelper();
        }
    }
}