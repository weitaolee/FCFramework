namespace FC.Framework.EnterpriseLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using FC.Framework.Utilities;
    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    /// <summary>
    /// 基于Microsoft EnterpriseLibrary的缓存实现
    /// </summary>
    public class EnterpriseCache : ICache
    {
        private ICacheManager _manager;

        [DebuggerStepThrough]
        public EnterpriseCache(string cacheName)
        {
            _manager = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
        }

       

        public object Get(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");
            return _manager.GetData(key);
        }

        public bool TryGet(string key, out object value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            value = null;

            if (_manager.Contains(key))
            {
                value = _manager.GetData(key);
                if (value != null)
                {
                    return true;
                }
            }

            return false;

        }

        public T Get<T>(string key) 
        {
            Check.Argument.IsNotEmpty(key, "key");

            return (T)_manager.GetData(key);
        }

        public bool TryGet<T>(string key, out T value) 
        {
            Check.Argument.IsNotEmpty(key, "key");

            value = default(T);

            if (_manager.Contains(key))
            {
                object existingValue = _manager.GetData(key);

                if (existingValue != null)
                {
                    value = (T)existingValue;

                    return true;
                }
            }

            return false;
        }

        public void Add<T>(string key, T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            _manager.Add(key, value);
        }

        public void Add<T>(string key, T value, DateTime absoluteExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotInPast(absoluteExpiration, "absoluteExpiration");

            _manager.Add(key, value, CacheItemPriority.Normal, null, new AbsoluteTime(absoluteExpiration));
        }

        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNegativeOrZero(slidingExpiration, "slidingExpiration");

            _manager.Add(key, value, CacheItemPriority.Normal, null, new SlidingTime(slidingExpiration));
        }

        public void Remove(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            _manager.Remove(key);
        }

        public void Clear()
        {
            _manager.Flush();
        }

    }
}
