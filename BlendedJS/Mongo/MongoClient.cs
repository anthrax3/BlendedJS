using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using BlendedJS.Mongo;

namespace BlendedJS.Mongo
{
    public class MongoClient : JsObject
    {
        private MongoDB.Driver.MongoClient _client;
        private object _connectionString;
        private Console _console = BlendedJSEngine.Console.Value;

        public MongoClient(object connectionStringOrOptions)
        {
            BlendedJSEngine.Clients.Value.Add(this);
            _connectionString = connectionStringOrOptions is string s ? s : connectionStringOrOptions.GetProperty("connectionString").ToStringOrDefault();
            _client = new MongoDB.Driver.MongoClient(_connectionString.ToStringOrDefault());
            string databaseName = MongoUrl.Create(_connectionString.ToStringOrDefault()).DatabaseName;
            foreach (var collectionName in _client.GetDatabase(databaseName).ListCollections().ToList())
            {
                string name = collectionName.GetValue("name").ToString();
                var collection = _client.GetDatabase(databaseName).GetCollection<BsonDocument>(name);
                this[name] = new MongoCollection(name, collection, _console);
            }
        }
    }
}