using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FC.Framework
{
    public interface IEventHandlerContainer
    {
        IOrderedEnumerable<MethodInfo> FindHandlerMethods<TEvent>()
            where TEvent : IDomainEvent;


        IOrderedEnumerable<MethodInfo> FindHandlerMethods<TEvent>(EventDispatchStrategy executionStrategy)
            where TEvent : IDomainEvent;

        void RegisterHandler(Type handlerType);

        void RegisterHandlers(Assembly assemblyToScan);

        void RegisterHandlers(IEnumerable<Assembly> assembliesToScan);
    }
}
