using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlendedJS.Mongo.Tests
{
    public class TestData
    {
        public static string MongoConnectionString = "mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40";
        public static string MongoDatabase = "heroku_rgzrhk40";

        public TestData()
        {

        }

        public static void Prepare(string collectionName, string pathToDocuments)
        {
            var collection = GetCollection(collectionName);
            collection.DeleteMany("{}".ToBsonDocument());
            IList<string> documents = GetDocuments(pathToDocuments);
            foreach(string document in documents)
            {
                collection.InsertOne(document.ToBsonDocument());
            }
        }

        public static IList<string> GetDocuments(string pathToDocuments)
        {
            IList<string> documents = new List<string>();
            StringBuilder document = new StringBuilder();

            string[] lines = File.ReadAllLines("Mongo/" + pathToDocuments);
            foreach(string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    documents.Add(document.ToString());
                    document.Clear();
                }
                document.AppendLine(line);
            }
            if (document.Length > 0)
                documents.Add(document.ToString());

            return documents;
        }

        public static IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            MongoDB.Driver.MongoClient client = new MongoDB.Driver.MongoClient(MongoConnectionString);
            var database = client.GetDatabase(MongoDatabase);
            var collection = database.GetCollection<MongoDB.Bson.BsonDocument>(collectionName);
            return collection;
        }
    }
}
