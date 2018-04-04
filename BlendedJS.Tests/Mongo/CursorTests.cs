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
        public void Find_IterateTheReturnedCursor()
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
    }
}
