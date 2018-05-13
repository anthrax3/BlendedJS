using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Redis
{
    [TestClass]
    public class RedisStringsTests
    {
        private string _testRedisUrl = "ec2-52-18-191-147.eu-west-1.compute.amazonaws.com:12209,name=h,password=p7d22f3439a2b9210fe68c043b61936b22b9fcfaf499c6b9e5c48088ec7291c86";


        [TestMethod]
        public void Append()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.append('mykey','hello');
                    redis.append('mykey',' world');
                    redis.get('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                var value = result.Value as string;
                Assert.AreEqual("hello world", value);
            }
        }

        [TestMethod]
        public void BitCount()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey','foobar');
                    redis.bitcount('mykey');
                ");
                Assert.AreEqual((double)26, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey','foobar');
                    redis.bitcount('mykey', 0, 0);
                ");
                Assert.AreEqual((double)4, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey','foobar');
                    redis.bitcount('mykey', 1,1);
                ");
                Assert.AreEqual((double)6, (double)result.Value);
            }
        }

        [TestMethod]
        public void BitPos()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey','\xff\xf0\x00');
                    redis.bitpos('mykey', 0);
                ");
                Assert.AreEqual((double)12, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', '\x00\xff\xf0');
                    redis.bitpos('mykey', 1, 0);
                ");
                Assert.AreEqual((double)8, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', '\x00\xff\xf0');
                    redis.bitpos('mykey', 1, 2);
                ");
                Assert.AreEqual((double)16, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', '\x00\x00\x00');
                    redis.bitpos('mykey', 1);
                ");
                Assert.AreEqual((double)-1, (double)result.Value);
            }
        }

        [TestMethod]
        public void SetGet()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('test key');
                    redis.setget('test key','test value');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                var value = result.Value as string;
                Assert.AreEqual("test value", value);
            }
        }

        [TestMethod]
        public void Decr()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey','10');
                    redis.decr('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                var value = result.Value as string;
                Assert.AreEqual(9, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey','234293482390480948029348230948');
                    redis.decr('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.IsTrue(result.ConsoleTest.Contains("ERR value is not an integer or out of range"));
            }
        }

        [TestMethod]
        public void DecrBy()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', '10');
                    redis.decrby('mykey', 3);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                var value = result.Value as string;
                Assert.AreEqual(7, (double)result.Value);
            }
        }

        [TestMethod]
        public void Get()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey','Hello');
                    redis.get('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", (string)result.Value);
            }
        }

        [TestMethod]
        public void GetBit()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setbit('mykey', 7, 1);
                    redis.getbit('mykey', 0);
                ");
                Assert.AreEqual((double)0, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setbit('mykey', 7, 1);
                    redis.getbit('mykey', 7);
                ");
                Assert.AreEqual((double)1, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setbit('mykey', 7, 1);
                    redis.getbit('mykey', 100);
                ");
                Assert.AreEqual((double)0, (double)result.Value);
            }
        }

        [TestMethod]
        public void GetRange()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'This is a string');
                    redis.getrange('mykey', 0, 3);
                ");
                Assert.AreEqual("This", (string)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'This is a string');
                    redis.getrange('mykey', 0, -1);
                ");
                Assert.AreEqual("This is a string", (string)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'This is a string');
                    redis.getrange('mykey', 10, 100);
                ");
                Assert.AreEqual("string", (string)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'This is a string');
                    redis.getrange('mykey', 0, 3);
                ");
                Assert.AreEqual("This", (string)result.Value);
            }
        }

        [TestMethod]
        public void Incr()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', '10');
                    redis.incr('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(11, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', '10');
                    redis.incr('mykey');
                    redis.get('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("11", (string)result.Value);
            }
        }

        [TestMethod]
        public void IncrBy()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', '10');
                    redis.incrby('mykey', 5);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(15, (double)result.Value);
            }
        }

        [TestMethod]
        public void IncrByFloat()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 10.50);
                    redis.incrbyfloat('mykey', 0.1);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(10.6, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 10.50);
                    redis.incrbyfloat('mykey', 0.1);
                    redis.incrbyfloat('mykey', -5);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(5.6, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 5.0e3);
                    redis.incrbyfloat('mykey', 2.0e2);
    
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(5200, (double)result.Value);
            }
        }

        [TestMethod]
        public void MGet()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('key1');
                    redis.del('key2');
                    redis.set('key1', 'Hello');
                    redis.set('key2', 'World');
                    redis.mget('key1','key2','not exsiting key');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", ((object[])result.Value)[0]);
                Assert.AreEqual("World", ((object[])result.Value)[1]);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('key1');
                    redis.del('key2');
                    redis.set('key1', 'Hello');
                    redis.set('key2', 'World');
                    redis.mget(['key1','key2','not exsiting key']);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", ((object[])result.Value)[0]);
                Assert.AreEqual("World", ((object[])result.Value)[1]);
            }
        }

        [TestMethod]
        public void MSet()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('key1');
                    redis.del('key2');
                    redis.mset(['key1', 'Hello', 'key2', 'World']);
                    redis.mget(['key1','key2']);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", ((object[])result.Value)[0]);
                Assert.AreEqual("World", ((object[])result.Value)[1]);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('key1');
                    redis.del('key2');
                    redis.mset('key1', 'Hello', 'key2', 'World');
                    redis.mget('key1','key2');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", ((object[])result.Value)[0]);
                Assert.AreEqual("World", ((object[])result.Value)[1]);
            }
        }

        [TestMethod]
        public void MSetNX()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('key1');
                    redis.del('key2');
                    redis.msetnx(['key1', 'Hello', 'key2', 'World']);
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
                    redis.del('key3');
                    redis.msetnx(['key1', 'Hello', 'key2', 'World']);
                    redis.msetnx(['key2', 'Hello', 'key3', 'World']);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)0, (double)result.Value);
            }
        }

        [TestMethod]
        public void PSetEx()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.psetex('mykey', 1000, 'Hello');
                    redis.pttl('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.IsTrue((double)result.Value > (double)900);
            }
        }

        [TestMethod]
        public void SetBit()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setbit('mykey', 7, 1);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(0, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setbit('mykey', 7, 1);
                    redis.setbit('mykey', 7, 0);
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(1, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setbit('mykey', 7, 1);
                    redis.setbit('mykey', 7, 0);
                    redis.get('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("\u0000", (string)result.Value);
            }
        }

        [TestMethod]
        public void SetEx()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setex('mykey', 11, 'Hello');
                    redis.ttl('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(10, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setex('mykey', 11, 'Hello');
                    redis.ttl('mykey');
                    redis.get('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", (string)result.Value);
            }
        }

        [TestMethod]
        public void SetNx()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setnx('mykey', 'Hello');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(1, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setnx('mykey', 'Hello');
                    redis.setnx('mykey', 'World');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(0, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setnx('mykey', 'Hello');
                    redis.setnx('mykey', 'World');
                    redis.get('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello", (string)result.Value);
            }
        }

        [TestMethod]
        public void SetRange()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello World')
                    redis.setrange('mykey', 6, 'Redis');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(11, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello World')
                    redis.setrange('mykey', 6, 'Redis');
                    redis.get('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("Hello Redis", (string)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setrange('mykey', 6, 'Redis');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual((double)11, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.setrange('mykey', 6, 'Redis');
                    redis.get('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual("\u0000\u0000\u0000\u0000\u0000\u0000Redis", (string)result.Value);
            }
        }

        [TestMethod]
        public void StrLen()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.set('mykey', 'Hello World')
                    redis.strlen('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(11, (double)result.Value);
            }

            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("testRedisUrl", _testRedisUrl);

                var result = engine.ExecuteScript(@"
                    var redis = new RedisClient(this.testRedisUrl);
                    redis.del('mykey');
                    redis.strlen('mykey');
                ");

                System.Console.WriteLine(result.ConsoleTest);
                Assert.AreEqual(0, (double)result.Value);
            }

        }

    }
}
