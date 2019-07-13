using System;
using System.Linq;
using System.Net;

using FC.Framework;
using FC.Framework.Utilities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace FC.Framework.Plusins.Redis
{
    public class RedisCache : ICache
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisCache(string host)
        {
            Check.Argument.IsNotEmpty(host, nameof(host));
            _redis = ConnectionMultiplexer.Connect(host);
        }

        public object Get(string key)
        {
            return this._redis.GetDatabase().StringGet(key);
        }

        public bool TryGet(string key, out object value)
        {
            var result = false;
            try
            {
                var redisValue = this._redis.GetDatabase().StringGet(key);
                value = redisValue.IsNullOrEmpty ? null : redisValue.ToString();
                result = value != null;
            }
            catch (Exception ex)
            {
                value = null;
            }

            return result;
        }

        public T Get<T>(string key)
        {
            var data = this._redis.GetDatabase().StringGet(key);

            if (!data.IsNullOrEmpty)
                return JsonConvert.DeserializeObject<T>(data);
            else return default(T);
        }

        public bool TryGet<T>(string key, out T value)
        {
            var result = false;
            try
            {
                value = this.Get<T>(key);
                result = value != null;
            }
            catch (Exception ex)
            {
                value = default(T);
            }

            return result;
        }
        public void Add<T>(string key, T value, DateTime absoluteExpiration)
        {
            var notExpirated = absoluteExpiration > DateTime.Now;
            var timespan = absoluteExpiration - DateTime.Now;
            if (notExpirated)
            {
                var data = JsonConvert.SerializeObject(value);
                this._redis.GetDatabase().StringSet(key, data, timespan > TimeSpan.FromSeconds(10) ? timespan : TimeSpan.FromSeconds(10));
            }
        }

        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (slidingExpiration.TotalSeconds > 0)
            {
                var data = JsonConvert.SerializeObject(value);
                this._redis.GetDatabase().StringSet(key, data, slidingExpiration > TimeSpan.FromSeconds(10) ? slidingExpiration : TimeSpan.FromSeconds(10));
            }
        }

        public void Remove(string key)
        {
            this._redis.GetDatabase().KeyDelete(key);
        }
    }
}