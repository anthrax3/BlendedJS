using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Redis
{
    [TestClass]
    public class RedisTests
    {
        [TestMethod]
        public void KeyExists()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"console.log('111');
                    var redis = new RedisClient();
                    conosle.log('eee');
                    redis.KeyExists('test');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                var value = result.Value as bool?;
                Assert.IsFalse(value.Value);
            }
        }

    }
}
