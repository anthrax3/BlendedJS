using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class UpdateManyTests
    {
        [TestMethod]
        public void UpdateMany_SetOneField()
        {
            TestData.Prepare("restaurant", "TestData/restaurant2.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
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
            TestData.Prepare("restaurant", "TestData/restaurant2.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
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
            TestData.Prepare("inspectors", "TestData/inspectors.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
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
