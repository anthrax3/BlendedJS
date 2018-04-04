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
            BlendedJSEngine engine = new BlendedJSEngine();
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

        [TestMethod]
        public void Connection_ConnectionStringNotSpecified()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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

        [TestMethod]
        public void Connection_NotSupportedProvider()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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


        [TestMethod]
        public void Connection_CannotConnectToDb()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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

        [TestMethod]
        public void Query_SyntaxError()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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

        [TestMethod]
        public void Query_SelectRows()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                    sqlClient.query('select * from employees');
                ");
            Assert.IsNotNull(result);
            Assert.AreEqual(8, ((object[])result.Value).Length);
        }

        [TestMethod]
        public void Query_SelectWithParameters()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                    sqlClient.query({
                        sql:'select * from employees where EmployeeId=@EmployeeId', 
                        parameters:{EmployeeId:1}
                });
                ");
            Assert.IsNotNull(result);
            Assert.AreEqual(1, ((object[])result.Value).Length);
        }

        [TestMethod]
        public void Query_UpdateOneItem()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                    sqlClient.query('update employees set FirstName = ""Kowalski"" where EmployeeId=1');
                ");
            Assert.IsNotNull(result);
            Assert.AreEqual(1.0, result.Value);
        }

        [TestMethod]
        public void Query_UpdateZeroItems()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
                    sqlClient.query('update employees set FirstName = ""Kowalski"" where EmployeeId=123123');
                ");
            Assert.IsNotNull(result);
            Assert.AreEqual(0.0, result.Value);
        }

        [TestMethod]
        public void Query_MySql_ConnectionString()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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
            Assert.AreEqual(1, ((object[])result.Value).Length);
            Assert.AreEqual(1, ((object[])result.Value)[0].GetProperty("ID"));
            Assert.AreEqual("daniel", ((object[])result.Value)[0].GetProperty("Name"));
        }

        [TestMethod]
        public void Query_MySql_ConnectionProperties()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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
            Assert.AreEqual(1, ((object[])result.Value).Length);
            Assert.AreEqual(1, ((object[])result.Value)[0].GetProperty("ID"));
            Assert.AreEqual("daniel", ((object[])result.Value)[0].GetProperty("Name"));
        }

        [TestMethod]
        public void Query_Odbc_ConnectionString()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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
            Assert.AreEqual(1, ((object[])result.Value).Length);
            Assert.AreEqual(1, ((object[])result.Value)[0].GetProperty("ID"));
            Assert.AreEqual("daniel", ((object[])result.Value)[0].GetProperty("Name"));
        }

        [TestMethod]
        public void Query_Odbc_ConnectionProperties()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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
            Assert.AreEqual(1, ((object[])result.Value).Length);
            Assert.AreEqual(1, ((object[])result.Value)[0].GetProperty("ID"));
            Assert.AreEqual("daniel", ((object[])result.Value)[0].GetProperty("Name"));
        }

        [TestMethod]
        public void Query_MariaDb_ConnectionString()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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
            Assert.AreEqual(1, ((object[])result.Value).Length);
            Assert.AreEqual(1, ((object[])result.Value)[0].GetProperty("ID"));
            Assert.AreEqual("daniel", ((object[])result.Value)[0].GetProperty("Name"));
        }

        [TestMethod]
        public void Query_MariaDb_ConnectionProperties()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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
            Assert.AreEqual(1, ((object[])result.Value).Length);
            Assert.AreEqual(1, ((object[])result.Value)[0].GetProperty("ID"));
            Assert.AreEqual("daniel", ((object[])result.Value)[0].GetProperty("Name"));
        }

        [TestMethod]
        public void Query_Postgres_ConnectionString()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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
            Assert.AreEqual(1, ((object[])result.Value).Length);
            Assert.AreEqual(1, ((object[])result.Value)[0].GetProperty("id"));
            Assert.AreEqual("daniel", ((object[])result.Value)[0].GetProperty("name"));
        }

        [TestMethod]
        public void Query_Postgres_ConnectionProperties()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
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
            Assert.AreEqual(1, ((object[])result.Value).Length);
            Assert.AreEqual(1, ((object[])result.Value)[0].GetProperty("id"));
            Assert.AreEqual("daniel", ((object[])result.Value)[0].GetProperty("name"));
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
