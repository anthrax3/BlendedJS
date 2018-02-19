using BlendedJS.Mongo.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class FindOneAndDeleteTests
    {
        [TestMethod]
        public void FindOneAndDelete_DeleteDocument()
        {
            TestData.Prepare("scores", "TestData/scores.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
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
            mongo.Jint.SetValue("mongoConnectionString", TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
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
            mongo.Jint.SetValue("mongoConnectionString", TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
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
