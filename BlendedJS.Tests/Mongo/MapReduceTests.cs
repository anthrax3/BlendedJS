using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using BlendedJS.Mongo;
using BlendedJS.Mongo.Tests;

namespace BlendedJS.Tests.Mongodb
{
    [TestClass]
    public class MapReduceTests
    {
        [TestMethod]
        public void FindOneAndDelete_DeleteDocument()
        {
            TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    var mapFunction1 = 'function() { emit(this.cust_id, this.price); }';
                    var reduceFunction1 = 'function(keyCustId, valuesPrices) { return Array.sum(valuesPrices); }';
                    db.orders.mapReduce(
                         mapFunction1,
                         reduceFunction1,
                         { out: ""map_reduce_example"" }
                       )
                ");
            var items = ((IEnumerable<object>)results.Value).ToList();

        }
    }
}
