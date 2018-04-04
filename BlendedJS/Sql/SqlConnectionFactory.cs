using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using IBM.Data.DB2.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace BlendedJS.Sql
{
    public class SqlConnectionFactory
    {
        public  IDbConnection CreateConnection(string provider, string connectionString)
        {
            if (string.IsNullOrEmpty(provider) || string.IsNullOrEmpty(connectionString))
                throw new Exception("provider and connectionString have to be specified. You can do it with client initialization. Example:\nvar sqlClient = new  SqlClient({provider:'MySql',connectionString:'SERVER=...;DATABASE=...;UID=...;PASSWORD=...;'}); \n\nor:\n var sqlClient = new  SqlClient({provider:'MySql',server:'...',database:'...',user:'...',password:'...'});");

            IDbConnection connection = null;
            if (provider.SafeEquals("MySql") || provider.SafeEquals("MariaDb"))
                connection = new MySqlConnection(connectionString);
            if (provider.SafeEquals("Sqlite"))
                connection = new SqliteConnection(connectionString);
            if (provider.SafeEquals("SqlServer"))
                connection = new SqlConnection(connectionString);
            if (provider.SafeEquals("PostgreSQL") || provider.SafeEquals("Postgres"))
                connection = new NpgsqlConnection(connectionString);
            if (provider.SafeEquals("Oracle"))
                connection = new OracleConnection(connectionString);
            if (provider.SafeEquals("Odbc"))
                connection = new OdbcConnection(connectionString);
            if (provider.SafeEquals("DB2"))
                connection = new DB2Connection(connectionString);

            if (connection == null)
                throw new Exception("Not supported provider: " + provider + ". Supported providers: Sqlite,SqlServer,MySql,MariaDb,PostgreSQL,Oracle,Odbc. Example:\nvar sqlClient = new  SqlClient({provider:'MySql',connectionString:'SERVER=...;DATABASE=...;UID=...;PASSWORD=...;'}); \n\nor:\n var sqlClient = new  SqlClient({provider:'MySql',server:'...',database:'...',user:'...',password:'...'});");

            return connection;
        }
    }
}