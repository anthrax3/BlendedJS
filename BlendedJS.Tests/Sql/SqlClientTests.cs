using System;
using System.Collections.Generic;
using System.Text;
using BlendedJS.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Sql
{
    [TestClass]
    public class SqlClientTests
    {
        [TestMethod]
        public void Query_CannotConnectToDb()
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
    }
}
