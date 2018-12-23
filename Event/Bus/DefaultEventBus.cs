using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FC.Framework.Utilities;
using FC.Framework.Domain;

namespace FC.Framework
{
    public class DefaultEventBus : IEventBus
    {
        private IEventHandlerContainer _eventContariner;
        private IEventHandlerActivator _eventHandlerActivor;
        private IEventHandlerInvoker _handlerInvoker; //去除了反射调用，Invoker暂时保留

        #region ctor
        public DefaultEventBus() : this(new EventHandlerContainer()) { }

        public DefaultEventBus(IEventHandlerContainer eventHandlerContariner) :
            this(eventHandlerContariner, new EventHandlerActivator(), new FastEventHandlerInvoker()) { }

        public DefaultEventBus(IEventHandlerContainer eventHandlerContariner,
                               IEventHandlerActivator eventHandlerActivor,
                               IEventHandlerInvoker handlerInvoker)
        {
            Check.Argument.IsNotNull(eventHandlerContariner, "eventHandlerContariner");
            Check.Argument.IsNotNull(eventHandlerActivor, "eventHandlerActivor");
            Check.Argument.IsNotNull(handlerInvoker, "handlerInvoker");

            this._eventContariner = eventHandlerContariner;
            this._eventHandlerActivor = eventHandlerActivor;
            this._handlerInvoker = handlerInvoker;
        }
        #endregion

        #region PublishEvent
        public virtual void Publish<TEvent>(TEvent @event)
           where TEvent : IDomainEvent
        {
            this.Publish(@event, EventDispatchStrategy.OnEventRaised);
        }

        public virtual void Publish<TEvent>(TEvent @event, EventDispatchStrategy dispatchStrategy)
            where TEvent : IDomainEvent
        {
            var handlerMethods = this._eventContariner.FindHandlerMethods<TEvent>(dispatchStrategy);

            if (handlerMethods == null || !handlerMethods.Any())
            {
                return;
                //throw new EventHandlerExecption("no handler of event type(" + typeof(TEvent).Name + ").");
            } 
            foreach (var method in handlerMethods)
            {
                HandlerInvoke(method, @event);
            }
        }

        #endregion

        private void HandlerInvoke<TEvent>(MethodInfo handleMethod, TEvent arg)
            where TEvent : IDomainEvent
        {
            var handlerType = handleMethod.DeclaringType;
            IEventHandler<TEvent> handler;
            try
            {
                handler = this._eventHandlerActivor.CreateInstance(handlerType) as IEventHandler<TEvent>;
            }
            catch (Exception ex)
            {
                throw new EventHandlerExecption("Faile to create handler instance, see inner exception for detail,Handler type:" + handlerType.Name + ".", ex);
            }

            try
            {
                handler.Handle(arg);
            }
            catch (IoCException)
            {
                throw;
            }
            catch (DomainException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EventHandlerExecption("Faile to execute event handler, see inner exception for detail. Handler type:" + handlerType.Name + ".", ex);
            }
        }
    }
}
