﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class AggregateTests
    {
        [TestMethod]
        public void Aggregate_GroupByCalculateSum()
        {
            TestData.TestData.Prepare("orders", "TestData/orders.json");
            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.orders.aggregate([
                     { $match: { status: ""A"" } },
                     { $group: { _id: ""$cust_id"", total: { $sum: ""$amount"" } } },
                     { $sort: { total: -1 } }
                   ]);
                ");

            var items = ((IEnumerable<object>)results.Value).ToList();
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void Aggregate_PerformLargeSortOperationWithExternalSor()
        {
            TestData.TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient(this.mongoConnectionString);
                db.orders.aggregate(
                    [
                        { $project : { cusip: 1, date: 1, price: 1, _id: 0 } },
                        { $sort : { cusip : 1, date: 1 } }
                    ],
                    {
                        allowDiskUse: true
                    }
                   )");
            var items = ((IEnumerable<object>)results.Value).ToList();
            Assert.AreEqual(5, items.Count);
        }

        [TestMethod]
        public void Aggregate_SpecifyInitialBatchSize()
        {
            TestData.TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
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
            var items = ((IEnumerable<object>)results.Value).ToList();
            Assert.AreEqual(2, items.Count);
        }
    }
}
