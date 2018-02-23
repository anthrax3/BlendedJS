using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FluentFTP;

namespace BlendedJS.Ftp
{
    public class FtpClient : Dictionary<string, object>
    {
        private FluentFTP.FtpClient _client;
        public FtpClient(object options)
        {
            _client = new FluentFTP.FtpClient();

            var host = options.GetProperty("host").ToStringOrDefault();
            if (host != null)
                _client.Host = host;

            var port = options.GetProperty("port").ToIntOrDefault();
            if (port != null)
                _client.Port = port.Value;

            var user = options.GetProperty("user").ToStringOrDefault();
            var password = options.GetProperty("password").ToStringOrDefault();
            if (user != null && password != null)
                _client.Credentials = new NetworkCredential(user, password);
        }

        public object[] list()
        {
            return list(null);
        }

        public object[] list(object path)
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

                foreach (FtpListItem item in _client.GetListing(path.ToStringOrDefault()))
                {
                    list.Add(
                        new Dictionary<string, object>
                        {
                            {"name", item.Name},
                            {"fullName", item.FullName},
                            {"type", item.Type.ToStringOrDefault()},
                            {"modified", item.Modified},
                        });
                }
            }
            finally
            {
                _client.Disconnect();
            }

            return list.ToArray();
        }
    }
}
