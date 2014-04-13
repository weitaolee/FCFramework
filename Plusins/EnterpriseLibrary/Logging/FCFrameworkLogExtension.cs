using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework.EnterpriseLibrary
{
    public static class FCFrameworkLogExtension
    {
        public static FCFramework UseEnterpriseLibraryLog(this FCFramework framework, int frameToSkip = 4)
        {
            IoC.Register<ILog>(new EnterpriseLog(frameToSkip));

            return framework;
        }
    }
}
