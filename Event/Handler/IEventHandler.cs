namespace FC.Framework
{
    public interface IEventHandler { }

    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : IDomainEvent
    { 
        void Handle(TEvent @event);
    }
}
