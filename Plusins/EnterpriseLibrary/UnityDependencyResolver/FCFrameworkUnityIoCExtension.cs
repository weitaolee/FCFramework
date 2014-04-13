using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework.EnterpriseLibrary
{
    public static class FCFrameworkUnityIoCExtension
    {
        public static FCFramework UseEnterpriseLibraryIoC(this FCFramework framework)
        {
            IoC.InitializeWith(new DependencyResolverFactory(typeof(UnityDependencyResolver)));

            return framework;
        }
    }
}
