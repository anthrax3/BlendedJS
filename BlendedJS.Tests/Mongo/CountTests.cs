using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class CountTests
    {
        [TestMethod]
        public void Count_CountAllDocumentsInCollection()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient(this.mongoConnectionString);
                db.bios.count();
                ");
            System.Console.Write(results.ConsoleTest);
            int count = Convert.ToInt32(results.Value);
            Assert.AreEqual(10, count);
        }

        [TestMethod]
        public void Count_CountAllDocumentsAfterFind()
        {
            TestData.TestData.Prepare("bios", "TestData/bios.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient(this.mongoConnectionString);
                db.bios.find().count()
                ");

            int count = Convert.ToInt32(results.Value);
            Assert.AreEqual(10, count);
        }

        [TestMethod]
        public void Count_CountAllDocumentsThatMatchQuery()
        {
            TestData.TestData.Prepare("students", "TestData/students.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient(this.mongoConnectionString);
                db.students.count( { score: { $gt: 0, $lt: 2 } } )
                ");

            int count = Convert.ToInt32(results.Value);
            Assert.AreEqual(2, count);
        }


        [TestMethod]
        public void Count_CountAllDocumentsAfterFindWithFilter()
        {
            TestData.TestData.Prepare("students", "TestData/students.json");

            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                var db = new MongoClient(this.mongoConnectionString);
                db.students.find( { score: { $gt: 0, $lt: 2 } } ).count()
                ");

            int count = Convert.ToInt32(results.Value);
            Assert.AreEqual(2, count);
        }
    }
}
