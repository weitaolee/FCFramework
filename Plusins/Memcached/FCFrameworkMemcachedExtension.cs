
using DFramework.Plusins.Memcached;

namespace FC.Framework.Memcached
{
    public static class DFrameworkMemcachedExtension
    {
        /// <summary>
        ///  CouchBase cache plusins
        /// </summary>
        /// <param name="framework"></param>
        /// <returns>FCFramework</returns>
        public static FCFramework UseMemcached(this FCFramework framework, string memcacheServerIp, string zone = "", string ocsUser = "", string ocsPassword = "")
        {
            IoC.Register<ICache>(new Memcache(memcacheServerIp, zone, ocsUser, ocsPassword));

            return framework;
        }
    }
}
