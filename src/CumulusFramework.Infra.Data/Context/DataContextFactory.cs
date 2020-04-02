using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using CumulusFramework.Domain.Entity;
using CumulusFramework.Domain.Enumeration;
using NHibernate;
using System;
using System.Configuration;
using System.Reflection;

namespace CumulusFramework.Infra.Data.Context
{
    public static class DataContextFactory
    {
        private const String MAPPING_KEY = "AssemblyWithMapping";
        private const String SECONDMAPPING_KEY = "SecondAssemblyWithMapping";
        private const String THIRDMAPPING_KEY = "ThirdAssemblyWithMapping";
        private const String LOCALDBPATH_KEY = "LocalDatabasePath";

        public static ISessionFactory GetContextForConnection(DBEnvironment environment)
        {
            ISessionFactory factory = null;
            var dbms = (DBMSTypeEnum)environment.DBMS;

            try
            {
                switch (dbms)
                {
                    case DBMSTypeEnum.Firebird:
                        factory = GetFirebirdSessionFactory();
                        break;
                    case DBMSTypeEnum.MySQL:
                        factory = GetMySQLSessionFactory(environment);
                        break;
                    case DBMSTypeEnum.Oracle:
                        factory = GetOracleSessionFactory(environment);
                        break;
                    case DBMSTypeEnum.PostgreSQL:
                        factory = GetPostgreSQLSessionFactory(environment);
                        break;
                    case DBMSTypeEnum.SqlServer:
                        factory = GetSQLServerSessionFactory(environment);
                        break;
                    default:
                        factory = null;
                        break;
                }

                return factory;
            }
            catch (Exception ex)
            {
                String log = ex.Message;
                throw;
            }
        }

        private static ISessionFactory GetSQLServerSessionFactory(DBEnvironment _environment)
        {
            ISessionFactory factory = null;

            String connectionString = DatabaseUtil.GetConnectionStringByEnvironment(_environment);

            Assembly mapping = GetAssemblyWithMapping();

            MsSqlConfiguration configurer = GetMSMsSqlConfigurationByEnvironment(_environment);

            FluentConfiguration config = Fluently.Configure()
            .Database(configurer)
            .CurrentSessionContext("context")
            .Cache(
                c => c.UseQueryCache().UseSecondLevelCache()
            )
            .Mappings(m => m.FluentMappings.AddFromAssembly(mapping)
            );

            Assembly secondMapping = GetSecondAssemblyWithMapping();
            Assembly thirdMapping = GetThirdAssemblyWithMapping();

            factory = config.BuildSessionFactory();

            return factory;
        }

        private static MsSqlConfiguration GetMSMsSqlConfigurationByEnvironment(DBEnvironment _environment)
        {
            var connectionString = DatabaseUtil.GetConnectionStringByEnvironment(_environment);

            MsSqlConfiguration config = null;

            var version = (DBMSSQLServerVersion)_environment.DBMSVersion;

            switch (version)
            {
                case DBMSSQLServerVersion.Versao2005:
                    config = MsSqlConfiguration.MsSql2005.ConnectionString(connectionString);
                    break;
                case DBMSSQLServerVersion.Versao2008:
                    config = MsSqlConfiguration.MsSql2008.ConnectionString(connectionString);
                    break;
                case DBMSSQLServerVersion.Versao2012:
                    config = MsSqlConfiguration.MsSql2012.ConnectionString(connectionString);
                    break;
                default:
                    config = MsSqlConfiguration.MsSql2008.ConnectionString(connectionString);
                    break;
            }

            return config;
        }

        private static ISessionFactory GetPostgreSQLSessionFactory(DBEnvironment _environment)
        {
            ISessionFactory factory = null;

            String connectionString = DatabaseUtil.GetConnectionStringByEnvironment(_environment);

            Assembly mapping = GetAssemblyWithMapping();

            FluentConfiguration config = Fluently.Configure()
            .Database(
            PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
            .CurrentSessionContext("context")
            .Cache(
                c => c.UseQueryCache().UseSecondLevelCache()
            )
            .Mappings(m => m.FluentMappings.AddFromAssembly(mapping)
            );

            factory = config.BuildSessionFactory();

            return factory;
        }

        private static ISessionFactory GetOracleSessionFactory(DBEnvironment _environment)
        {
            ISessionFactory factory = null;

            String connectionString = DatabaseUtil.GetConnectionStringByEnvironment(_environment);

            Assembly mapping = GetAssemblyWithMapping();

            FluentConfiguration config = Fluently.Configure()
            .Database(
            OracleClientConfiguration.Oracle10.ConnectionString(connectionString))
            .CurrentSessionContext("context")
            .Cache(
                c => c.UseQueryCache().UseSecondLevelCache()
            )
            .Mappings(m => m.FluentMappings.AddFromAssembly(mapping)
            );

            factory = config.BuildSessionFactory();

            return factory;
        }

        private static String GetLocalDatabasePath()
        {
            var path = ConfigurationManager.AppSettings[LOCALDBPATH_KEY];

            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentException("GetLocalDatabasePath");
            }
            return path;
        }

        private static Assembly GetSecondAssemblyWithMapping()
        {
            var assemblyName = ConfigurationManager.AppSettings[SECONDMAPPING_KEY];
            if (String.IsNullOrEmpty(assemblyName))
            {
                return null;
            }
            return Assembly.LoadFrom(assemblyName.EndsWith(".dll") ? assemblyName : assemblyName + ".dll");
        }

        private static Assembly GetThirdAssemblyWithMapping()
        {
            var assemblyName = ConfigurationManager.AppSettings[THIRDMAPPING_KEY];
            if (String.IsNullOrEmpty(assemblyName))
            {
                return null;
            }
            return Assembly.LoadFrom(assemblyName.EndsWith(".dll") ? assemblyName : assemblyName + ".dll");
        }

        private static Assembly GetAssemblyWithMapping()
        {
            var assemblyName = ConfigurationManager.AppSettings[MAPPING_KEY];
            if (String.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentException("GetAssemblyWithMapping");
            }
            return Assembly.LoadFrom(assemblyName.EndsWith(".dll") ? assemblyName : assemblyName + ".dll");
        }

        private static ISessionFactory GetMySQLSessionFactory(DBEnvironment _environment)
        {
            ISessionFactory factory = null;

            String connectionString = DatabaseUtil.GetConnectionStringByEnvironment(_environment);

            FluentConfiguration config = Fluently.Configure()
            .Database(
            //MySQLConfiguration.Standard.ConnectionString(connectionString).ShowSql())
            MySQLConfiguration.Standard.ConnectionString(connectionString))
            .CurrentSessionContext("context")
            .Cache(
                c => c.UseQueryCache().UseSecondLevelCache()
            );

            //Mapeia os assemblies externos primeiro, geralmente o GetAssemblyWithMapping vai ser o assembly de mapeamento
            //do projeto principal
            Assembly secondMapping = GetSecondAssemblyWithMapping();

            if (secondMapping != null)
            {
                config.Mappings(m => m.FluentMappings.AddFromAssembly(secondMapping));
            }

            Assembly thirdMapping = GetThirdAssemblyWithMapping();

            if (thirdMapping != null)
            {
                config.Mappings(m => m.FluentMappings.AddFromAssembly(thirdMapping));
            }

            Assembly mapping = GetAssemblyWithMapping();

            config.Mappings(m => m.FluentMappings.AddFromAssembly(mapping));


            factory = config.BuildSessionFactory();

            return factory;
        }

        private static ISessionFactory GetFirebirdSessionFactory()
        {
            return null;
        }
    }
}
