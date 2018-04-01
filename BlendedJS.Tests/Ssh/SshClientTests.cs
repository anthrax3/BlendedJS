using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Tests.Ssh
{
    [TestClass]
    public class SshClientTests
    {
        [TestMethod]
        public void Tunnel()
        {
            BlendedJSEngine engine = new BlendedJSEngine();
            var result = engine.ExecuteScript(
                @"
                    var sshClient = new SshClient({host:'',user:'', privateKey:''});
                    sshClient.tunnel();
                ");
            Assert.IsNull(result.Value);
        }
    }
}
