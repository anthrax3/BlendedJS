using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using BlendedJS.Mongo;

namespace BlendedJS.Mongo
{
    public class MongoClient : Dictionary<string, object>
    {
        private MongoDB.Driver.MongoClient _client;
        private object _connectionString;
        private IConsole console = BlendedJSEngine.console;

        public MongoClient(object connectionString)
        {
            _connectionString = connectionString;
            _client = new MongoDB.Driver.MongoClient(connectionString.ToStringOrDefault());

            foreach (var collectionName in _client.GetDatabase("heroku_rgzrhk40").ListCollections().ToList())
            {
                string name = collectionName.GetValue("name").ToString();
                var collection = _client.GetDatabase("heroku_rgzrhk40").GetCollection<BsonDocument>(name);
                this[name] = new MongoCollection(name, collection, console);
            }
        }
    }
}