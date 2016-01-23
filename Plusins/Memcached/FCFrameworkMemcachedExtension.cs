
using System.Net;
using DFramework.Plusins.Memcached;

namespace FC.Framework.Memcached
{
    public static class DFrameworkMemcachedExtension
    {
        public static FCFramework UseMemcached(this FCFramework framework, string memcachedHost, int port, string zone = "", string ocsUser = "", string ocsPassword = "")
        {
            IoC.Register<ICache>(new Memcache(memcachedHost, port, zone, ocsUser, ocsPassword));

            return framework;
        }
        public static FCFramework UseMemcached(this FCFramework framework, string configSection)
        {
            IoC.Register<ICache>(new Memcache(configSection));

            return framework;
        }
        public static FCFramework UseMemcached(this FCFramework framework, params IPEndPoint[] servers)
        {
            IoC.Register<ICache>(new Memcache(servers));

            return framework;
        }
    }
}
