using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System.Linq;
using System.Collections.Generic;


namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class EvaludatedTests
    {
        [TestMethod]
        public void Count_CountAllDocumentsInCollection()
        {
            TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.orders.aggregate([
                     { $match: { status: ""A"" } },
                     { $group: { _id: ""$cust_id"", total: { $sum: ""$amount"" } } },
                     { $sort: { total: -1 } }
                   ])");

            var items = ((EvaluatedCursor<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void Aggregate_PerformLargeSortOperationWithExternalSor()
        {
            TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.orders.aggregate(
                    [
                        { $project : { cusip: 1, date: 1, price: 1, _id: 0 } },
                        { $sort : { cusip : 1, date: 1 } }
                    ],
                    {
                        allowDiskUse: true
                    }
                   )");
            var items = ((EvaluatedCursor<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(5, items.Count);
        }

        [TestMethod]
        public void Aggregate_SpecifyInitialBatchSize()
        {
            TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.orders.aggregate(
                        [
                        { $match: { status: ""A"" } },
                        { $group: { _id: ""$cust_id"", total: { $sum: ""$amount"" } } },
                        { $sort: { total: -1 } },
                        { $limit: 2 }
                        ],
                        {
                            cursor: { batchSize: 0 }
                        }
                   )");
            var items = ((EvaluatedCursor<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(2, items.Count);
        }
    }
}
