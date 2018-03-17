using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class FindOneTests
    {
        [TestMethod]
        public void FindOne_ReturnsOne()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.findOne()
                ");
            var firstDocument = (BsonDocument)results.Value;
            Assert.IsNotNull(firstDocument);
            Assert.IsTrue(firstDocument.ToString().StartsWith("{ \"_id\" : 1,"));
        }

        [TestMethod]
        public void FindOne_WithQuerySpecificatione()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.findOne(
                       {
                         $or: [
                                { 'name.first' : /^G/ },
                                { birth: { $lt: new Date('01/01/1945') } }
                              ]
                       }
                    );
                ");

            var firstDocument = (BsonDocument)results.Value;
            Assert.IsNotNull(firstDocument);
            Assert.IsTrue(firstDocument.ToString().StartsWith("{ \"_id\" : 3,"));
        }

        [TestMethod]
        public void FindOne_WithProjection()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.findOne(
                        { },
                        { name: 1, contribs: 1 }
                    )
                ");

            var firstDocument = (BsonDocument)results.Value;
            Assert.IsNotNull(firstDocument);
            Assert.AreEqual("{ \"_id\" : 1, \"name\" : { \"first\" : \"John\", \"last\" : \"Backus\" }, \"contribs\" : [\"Fortran\", \"ALGOL\", \"Backus-Naur Form\", \"FP\"] }", firstDocument.ToString());
        }

        [TestMethod]
        public void FindOne_ReturnAllButExcludedFields()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.findOne(
                       { contribs: 'OOP' },
                       { _id: 0, 'name.first': 0, birth: 0 }
                    )
                ");

            var firstDocument = ((BsonDocument)results.Value).ToString();
            Assert.IsFalse(firstDocument.Contains("_id"));
            Assert.IsFalse(firstDocument.Contains("first"));
        }

        [TestMethod]
        public void FindOne_PrintResults()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    var myDocument = db.bios.findOne();

                    if (myDocument) {
                       var myName = myDocument.name;

                       print (tojson(myName));
                    }
                ");


            Assert.AreEqual(1, results.Logs.Count);
            Assert.AreEqual("{ \"first\" : \"John\", \"last\" : \"Backus\" }", results.Logs[0].Arg1);
        }
    }
}
