using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo.Tests
{

    [TestClass]
    public class InsertOneTests
    {
        [TestMethod]
        public void Insert_ReturnGeneratedId()
        {
            TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.products.insertOne( { item: ""card"", qty: 15 } );
                ");

            var result = (InsertResult)results.Value;
            Assert.IsTrue((bool)result.acknowledged);
            Assert.IsNotNull(result.insertedId);
        }

        [TestMethod]
        public void Insert_WithId()
        {
            TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                    db.products.insertOne( { _id: 10, item: ""box"", qty: 20 } );
                ");

            var result = (InsertResult)results.Value;
            Assert.IsTrue((bool)result.acknowledged);
            Assert.AreEqual("10", result.insertedId.ToString());
        }

        [TestMethod]
        public void Insert_DuplicateId()
        {
            TestData.Prepare("products", "TestData/products.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                    try {
                        var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                        db.products.insertOne( { _id: 101, ""item"" : ""packing peanuts"", ""qty"" : 200 } );
                    } catch (e) {
                       print(e);
                    }
                ");
            Assert.IsTrue(results.Output[0].Message.Contains("duplicate key error index"));
        }



        
    }
}
