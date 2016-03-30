using System;
using System.Collections.Generic;
using System.Reflection;
using FC.Framework.Repository;
using FC.Framework.Utilities;
using System.IO;
using NHibernate;
using NHibernate.Mapping.ByCode;

namespace FC.Framework.NHibernate
{
    public static class FCFrameworkNHibernateExtension
    {
        /// <summary>
        /// Use NHibernate
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="connString">连接字符</param>
        /// <param name="mapperAssemblies">NHibernate Mapper Assemblies</param>
        /// <param name="interceptor"></param>
        /// <param name="nhibernateConfigFileName">NHibernate config file<remarks>default:hibernate.config</remarks></param>
        /// <returns></returns>
        public static FCFramework UseNHibernate(this FCFramework framework,
                                                ConnectionString connString,
                                                IEnumerable<Assembly> mapperAssemblies,
                                                IInterceptor interceptor = null,
                                                string nhibernateConfigFileName = "hibernate.config")
        {
            Check.Argument.IsNotEmpty(mapperAssemblies, "mapperAssemblies");
            Check.Argument.IsNotNull(connString, "connString");

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nhibernateConfigFileName);

            IoC.Register<IUnitOfWork, UnitOfWork>();
            IoC.Register<IRepository, Repository>();
            IoC.Register<ConnectionString>(connString);
            IoC.Register<IDatabaseFactory, DatabaseFactory>(LifeStyle.Singleton);

            SessionManager.Initalize(connString.Value, mapperAssemblies, interceptor,filePath);

            return framework;
        }

        /// <summary>
        /// Use NHibernate
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="connString">连接字符</param>
        /// <param name="mapperBuildAction">NHibernate Mapper Assemblies</param>
        /// <param name="interceptor"></param>
        /// <param name="nhibernateConfigFileName">NHibernate config file<remarks>default:hibernate.config</remarks></param>
        /// <returns></returns>
        public static FCFramework UseNHibernate(this FCFramework framework,
                                                ConnectionString connString,
                                                Func<ModelMapper> mapperBuildAction,
                                                IInterceptor interceptor = null,
                                                string nhibernateConfigFileName = "hibernate.config")
        {
            Check.Argument.IsNotNull(mapperBuildAction, "mapperBuildAction");
            Check.Argument.IsNotNull(connString, "connString");

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nhibernateConfigFileName);

            IoC.Register<IUnitOfWork, UnitOfWork>();
            IoC.Register<IRepository, Repository>();
            IoC.Register<ConnectionString>(connString);
            IoC.Register<IDatabaseFactory, DatabaseFactory>(LifeStyle.Singleton);

            SessionManager.Initalize(connString.Value, mapperBuildAction(), interceptor, filePath);

            return framework;
        }

        /// <summary>
        /// Use NHibernate
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="connString">连接字符</param>
        /// <param name="mapper">NHibernate Mapper Assemblies</param>
        /// <param name="interceptor"></param>
        /// <param name="nhibernateConfigFileName">NHibernate config file<remarks>default:hibernate.config</remarks></param>
        /// <returns></returns>
        public static FCFramework UseNHibernate(this FCFramework framework,
                                                ConnectionString connString,
                                                ModelMapper mapper,
                                                IInterceptor interceptor = null,
                                                string nhibernateConfigFileName = "hibernate.config")
        {
            Check.Argument.IsNotNull(mapper, "mapperBuildAction");
            Check.Argument.IsNotNull(connString, "connString");

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nhibernateConfigFileName);

            IoC.Register<IUnitOfWork, UnitOfWork>();
            IoC.Register<IRepository, Repository>();
            IoC.Register<ConnectionString>(connString);
            IoC.Register<IDatabaseFactory, DatabaseFactory>(LifeStyle.Singleton);

            SessionManager.Initalize(connString.Value, mapper, interceptor, filePath);

            return framework;
        }
    }
}
