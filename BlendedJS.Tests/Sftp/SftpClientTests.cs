using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlendedJS.Tests.Sftp
{
    [TestClass]
    public class SftpClientTests
    {
        [TestMethod]
        public void List_CannotConnectToTheHost()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                try {
                    var sftpClient = new SftpClient({host:'example.com', user:'demo', password:'password'});
                    sftpClient.list();
                } catch(err) {
                    console.log(err);
                }
                ");

                Assert.IsNull(result.Value);
                Assert.IsTrue(result.ConsoleTest.Contains("Cannot connect to the host."));
            }
        }

        [TestMethod]
        public void List_ReturnsFiles()
        {
            using (BlendedJSEngine engine = new BlendedJSEngine())
            {
                var result = engine.ExecuteScript(
                    @"
                    var sftpClient = new SftpClient({host:'test.rebex.net', user:'demo', password:'password'});
                    sftpClient.list('/');
                ");

                Assert.IsNotNull(result.Value);
            }
        }
    }
}
