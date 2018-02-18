using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System.Linq;
using System.Collections.Generic;

namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class DeleteManyTests
    {
        [TestMethod]
        public void DeleteMany_DeleteMultipleDocuments()
        {
            TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"try {
                       var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                       db.orders.deleteMany( { ""amount"" :25 } );
                    } catch (e) {
                       print(e);
                        }");

            var acknowledged = (MongoDB.Driver.DeleteResult.Acknowledged)results.Value;
            Assert.AreEqual(true, acknowledged.IsAcknowledged);
            Assert.AreEqual(2, acknowledged.DeletedCount);
        }

        [TestMethod]
        public void DeleteMany_WithWriteConcern()
        {
            TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.orders.deleteMany(
                    { ""cust_id"" : ""abc1"" },
                    { w: ""majority"", wtimeout: 10 });");

            var acknowledged = (MongoDB.Driver.DeleteResult.Acknowledged)results.Value;
            Assert.AreEqual(true, acknowledged.IsAcknowledged);
            Assert.AreEqual(2, acknowledged.DeletedCount);
        }
    }
}

