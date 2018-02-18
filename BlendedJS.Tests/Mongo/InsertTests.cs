using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class InsertTests
    {
        [TestMethod]
        public void Insert_OneWithId()
        {
            TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.products.insert( { _id: 10, item: ""box"", qty: 20 } )
                ");

            var result = (InsertResult)results.Value;
            Assert.AreEqual(true, result.acknowledged);
            Assert.IsNotNull(result.insertedId);
        }

        [TestMethod]
        public void Insert_ManyWithAndWithoutId()
        {
            TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
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
            TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.products.insert(
                        [
                            { _id: 20, item: ""lamp"", qty: 50, type: ""desk"" },
                            { _id: 20, item: ""lamp"", qty: 20, type: ""floor"" },
                            { item: ""bulk"", qty: 100 }
                        ],
                        { ordered: false }
                    )
                ");

            Assert.IsTrue(results.Output[0].Message.Contains("duplicate key error index"));
        }
    }
}
