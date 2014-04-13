using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework.EnterpriseLibrary
{
    public static class FCFrameworkEnterpriseCacheExtension
    {
        /// <summary>
        /// 使用EnterpriseLibrary.Caching作为FCFramework的缓存组件
        /// </summary>
        /// <param name="framework"></param>
        /// <returns>FCFramework</returns>
        public static FCFramework UseEnterpriseLibraryCache(this FCFramework framework, string cacheName = "Microsoft.EnterpriseCache")
        {
            IoC.Register<ICache>(new EnterpriseCache(cacheName));

            return framework;
        }
    }
}
