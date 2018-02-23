using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Mongo
{
    [TestClass]
    public class DropTests
    {
        [TestMethod]
        public void Drop()
        {
            TestData.TestData.Prepare("orders", "TestData/orders.json");
            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("mongoConnectionString", TestData.TestData.MongoConnectionString);
            var results = mongo.ExecuteScript(
                @"
                    var db = new MongoClient(this.mongoConnectionString);
                    db.orders.drop();
                ");
        }
    }
}