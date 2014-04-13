using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    public interface IEventHandler { }

    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : IDomainEvent
    { 
        void Handle(TEvent @event);
    }
}
