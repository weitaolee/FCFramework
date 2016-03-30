namespace FC.Framework
{
    public static class EventHandlerExtension
    {
        public static void RemoveCache(this IEventHandler eventHandler, string cacheKey)
        {
            IoC.Resolve<ICache>().Remove(cacheKey);
        }

        public static void Apply<TEvent>(this IEventHandler eventHandler, TEvent @event)
            where TEvent : IDomainEvent
        {
            EventBus.Apply(@event);
        }
    }
}
