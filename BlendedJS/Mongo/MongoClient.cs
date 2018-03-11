using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using BlendedJS.Mongo;

namespace BlendedJS.Mongo
{
    public class MongoClient : BaseObject
    {
        private MongoDB.Driver.MongoClient _client;
        private object _connectionString;
        private Console _console = BlendedJSEngine.Console;

        public MongoClient(object connectionStringOrOptions)
        {
            _connectionString = connectionStringOrOptions is string s ? s : connectionStringOrOptions.GetProperty("connectionString").ToStringOrDefault();
            _client = new MongoDB.Driver.MongoClient(_connectionString.ToStringOrDefault());

            foreach (var collectionName in _client.GetDatabase("heroku_rgzrhk40").ListCollections().ToList())
            {
                string name = collectionName.GetValue("name").ToString();
                var collection = _client.GetDatabase("heroku_rgzrhk40").GetCollection<BsonDocument>(name);
                this[name] = new MongoCollection(name, collection, _console);
            }
        }
    }
}