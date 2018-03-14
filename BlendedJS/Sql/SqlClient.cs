using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Npgsql;

namespace BlendedJS.Sql
{
    public class SqlClient : BaseObject
    {
        private object _provider;
        private object _connectionString;

        public SqlClient(object options)
        {
            BlendedJSEngine.Clients.Add(this);
            _provider = options.GetProperty("provider").ToStringOrDefault();
            _connectionString = options.GetProperty("connectionString").ToStringOrDefault();
        }
        public object query(object sqlOrOptions)
        {
            string query = sqlOrOptions.GetProperty("sql").ToStringOrDefault(sqlOrOptions.ToStringOrDefault());
            var parameters = sqlOrOptions.GetProperty("parameters") as IDictionary<string, object>;

            using (IDbConnection connection = new SqlConnectionFactory().CreateConnection(_provider.ToStringOrDefault(), _connectionString.ToStringOrDefault()))
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                AddParameters(command, parameters);
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("Cannot connect to the database. " + ex.Message, ex);
                }

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.RecordsAffected >= 0)
                            return reader.RecordsAffected;
                        return
                            ReadAllRecords(reader).ToArray();
                    }
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
        }

        public List<object> ReadAllRecords(IDataReader reader)
        {
            List<object> items = new List<object>();
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

                items.Add(item);
            }

            return items;
        }

        public void AddParameters(IDbCommand command, IDictionary<string,object> parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = "@" + parameter.Key;
                    dbParameter.Value = parameter.Value ?? DBNull.Value;
                    command.Parameters.Add(dbParameter);
                }
            }
        }
    }
}
