using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra;

namespace BlendedJS.Cassandra
{
    public class CassandraClient : JsObject, IDisposable
    {
        private Cluster _cluster;
        private ISession _session;
        public CassandraClient(object options)
        {
            BlendedJSEngine.Clients.Value.Add(this);

            List<string> addresses = new List<string>();
            object host = options.GetProperty("host");
            if (host != null)
            {
                if (host is object[])
                    addresses.AddRange(((object[])host).Select(x => x.ToString()));
                else
                    addresses.Add(host.ToString());
            }
            object hosts = options.GetProperty("hosts");
            if (hosts != null)
            {
                if (hosts is object[])
                    addresses.AddRange(((object[])hosts).Select(x => x.ToString()));
                else
                    addresses.Add(hosts.ToString());
            }

            if (addresses.Count == 0)
                throw new Exception("host is required and was not provided.");

            _cluster = Cluster.Builder()
                .AddContactPoints(addresses)
                .Build();
        }

        public void connect()
        {
            if (_session == null)
                _session = _cluster.Connect();
        }

        public void connect(object keyspace)
        {
            if (_session == null)
                _session = _cluster.Connect(keyspace.ToStringOrDefault());
        }

        public void disconnect()
        {
            _session.Dispose();
            _session = null;
        }

        public object execute(object query)
        {
            connect();
            var rows = _session.Execute(query.ToStringOrDefault());
            return rows.Select(x =>
            {
                JsObject obj = new JsObject();
                foreach (var column in rows.Columns)
                    obj[column.Name] = x[column.Index];
                return obj;
            }).ToArray();
        }

        public object execute(object query, object[] parameters)
        {
            PreparedStatement statement = _session.Prepare(query.ToStringOrDefault());
            var boundStatement = statement.Bind(parameters.MapDotNetTypes());
            var rows = _session.Execute(boundStatement);
            return rows.Select(x =>
            {
                JsObject obj = new JsObject();
                foreach (var column in rows.Columns)
                    obj[column.Name] = x[column.Index];
                return obj;
            }).ToArray();
        }

        public void Dispose()
        {
            _session?.Dispose();
            _cluster?.Dispose();
        }
    }
}
