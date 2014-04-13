using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FC.Framework.Utilities;

namespace FC.Framework
{
    public static class EventHandlerExtension
    {
        public static void RemoveCache(this IEventHandler eventHandler, string cacheKey)
        {
            IoC.Resolve<ICache>().Remove(cacheKey);
        }
    }


}
