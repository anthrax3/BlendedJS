using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System.Linq;
using System.Collections.Generic;

namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class FindOneTests
    {
        [TestMethod]
        public void FindOne_ReturnsOne()
        {
            TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.bios.findOne()
                ");
            var firstDocument = (BsonDocument)results.Value;
            Assert.IsNotNull(firstDocument);
            Assert.IsTrue(firstDocument.ToString().StartsWith("{ \"_id\" : 1,"));
        }

        [TestMethod]
        public void FindOne_WithQuerySpecificatione()
        {
            TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
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
            TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
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
            TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
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
            TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    var myDocument = db.bios.findOne();

                    if (myDocument) {
                       var myName = myDocument.name;

                       print (tojson(myName));
                    }
                ");


            Assert.AreEqual(1, results.Output.Count);
            Assert.AreEqual("{ \"first\" : \"John\", \"last\" : \"Backus\" }", results.Output[0].Message);
        }
    }
}
