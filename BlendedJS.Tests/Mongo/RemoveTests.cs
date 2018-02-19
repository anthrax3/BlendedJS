using BlendedJS.Mongo.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class RemoveTests
    {
        [TestMethod]
        public void Remove_All()
        {
            TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.remove( { } )
                ");

            MongoDB.Driver.DeleteResult.Acknowledged result = (MongoDB.Driver.DeleteResult.Acknowledged)results.Value;
            Assert.AreEqual(true, result.IsAcknowledged);
            //TODO: IsAcknowledged OR Acknowledged
            Assert.AreEqual(10, result.DeletedCount);
        }

        [TestMethod]
        public void Remove_WithFilter()
        {
            TestData.Prepare("inspectors", "TestData/inspectors.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.inspectors.remove( { Sector: { $gt: 2 }});
                ");

            MongoDB.Driver.DeleteResult.Acknowledged result = (MongoDB.Driver.DeleteResult.Acknowledged)results.Value;
            Assert.AreEqual(true, result.IsAcknowledged);
            //TODO: IsAcknowledged OR Acknowledged
            Assert.AreEqual(2, result.DeletedCount);
        }

        [TestMethod]
        public void Remove_SingleElement()
        {
            TestData.Prepare("inspectors", "TestData/inspectors.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.inspectors.remove( { Sector: { $gt: 2 }}, true);
                ");

            MongoDB.Driver.DeleteResult.Acknowledged result = (MongoDB.Driver.DeleteResult.Acknowledged)results.Value;
            Assert.AreEqual(true, result.IsAcknowledged);
            //TODO: IsAcknowledged OR Acknowledged
            Assert.AreEqual(1, result.DeletedCount);
        }
    }
}
