
using System;

namespace FC.Framework.Domain
{
    public interface IAggregateRoot : IEntity
    {
        int ID { get; }
    }
}
