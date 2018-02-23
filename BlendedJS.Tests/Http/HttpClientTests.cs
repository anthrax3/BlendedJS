using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlendedJS.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Http
{
    [TestClass]
    public class HttpClientTests
    {
        [TestMethod]
        public void Get_Website()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    var httpClient = new HttpClient();
                    httpClient.get({url:'https://www.theguardian.com'});
                ").Value as HttpResponse;
            Assert.AreEqual(200, result.statusCode);
            Assert.AreEqual("OK", result.reasonPhrase);
            Assert.IsTrue(result.body.ToString().Contains("theguardian"));
            Assert.AreEqual("keep-alive", result.headers.GetProperty("Connection"));
        }

        [TestMethod]
        public void Get_Json()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    var httpClient = new HttpClient();
                    var response = httpClient.get({
                        url:'https://jsonplaceholder.typicode.com/posts',
                        headers: {'Content-Type':'application/json'}
                    });
                    JSON.parse(response.body);
                ").Value as object[];
            Assert.AreEqual((double)1, result[0].GetProperty("userId"));
        }

        [TestMethod]
        public void Get_Json_BySpecifyingJustUrl()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    var httpClient = new HttpClient();
                    var response = httpClient.get('https://jsonplaceholder.typicode.com/posts');
                    JSON.parse(response.body);
                ").Value as object[];
            Assert.AreEqual((double)1, result[0].GetProperty("userId"));
        }

        [TestMethod]
        public void Post_Json()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    var httpClient = new HttpClient();
                    var response = httpClient.post({
                        url:'https://jsonplaceholder.typicode.com/posts',
                        headers: {'Content-Type':'application/json'},
                        body: { userId: 1, title: 'bla', body: 'bla bla bla'}
                    });
                    JSON.parse(response.body);
                ").Value;
            Assert.AreEqual((double)101, result.GetProperty("id"));
        }

        [TestMethod]
        public void Put_Json()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    var httpClient = new HttpClient();
                    var response = httpClient.put({
                        url:'https://jsonplaceholder.typicode.com/posts/1',
                        headers: {'Content-Type':'application/json'},
                        body: { userId: 1, title: 'bla', body: 'bla bla bla'}
                    });
                    JSON.parse(response.body);
                ").Value;
            Assert.AreEqual((double)1, result.GetProperty("id"));
        }

        [TestMethod]
        public void Delete_Json()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    var httpClient = new HttpClient();
                    httpClient.delete({
                        url:'https://jsonplaceholder.typicode.com/posts/1'
                    });
                ").Value as HttpResponse;
            Assert.AreEqual(200, result.statusCode);
            Assert.AreEqual("OK", result.reasonPhrase);
            Assert.AreEqual("{}", result.body);
        }

        [TestMethod]
        public void Get_UrlIsInvalid_ThrowError()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    try {
                        var httpClient = new HttpClient();
                        httpClient.get('asdf://asdf');
                    }catch(err) {
                        console.log(err);
                    }
                ");
            Assert.IsTrue(result.ConsoleTest.Contains("Only 'http' and 'https' schemes are allowed"));
        }

        [TestMethod]
        public void Get_404()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    var httpClient = new HttpClient();
                    httpClient.get('http://example.com/asdf/asdf/asfd');
                ").Value as HttpResponse;
            Assert.AreEqual(404, result.statusCode);
            Assert.AreEqual("Not Found", result.reasonPhrase);


        }
    }
}
