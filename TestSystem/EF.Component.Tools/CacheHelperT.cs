using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace EF.Component.Tools
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        bool Add<T>(string key, T value);

        /// <summary>
        /// 增加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="duration">持续时间</param>
        /// <returns>结果</returns>
        bool Add<T>(string key, T value, TimeSpan duration);

        /// <summary>
        /// 清除
        /// </summary>
        void Clear();
        /// <summary>
        /// 清除
        /// </summary>
        void ClearByPrefix();

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        T Get<T>(string key);

        /// <summary>
        /// 多线程获取
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns>值集合</returns>
        IDictionary<string, object> MultiGet(IList<string> keys);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        bool Set<T>(string key, T value);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="duration">持续时间</param>
        /// <returns>结果</returns>
        bool Set<T>(string key, T value, TimeSpan duration);
    }


    /// <summary>
    /// 缓存基类
    /// </summary>
    public abstract class CacheBase : ICache
    {
        private TimeSpan maxDuration = TimeSpan.FromHours(1);

        /// <summary>
        /// 最长持续时间
        /// </summary>
        public TimeSpan MaxDuration
        {
            get
            {
                return this.maxDuration;
            }
            set
            {
                this.maxDuration = value;
            }
        }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix
        {
            get;
            set;
        }

        public bool Add<T>(string key, T value)
        {
            return this.Add<T>(key, value, this.MaxDuration);
        }

        public abstract bool Add<T>(string key, T value, TimeSpan duration);

        public abstract void Clear();
        public abstract void ClearByPrefix();

        public abstract T Get<T>(string key);

        /// <summary>
        ///  获取全名
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>全名</returns>
        public virtual string GetFullName(string key)
        {
            string result = key;
            if (!string.IsNullOrEmpty(this.Prefix))
            {
                result = string.Format("{0}.{1}", this.Prefix, key);
            }

            return result;
        }

        public abstract IDictionary<string, object> MultiGet(IList<string> keys);

        public abstract void Remove(string key);

        public bool Set<T>(string key, T value)
        {
            return this.Set<T>(key, value, this.MaxDuration);
        }

        public abstract bool Set<T>(string key, T value, TimeSpan duration);
    }

    /// <summary>
    /// Aspnet缓存
    /// </summary>
    /// 
    public class AspnetCache : CacheBase
    {
        private System.Web.Caching.Cache cache = HttpRuntime.Cache;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AspnetCache()
            : this("Common.Cache")
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prefix">前缀</param>
        public AspnetCache(string prefix)
        {
            this.Prefix = prefix;
        }

        public override bool Add<T>(string key, T value, TimeSpan duration)
        {
            bool result = false;
            if (value != null)
            {
                if (duration <= TimeSpan.Zero)
                {
                    duration = this.MaxDuration;
                }
                result = this.cache.Add(this.GetFullName(key), value, null, DateTime.Now.Add(duration), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null) == null;
            }

            return result;
        }

        public override void Clear()
        {
            //获取键集合
            IList<string> keys = new List<string>();
            IDictionaryEnumerator caches = this.cache.GetEnumerator();
            while (caches.MoveNext())
            {
                string key = caches.Key.ToString();
                //if (key.StartsWith(this.Prefix))
                //{
                keys.Add(key);
                //}
            }
            //移除全部
            foreach (string key in keys)
            {
                this.cache.Remove(key);
            }
        }
        public override void ClearByPrefix()
        {
            //获取键集合
            IList<string> keys = new List<string>();
            IDictionaryEnumerator caches = this.cache.GetEnumerator();
            while (caches.MoveNext())
            {
                string key = caches.Key.ToString();
                if (key.StartsWith(this.Prefix))
                {
                    keys.Add(key);
                }
            }
            //移除全部
            foreach (string key in keys)
            {
                this.cache.Remove(key);
            }
        }

        public override T Get<T>(string key)
        {
            T result = default(T);
            object value = this.cache.Get(this.GetFullName(key));
            if (value is T)
            {
                result = (T)value;
            }

            return result;
        }

        public override IDictionary<string, object> MultiGet(IList<string> keys)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in keys)
            {
                result.Add(key, this.Get<object>(key));
            }

            return result;
        }

        public override void Remove(string key)
        {
            this.cache.Remove(this.GetFullName(key));
        }

        public override bool Set<T>(string key, T value, TimeSpan duration)
        {
            bool result = false;
            if (value != null)
            {
                if (duration <= TimeSpan.Zero)
                {
                    duration = this.MaxDuration;
                }
                this.cache.Insert(this.GetFullName(key), value, null, DateTime.Now.Add(duration), System.Web.Caching.Cache.NoSlidingExpiration);
                result = true;
            }

            return result;
        }
    }
}
