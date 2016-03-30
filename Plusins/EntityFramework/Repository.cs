using FC.Framework.Domain;
using FC.Framework.Repository;
using FC.Framework.Utilities;

namespace FC.Framework.EntityFramework
{
    public class Repository : IRepository
    {
        private IDatabase _database;

        public Repository(IDatabaseFactory databaseFactory)
        {
            Check.Argument.IsNotNull(databaseFactory, "databaseFactory");

            this._database = databaseFactory.DatabaseInstance;
        }
        public void Add<T>(T aggregate) where T : class, IAggregateRoot
        {
            Check.Argument.IsNotNull(aggregate, "aggregate");

            this._database.InsertOnSubmit<T>(aggregate);
        }

        public T FindById<T>(int id) where T : class,IAggregateRoot
        {
            Check.Argument.IsNotNegativeOrZero(id, "id");

            return this._database.FindByID<T>(id);
        }
    }
}
