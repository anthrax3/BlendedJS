using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentFTP;

namespace BlendedJS.Ssh
{
    public class SshClient : BaseObject, IDisposable
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

        public void connect()
        {
            try
            {
                if (_client.IsConnected == false)
                    _client.Connect();
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot connect to the host. " + ex.Message, ex);
            }
        }

        public object command(string command)
        {
            List<object> list = new List<object>();
            connect();
            _client.RunCommand(command);
            return list;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
