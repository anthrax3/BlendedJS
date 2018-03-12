using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Sftp
{
    public class SftpClient : Dictionary<string, object>, IDisposable
    {
        private Renci.SshNet.SftpClient _client;
        public SftpClient(object options)
        {
            BlendedJSEngine.Clients.Add(this);
            var host = options.GetProperty("host").ToStringOrDefault();
            var port = options.GetProperty("port").ToIntOrDefault();
            var user = options.GetProperty("user").ToStringOrDefault();
            var password = options.GetProperty("password").ToStringOrDefault();

            var connectionInfo = new Renci.SshNet.ConnectionInfo(host, user,
                new Renci.SshNet.PasswordAuthenticationMethod(user, password));
            //new Renci.SshNet.PrivateKeyAuthenticationMethod("rsa.key"));
            _client = new Renci.SshNet.SftpClient(connectionInfo);
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

        public void end()
        {
            _client.Disconnect();
        }

        public object[] list()
        {
            return list(null);
        }

        public object[] list(object path)
        {
            connect();
            List<object> list = new List<object>();
            foreach (var item in _client.ListDirectory(path.ToStringOrDefault()))
            {
                list.Add(new Dictionary<string, object>
                {
                    {"name", item.Name},
                    {"fullName", item.FullName},
                    //{"type", item.Type.ToStringOrDefault()},
                    {"modified", item.LastWriteTimeUtc},
                });
            }
            return list.ToArray();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
