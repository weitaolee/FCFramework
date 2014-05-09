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

        public static void Apply(this IEventHandler eventHandler, IDomainEvent @event)
        {
            EventBus.Apply(@event);
        }
    }


}
