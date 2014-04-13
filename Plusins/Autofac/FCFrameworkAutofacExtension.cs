using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Framework.Autofac
{
    public static class FCFrameworkAutofacExtension
    { /// <summary>
        /// 使用EnterpriseLibrary.Caching作为FCFramework的缓存组件
        /// </summary>
        /// <param name="framework"></param>
        /// <returns>FCFramework</returns>
        public static FCFramework UseAutofac(this FCFramework framework)
        {
            IoC.InitializeWith(new DependencyResolverFactory(typeof(AutofacDependencyResolver)));

            return framework;
        }
    }
}
