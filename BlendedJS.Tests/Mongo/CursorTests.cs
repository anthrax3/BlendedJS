using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class CursorTests
    {
        [TestMethod]
        public void Find_HasNext_Next()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    var myCursor = db.bios.find( );

                    while(myCursor.hasNext())
                    {
                        var myDocument = myCursor.next();
                        print(tojson(myDocument));
                    }
                ");

            Assert.AreEqual(10, results.Logs.Count);
        }

        [TestMethod]
        public void Find_Next()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    var first = db.bios.find().next();
                    console.log(first);
                    
                ");

            Assert.AreEqual(1, results.Logs.Count);
        }

        [TestMethod]
        public void Find_ToArray()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    var items = db.bios.find().toArray();
                    console.log(items);
                    
                ");

            Assert.AreEqual(1, results.Logs.Count);
        }
    }
}
