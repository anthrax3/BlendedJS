using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
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
            return connection;
        }
    }
}