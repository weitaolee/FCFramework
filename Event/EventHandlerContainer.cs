using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FC.Framework.Domain;
using FC.Framework.Utilities;

namespace FC.Framework
{
    public class EventHandlerContainer : IEventHandlerContainer
    {
        private readonly object _writeLock = new object();
        private Dictionary<Type, List<MethodInfo>> _handlerMethodsForEventType = new Dictionary<Type, List<MethodInfo>>();

        public IEnumerable<MethodInfo> FindHandlerMethods<TEvent>()
            where TEvent : IDomainEvent
        {
            var handlers = default(List<MethodInfo>);

            this._handlerMethodsForEventType.TryGetValue(typeof(TEvent), out handlers);

            return handlers;
        }

        public IEnumerable<MethodInfo> FindHandlerMethods<TEvent>(EventDispatchStrategy dispatchStrategy)
            where TEvent : IDomainEvent
        {
            Check.Argument.IsNotNull(dispatchStrategy, "dispatchStrategy");

            var handlers = default(List<MethodInfo>);

            if (this._handlerMethodsForEventType.TryGetValue(typeof(TEvent), out handlers))
            {
                if (dispatchStrategy == EventDispatchStrategy.OnEventRaised)
                    handlers = handlers.Where(h => !TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(h, typeof(AwaitCommittedAttribute))).ToList();
                else if (dispatchStrategy == EventDispatchStrategy.OnCommitted)
                    handlers = handlers.Where(h => TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(h, typeof(AwaitCommittedAttribute))).ToList();
            }

            return handlers;
        }

        public void RegisterHandler(Type handlerType)
        {
            Check.Argument.IsNotNull(handlerType, "handlerType");

            if (!handlerType.IsClass || handlerType.IsAbstract || handlerType.IsGenericType || handlerType.IsInterface) return;

            //忽略继承IEntity的聚合根内部EventHanlder
            if (handlerType.GetInterfaces().Count(i => i == typeof(IEntity)) > 0) return;

            //获取该事件处理器类型响应的事件类型集合
            var eventTypes = TypeUtil.GetGenericArgumentTypes(handlerType, typeof(IEventHandler<>));
            if (eventTypes == null || eventTypes.Count() == 0) return;

            var handlerMethods = handlerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                            .Where(m => m.Name == "Handle" && m.ReturnType == typeof(void));

            lock (_writeLock)
            {
                foreach (var etype in eventTypes)
                {
                    List<MethodInfo> containHandlerMethods = null;

                    if (!this._handlerMethodsForEventType.TryGetValue(etype, out containHandlerMethods))
                    {
                        containHandlerMethods = new List<MethodInfo>();
                        this._handlerMethodsForEventType.Add(etype, containHandlerMethods);
                    }
                    foreach (var method in handlerMethods)
                    {
                        var methodParameters = method.GetParameters();
                        if (methodParameters.Length == 1 && methodParameters[0].ParameterType == etype)
                        {
                            containHandlerMethods.Add(method);
                            break;
                        }
                    }
                }
            }
        }

        public void RegisterHandlers(Assembly assemblyToScan)
        {
            Check.Argument.IsNotNull(assemblyToScan, "assemblyToScan");

            foreach (Type type in assemblyToScan.GetTypes())
            {
                RegisterHandler(type);
            }
        }

        public void RegisterHandlers(IEnumerable<Assembly> assembliesToScan)
        {
            Check.Argument.IsNotNull(assembliesToScan, "assembliesToScan");

            foreach (var assemblyToScan in assembliesToScan)
            {
                foreach (Type type in assemblyToScan.GetTypes())
                {
                    RegisterHandler(type);
                }
            }
        }
    }
}
