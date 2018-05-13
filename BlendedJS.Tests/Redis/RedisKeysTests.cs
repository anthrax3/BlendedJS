using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Redis
{
    [TestClass]
    public class RedisKeysTests
    {
        private string _testRedisUrl = "ec2-52-18-191-147.eu-west-1.compute.amazonaws.com:12209,name=h,password=p7d22f3439a2b9210fe68c043b61936b22b9fcfaf499c6b9e5c48088ec7291c86";

        [TestMethod]
        public void Del()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.set('key1', 'Hello');
                    redis.set('key2', 'World');
                    redis.del('key1', 'key2', 'key3');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)2, (double)result.Value);
            }
        }

        [TestMethod]
        public void Dump()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 10);
                    redis.dump('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("\u0000\xC0\n\b\u0000ײ\xBB\xFA\xA7\xB7\xE9\x83", (string)result.Value);
            }
        }

        [TestMethod]
        public void Exists()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('key1');
                    redis.del('key2');
                    redis.set('key1', 'Hello');
                    redis.exists('key1');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)1, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('key1');
                    redis.del('key2');
                    redis.set('key1', 'Hello');
                    redis.exists('nosuchkey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)0, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('key1');
                    redis.del('key2');
                    redis.set('key1', 'Hello');
                    redis.exists('nosuchkey');
                    redis.set('key2', 'Worlds');
                    redis.exists('key1' ,'key2', 'nosuchkey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)0, (double)result.Value);
            }
        }

        [TestMethod]
        public void Expire()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello');
                    redis.expire('mykey', 10);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)1, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello');
                    redis.expire('mykey', 10);
                    redis.ttl('mykey');
                    redis.set('mykey', 'Worlds');
                    redis.ttl('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)-1, (double)result.Value);
            }
        }

        [TestMethod]
        public void ExpireAt()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello');
                    redis.expireat('mykey', 1293840000);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)1, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello');
                    redis.expireat('mykey', 1293840000);
                    redis.exists('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)0, (double)result.Value);
            }
        }

        [TestMethod]
        public void Keys()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.set('firstname', 'Jack');
                    redis.set('lastname', 'White');
                    redis.keys('*name*');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Jack", ((object[])result.Value)[0]);
                Assert.AreEqual("White", ((object[])result.Value)[1]);
            }
        }

        [TestMethod]
        public void Persist()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello');
                    redis.expire('mykey', 10);
                    redis.persist('mykey');
                    redis.ttl('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)-1, (double)result.Value);
            }
        }

        [TestMethod]
        public void PExpire()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello');
                    redis.pexpire('mykey', 1500);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)1, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello');
                    redis.pexpire('mykey', 1500);
                    redis.ttl('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)1, (double)result.Value);
            }
        }

        [TestMethod]
        public void PExpireAt()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello');
                    redis.pexpireat('mykey', 1555555555005);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)1, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello');
                    redis.pexpireat('mykey', 1555555555005);
                    redis.ttl('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.IsTrue((double)result.Value > 0);
            }
        }

        [TestMethod]
        public void PTtl()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.set('mykey', 'Hello');
                    redis.expire('mykey', 1);
                    redis.pttl('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.IsTrue((double)result.Value > 900);
            }
        }

        [TestMethod]
        public void RandomKey()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.randomkey();
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.IsNotNull((string)result.Value);
            }
        }

        [TestMethod]
        public void Rename()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.del('myotherkey');
                    redis.set('mykey', 'Hello');
                    redis.rename('mykey', 'myotherkey');
                    redis.get('myotherkey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", (string)result.Value);
            }
        }

        [TestMethod]
        public void RenameNx()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.del('myotherkey');
                    redis.set('mykey', 'Hello');
                    redis.set('myotherkey', 'World');
                    redis.renamenx('mykey', 'myotherkey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(0, (double)result.Value);
            }
        }

        [TestMethod]
        public void Restore()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.restore('mykey', 0, `\n\x17\x17\x00\x00\x00\x12\x00\x00\x00\x03\x00\
                        x00\xc0\x01\x00\x04\xc0\x02\x00\x04\xc0\x03\x00\
                        xff\x04\x00u#<\xc0;.\xe9\xdd`);
                    redis.type('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("list", (string)result.Value);
            }
        }

        [TestMethod]
        public void Scan()
        {
            throw new System.Exception("TODO");
        }

        [TestMethod]
        public void Unlink()
        {
            throw new System.Exception("TODO");
        }

        [TestMethod]
        public void Wait()
        {
            throw new System.Exception("TODO");
        }

        [TestMethod]
        public void Touch()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.set('key1', 'Hello');
                    redis.set('key2', 'World');
                    redis.touch('key1', 'key2');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(2, (double)result.Value);
            }
        }

        [TestMethod]
        public void Ttl()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('key1');
                    redis.set('key1', 'Hello');
                    redis.expire('key1', 10);
                    redis.ttl('key1');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(10, (double)result.Value);
            }
        }

        [TestMethod]
        public void Type()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.set('key1', 'Hello');
                    redis.type('key1');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("String", (string)result.Value);
            }
        }
    }
}
