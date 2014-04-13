using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FC.Framework.Utilities;

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
