using CumulusFramework.Domain.Entity;
using CumulusFramework.Domain.Enumeration;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CumulusFramework.Infra.Data
{
    public static class DatabaseUtil
    {
        private static String GetMySQLConnectionStringByEnvironment(DBEnvironment _environment)
        {
            return String.Format(@"Server = {0}; Port = {1}; Database = {2}; Uid = {3};Pwd = {4};", _environment.Hostname, _environment.Port, _environment.Instance, _environment.User, _environment.Pass);
        }

        private static String GetSQLServerConnectionStringByEnvironment(DBEnvironment _environment)
        {
            return String.Format(@"Server={0};Database={1};User Id={2};
                                Password={3};", _environment.Hostname, _environment.Instance, _environment.User, _environment.Pass);
        }

        private static String GetPostgreSQLConnectionStringByEnvironment(DBEnvironment _environment)
        {
            return String.Format(@"User ID={0};Password={1};Host={2};Port={3};Database={4};
	                            Pooling=true;Connection Lifetime=0;", _environment.User, _environment.Pass, _environment.Hostname, _environment.Port, _environment.Instance);
        }

        private static String GetFirebirdConnectionStringByEnvironment(DBEnvironment _environment)
        {
            return String.Format(@"User={0};Password={1};Database={2};DataSource={3};
                                    Port={4};Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;
                                    MinPoolSize=0;MaxPoolSize=50;ServerType=0;", _environment.User, _environment.Pass, _environment.Instance, _environment.Hostname, _environment.Port);
        }

        private static String GetOracleConnectionStringByEnvironment(DBEnvironment _environment)
        {
            return String.Format(@"Data Source={0};User ID={1};Password={2}", _environment.Hostname + ":" + _environment.Port + "/" + _environment.Instance, _environment.User, _environment.Pass);
        }
        
        public static String GetConnectionStringByEnvironment(DBEnvironment _environment)
        {
            String  connectionString = String.Empty;

            var dbms = (DBMSTypeEnum)_environment.DBMS;

            try
            {
                switch (dbms)
                {
                    case DBMSTypeEnum.Firebird:
                        connectionString = GetFirebirdConnectionStringByEnvironment(_environment);
                        break;
                    case DBMSTypeEnum.MySQL:
                        connectionString = GetMySQLConnectionStringByEnvironment(_environment);
                        break;
                    case DBMSTypeEnum.Oracle:
                        connectionString = GetOracleConnectionStringByEnvironment(_environment);
                        break;
                    case DBMSTypeEnum.PostgreSQL:
                        connectionString = GetPostgreSQLConnectionStringByEnvironment(_environment);
                        break;
                    case DBMSTypeEnum.SqlServer:
                        connectionString = GetSQLServerConnectionStringByEnvironment(_environment);
                        break;
                    default:
                        connectionString = String.Empty;
                        break;
                }

                return connectionString;
            }
            catch (Exception ex)
            {
                String log = ex.Message;
                throw;
            }
        }

        private static MySql.Data.MySqlClient.MySqlConnection GetMySQLConnection(DBEnvironment _environment)
        {
            var connectionString = GetMySQLConnectionStringByEnvironment(_environment);
            try
            {
                return new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static SqlConnection GetSQLServerConnection(DBEnvironment _environment)
        {
            var connectionString = GetSQLServerConnectionStringByEnvironment(_environment);
            try
            {
                return new SqlConnection(connectionString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Npgsql.NpgsqlConnection GetPostgreSQLConnection(DBEnvironment _environment)
        {
            var connectionString = GetPostgreSQLConnectionStringByEnvironment(_environment);
            try
            {
                return new Npgsql.NpgsqlConnection(connectionString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static FirebirdSql.Data.FirebirdClient.FbConnection GetFirebirdConnection(DBEnvironment _environment)
        {
            var connectionString = GetFirebirdConnectionStringByEnvironment(_environment);
            try
            {
                return new FirebirdSql.Data.FirebirdClient.FbConnection(connectionString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IDbConnection GetConnection(DBEnvironment _environment)
        {
            DbConnection connection = null;

            var dbms = (DBMSTypeEnum)_environment.DBMS;

            try
            {
                switch (dbms)
                {
                    case DBMSTypeEnum.Firebird:
                        connection = GetFirebirdConnection(_environment);
                        break;
                    case DBMSTypeEnum.MySQL:
                        connection = GetMySQLConnection(_environment);
                        break;
                    case DBMSTypeEnum.Oracle:
                        connection = GetOracleConnection(_environment);
                        break;
                    case DBMSTypeEnum.PostgreSQL:
                        connection = GetPostgreSQLConnection(_environment);
                        break;
                    case DBMSTypeEnum.SqlServer:
                        connection = GetSQLServerConnection(_environment);
                        break;
                    default:
                        connection = null;
                        break;
                }

                return connection;
            }
            catch (Exception ex)
            {
                String log = ex.Message;
                throw;
            }
        }

        private static Oracle.ManagedDataAccess.Client.OracleConnection GetOracleConnection(DBEnvironment _environment)
        {
            var connectionString = GetOracleConnectionStringByEnvironment(_environment);
            try
            {
                return new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Boolean ConnectToMySQL(DBEnvironment _environment)
        {
            try
            {
                var connectionString = GetMySQLConnectionStringByEnvironment(_environment);
                using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Boolean ConnectToSQLServer(DBEnvironment _environment)
        {
            try
            {
                var connectionString = GetSQLServerConnectionStringByEnvironment(_environment);
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Boolean ConnectToPostgreSQL(DBEnvironment _environment)
        {
            try
            {
                var connectionString = GetPostgreSQLConnectionStringByEnvironment(_environment);
                using (var connection = new Npgsql.NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Boolean ConnectToFirebird(DBEnvironment _environment)
        {
            try
            {
                var connectionString = GetFirebirdConnectionStringByEnvironment(_environment);
                using (var connection = new FirebirdSql.Data.FirebirdClient.FbConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Boolean ConnectToOracle(DBEnvironment _environment)
        {
            try
            {
                var connectionString = GetOracleConnectionStringByEnvironment(_environment);
                using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
