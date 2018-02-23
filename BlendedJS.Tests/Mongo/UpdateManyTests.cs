using System;
using BlendedJS.Mongo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class UpdateManyTests
    {
        [TestMethod]
        public void UpdateMany_SetOneField()
        {
            TestData.TestData.Prepare("restaurant", "TestData/restaurant2.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                   db.restaurant.updateMany(
                      { violations: { $gt: 4 } },
                      { $set: { ""Review"" : true } }
                   );
                ");

            var result = (UpdateResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)2, result.matchedCount);
            Assert.AreEqual((Int64)2, result.modifiedCount);
        }

        [TestMethod]
        public void UpdateMany_NoMatches()
        {
            TestData.TestData.Prepare("restaurant", "TestData/restaurant2.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                   db.restaurant.updateMany(
                      { violations: { $gt: 100 } },
                      { $set: { ""Review"" : true } }
                   );
                ");

            var result = (UpdateResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual((Int64)0, result.matchedCount);
            Assert.AreEqual((Int64)0, result.modifiedCount);
        }

        [TestMethod]
        public void UpdateMany_UpsertAndGenerateId()
        {
            TestData.TestData.Prepare("inspectors", "TestData/inspectors.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.inspectors.updateMany(
                      { ""Sector"" : { $gt : 4 }, ""inspector"" : ""R.Coltrane"" },
                      { $set: { ""Patrolling"" : false } },
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
