using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Ftp
{
    [TestClass]
    public class FtpClientTests
    {
        [TestMethod]
        public void List_CannotConnectToTheHost()
        {
            BlendedJSEngine mongo = new BlendedJSEngine();
            var result = mongo.ExecuteScript(
                @"
                try {
                    var ftpClient = new FtpClient({host:'example.com', user:'demo', password:'password'});
                    ftpClient.list();
                } catch(err) {
                    console.log(err);
                }
                ");

            Assert.IsNull(result.Value);
            Assert.IsTrue(result.ConsoleTest.Contains("Cannot connect to the host."));
        }
    }
}
