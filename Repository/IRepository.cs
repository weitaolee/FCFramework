﻿using FC.Framework.Domain;

namespace FC.Framework.Repository
{
    public interface IRepository
    {
        void Add<T>(T aggregate) where T : class, IAggregateRoot;

        T FindById<T>(int id) where T : class, IAggregateRoot;
    }
}
