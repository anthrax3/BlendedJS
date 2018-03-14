using System;
using System.Collections.Generic;
using System.Text;
using BlendedJS.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests
{
    [TestClass]
    public class ConsoleTests
    {
        [TestMethod]
        public void Log_String()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    log('bla bla bla');
                ");
            Assert.AreEqual("2: bla bla bla", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_String()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    console.log('bla bla bla');
                ");
            Assert.AreEqual("2: bla bla bla", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_SumOfNumber()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    console.log(2 + 2);
                ");
            Assert.AreEqual("2: 4", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_Object()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    var obj = {id:1, 'name':'dan'};
                    console.log(obj);
                ");
            Assert.AreEqual("3: {\"id\":1.0,\"name\":\"dan\"}", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_StringPlusJsObject()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    var obj = {id:1, 'name':'dan'};
                    console.log('text ' + obj);
                ");
            Assert.AreEqual("3: text [object Object]", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_StringPlusJsArray()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    var array = [1,2,3];
                    console.log('text ' + array);
                ");
            Assert.AreEqual("3: text 1,2,3", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_StringPlusJsArrayOfObjects()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    var array = [{a:1},{a:2},{a:3}];
                    console.log('text ' + array);
                ");
            Assert.AreEqual("3: text [object Object],[object Object],[object Object]", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_StringPlusCsHttpClient()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    var obj = HttpClient();
                    console.log('text ' + obj);
                ");
            Assert.AreEqual("3: text [object HttpClient]", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_StringPlusCsObject()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            Dictionary<string, object> obj = new Dictionary<string, object>();
            engine.Jint.SetValue("TestClass", new Func<object>(() => obj.ToJsObject()));

            var result = engine.ExecuteScript(
                @"
                    var obj = TestClass();
                    console.log('text ' + obj);
                ");
            Assert.AreEqual("3: text [object Object]", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_StringPlusCsArray()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            Dictionary<string, object> obj = new Dictionary<string, object>();
            engine.Jint.SetValue("TestClass", new Func<object>(() => new object[] { 1, 2, 3 }));

            var result = engine.ExecuteScript(
                @"
                    var obj = TestClass();
                    console.log('text ' + obj);
                ");
            Assert.AreEqual("3: text 1,2,3", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_StringPlusCsArrayOfObjects()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            Dictionary<string, object> obj = new Dictionary<string, object>();
            engine.Jint.SetValue("TestClass", new Func<object>(() => new object[] { new Object(), new Object(), new Object() }));

            var result = engine.ExecuteScript(
                @"
                    var obj = TestClass();
                    console.log('text ' + obj);
                ");
            Assert.AreEqual("3: text [object Object],[object Object],[object Object]", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_CatchedJsError()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    try {
                        doSomeJob();
                    }
                    catch(err) {
                        console.log('catch ' + err);
                    }
                ");
            Assert.AreEqual("6: catch ReferenceError: doSomeJob is not defined", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_CatchedNetError()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            engine.Jint.SetValue("doSomeJob", new Action<object>(x => throw new Exception("bla bla bla")));
            var result = engine.ExecuteScript(
                @"
                    try {
                        doSomeJob();
                    }
                    catch(err) {
                        console.log('catch ' + err);
                    }
                ");
            Assert.AreEqual("6: catch Error: bla bla bla", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_NotCatchedNetError()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            engine.Jint.SetValue("doSomeJob", new Action<object>(x => throw new Exception("bla bla bla")));
            var result = engine.ExecuteScript(
                @"
                    doSomeJob();
                ");
            Assert.AreEqual("2: ERROR: bla bla bla", result.ConsoleTest);
        }
    }
}
