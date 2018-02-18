using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class FindOneAndDeleteTests
    {
        [TestMethod]
        public void FindOneAndDelete_DeleteDocument()
        {
            TestData.Prepare("scores", "TestData/scores.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.scores.findOneAndDelete(
                       { ""name"" : ""M. Tagnum"" }
                    )
                ");
            var document = (BsonDocument)results.Value;
            Assert.IsNotNull(document);
            Assert.IsTrue(document.ToString().StartsWith("{ \"_id\" : 6312,"));
        }

        [TestMethod]
        public void FindOneAndDelete_SortAndDeleteDocument()
        {
            TestData.Prepare("scores", "TestData/scores.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.scores.findOneAndDelete(
                       { ""name"" : ""A. MacDyver"" },
                       { sort: { ""points"" : 1 } }
                    )
                ");
            var document = (BsonDocument)results.Value;
            Assert.IsNotNull(document);
            Assert.IsTrue(document.ToString().StartsWith("{ \"_id\" : 6322,"));
        }

        [TestMethod]
        public void FindOneAndDelete_ProjectingDeletedDocument()
        {
            TestData.Prepare("scores", "TestData/scores.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.scores.findOneAndDelete(
                       { ""name"" : ""A. MacDyver"" },
                       { sort: { ""points"" : 1 }, projection: { ""assignment"" : 1 } }
                    )
                ");
            var document = (BsonDocument)results.Value;
            Assert.IsNotNull(document);
            Assert.IsTrue(document.ToString().StartsWith("{ \"_id\" : 6322,"));
        }
    }
}
