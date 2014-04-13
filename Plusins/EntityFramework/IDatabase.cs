using System;
using System.Collections.Generic;
using System.Linq;
using FC.Framework.Domain;

namespace FC.Framework.EntityFramework
{
    public interface IDatabase : IDisposable
    {
        IQueryable<T> GetDataSource<T>() where T : class, IEntity;

        T FindByID<T>(int entityID) where T : class,IEntity;

        IEnumerable<T> FindAll<T>() where T : class,IEntity;

        void InsertOnSubmit<T>(T entity) where T : class,IEntity;

        void InsertAllOnSubmit<T>(IEnumerable<T> entities) where T : class,IEntity;

        void DeleteOnSubmit<T>(T entity) where T : class,IEntity;

        void DeleteAllOnSubmit<T>(IEnumerable<T> entities) where T : class,IEntity;

        void Commit();
    }
}
