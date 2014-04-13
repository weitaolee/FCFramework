using System;
using FC.Framework;
using FC.Framework.Utilities;
namespace FC.Framework.Domain
{
    public static class EntityExtension
    {
        public static void RemoveCache(this IEntity eventHandler, string cacheKey)
        {
            IoC.Resolve<ICache>().Remove(cacheKey);
        }

        public static void RaiseEvent<TEvent>(this IEntity entity, TEvent @event) where TEvent : IDomainEvent
        {
            Check.Argument.IsNotNull(@event, "ent");

            var eventHandler = entity as IEventHandler<TEvent>;

            if (eventHandler != null)
            {
                //first of all,call the inner event handler , second , send the event to eventbus
                eventHandler.Handle(@event);
            }

            EventBus.Apply(@event);
        }

        public static void ThrowDomainException(this IEntity entity, string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            var e = new EventHandlerExecption(message);

            throw e;
        }

        public static void ThrowDomainException(this IEntity entity, string message, Exception innerException)
        {
            Check.Argument.IsNotEmpty(message, "message");

            var e = new EventHandlerExecption(message, innerException);

            throw e;
        }
    }
}
