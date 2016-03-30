namespace FC.Framework
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent ent)
            where TEvent : IDomainEvent;

        void Publish<TEvent>(TEvent ent, EventDispatchStrategy executionStrategy)
                   where TEvent : IDomainEvent;

    }
}
