using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class FindTests
    {
        [TestMethod]
        public void Find_SimpleQuery_ReturnList()
        {
            TestData.TestData.Prepare("collection", "TestData/fruits.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.collection.find({ qty: { $gt: 4 } })
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void Find_ReturnAllDocuments()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find();
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(10, items.Count);
        }

        [TestMethod]
        public void Find_ByIntId_ReturnsOneDocument()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find( { _id: 5 } )
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(1, items.Count);
        }

        [TestMethod]
        public void Find_ByObjectId_ReturnsOneDocument()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find( { _id: ObjectId(""51e062189c6ae665454e301d"") } );
                ");
            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(1, items.Count);
        }

        [TestMethod]
        public void Find_In_ByIntIdAndObjectId_ReturnsOneDocument()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find({_id: { $in: [ 5,  ObjectId(""51e062189c6ae665454e301d"") ] }})
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void Find_FieldContainAnArray()
        {
            TestData.TestData.Prepare("students", "TestData/students.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.students.find( { score: { $gt: 0, $lt: 2 } } );
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void Find_ArrayOfDocuments()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find(
                       {
                          awards: {
                                    $elemMatch: {
                                         award: ""Turing Award"",
                                         year: { $gt: 1980 }
                            }
                        }
                    }
                    )
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(3, items.Count);
        }

        [TestMethod]
        public void Find_ExactMatchesOnEmbeddedDocuments()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find(
                        {
                          name: {
                                  first: ""Yukihiro"",
                                  last: ""Matsumoto""
                                }
                        }
                    )
                    ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public void Find_FieldsOfAnEmbeddedDocument()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find(
                       {
                         ""name.first"": ""Yukihiro"",
                         ""name.last"": ""Matsumoto""
                       }
                    )
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(1, items.Count);
        }

        [TestMethod]
        public void Find_Projection_Exclude_idField()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find(
                       { },
                       { name: 1, contribs: 1, _id: 0 }
                    )
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(10, items.Count);
            foreach(var item in items)
            {
                Assert.AreEqual(2, item.ElementCount);
                Assert.IsNotNull(item.GetElement("name"));
                Assert.IsNotNull(item.GetElement("contribs"));
            }
        }

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

                    var myDocument = myCursor.hasNext() ? myCursor.next() : null;

                    if (myDocument) {
                        var myName = myDocument.name;
                        print (tojson(myName));
                    }
                ");

            Assert.AreEqual(1, results.Console.Count);
            Assert.AreEqual("{ \"first\" : \"John\", \"last\" : \"Backus\" }", results.Console[0].Message);
        }

        [TestMethod]
        public void Find_ForEachTheReturnedCursor()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    var myCursor = db.bios.find( );

                    myCursor.forEach(printjson);
                ");

            Assert.AreEqual(10, results.Console.Count);
        }

        [TestMethod]
        public void Find_Sort()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find().sort( { name: 1 } )
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(10, items.Count);
            Assert.AreEqual("{ \"first\" : \"Dennis\", \"last\" : \"Ritchie\" }", items.First().GetElement("name").Value.ToString());
            Assert.AreEqual("{ \"first\" : \"Yukihiro\", \"aka\" : \"Matz\", \"last\" : \"Matsumoto\" }", items.Last().GetElement("name").Value.ToString());
        }

        [TestMethod]
        public void Find_Limit()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find().limit( 5 )
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(5, items.Count);
        }

        [TestMethod]
        public void Find_Skip()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find().skip( 8 )
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void Find_Collation()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find( { ""name.last"": ""hopper"" } ).collation( { locale: ""en_US"", strength: 1 } );
                ");

            var items = ((IEnumerable<BsonDocument>)results.Value).ToList();
            Assert.AreEqual(1, items.Count);
        }

        [TestMethod]
        public void Find_LimiAndSort()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.bios.find().sort( { name: 1 } ).limit( 5 )
                ").Value as IEnumerable<BsonDocument>;


            Assert.AreEqual(5, results.ToList().Count);
        }
    }
}
