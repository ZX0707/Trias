using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Trias.Tool
{
    public class CacheHelper
    {
        public static Cache cache = HttpRuntime.Cache;

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public T GetCache<T>(string cacheKey) where T : class
        {
            if (cache[cacheKey] != null)
            {
                return (T)cache[cacheKey];
            }
            return default(T);
        }

        /// <summary>
        /// 设置缓存，默认过期时间一年
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">缓存对象</param>
        /// <param name="cacheKey">键</param>
        public void WriteCache<T>(T value, string cacheKey) where T : class
        {
            cache.Insert(cacheKey, value, null, DateTime.Now.AddDays(365), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">缓存对象</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        public void WriteCache<T>(T value, string cacheKey, DateTime expireTime) where T : class
        {
            cache.Insert(cacheKey, value, null, expireTime, Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public void RemoveCache(string cacheKey)
        {
            cache.Remove(cacheKey);
        }

        /// <summary>
        /// 清楚所有缓存
        /// </summary>
        public void RemoveCache()
        {
            IDictionaryEnumerator CacheEnum = cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                cache.Remove(CacheEnum.Key.ToString());
            }
        }
    }
}