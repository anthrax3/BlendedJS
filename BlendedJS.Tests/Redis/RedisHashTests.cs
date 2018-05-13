using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Redis
{
    [TestClass]
    public class RedisHashTests
    {
        private string _testRedisUrl = "ec2-52-18-191-147.eu-west-1.compute.amazonaws.com:12209,name=h,password=p7d22f3439a2b9210fe68c043b61936b22b9fcfaf499c6b9e5c48088ec7291c86";


        [TestMethod]
        public void HDel()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 'foo');
                    redis.hdel('myhash', 'field1');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)1, (double)result.Value);
            }
        }

        [TestMethod]
        public void HExists()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 'foo');
                    redis.hexists('myhash', 'field1');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)1, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 'foo');
                    redis.hexists('myhash', 'field1');
                    redis.hdel('myhash', 'field1');
                    redis.hexists('myhash', 'field1');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)0, (double)result.Value);
            }
        }

        [TestMethod]
        public void HGetAll()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 'Hello');
                    redis.hset('myhash', 'field2', 'World');
                    redis.hgetall('myhash');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                var results = result.Value as JsObject;
                Assert.AreEqual("Hello", results.GetProperty("field1"));
                Assert.AreEqual("World", results.GetProperty("field2"));
            }
        }

        [TestMethod]
        public void HGet()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 'foo');
                    redis.hget('myhash', 'field1');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("foo", (string)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 'foo');
                    redis.hexists('myhash', 'field1');
                    redis.hdel('myhash', 'field1');
                    redis.hget('myhash', 'field1');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(null, result.Value);
            }
        }

        [TestMethod]
        public void HIncrBy()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 5);
                    redis.hincrby('myhash', 'field1', 1);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(6, (double)result.Value);
            }
        }

        [TestMethod]
        public void HIncrByFloat()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 5);
                    redis.hincrbyfloat('myhash', 'field1', 1);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(6, (double)result.Value);
            }
        }

        [TestMethod]
        public void HKeys()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 'Hello');
                    redis.hset('myhash', 'field2', 'World');
                    redis.hkeys('myhash');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("field1", ((object[])result.Value)[0]);
                Assert.AreEqual("field2", ((object[])result.Value)[1]);
            }
        }

        [TestMethod]
        public void HLen()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 'Hello');
                    redis.hset('myhash', 'field2', 'World');
                    redis.hlen('myhash');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)2, (double)result.Value);
            }
        }

        [TestMethod]
        public void HMGet()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hset('myhash', 'field1', 'Hello');
                    redis.hset('myhash', 'field2', 'World');
                    redis.hmget('myhash', 'field1', 'field2', 'nofield');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", ((object[])result.Value)[0]);
                Assert.AreEqual("World", ((object[])result.Value)[1]);
                Assert.AreEqual(null, ((object[])result.Value)[2]);
            }
        }

        [TestMethod]
        public void HMSet()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.hmset('myhash', 'field1', 'Hello', 'field2', 'World');
                    redis.hmget('myhash', 'field1', 'field2', 'nofield');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", ((object[])result.Value)[0]);
                Assert.AreEqual("World", ((object[])result.Value)[1]);
                Assert.AreEqual(null, ((object[])result.Value)[2]);
            }
        }

    }
}
