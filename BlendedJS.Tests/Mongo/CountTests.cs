using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo.Tests
{
    [TestClass]
    public class CountTests
    {
        [TestMethod]
        public void Count_CountAllDocumentsInCollection()
        {
            TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.bios.count()
                ");

            int count = Convert.ToInt32(results.Value);
            Assert.AreEqual(10, count);
        }

        [TestMethod]
        public void Count_CountAllDocumentsAfterFind()
        {
            TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.bios.find().count()
                ");

            int count = Convert.ToInt32(results.Value);
            Assert.AreEqual(10, count);
        }

        [TestMethod]
        public void Count_CountAllDocumentsThatMatchQuery()
        {
            TestData.Prepare("students", "TestData/students.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.students.count( { score: { $gt: 0, $lt: 2 } } )
                ");

            int count = Convert.ToInt32(results.Value);
            Assert.AreEqual(2, count);
        }


        [TestMethod]
        public void Count_CountAllDocumentsAfterFindWithFilter()
        {
            TestData.Prepare("students", "TestData/students.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient('mongodb://ipl:qwerty123@ds147510.mlab.com:47510/heroku_rgzrhk40');
                db.students.find( { score: { $gt: 0, $lt: 2 } } ).count()
                ");

            int count = Convert.ToInt32(results.Value);
            Assert.AreEqual(2, count);
        }
    }
}
