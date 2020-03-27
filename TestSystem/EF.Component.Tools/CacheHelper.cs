using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Component.Tools
{

    /// <summary>
    ///CacheHelper 的摘要说明
    /// </summary>
    public static class CacheHelper
    {
        private static System.Web.Caching.Cache cache = System.Web.HttpContext.Current.Cache;

        #region 数据缓存

        //当缓存建立后过minute分钟就过期
        /// <summary>
        ///以key键值，把value赋给Cache[key].固定过期时间。当缓存建立后过minute分钟就过期.
        /// </summary>
        ///<param name="key">Cache键值</param>
        ///<param name="value">给Cache[key]赋的值</param>
        ///<param name="minute">固定过期分钟</param>
        public static void AddCacheFromMinutes(string key, object value, int minute)
        {
            cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minute));
        }

        //缓存建立后，过期时间为可调，比如1：20秒建立的缓存过期时间应该是6：20但如果在3：20有人访问了缓存，则过期时间将调整为8：20，以此类推……
        /// <summary>
        ///以key键值，把value赋给Cache[key].可调过期时间. 缓存被调用minute分钟后，再过期
        /// </summary>
        ///<param name="key">Cache键值</param>
        ///<param name="value">给Cache[key]赋的值</param>
        ///<param name="minute">可调过期分钟</param>
        public static void AddCacheAddMinutes(string key, object value, int minute)
        {
            cache.Insert(key, value, null, DateTime.Now.AddMinutes(minute), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        ///以key键值，把value赋给Cache[key].如果不主动清空，会一直保存在内存中.
        /// </summary>
        ///<param name="key">Cache键值</param>
        ///<param name="value">给Cache[key]赋的值</param>
        public static void AddCache(string key, object value)
        {
            cache.Insert(key, value);
        }

        /// <summary>
        ///清除Cache[key]的值
        /// </summary>
        ///<param name="key"></param>
        public static void RemoveCache(string key)
        {
            cache.Remove(key);
        }

        /// <summary>
        /// 清空Cache对象
        /// </summary>
        public static void Clear()
        {
            System.Collections.IDictionaryEnumerator CacheEnum = cache.GetEnumerator();
            System.Collections.ArrayList al = new System.Collections.ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }
            foreach (string key in al)
            {
                cache.Remove(key);
            }
        }

        /// <summary>
        ///根据key值，返回Cache[key]的值
        /// </summary>
        ///<param name="key"></param>
        public static object GetCacheByKey(string key)
        {
            return cache.Get(key);
        }

        #endregion
    }
}
