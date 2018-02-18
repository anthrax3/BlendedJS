using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class UpdateOneTests
    {
        [TestMethod]
        public void UpdateOne_SetOneField()
        {
            TestData.Prepare("restaurant", "TestData/restaurant.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                   db.restaurant.updateOne(
                      { ""name"" : ""Central Perk Cafe"" },
                      { $set: { ""violations"" : 3 } }
                   );
        ");

            var result = (UpdateResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)1, result.matchedCount);
            Assert.AreEqual((Int64)1, result.modifiedCount);
            //{ "acknowledged" : true, "matchedCount" : 1, "modifiedCount" : 1 }
        }

        [TestMethod]
        public void UpdateOne_NoMatches()
        {
            TestData.Prepare("restaurant", "TestData/restaurant.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.restaurant.updateOne(
                  { ""name"" : ""Not existing place"" },
                  { $set: { ""violations"" : 3 } }
               );
        ");

            var result = (UpdateResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)0, result.matchedCount);
            Assert.AreEqual((Int64)0, result.modifiedCount);
        }

        [TestMethod]
        public void UpdateOne_UpsertWithId()
        {
            TestData.Prepare("restaurant", "TestData/restaurant.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.restaurant.updateOne(
                      { ""name"" : ""Pizza Rat's Pizzaria"" },
                      { $set: { ""_id"" : 4, ""violations"" : 7, ""borough"" : ""Manhattan"" } },
                      { upsert: true }
                   );
                ");

            var result = (UpdateResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)0, result.matchedCount);
            Assert.AreEqual((Int64)0, result.modifiedCount);
            Assert.AreEqual(MongoDB.Bson.BsonDouble.Create(4), result.upsertedId);
        }

        [TestMethod]
        public void UpdateOne_UpsertAndGenerateId()
        {
            TestData.Prepare("restaurant", "TestData/restaurant.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                   db.restaurant.updateOne(
                      { ""violations"" : { $gt: 10} },
                      { $set: { ""Closed"" : true } },
                      { upsert: true }
                   );
            ");

            var result = (UpdateResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)0, result.matchedCount);
            Assert.AreEqual((Int64)0, result.modifiedCount);
            Assert.IsTrue(result.upsertedId is BsonObjectId);
        }
    }
}
