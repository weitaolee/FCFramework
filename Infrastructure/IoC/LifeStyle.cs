using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    public enum LifeStyle
    {
        Transient,
        Singleton,
        PerHttpRequest,
        PerThread
    }
}
