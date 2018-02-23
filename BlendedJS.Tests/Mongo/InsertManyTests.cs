using BlendedJS.Mongo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class InsertManyTests
    {
        [TestMethod]
        public void InsertMany_ReturnGeneratedIds()
        {
            TestData.TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.products.insertMany( [
                      { item: ""card"", qty: 15 },
                      { item: ""envelope"", qty: 20 },
                      { item: ""stamps"" , qty: 30 }
                    ] );
            ");

            var result = (InsertResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual(3, ((object[])result.insertedIds).Length);
        }

        [TestMethod]
        public void InsertMany_WithIds()
        {
            TestData.TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.products.insertMany( [
                          { _id: 10, item: ""large box"", qty: 20 },
                          { _id: 11, item: ""small box"", qty: 55 },
                          { _id: 12, item: ""medium box"", qty: 30 }
                       ] );
            ");

            var result = (InsertResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.AreEqual(3, ((object[])result.insertedIds).Length);
        }

        [TestMethod]
        public void InsertMany_DuplicatedId()
        {
            TestData.TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    try {
                        var db = new MongoClient(this.mongoConnectionString);
                        db.products.insertMany( [
                          { _id: 13, item: ""envelopes"", qty: 60 },
                          { _id: 13, item: ""stamps"", qty: 110 },
                          { _id: 14, item: ""packing tape"", qty: 38 }
                           ] );
                    } catch (e) {
                        print(e);
                    }
                ");

            Assert.IsTrue(results.Console[0].Message.Contains("duplicate key error index"));
        }
    }
}
