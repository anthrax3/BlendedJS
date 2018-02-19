using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Sql
{
    [TestClass]
    public class SqlClientTests
    {
        [TestMethod]
        public void Query_Select()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                    var sqlClient = new  SqlClient('Sqlite','Data Source = chinook.db;');
                    sqlClient.query('select * from employees');
                ").Value;
            Assert.IsNotNull(result);
            Assert.AreEqual(8, ((object[])result).Length);
        }
    }
}
