using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class UpdateTests
    {
        [TestMethod]
        public void Update_SpecificFields()
        {
            TestData.Prepare("books", "TestData/books.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.books.update(
                       { _id: 1 },
                       {
                         $inc: { stock: 5 },
                         $set: {
                           item: ""ABC123"",
                           ""info.publisher"": ""2222"",
                           tags: [""software""],
                           ""ratings.1"": { by: ""xyz"", rating: 3 }
                            }
                        }
                    )
                ");

            var result = (UpdateResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)1, result.matchedCount);
            Assert.AreEqual((Int64)1, result.modifiedCount);
            //{ "acknowledged" : true, "matchedCount" : 1, "modifiedCount" : 1 }
        }

        [TestMethod]
        public void Update_ReplaceDocument()
        {
            TestData.Prepare("books", "TestData/books.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.books.update(
                       { item: ""XYZ123"" },
                       {
                        item: ""XYZ123"",
                         stock: 10,
                         info: { publisher: ""2255"", pages: 150 },
                         tags: [""baking"", ""cooking""]
                       }
                    )
                ");

            var result = (ReplaceOneResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)1, result.matchedCount);
            Assert.AreEqual((Int64)1, result.modifiedCount);
            //{ "acknowledged" : true, "matchedCount" : 1, "modifiedCount" : 1 }
        }

        [TestMethod]
        public void Update_Upsert()
        {
            TestData.Prepare("books", "TestData/books.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.books.update(
                       { item: ""ZZZ135"" },
                       {
                         item: ""ZZZ135"",
                         stock: 5,
                         tags: [""database""]
                        },
                        { upsert: true }
                    )
                ");

            var result = (ReplaceOneResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)0, result.matchedCount);
            Assert.AreEqual((Int64)0, result.modifiedCount);
            Assert.IsTrue(result.upsertedId is BsonObjectId);
            //{ "acknowledged" : true, "matchedCount" : 1, "modifiedCount" : 1 }
        }

        [TestMethod]
        public void Update_Multi()
        {
            TestData.Prepare("books", "TestData/books.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.books.update(
                       { stock: { $lte: 10 } },
                       { $set: { reorder: true } },
                       { multi: true }
                    )
                ");

            var result = (UpdateResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)1, result.matchedCount);
            Assert.AreEqual((Int64)1, result.modifiedCount);
            //{ "acknowledged" : true, "matchedCount" : 1, "modifiedCount" : 1 }
        }

        [TestMethod]
        public void Update_UpsertMulti()
        {
            TestData.Prepare("books", "TestData/books.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.books.update(
                       { item: ""EFG222"" },
                       { $set: { reorder: false, tags: [""literature"", ""translated""] } },
                       { upsert: true, multi: true }
                    )
                ");

            var result = (UpdateResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)2, result.matchedCount);
            Assert.AreEqual((Int64)2, result.modifiedCount);
        }


        
    }
}
