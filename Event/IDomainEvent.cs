using System;

namespace FC.Framework
{
    public interface IDomainEvent
    {
        DateTime UTCTimestamp { get; }
    }

    public class DomainEvent : IDomainEvent
    {
        public DateTime UTCTimestamp { get { return DateTime.Now.ToUniversalTime(); } }
    }
}
