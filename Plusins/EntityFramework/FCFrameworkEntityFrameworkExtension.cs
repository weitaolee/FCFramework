using System;
using System.Collections.Generic;
using System.Linq;
using FC.Framework.Domain;
using FC.Framework.Repository;
using System.Data.Entity;
using FC.Framework.Utilities;

namespace FC.Framework.EntityFramework
{
    public static class FCFrameworkEntityFrameworkExtension
    {
        /// <summary>
        /// Use EntityFrame
        /// </summary>
        /// <param name="framework">FCFramework</param>
        /// <param name="database">the database class type</param>
        /// <param name="connString">database connection string</param>
        /// <returns></returns>
        public static FCFramework UseEntityFramework(this FCFramework framework, Type databaseImplType, ConnectionString connString)
        {
            Check.Argument.IsNotNull(databaseImplType, "databaseImplType");
            Check.Argument.IsNotNull(connString, "connString");

            IoC.Register<IUnitOfWork, UnitOfWork>();
            IoC.Register<IRepository, Repository>();
            IoC.Register<ConnectionString>(connString);
            IoC.Register<IDatabaseFactory, DatabaseFactory>(LifeStyle.Singleton);
            IoC.Register(typeof(IDatabase), databaseImplType);

            return framework;
        }
    }
}
