using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using FluentFTP;
using Renci.SshNet;

namespace BlendedJS.Ssh
{
    public class SshClient : JsObject, IDisposable
    {
        private Renci.SshNet.SshClient _client;
        private ForwardedPortLocal _forwardedPort;
        public SshClient(object options)
        {
            BlendedJSEngine.Clients.Value.Add(this);
            var host = options.GetProperty("host").ToStringOrDefault();
            var port = options.GetProperty("port").ToIntOrDefault(22);
            var user = options.GetProperty("user").ToStringOrDefault();
            var password = options.GetProperty("password").ToStringOrDefault();
            var privateKey = options.GetProperty("privateKey").ToStringOrDefault();

            if (string.IsNullOrEmpty(privateKey) == false)
            {
                byte[] primateKeyBytes = Encoding.UTF8.GetBytes(privateKey);
                PrivateKeyFile primateKeyFile = new PrivateKeyFile(new MemoryStream(primateKeyBytes));
                var connectionInfo = new Renci.SshNet.ConnectionInfo(host, port, user,
                    new PrivateKeyAuthenticationMethod(user, primateKeyFile));
                _client = new Renci.SshNet.SshClient(connectionInfo);
            }
            else
            {
                var connectionInfo = new Renci.SshNet.ConnectionInfo(host, port, user,
                    new PasswordAuthenticationMethod(user, password));
                _client = new Renci.SshNet.SshClient(connectionInfo);
            }
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

        public object command(object command)
        {
            connect();
            return _client.RunCommand(command.ToStringOrDefault());
        }

        public void tunnel(object options)
        {
            //_client.ErrorOccurred += (s, args) => args.Dump();
            //_forwardedPort.Exception += (s, args) => args.Dump();
            //_forwardedPort.RequestReceived += (s, args) => args.Dump();
            _forwardedPort = new ForwardedPortLocal(
                options.GetProperty("boundHost").ToStringOrDefault(),
                (uint)options.GetProperty("boundPort").ToIntOrDefault(),
                options.GetProperty("host").ToStringOrDefault(),
                (uint)options.GetProperty("port").ToIntOrDefault());
            _client.AddForwardedPort(_forwardedPort);
            _forwardedPort.Start();
            connect();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _forwardedPort?.Dispose();
        }
    }
}
