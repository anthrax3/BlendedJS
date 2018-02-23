using System;
using BlendedJS.Mongo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class ReplaceOneTests
    {
        [TestMethod]
        public void ReplaceOne_SetOneField()
        {
            TestData.TestData.Prepare("restaurant", "TestData/restaurant.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
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
            TestData.TestData.Prepare("restaurant", "TestData/restaurant.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
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
