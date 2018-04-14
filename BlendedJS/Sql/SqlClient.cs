using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Npgsql;

namespace BlendedJS.Sql
{
    public class SqlClient : BaseObject, IDisposable
    {
        private object _provider;
        private object _connectionString;
        private IDbConnection _connection;
        private IDbTransaction _dbTransaction;
        private List<IDbCommand> _dbCommands = new List<IDbCommand>();

        public SqlClient(){ }
        public SqlClient(object options, object ar2) : this(options) { }
        public SqlClient(object options, object ar2, object arg3) : this(options) { }
        public SqlClient(object options, object ar2, object arg3, object arg4) : this(options) { }
        public SqlClient(object options, object ar2, object arg3, object arg4, object arg5) : this(options) { }

        public SqlClient(object options)
        {
            BlendedJSEngine.Clients.Value.Add(this);

            _provider = options.GetProperty("provider").ToStringOrDefault();
            _connectionString = options.GetProperty("connectionString").ToStringOrDefault();

            string connectionUrl = options.GetProperty("connectionUrl").ToStringOrDefault();
            if (string.IsNullOrEmpty(connectionUrl) == false)
            {
                Uri parsedUrl = new Uri(connectionUrl);
                _provider = parsedUrl.Scheme;
                _connectionString += ("SERVER=" + parsedUrl.Host + ";");
                _connectionString += ("DATABASE=" + parsedUrl.LocalPath.Substring(1) + ";");
                if (string.IsNullOrEmpty(parsedUrl.UserInfo) == false)
                {
                    string[] userCredentials = parsedUrl.UserInfo.Split(new[] { ':' });
                    if (userCredentials.Length == 2)
                    {
                        _connectionString += ("UID=" + userCredentials[0] + ";");
                        _connectionString += ("PASSWORD=" + userCredentials[1] + ";");
                    }
                }
            }

            string driver = options.GetProperty("driver").ToStringOrDefault();
            if (string.IsNullOrEmpty(driver) == false)
                _connectionString += ("DRIVER=" + driver + ";");
            string server = options.GetProperty("server").ToStringOrDefault();
            if (string.IsNullOrEmpty(server) == false)
                _connectionString +=("SERVER=" + server + ";");
            string database = options.GetProperty("database").ToStringOrDefault();
            if (string.IsNullOrEmpty(database) == false)
                _connectionString += ("DATABASE=" + database + ";");
            string dataSource = options.GetProperty("dataSource").ToStringOrDefault();
            if (string.IsNullOrEmpty(dataSource) == false)
                _connectionString += ("DATA SOURCE=" + dataSource + ";");
            string user = options.GetProperty("user").ToStringOrDefault();
            if (string.IsNullOrEmpty(user) == false)
                _connectionString += ("UID=" + user + ";");
            string password = options.GetProperty("password").ToStringOrDefault();
            if (string.IsNullOrEmpty(password) == false)
                _connectionString += ("PASSWORD=" + password + ";");
        }

        public void connect()
        {
            try
            {
                if (_connection == null)
                    _connection = new SqlConnectionFactory().CreateConnection(_provider.ToStringOrDefault(), _connectionString.ToStringOrDefault());
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot connect to the database. " + ex.Message, ex);
            }
        }

        public void end()
        {
            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }

        public void transaction()
        {
            if (_connection == null)
                throw new Exception("Cannot begin transaction. Connection is closed");

            if (_dbTransaction != null)
                throw new Exception("Cannot begin transaction. Transaction is already started");

            _dbTransaction = _connection.BeginTransaction();
        }

        public void commit()
        {
            if (_dbTransaction == null)
                throw new Exception("Cannot commit transaction. Transaction is not begun");

            _dbTransaction?.Commit();
            _dbTransaction = null;
        }

        public void rollback()
        {
            if (_dbTransaction == null)
                throw new Exception("Cannot rollback transaction. Transaction is not begun");

            _dbTransaction?.Rollback();
            _dbTransaction = null;
        }

        public object query(object sql, object parameters)
        {
            return query(new Object(new Dictionary<string, object>()
            {
                { "sql", sql },
                { "parameters", parameters }
            }));
        }

        public object query(object sqlOrOptions)
        {
            using (var reader = ExecuteReader(sqlOrOptions))
            {
                object[] items = AsEnumerable(reader).ToArray();
                if (items.Length == 0 && reader.RecordsAffected >= 0)
                    return reader.RecordsAffected;
                else
                    return items;
            }
        }

        public object cursor(object sql, object parameters)
        {
            return cursor(new Object(new Dictionary<string, object>()
            {
                { "sql", sql },
                { "parameters", parameters }
            }));
        }

        public object cursor(object sqlOrOptions)
        {
            var reader = ExecuteReader(sqlOrOptions);
            var enumerable = AsEnumerable(reader);
            return new SqlCursor(enumerable, reader);
        }

        private IDataReader ExecuteReader(object sqlOrOptions)
        {
            string query = sqlOrOptions.GetProperty("sql").ToStringOrDefault(sqlOrOptions.ToStringOrDefault());
            var parameters = sqlOrOptions.GetProperty("parameters") as IDictionary<string, object>;

            connect();
            IDbCommand command = _connection.CreateCommand();
            _dbCommands.Add(command);
            command.CommandText = query;
            AddParameters(command, parameters);
            try
            {
                return command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                throw new Exception(string.Format("Cannot run the query. Msg {0}, State {1}, Line {2}. {3}",
                    ex.Number, ex.State, ex.LineNumber, ex.Message), ex);
            }
            catch (MySqlException ex)
            {
                throw new Exception(string.Format("Cannot run the query. Code {0}, State {1}, Number {2}. {3}",
                    ex.Code, ex.SqlState, ex.Number, ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot run the query. " + ex.Message, ex);
            }
        }

        private IEnumerable<object> AsEnumerable(IDataReader reader)
        {
            while (reader.Read())
            {
                var item = new Object();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object value = reader.GetValue(i);
                    if (value is DBNull)
                        value = null;

                    item[reader.GetName(i)] = value;
                }

                yield return item;
            }
        }

        private void AddParameters(IDbCommand command, IDictionary<string,object> parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = "@" + parameter.Key;
                    dbParameter.Value = parameter.Value.MapDotNetType() ?? DBNull.Value;
                    command.Parameters.Add(dbParameter);
                }
            }
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
            _connection?.Dispose();
            _dbCommands?.ForEach(x => x?.Dispose());
        }
    }
}
