using System;
using System.Linq;
using System.Net;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using FC.Framework;
using FC.Framework.Utilities;

namespace DFramework.Plusins.Memcached
{
    public class Memcache : ICache
    {
        private readonly MemcachedClient _memClient;

        public Memcache(string section)
        {
            Check.Argument.IsNotEmpty(section, "section");
            this._memClient = new MemcachedClient(section);
        }


        public Memcache(IPEndPoint[] servers)
        {
            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();

            Check.Argument.IsNotEmpty(servers, "servers");

            servers.ForEach(s => memConfig.Servers.Add(s));
            memConfig.Protocol = MemcachedProtocol.Binary;
            memConfig.SocketPool.MinPoolSize = 5;
            memConfig.SocketPool.MaxPoolSize = 200;
            this._memClient = new MemcachedClient(memConfig);
        }


        /// <summary>
        /// Support Aliyun OCS
        /// </summary>
        /// <param name="host">OCS server Ip</param>
        /// <param name="port"></param>
        /// <param name="zone"></param>
        /// <param name="ocsUser"></param>
        /// <param name="ocsPassword"></param>
        public Memcache(string host, int port, string zone = "", string ocsUser = "", string ocsPassword = "")
        {
            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
            IPAddress address;

            if (!IPAddress.TryParse(host, out address))
            {
                // not an ip, resolve from dns
                // TODO we need to find a way to specify whihc ip should be used when the host has several
                var entry = System.Net.Dns.GetHostEntry(host);
                address = entry.AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                if (address == null)
                    throw new ArgumentException(String.Format("Could not resolve host '{0}'.", host));
            }

            IPEndPoint ipEndPoint = new IPEndPoint(address, port);

            memConfig.Servers.Add(ipEndPoint);
            memConfig.Protocol = MemcachedProtocol.Binary;
            if (!string.IsNullOrEmpty(ocsUser) && !string.IsNullOrEmpty(ocsPassword))
            {
                memConfig.Authentication.Type = typeof(PlainTextAuthenticator);
                memConfig.Authentication.Parameters["zone"] = zone;
                memConfig.Authentication.Parameters["userName"] = ocsUser;
                memConfig.Authentication.Parameters["password"] = ocsPassword;
            }
            memConfig.SocketPool.MinPoolSize = 5;
            memConfig.SocketPool.MaxPoolSize = 200;
            this._memClient = new MemcachedClient(memConfig);
        }

        public object Get(string key)
        {
            return this._memClient.Get<object>(key);
        }

        public bool TryGet(string key, out object value)
        {
            var result = false;
            try
            {
                value = this._memClient.Get<object>(key);
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
            return this._memClient.Get<T>(key);
        }

        public bool TryGet<T>(string key, out T value)
        {
            var result = false;
            try
            {
                value = this._memClient.Get<T>(key);
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
            if (notExpirated)
                this._memClient.Store(StoreMode.Set, key, value, absoluteExpiration);
        }

        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (slidingExpiration.TotalSeconds > 0)
                this._memClient.Store(StoreMode.Set, key, value, slidingExpiration);
        }

        public void Remove(string key)
        {
            this._memClient.Remove(key);
        }
    }
}