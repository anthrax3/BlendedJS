using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.ElasticSearch
{
    [TestClass]
    public class ElasticSearchTests
    {
        [TestMethod]
        public void CreateIndex_CreateItem_ReturnAllItems()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var httpClient = new HttpClient();
                    var indexExists = httpClient.head('http://paas:4ab1a233154b398346e5e56175fce2e6@gloin-eu-west-1.searchly.com/twitter');
                    console.log('Index exists:');
                    console.log(indexExists);

                    if (indexExists.statusCode != 200)
                    {
                        var indexCreated = httpClient.put({
                            url:'http://paas:4ab1a233154b398346e5e56175fce2e6@gloin-eu-west-1.searchly.com/twitter',
                            body: {'settings' : {'index' : { 'number_of_shards' : 3, 'number_of_replicas' : 2 }}}
                        });
                        console.log('Index created:');
                        console.log(indexCreated);
                    }

                    var itemCreated = httpClient.post({
                        url:'http://paas:4ab1a233154b398346e5e56175fce2e6@gloin-eu-west-1.searchly.com/twitter/_doc/1',
                        headers: {'Content-Type':'application/json'},
                        body: {'user':'kimchy', 'post_date':'2009-11-15T14:12:12', 'message':'trying out Elasticsearch'}
                    });
                    console.log('item created');
                    console.log(itemCreated);

                    var matchAllItems = httpClient.post({
                        url:'http://paas:4ab1a233154b398346e5e56175fce2e6@gloin-eu-west-1.searchly.com/twitter/_search',
                        headers: {'Content-Type':'application/json'},
                        body: {'query': {'match_all': { }}}
                    });
                    console.log(matchAllItems);

                    var matchAllItemsBody = JSON.parse(matchAllItems.body);
                    var matchAllItemsHits = matchAllItemsBody.hits.hits.map(function(a) {return a['_source'];});;
                    console.log(matchAllItemsHits);
                    matchAllItemsHits;
                ");

                System.Console.WriteLine(result.ConsoleTest);
                var items = result.Value as object[];
                Assert.AreEqual(1, items.Length);
                Assert.IsNotNull("kimchy", (((IDictionary<string,object>)items[0])["user"]).ToString());
            }
        }
    }
}
