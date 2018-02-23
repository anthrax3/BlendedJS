using System;
using System.Collections.Generic;
using System.Text;
using BlendedJS.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Sql
{
    [TestClass]
    public class SqlParserTests
    {
        [TestMethod]
        public void FindParameters_NoParameter()
        {
            SqlParser sqlParser = new SqlParser();
            var paramters = sqlParser.FindParameters("SELECT * FROM Users");

            Assert.AreEqual(0, paramters.Count);
        }

        [TestMethod]
        public void FindParameters_WithOneParameter()
        {
            SqlParser sqlParser = new SqlParser();
            var parameters = sqlParser.FindParameters("SELECT * FROM Users WHERE Id = @Id");

            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual("Id", parameters[0]);
        }

        [TestMethod]
        public void FindParameters_WithTwoParameters()
        {
            SqlParser sqlParser = new SqlParser();
            var parameters = sqlParser.FindParameters("SELECT * FROM Users WHERE Id = @Id AND Name = @Name");

            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual("Id", parameters[0]);
            Assert.AreEqual("Name", parameters[1]);
        }
        [TestMethod]
        public void FindParameters_WithTwoParametersWhenInsert()
        {
            SqlParser sqlParser = new SqlParser();
            var parameters = sqlParser.FindParameters(@"INSERT INTO [dbo].[DimCustomer]
                      ([GeographyKey]
                       ,[CustomerAlternateKey])
                 VALUES (
                       @GeographyKey
                       ,@CustomerAlternateKey)
            ");

            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual("GeographyKey", parameters[0]);
            Assert.AreEqual("CustomerAlternateKey", parameters[1]);
        }
    }
}
