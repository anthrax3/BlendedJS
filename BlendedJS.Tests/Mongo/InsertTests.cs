using BlendedJS.Mongo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class InsertTests
    {
        [TestMethod]
        public void Insert_OneWithId()
        {
            TestData.TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.products.insert( { _id: 10, item: ""box"", qty: 20 } )
                ");

            var result = (InsertResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.IsNotNull(result.insertedId);
        }

        [TestMethod]
        public void Insert_ManyWithAndWithoutId()
        {
            TestData.TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.products.insert(
                       [
                         { _id: 11, item: ""pencil"", qty: 50, type: ""no.2"" },
                         { item: ""pen"", qty: 20 },
                         { item: ""eraser"", qty: 25 }
                       ])
                ");

            var result = (InsertResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.IsNotNull(result.insertedIds);
            Assert.AreEqual(3, ((object[])result.insertedIds).Length);
        }

        [TestMethod]
        public void Insert_Unordered()
        {
            TestData.TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.products.insert(
                        [
                            { _id: 20, item: ""lamp"", qty: 50, type: ""desk"" },
                            { _id: 20, item: ""lamp"", qty: 20, type: ""floor"" },
                            { item: ""bulk"", qty: 100 }
                        ],
                        { ordered: false }
                    )
                ");

            Assert.IsTrue(results.Console[0].Arg1.ToString().Contains("duplicate key error index"));
        }
    }
}
