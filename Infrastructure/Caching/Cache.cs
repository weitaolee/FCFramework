
namespace FC.Framework
{
    using System;
    using System.Diagnostics;
    using FC.Framework.Utilities;
    public static class Cache
    {
        private static ICache _internalCache;
        private static System.Web.Caching.Cache _webCache;
        static Cache()
        {
            try { _internalCache = IoC.Resolve<ICache>(); }
            catch { if (_internalCache == null) _webCache = new System.Web.Caching.Cache(); }

        }

        /// <summary>
        /// get cache data
        /// </summary>
        /// <param name="key">cache data's key</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static object Get(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            if (_internalCache != null)
                return _internalCache.Get(key);
            else
                return _webCache.Get(key);
        }
        /// <summary>
        /// try get cache data
        /// </summary>
        /// <param name="key">cache data's key</param>
        /// <param name="value">cache data</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool TryGet(string key, out object value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            if (_internalCache != null)
                return _internalCache.TryGet(key, out value);
            else
            {
                try
                {
                    value = _webCache.Get(key);
                    return true;
                }
                catch
                {
                    value = null;
                    return false;
                }
            }

        }
        /// <summary>
        ///  get cache data
        /// </summary>
        /// <typeparam name="T">cache data's type</typeparam>
        /// <param name="key">cache data's key</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T Get<T>(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            if (_internalCache != null)
                return _internalCache.Get<T>(key);
            else
            {
                try
                {
                    return (T)_webCache.Get(key);
                }
                catch
                {
                    return default(T);
                }
            }
        }
        /// <summary>
        /// try get cache data
        /// </summary>
        /// <typeparam name="T">cache data's type</typeparam>
        /// <param name="key">cache data's key</param>
        /// <param name="value">获取到的cache data</param>
        /// <returns>如果获取成功返回true,否则false</returns>
        [DebuggerStepThrough]
        public static bool TryGet<T>(string key, out T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            if (_internalCache != null)
                return _internalCache.TryGet<T>(key, out value);
            else
            {
                try
                {
                    value = (T)_webCache.Get(key);
                    return true;
                }
                catch
                {
                    value = default(T);
                    return false;
                }
            }
        }

        /// <summary>
        /// add data to cache
        /// </summary>
        /// <param name="key">cache data's key</param>
        /// <param name="value">cache data</param>
        /// <param name="absoluteExpiration">  </param>
        [DebuggerStepThrough]
        public static void Add<T>(string key, T value, DateTime absoluteExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotInPast(absoluteExpiration, "absoluteExpiration");

            if (_internalCache != null)
                _internalCache.Add(key, value, absoluteExpiration);
            else
            {
                _webCache.Add(key, value, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
            }
        }
        /// <summary>
        /// add data to cache
        /// </summary>
        /// <param name="key">cache data's key</param>
        /// <param name="value">cache data</param>
        /// <param name="absoluteExpiration"> </param>
        [DebuggerStepThrough]
        public static void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNegativeOrZero(slidingExpiration, "slidingExpiration");
            if (_internalCache != null)
                _internalCache.Add(key, value, slidingExpiration);
            else
            {
                _webCache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
            }
        }
        /// <summary>
        /// remove cache data  
        /// </summary>
        /// <param name="key">the cache data key</param>
        [DebuggerStepThrough]
        public static void Remove(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");
            if (_internalCache != null)
                _internalCache.Remove(key);
            else
                _webCache.Remove(key);
        }
    }
}
