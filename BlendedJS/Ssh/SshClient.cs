using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentFTP;

namespace BlendedJS.Ssh
{
    public class SshClient : Dictionary<string, object>
    {
        private Renci.SshNet.SshClient _client;
        public SshClient(object options)
        {
            BlendedJSEngine.Clients.Value.Add(this);
            var host = options.GetProperty("host").ToStringOrDefault();
            var port = options.GetProperty("port").ToIntOrDefault();
            var user = options.GetProperty("user").ToStringOrDefault();
            var password = options.GetProperty("password").ToStringOrDefault();

            var connectionInfo = new Renci.SshNet.ConnectionInfo(host, user,
                new Renci.SshNet.PasswordAuthenticationMethod(user, password));
                //new Renci.SshNet.PrivateKeyAuthenticationMethod("rsa.key"));
            _client = new Renci.SshNet.SshClient(connectionInfo);
        }

        public object runCommand(string command)
        {
            List<object> list = new List<object>();
            try
            {
                try
                {
                    _client.Connect();
                }
                catch (Exception ex)
                {
                    throw new Exception("Cannot connect to the host. " + ex.Message, ex);
                }

                _client.RunCommand(command);
            }
            finally
            {
                _client.Disconnect();
            }

            return null;
        }
    }
}
