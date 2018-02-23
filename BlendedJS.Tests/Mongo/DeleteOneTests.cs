using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class DeleteOneTests
    {
        [TestMethod]
        public void DeleteOne_DeleteDocument()
        {
            TestData.TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient(this.mongoConnectionString);
                db.orders.deleteOne( { ""_id"" : 2 } );
                ");

            var acknowledged = (MongoDB.Driver.DeleteResult.Acknowledged)results.Value;
            Assert.AreEqual(true, acknowledged.IsAcknowledged);
            Assert.AreEqual(1, acknowledged.DeletedCount);
        }

        [TestMethod]
        public void DeleteOne_DeleteDocumentByISODate()
        {
            TestData.TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient(this.mongoConnectionString);
                db.orders.deleteOne( { ""ord_date"" : { $eq: ISODate(""2013-10-01T17:04:11.102Z"") } } );
                ");

            var acknowledged = (MongoDB.Driver.DeleteResult.Acknowledged)results.Value;
            Assert.AreEqual(true, acknowledged.IsAcknowledged);
            Assert.AreEqual(1, acknowledged.DeletedCount);
        }

        [TestMethod]
        public void DeleteOne_WithWriteConcern()
        {
            TestData.TestData.Prepare("orders", "TestData/orders.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient(this.mongoConnectionString);
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
