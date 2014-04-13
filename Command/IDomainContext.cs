using FC.Framework.Domain;
using FC.Framework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC.Framework
{
    public class DefaultDomainContext : IDomainContext
    {
        private IRepository _repository;

        public DefaultDomainContext()
        {
            this._repository = IoC.Resolve<IRepository>();
        }

        public void Add<T>(T domain) where T : class, IAggregateRoot
        {
            this._repository.Add<T>(domain);
        }

        public T Get<T>(int id) where T : class, IAggregateRoot
        {
            return this._repository.FindById<T>(id);
        }
    }
    public interface IDomainContext
    {
        void Add<T>(T domain) where T : class, IAggregateRoot;

        T Get<T>(int id) where T : class, IAggregateRoot;
    }
}
