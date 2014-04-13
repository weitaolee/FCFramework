using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework.CouchbaseCache
{
    public static class FCFrameworkEnterpriseCacheExtension
    {
        /// <summary>
        /// 使用CouchBase作为FCFramework的缓存组件
        /// </summary>
        /// <param name="framework"></param>
        /// <returns>FCFramework</returns>
        public static FCFramework UseCouchbaseCache(this FCFramework framework)
        {
            IoC.Register<ICache>(new CouchbaseCache());

            return framework;
        }
    }
}
