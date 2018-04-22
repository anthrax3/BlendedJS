using System;
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
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    function main()
                    {
                        var httpClient = new HttpClient();
                        var response = httpClient.get({url:'https://www.theguardian.com'});
                        console.log(response);
                        return response;
                    }
                    main();
                ");
            System.Console.WriteLine(result.ConsoleTest);
            HttpResponse response = result.Value as HttpResponse;
            Assert.AreEqual(200, response.statusCode);
            Assert.AreEqual("OK", response.reasonPhrase);
            Assert.IsTrue(response.body.ToString().Contains("theguardian"));
            Assert.AreEqual("keep-alive", response.headers.GetProperty("Connection"));
        }

        [TestMethod]
        public void Get_Json()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    function main()
                    {
                        var httpClient = new HttpClient();
                        var response = httpClient.get({
                            url:'https://jsonplaceholder.typicode.com/posts',
                            headers: {'Content-Type':'application/json'}
                        });
                        response.bodyJson = JSON.parse(response.body);                        
                        console.log(response);
                        return response;
                    }
                    main();
                ");
            System.Console.WriteLine(result.ConsoleTest);
            HttpResponse response = result.Value as HttpResponse;
            Assert.AreEqual(200, response.statusCode);
            Assert.AreEqual("OK", response.reasonPhrase);
            object[] posts = response.GetProperty("bodyJson") as object[];
            Assert.AreEqual((double)1, posts[0].GetProperty("userId"));
        }

        [TestMethod]
        public void Get_Json_BySpecifyingJustUrl()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    function main()
                    {
                        var httpClient = new HttpClient();
                        var response = httpClient.get('https://jsonplaceholder.typicode.com/posts');
                        response.bodyJson = JSON.parse(response.body);                        
                        console.log(response);
                        return response;
                    }
                    main();
                ");
            System.Console.WriteLine(result.ConsoleTest);
            HttpResponse response = result.Value as HttpResponse;
            Assert.AreEqual(200, response.statusCode);
            Assert.AreEqual("OK", response.reasonPhrase);
            object[] posts = response.GetProperty("bodyJson") as object[];
            Assert.AreEqual((double)1, posts[0].GetProperty("userId"));
        }

        [TestMethod]
        public void Post_Json()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    function main()
                    {
                        var httpClient = new HttpClient();
                        var response = httpClient.post({
                            url:'https://jsonplaceholder.typicode.com/posts',
                            headers: {'Content-Type':'application/json'},
                            body: { userId: 1, title: 'bla', body: 'bla bla bla'}
                        });
                        response.bodyJson = JSON.parse(response.body);                        
                        console.log(response);
                        return response;
                    }
                    main();
                ");
            System.Console.WriteLine(result.ConsoleTest);
            HttpResponse response = result.Value as HttpResponse;
            Assert.AreEqual(201, response.statusCode);
            Assert.AreEqual("Created", response.reasonPhrase);
            object created = response.GetProperty("bodyJson");
            Assert.AreEqual((double)101, (double)created.GetProperty("id"));
        }

        [TestMethod]
        public void Put_Json()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    function main()
                    {
                        var httpClient = new HttpClient();
                        var response = httpClient.put({
                            url:'https://jsonplaceholder.typicode.com/posts/1',
                            headers: {'Content-Type':'application/json'},
                            body: { userId: 1, title: 'bla', body: 'bla bla bla'}
                        });
                        response.bodyJson = JSON.parse(response.body);                        
                        console.log(response);
                        return response;
                    }
                    main();
                ");
            System.Console.WriteLine(result.ConsoleTest);
            HttpResponse response = result.Value as HttpResponse;
            Assert.AreEqual(200, response.statusCode);
            Assert.AreEqual("OK", response.reasonPhrase);
            object updated = response.GetProperty("bodyJson");
            Assert.AreEqual((double)1, (double)updated.GetProperty("id"));
        }

        [TestMethod]
        public void Delete_Json()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    function main()
                    {
                        var httpClient = new HttpClient();
                        var response = httpClient.delete({
                            url:'https://jsonplaceholder.typicode.com/posts/1'
                        });
                        response.bodyJson = JSON.parse(response.body);                        
                        console.log(response);
                        return response;
                    }
                    main();
                ");
            System.Console.WriteLine(result.ConsoleTest);
            HttpResponse response = result.Value as HttpResponse;
            Assert.AreEqual(200, response.statusCode);
            Assert.AreEqual("OK", response.reasonPhrase);
            Assert.AreEqual("{}", response.body);
            object deleted = response.GetProperty("bodyJson");
            Assert.IsNotNull(deleted);
        }

        [TestMethod]
        public void Head_Json()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    function main()
                    {
                        var httpClient = new HttpClient();
                        var response = httpClient.head({
                            url:'https://jsonplaceholder.typicode.com/posts/1'
                        });
                        console.log(response);
                        return response;
                    }
                    main();
                ");
            System.Console.WriteLine(result.ConsoleTest);
            HttpResponse response = result.Value as HttpResponse;
            Assert.AreEqual(200, response.statusCode);
            Assert.AreEqual("OK", response.reasonPhrase);
            Assert.AreEqual("", response.body);
        }

        [TestMethod]
        public void Send_Head()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    function main()
                    {
                        var httpClient = new HttpClient();
                        var response = httpClient.send({
                            url:'https://jsonplaceholder.typicode.com/posts/1',
                            method:'head'
                        });
                        console.log(response);
                        return response;
                    }
                    main();
                ");
            System.Console.WriteLine(result.ConsoleTest);
            HttpResponse response = result.Value as HttpResponse;
            Assert.AreEqual(200, response.statusCode);
            Assert.AreEqual("OK", response.reasonPhrase);
            Assert.AreEqual("", response.body);
        }

        [TestMethod]
        public void Get_UrlIsInvalid_ThrowError()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
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
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    function main()
                    {
                        var httpClient = new HttpClient();
                        var response = httpClient.get('http://example.com/asdf/asdf/asfd');
                        console.log(response);
                        return response;
                    }
                    main();
                ");
            System.Console.WriteLine(result.ConsoleTest);
            HttpResponse response = result.Value as HttpResponse;
            Assert.AreEqual(404, response.statusCode);
            Assert.AreEqual("Not Found", response.reasonPhrase);
        }
    }
}
