using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests
{
    [TestClass]
    public class CursorTests
    {
        [TestMethod]
        public void First_OfTwoItems()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("cursor", new Cursor(new List<object> {1, 2}));
                var results = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        return cursor.first();
                    }
                    main();
                ");
                
                System.Console.WriteLine(results.ConsoleTest);
                Assert.AreEqual(1.0, results.Value);
            }
        }

        [TestMethod]
        public void Each_OverTwoItems()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("cursor", new Cursor(new List<object> { 1, 2 }));
                var results = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        var items = [];
                        cursor.each(function(item) {items.push(item)});
                        return items;
                    }
                    main();
                ");

                System.Console.WriteLine(results.ConsoleTest);
                Assert.AreEqual(2, ((object[])results.Value).Length);
                Assert.AreEqual(1.0, ((object[])results.Value)[0]);
                Assert.AreEqual(2.0, ((object[])results.Value)[1]);
            }
        }

        [TestMethod]
        public void WhileLoop_OverTwoItems()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("cursor", new Cursor(new List<object> { 1, 2 }));
                var results = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        var items = [];
                        while(cursor.hasNext())
                        {
                            var item = cursor.next();
                            items.push(item);
                            console.log(item);
                        }
                        return items;
                    }
                    main();
                ");

                System.Console.WriteLine(results.ConsoleTest);
                Assert.AreEqual(2, ((object[])results.Value).Length);
                Assert.AreEqual(1.0, ((object[])results.Value)[0]);
                Assert.AreEqual(2.0, ((object[])results.Value)[1]);
            }
        }

        [TestMethod]
        public void Next_OverTwoItems()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("cursor", new Cursor(new List<object> { 1, 2 }));
                var results = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        var items = [];
                        items.push(cursor.next());
                        items.push(cursor.next());
                        return items;
                    }
                    main();
                ");

                System.Console.WriteLine(results.ConsoleTest);
                Assert.AreEqual(2, ((object[])results.Value).Length);
                Assert.AreEqual(1.0, ((object[])results.Value)[0]);
                Assert.AreEqual(2.0, ((object[])results.Value)[1]);
            }
        }

        [TestMethod]
        public void Next_EmptyList_ReturnNull()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("cursor", new Cursor(new List<object> { }));
                var results = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        var items = [];
                        items.push(cursor.next());
                        items.push(cursor.next());
                        return items;
                    }
                    main();
                ");

                System.Console.WriteLine(results.ConsoleTest);
                Assert.AreEqual(2, ((object[])results.Value).Length);
                Assert.AreEqual(null, ((object[])results.Value)[0]);
                Assert.AreEqual(null, ((object[])results.Value)[1]);
            }
        }

        [TestMethod]
        public void HasNext_NextExists()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("cursor", new Cursor(new List<object> { 1, 2 }));
                var results = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        if (cursor.hasNext() != true)
                            throw 'cursor.hasNext() != true';
                        if (cursor.hasNext() != true)
                            throw 'cursor.hasNext() != true';
                        return cursor.next();
                    }
                    main();
                ");

                System.Console.WriteLine(results.ConsoleTest);
                Assert.AreEqual(1.0, results.Value);
            }
        }

        [TestMethod]
        public void HasNext_EmptyList_ReturnFalse()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                engine.Jint.SetValue("cursor", new Cursor(new List<object> { }));
                var results = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        var items = [];
                        items.push(cursor.hasNext());
                        items.push(cursor.hasNext());
                        return items;
                    }
                    main();
                ");

                System.Console.WriteLine(results.ConsoleTest);
                Assert.AreEqual(2, ((object[])results.Value).Length);
                Assert.AreEqual(false, ((object[])results.Value)[0]);
                Assert.AreEqual(false, ((object[])results.Value)[1]);
            }
        }
    }
}
