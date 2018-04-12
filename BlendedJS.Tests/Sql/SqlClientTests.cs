using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlendedJS.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Sql
{
    [TestClass]
    public class SqlClientTests
    {
        [TestMethod]
        public void Connection_ProviderNotSpecified()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    try {
                        var sqlClient = new  SqlClient();
                        sqlClient.query('select * from employees');
                    } catch(err) {
                        console.log(err);
                    }
                ");
                Assert.IsNull(result.Value);
                Assert.IsTrue(result.ConsoleTest.Contains("provider and connectionString have to be specified"));
            }
        }

        [TestMethod]
        public void Connection_ConnectionStringNotSpecified()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    try {
                        var sqlClient = new  SqlClient({provider:'SqlServer'});
                        sqlClient.query('select * from employees');
                    } catch(err) {
                        console.log(err);
                    }
                ");
                Assert.IsNull(result.Value);
                Assert.IsTrue(result.ConsoleTest.Contains("provider and connectionString have to be specified"));
            }
        }

        [TestMethod]
        public void Connection_NotSupportedProvider()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    try {
                        var sqlClient = new  SqlClient({provider:'bla bla', connectionString:'server:...'});
                        sqlClient.query('select * from employees');
                    } catch(err) {
                        console.log(err);
                    }
                ");
                Assert.IsNull(result.Value);
                Assert.IsTrue(result.ConsoleTest.Contains("Not supported provider:"));
            }
        }


        [TestMethod]
        public void Connection_CannotConnectToDb()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    try {
                        var sqlClient = new  SqlClient({provider:'SqlServer',connectionString:'Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;'});
                        sqlClient.query('select * from employees');
                    } catch(err) {
                        console.log(err);
                    }
                ");
                Assert.IsNull(result.Value);
                Assert.IsTrue(result.ConsoleTest.Contains("Cannot connect to the database. A network-related"));
            }
        }

        [TestMethod]
        public void Query_SyntaxError()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    try {
                        var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                        sqlClient.query('select adsf employees');
                    } catch(err) {
                        console.log(err);
                    }
                ");
                Assert.IsNull(result.Value);
                Assert.IsTrue(result.ConsoleTest.Contains("Cannot run the query. "));
            }
        }

        [TestMethod]
        public void Query_SelectRows()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                    sqlClient.query('select * from employees');
                ");
                Assert.IsNotNull(result);
                Assert.AreEqual(8, ((object[]) result.Value).Length);
            }
        }

        [TestMethod]
        public void Query_SelectWithParameters()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                    sqlClient.query({
                        sql:'select * from employees where EmployeeId=@EmployeeId', 
                        parameters:{EmployeeId:1}
                    });
                ");
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
            }
        }

        [TestMethod]
        public void Query_UpdateOneItem()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                    sqlClient.query('update employees set FirstName = ""Kowalski"" where EmployeeId=1');
                ");
                Assert.IsNotNull(result);
                Assert.AreEqual(1.0, result.Value);
            }
        }

        [TestMethod]
        public void Query_UpdateZeroItems()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                    sqlClient.query('update employees set FirstName = ""Kowalski"" where EmployeeId=123123');
                ");
                Assert.IsNotNull(result);
                Assert.AreEqual(0.0, result.Value);
            }
        }

        [TestMethod]
        public void Cursor_Each()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                        var cursor = sqlClient.cursor('select * from employees');
                        var items = [];
                        cursor.each(function(item) {items.push(item)});
                        return items;
                    }
                    main();
                ");
                Assert.IsNotNull(result);
                Assert.AreEqual(8, ((object[])result.Value).Length);
                IDictionary<string, object> firstRecord = ((object[])result.Value)[0] as IDictionary<string, object>;
                Assert.AreEqual((Int64)1, firstRecord["EmployeeId"]);
            }
        }

        [TestMethod]
        public void Cursor_WhileLoop()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                        var cursor = sqlClient.cursor('select * from employees');
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
                Assert.IsNotNull(result);
                System.Console.WriteLine(result.ConsoleTest);

                Assert.AreEqual(8, ((object[])result.Value).Length);
                IDictionary<string, object> firstRecord = ((object[])result.Value)[0] as IDictionary<string, object>;
                Assert.AreEqual((Int64)1, firstRecord["EmployeeId"]);
            }
        }

        [TestMethod]
        public void Cursor_Run2()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                        var employees = sqlClient.cursor('select * from employees');
                        var albums = sqlClient.cursor('select * from albums');
                        var items = [];
                        while(albums.hasNext())
                        {
                            var item = albums.next();
                            items.push(item);
                            console.log(item);
                        }
                        while(employees.hasNext())
                        {
                            var item = employees.next();
                            items.push(item);
                            console.log(item);
                        }
                        return items;
                    }
                    main();
                ");
                Assert.IsNotNull(result);
                System.Console.WriteLine(result.ConsoleTest);

                Assert.IsTrue(((object[])result.Value).Length  > 20);
            }
        }

        [TestMethod]
        public void Cursor_First()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                        var cursor = sqlClient.cursor('select * from employees');
                        return cursor.first();
                    }
                    main();
                ");
                Assert.IsNotNull(result);
                IDictionary<string, object> firstRecord = result.Value as IDictionary<string, object>;
                Assert.AreEqual((Int64)1, firstRecord["EmployeeId"]);
            }
        }

        [TestMethod]
        public void Query_Sqlite_ConnectionString()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data source=test.db;'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(Convert.ToInt64(1), ((object[]) result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Query_Sqlite_ConnectionProperties()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({
                        provider:'Sqlite',
                        dataSource:'test.db'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(Convert.ToInt64(1), ((object[]) result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Query_MySql_ConnectionUrl()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({connectionUrl:'mysql://b87e93ab08ac48:9f358192@eu-cdbr-west-02.cleardb.net/heroku_dc6ceea567ee53d?reconnect=true'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Query_MySql_ConnectionString()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({provider:'MySql',connectionString:'SERVER=eu-cdbr-west-02.cleardb.net;DATABASE=heroku_dc6ceea567ee53d;UID=b87e93ab08ac48;PASSWORD=9f358192;'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Query_MySql_ConnectionProperties()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({
                        provider:'MySql',
                        server:'eu-cdbr-west-02.cleardb.net',
                        database:'heroku_dc6ceea567ee53d',
                        user:'b87e93ab08ac48',
                        password:'9f358192'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Query_Odbc_ConnectionString()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({provider:'Odbc',connectionString:'Driver={MySQL ODBC 5.1 Driver};SERVER=eu-cdbr-west-02.cleardb.net;DATABASE=heroku_dc6ceea567ee53d;UID=b87e93ab08ac48;PASSWORD=9f358192;'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Query_Odbc_ConnectionProperties()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({
                        provider:'Odbc',
                        server:'eu-cdbr-west-02.cleardb.net',
                        database:'heroku_dc6ceea567ee53d',
                        driver:'{MySQL ODBC 5.1 Driver}',
                        user:'b87e93ab08ac48',
                        password:'9f358192'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Query_MariaDb_ConnectionUrl()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({connectionUrl:'mariadb://a92wi271nqdylv7v:cwfjedtktiywq2ul@rmspavs8mpub7dkq.chr7pe7iynqr.eu-west-1.rds.amazonaws.com/xy39fg5tb0y2wim0'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Cursor_MariaDb()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    function main()
                    {
                        var sqlClient = new  SqlClient({connectionUrl:'mariadb://a92wi271nqdylv7v:cwfjedtktiywq2ul@rmspavs8mpub7dkq.chr7pe7iynqr.eu-west-1.rds.amazonaws.com/xy39fg5tb0y2wim0'});
                        try {sqlClient.query('drop table employees');} catch(e){}
                        sqlClient.query('create table employees (ID int, Name varchar(255))');
                        sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                        var employees1 = sqlClient.cursor('select * from employees');
                        employees1.close();
                        var employees2 = sqlClient.cursor('select * from employees');
                        var items = [];
                        while(employees2.hasNext())
                        {
                            var item = employees2.next();
                            items.push(item);
                            console.log(item);
                        }
                        return items;
                    }
                    main();
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[])result.Value).Length);
                Assert.AreEqual(1, ((object[])result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[])result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Query_MariaDb_ConnectionString()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({provider:'MariaDb',connectionString:'SERVER=rmspavs8mpub7dkq.chr7pe7iynqr.eu-west-1.rds.amazonaws.com;DATABASE=xy39fg5tb0y2wim0;UID=a92wi271nqdylv7v;PASSWORD=cwfjedtktiywq2ul;'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Query_MariaDb_ConnectionProperties()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({
                        provider:'MariaDb',
                        server:'rmspavs8mpub7dkq.chr7pe7iynqr.eu-west-1.rds.amazonaws.com',
                        database:'xy39fg5tb0y2wim0',
                        user:'a92wi271nqdylv7v',
                        password:'cwfjedtktiywq2ul'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("ID"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("Name"));
            }
        }

        [TestMethod]
        public void Query_Postgres_ConnectionUrl()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({connectionUrl:'postgres://lcwvemac:Vm1Kbfp9XqaKsn1f1fLaHkrD0NipmIUQ@baasu.db.elephantsql.com:5432/lcwvemac'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("id"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("name"));
            }
        }

        [TestMethod]
        public void Query_Postgres_ConnectionString()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({provider:'Postgres',connectionString:'SERVER=baasu.db.elephantsql.com;DATABASE=lcwvemac;UID=lcwvemac;PASSWORD=Vm1Kbfp9XqaKsn1f1fLaHkrD0NipmIUQ;'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("id"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("name"));
            }
        }

        [TestMethod]
        public void Query_Postgres_ConnectionProperties()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sqlClient = new  SqlClient({
                        provider:'Postgres',
                        server:'baasu.db.elephantsql.com',
                        database:'lcwvemac',
                        user:'lcwvemac',
                        password:'Vm1Kbfp9XqaKsn1f1fLaHkrD0NipmIUQ'});
                    try {sqlClient.query('drop table employees');} catch(e){}
                    sqlClient.query('create table employees (ID int, Name varchar(255))');
                    sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
                    sqlClient.query('select * from employees');
                ");

                result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
                Assert.IsNotNull(result);
                Assert.AreEqual(1, ((object[]) result.Value).Length);
                Assert.AreEqual(1, ((object[]) result.Value)[0].GetProperty("id"));
                Assert.AreEqual("daniel", ((object[]) result.Value)[0].GetProperty("name"));
            }
        }

        //[TestMethod]
        //public void Query_Oracle_ConnectionString()
        //{
        //    BlendedJSEngine engine = new BlendedJSEngine();
        //    var result = engine.ExecuteScript(
        //        @"
        //            var sqlClient = new  SqlClient({provider:'Postgres',connectionString:'SERVER=ec2-54-247-125-137.eu-west-1.compute.amazonaws.com;DATABASE=d2q3au6llp06iq;UID=hqaloscirrxbzv;PASSWORD=2d9365e44a936b90a94d54eea727f154be2aaf0ac8b51259b36bf5890eea78e1;'});
        //            try {sqlClient.query('drop table employees');} catch(e){}
        //            sqlClient.query('create table employees (ID int, Name varchar(255))');
        //            sqlClient.query(""insert INTO  employees (ID,Name) VALUES (1, 'daniel')"");
        //            sqlClient.query('select * from employees');
        //        ");

        //    result.Logs.ForEach(x => System.Console.WriteLine(x.Arg1));
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, ((object[])result.Value).Length);
        //    Assert.AreEqual(1, ((object[])result.Value)[0].GetProperty("ID"));
        //    Assert.AreEqual("daniel", ((object[])result.Value)[0].GetProperty("Name"));
        //}
    }
}
