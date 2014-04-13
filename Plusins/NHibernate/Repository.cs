using System;
using System.Collections.Generic;
using FC.Framework.Domain;
using FC.Framework.Repository;
using FC.Framework.Utilities;
using NHibernate;

namespace FC.Framework.NHibernate
{
    public class Repository : IRepository
    {
        protected ISession _session;

        public Repository(IDatabaseFactory databaseFactory)
        {
            Check.Argument.IsNotNull(databaseFactory, "databaseFactory");

            this._session = databaseFactory.Session;
        }
        public void Add<T>(T aggregate) where T : class, IAggregateRoot
        {
            Check.Argument.IsNotNull(aggregate, "aggregate");

            this._session.Save(aggregate);
        }

        public T FindById<T>(int id) where T : class, IAggregateRoot
        {
            Check.Argument.IsNotNull(id, "id");

            return this._session.QueryOver<T>().Where(t => t.ID == id).SingleOrDefault();
        }
    }
}
