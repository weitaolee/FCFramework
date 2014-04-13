using FC.Framework;
using FC.Framework.Repository;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FC.Framework.Utilities;

namespace FC.Framework.NHibernate
{
    public class SessionManager
    {
        public static ISessionFactory SessionFactory { get; private set; }
        private static Configuration config;

        public static void Initalize(string connectString, IEnumerable<Assembly> mapperAssemblies, string configFile = "hibernate.config")
        {
            Check.Argument.IsNotEmpty(connectString, "connectString");

            config = new Configuration().Configure(configFile);

            config.DataBaseIntegration(db =>
            {
                db.ConnectionString = connectString;
            });

            var mapper = new ModelMapper();

            foreach (var assembly in mapperAssemblies)
            {
                mapper.AddMappings(assembly.GetTypes().Where(it => it.Name.EndsWith("Map")));
            }

            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            SessionFactory = config.BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void CreateTables()
        {
            //config tables
            SchemaMetadataUpdater.QuoteTableAndColumns(config);
            //此处初始化数据库，只能用在测试环境，稍候应提取独立方法用于创建数据库，避免在生产环境使用，导致生产环境数据被删除  
            //generator database
            new SchemaExport(config).Create(false, true);
        }
    }
}
