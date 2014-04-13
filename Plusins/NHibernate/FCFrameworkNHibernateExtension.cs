using System;
using System.Collections.Generic;
using System.Reflection;
using FC.Framework.Repository;
using FC.Framework.Utilities;
using System.IO;

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
        /// <param name="nhibernateConfigFileName">NHibernate config file<remarks>default:hibernate.config</remarks></param>
        /// <returns></returns>
        public static FCFramework UseNHibernate(this FCFramework framework,
                                                ConnectionString connString,
                                                IEnumerable<Assembly> mapperAssemblies,
                                                string nhibernateConfigFileName = "hibernate.config")
        {
            Check.Argument.IsNotEmpty(mapperAssemblies, "mapperAssemblies");
            Check.Argument.IsNotNull(connString, "connString");

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nhibernateConfigFileName);

            IoC.Register<IUnitOfWork, UnitOfWork>();
            IoC.Register<IRepository, Repository>();
            IoC.Register<ConnectionString>(connString);
            IoC.Register<IDatabaseFactory, DatabaseFactory>(LifeStyle.Singleton);

            SessionManager.Initalize(connString.Value, mapperAssemblies, filePath);
#if DEBUG
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
#endif

            return framework;
        }
    }
}
