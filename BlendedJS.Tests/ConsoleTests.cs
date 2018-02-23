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
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    log('bla bla bla');
                ");
            Assert.AreEqual("2: bla bla bla", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_String()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    console.log('bla bla bla');
                ");
            Assert.AreEqual("2: bla bla bla", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_SumOfNumber()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    console.log(2 + 2);
                ");
            Assert.AreEqual("2: 4", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_Object()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    var obj = {id:1, 'name':'dan'};
                    console.log(obj);
                ");
            Assert.AreEqual("3: {\"id\":1.0,\"name\":\"dan\"}", result.ConsoleTest);
        }

        [TestMethod]
        public void ConsoleLog_CatchedJsError()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
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
            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("doSomeJob", new Action<object>(x => throw new Exception("bla bla bla")));
            var result = mongo.ExecuteScript(
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
            BlendedJSEngine mongo = new BlendedJSEngine();
            mongo.Jint.SetValue("doSomeJob", new Action<object>(x => throw new Exception("bla bla bla")));
            var result = mongo.ExecuteScript(
                @"
                    doSomeJob();
                ");
            Assert.AreEqual("2: ERROR: bla bla bla", result.ConsoleTest);
        }
    }
}
