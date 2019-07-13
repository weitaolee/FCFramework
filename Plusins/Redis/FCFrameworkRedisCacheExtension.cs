
using FC.Framework.Plusins.Redis;
using System.Net;

namespace FC.Framework.Redis
{
    public static class FcFrameworkMemcachedExtension
    {
        public static FCFramework UseRedis(this FCFramework framework, string host)
        {
            IoC.Register<ICache>(new RedisCache(host));

            return framework;
        }
    }
}
