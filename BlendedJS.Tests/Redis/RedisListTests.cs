using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Redis
{
    [TestClass]
    public class RedisListTests
    {
        private string _testRedisUrl = "ec2-52-18-191-147.eu-west-1.compute.amazonaws.com:12209,name=h,password=p7d22f3439a2b9210fe68c043b61936b22b9fcfaf499c6b9e5c48088ec7291c86";


        [TestMethod]
        public void lindex()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.lpush('mylist', 'World');
                    redis.lpush('mylist', 'Hello');
                    redis.lindex('mylist', '0');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", (string)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.lpush('mylist', 'World');
                    redis.lpush('mylist', 'Hello');
                    redis.lindex('mylist', 0);
                    redis.lindex('mylist', -1);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("World", (string)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.lpush('mylist', 'World');
                    redis.lpush('mylist', 'Hello');
                    redis.lindex('mylist', 0);
                    redis.lindex('mylist', -1);
                    redis.lindex('mylist', 3);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(null, result.Value);
            }
        }

        [TestMethod]
        public void linsert()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'Hello');
                    redis.rpush('mylist', 'World');
                    redis.linsert('mylist', 'before', 'World', 'There');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)3, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'Hello');
                    redis.rpush('mylist', 'World');
                    redis.linsert('mylist', 'before', 'World', 'There');
                    redis.lrange('mylist', 0, -1);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", ((object[])result.Value)[0]);
                Assert.AreEqual("There", ((object[])result.Value)[1]);
                Assert.AreEqual("World", ((object[])result.Value)[2]);
            }
        }

        [TestMethod]
        public void llen()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'Hello');
                    redis.rpush('mylist', 'World');
                    redis.llen('mylist');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)2, (double)result.Value);
            }
        }

        [TestMethod]
        public void lpop()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'one');
                    redis.rpush('mylist', 'two');
                    redis.rpush('mylist', 'three');
                    redis.lpop('mylist');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("one", (string)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'one');
                    redis.rpush('mylist', 'two');
                    redis.rpush('mylist', 'three');
                    redis.lpop('mylist');
                    redis.lpop('mylist');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("two", (string)result.Value);
            }
        }

        [TestMethod]
        public void lpush()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.lpush('mylist', 'one');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)1, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.lpush('mylist', 'one');
                    redis.lpush('mylist', 'two');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)2, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.lpush('mylist', 'one');
                    redis.lpush('mylist', 'two');
                    redis.lrange('mylist', 0, -1);
                ");

                Assert.AreEqual("two", ((object[])result.Value)[0]);
                Assert.AreEqual("one", ((object[])result.Value)[1]);
            }

        }

        [TestMethod]
        public void lpushx()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.lpush('mylist', 'one');
                    redis.lpushx('mylist', 'two');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)2, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.lpushx('mylist', 'two');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)0, (double)result.Value);
            }
        }

        [TestMethod]
        public void lrange()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'one');
                    redis.rpush('mylist', 'two');
                    redis.rpush('mylist', 'three');
                    redis.lrange('mylist', 0, 0);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("one", ((object[])result.Value)[0]);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'one');
                    redis.rpush('mylist', 'two');
                    redis.rpush('mylist', 'three');
                    redis.lrange('mylist', 0, 0);
                    redis.lrange('mylist', -3, 2);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("one", ((object[])result.Value)[0]);
                Assert.AreEqual("two", ((object[])result.Value)[1]);
                Assert.AreEqual("three", ((object[])result.Value)[2]);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'one');
                    redis.rpush('mylist', 'two');
                    redis.rpush('mylist', 'three');
                    redis.lrange('mylist', 0, 0);
                    redis.lrange('mylist', -3, 2);
                    redis.lrange('mylist', 5, 10);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(0, ((object[])result.Value).Length);
            }
        }

        [TestMethod]
        public void lrem()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'hello');
                    redis.rpush('mylist', 'hello');
                    redis.rpush('mylist', 'foo');
                    redis.rpush('mylist', 'hello');
                    redis.lrem('mylist', -2, 'hello');
                    redis.lrange('mylist', 0, -1);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("hello", ((object[])result.Value)[0]);
                Assert.AreEqual("foo", ((object[])result.Value)[1]);
            }
        }

        [TestMethod]
        public void lset()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'one');
                    redis.rpush('mylist', 'two');
                    redis.rpush('mylist', 'three');
                    redis.lset('mylist', 0, 'four');
                    redis.lset('mylist', -2, 'five');
                    redis.lrange('mylist', 0, -1);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("four", ((object[])result.Value)[0]);
                Assert.AreEqual("five", ((object[])result.Value)[1]);
                Assert.AreEqual("three", ((object[])result.Value)[2]);
            }
        }

        [TestMethod]
        public void ltrim()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'one');
                    redis.rpush('mylist', 'two');
                    redis.rpush('mylist', 'three');
                    redis.ltrim('mylist', 1, -1);
                    redis.lrange('mylist', 0, -1);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("two", ((object[])result.Value)[0]);
                Assert.AreEqual("three", ((object[])result.Value)[1]);
            }
        }

        [TestMethod]
        public void rpop()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'one');
                    redis.rpush('mylist', 'two');
                    redis.rpush('mylist', 'three');
                    redis.rpop('mylist');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("three", result.Value);
            }
        }

        [TestMethod]
        public void rpoplpush()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.del('myotherlist');
                    redis.rpush('mylist', 'one');
                    redis.rpush('mylist', 'two');
                    redis.rpush('mylist', 'three');
                    redis.rpoplpush('mylist', 'myotherlist');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("three", (string)result.Value);
            }
        }

        [TestMethod]
        public void rpush()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'one');
                    redis.rpush('mylist', 'two');
                    redis.lrange('mylist', 0, -1);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("one", ((object[])result.Value)[0]);
                Assert.AreEqual("two", ((object[])result.Value)[1]);
            }
        }

        [TestMethod]
        public void rpushx()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpush('mylist', 'one');
                    redis.rpushx('mylist', 'two');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)2, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mylist');
                    redis.rpushx('mylist', 'two');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)0, (double)result.Value);
            }
        }
    }
}
