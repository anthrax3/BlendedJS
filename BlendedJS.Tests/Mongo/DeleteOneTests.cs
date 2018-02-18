using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class DeleteOneTests
    {
        [TestMethod]
        public void DeleteOne_DeleteDocument()
        {
            TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.orders.deleteOne( { ""_id"" : 2 } );
                ");

            var acknowledged = (MongoDB.Driver.DeleteResult.Acknowledged)results.Value;
            Assert.AreEqual(true, acknowledged.IsAcknowledged);
            Assert.AreEqual(1, acknowledged.DeletedCount);
        }

        [TestMethod]
        public void DeleteOne_DeleteDocumentByISODate()
        {
            TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.orders.deleteOne( { ""ord_date"" : { $eq: ISODate(""2013-10-01T17:04:11.102Z"") } } );
                ");

            var acknowledged = (MongoDB.Driver.DeleteResult.Acknowledged)results.Value;
            Assert.AreEqual(true, acknowledged.IsAcknowledged);
            Assert.AreEqual(1, acknowledged.DeletedCount);
        }

        [TestMethod]
        public void DeleteOne_WithWriteConcern()
        {
            TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.orders.deleteOne(
                       { ""_id"" : 4 },
                       { w: ""majority"", wtimeout: 1 });
                    ");

            var acknowledged = (MongoDB.Driver.DeleteResult.Acknowledged)results.Value;
            Assert.AreEqual(true, acknowledged.IsAcknowledged);
            Assert.AreEqual(1, acknowledged.DeletedCount);
        }
    }
}
