
namespace FC.Framework
{
    using System;
    using System.Diagnostics;
    using FC.Framework.Utilities;
    public static class Cache
    {
        private static ICache _internalCache;

        static Cache()
        {
            _internalCache = IoC.Resolve<ICache>();
        }

        /// <summary>
        /// 获得指定类型、特定key的缓存数据
        /// </summary>
        /// <param name="key">缓存数据key</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static object Get(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _internalCache.Get(key);
        }
        /// <summary>
        /// 试着获取指定类型、特定key的缓存数据
        /// </summary>
        /// <param name="key">缓存数据key</param>
        /// <param name="value">获取到的缓存数据</param>
        /// <returns>如果获取成功返回true,否则false</returns>
        [DebuggerStepThrough]
        public static bool TryGet(string key, out object value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _internalCache.TryGet(key, out value);
        }
        /// <summary>
        /// 获得指定类型、特定key的缓存数据
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存数据key</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T Get<T>(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _internalCache.Get<T>(key);
        }
        /// <summary>
        /// 试着获取指定类型、特定key的缓存数据
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存数据key</param>
        /// <param name="value">获取到的缓存数据</param>
        /// <returns>如果获取成功返回true,否则false</returns>
        [DebuggerStepThrough]
        public static bool TryGet<T>(string key, out T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _internalCache.TryGet<T>(key, out value);
        }

        /// <summary>
        /// 添加数据到缓存中
        /// </summary>
        /// <param name="key">缓存数据key</param>
        /// <param name="value">缓存数据</param>
        [DebuggerStepThrough]
        public static void Add<T>(string key, T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            _internalCache.Add(key, value);
        }
        /// <summary>
        /// 添加数据到缓存中
        /// </summary>
        /// <param name="key">缓存数据key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        [DebuggerStepThrough]
        public static void Add<T>(string key, T value, DateTime absoluteExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotInPast(absoluteExpiration, "absoluteExpiration");

            _internalCache.Add(key, value, absoluteExpiration);
        }
        /// <summary>
        /// 添加数据到缓存中
        /// </summary>
        /// <param name="key">缓存数据key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="absoluteExpiration">相对过期时间</param>
        [DebuggerStepThrough]
        public static void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNegativeOrZero(slidingExpiration, "slidingExpiration");

            _internalCache.Add(key, value, slidingExpiration);
        }
        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        [DebuggerStepThrough]
        public static void Remove(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            _internalCache.Remove(key);
        }
    }
}
