using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Cassandra
{
    [TestClass]
    public class CassandraTests
    {
        [TestMethod]
        public void Execute_QueryWithNoPrameters()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var cassandraClient = new CassandraClient({host:'localhost'});

                    var keyspaceCreated = cassandraClient.execute(""CREATE KEYSPACE IF NOT EXISTS test WITH REPLICATION = { 'class' : 'NetworkTopologyStrategy', 'datacenter1' : 3 };"");
                    console.log(keyspaceCreated);

                    var tableCreated = cassandraClient.execute('CREATE TABLE  IF NOT EXISTS test.employees(id int PRIMARY KEY, name text);');
                    console.log(tableCreated);
                    
                    
                    var rowInserted = cassandraClient.execute(""INSERT INTO test.employees (id, name) VALUES (1, 'daniel');"");
                    console.log(rowInserted);

                    var rows = cassandraClient.execute(""SELECT * FROM test.employees;"");
                    console.log(rows);

                    var tableDroped = cassandraClient.execute('DROP TABLE test.employees;');
                    console.log(tableDroped);

                    rows;
                ");

                System.Console.WriteLine(result.ConsoleTest);
                var items = result.Value as object[];
                Assert.AreEqual(1, items.Length);
                Assert.IsNotNull("daniel", (((IDictionary<string, object>)items[0])["name"]).ToString());
            }
        }

        [TestMethod]
        public void Execute_QueryWithPrameters()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var cassandraClient = new CassandraClient({host:'localhost'});

                    var keyspaceCreated = cassandraClient.execute(""CREATE KEYSPACE IF NOT EXISTS test WITH REPLICATION = { 'class' : 'NetworkTopologyStrategy', 'datacenter1' : 3 };"");
                    console.log(keyspaceCreated);

                    var tableCreated = cassandraClient.execute('CREATE TABLE  IF NOT EXISTS test.employees(id int PRIMARY KEY, name text);');
                    console.log(tableCreated);
                    
                    var id = 1;
                    var name = 'daniel';
                    var rowInserted = cassandraClient.execute(""INSERT INTO test.employees (id, name) VALUES (?, ?);"",
                        [id, name]);
                    console.log(rowInserted);

                    var rows = cassandraClient.execute(""SELECT * FROM test.employees;"");
                    console.log(rows);

                    var tableDroped = cassandraClient.execute('DROP TABLE test.employees;');
                    console.log(tableDroped);

                    rows;
                ");

                System.Console.WriteLine(result.ConsoleTest);
                var items = result.Value as object[];
                Assert.AreEqual(1, items.Length);
                Assert.IsNotNull("daniel", (((IDictionary<string, object>)items[0])["name"]).ToString());
            }
        }

        [TestMethod]
        public void Connect_NotExistingKeySpace()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var cassandraClient = new CassandraClient({host:'localhost'});
                    cassandraClient.connect('NotExists');
                ");
                System.Console.WriteLine(result.ConsoleTest);
                Assert.IsTrue(result.ConsoleTest.Contains("Keyspace 'NotExists' does not exist"));
            }
        }

        [TestMethod]
        public void Connect_NoError()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var cassandraClient = new CassandraClient({host:'localhost'});
                    cassandraClient.connect();
                ");
                System.Console.WriteLine(result.ConsoleTest);
                Assert.IsTrue(string.IsNullOrEmpty(result.ConsoleTest));
            }
        }

    }
}
