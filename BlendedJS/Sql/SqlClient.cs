using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BlendedJS.Sql
{
    public class SqlClient : Dictionary<string, object>
    {
        private object _provider;
        private object _connectionString;

        public SqlClient(object provider, object connectionString)
        {
            _provider = provider;
            _connectionString = connectionString;
        }
        public object query(object query)
        {
            using (IDbConnection connection = new SqlConnectionFactory().CreateConnection(_provider.ToStringOrDefault(), _connectionString.ToStringOrDefault()))
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = query.ToStringOrDefault();
                connection.Open();
                List<object> items = new List<object>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> item = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object value = reader.GetValue(i);
                            if (value is DBNull)
                                value = null;
                            
                            item[reader.GetName(i)] = value;
                        }
                        items.Add(item);
                    }
                }
                return items.ToArray();
            }
        }
    }
}
