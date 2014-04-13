using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace FC.Framework
{
    public interface IEventHandlerContainer
    {
        IEnumerable<MethodInfo> FindHandlerMethods<TEvent>()
            where TEvent : IDomainEvent;


        IEnumerable<MethodInfo> FindHandlerMethods<TEvent>(EventDispatchStrategy executionStrategy)
            where TEvent : IDomainEvent;

        void RegisterHandler(Type handlerType);

        void RegisterHandlers(Assembly assemblyToScan);

        void RegisterHandlers(IEnumerable<Assembly> assembliesToScan);
    }
}
