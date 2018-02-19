using System.Collections.Generic;
using System.Linq;
using BlendedJS.Mongo.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class MapReduceTests
    {
        [TestMethod]
        public void FindOneAndDelete_DeleteDocument()
        {
            TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
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
