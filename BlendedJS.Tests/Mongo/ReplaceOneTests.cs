using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class ReplaceOneTests
    {
        [TestMethod]
        public void ReplaceOne_SetOneField()
        {
            TestData.Prepare("restaurant", "TestData/restaurant.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.restaurant.replaceOne(
                      { ""name"" : ""Central Perk Cafe"" },
                      { ""name"" : ""Central Pork Cafe"", ""Borough"" : ""Manhattan"" }
                   );
                ");

            var result = (ReplaceOneResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)1, result.matchedCount);
            Assert.AreEqual((Int64)1, result.modifiedCount);
        }

        [TestMethod]
        public void ReplaceOne_Upsert()
        {
            TestData.Prepare("restaurant", "TestData/restaurant.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.restaurant.replaceOne(
                      { ""name"" : ""Pizza Rat's Pizzaria"" },
                      { ""_id"": 4, ""name"" : ""Pizza Rat's Pizzaria"", ""Borough"" : ""Manhattan"", ""violations"" : 8 },
                      { upsert: true }
                   );
            ");

            var result = (ReplaceOneResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)0, result.matchedCount);
            Assert.AreEqual((Int64)0, result.modifiedCount);
            Assert.AreEqual(MongoDB.Bson.BsonDouble.Create(4), result.upsertedId);
        }
    }
}
