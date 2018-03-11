using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BlendedJS.Http
{
    //https://github.com/request/request
    public class HttpClient : BaseObject
    {
        public HttpClient()
        {
        }

        public object get(object options)
        {
            return send(options, "GET");
        }

        public object post(object options)
        {
            return send(options, "POST");
        }

        public object put(object options)
        {
            return send(options, "PUT");
        }

        public object delete(object options)
        {
            return send(options, "DELETE");
        }

        public object send(object options)
        {
            return send(options, null);
        }

        private object send(object options, string method)
        {
            string url = options is string s ? s : options.GetProperty("url").ToStringOrDefault();
            method = method ?? options.GetProperty("method").ToStringOrDefault();
            NetworkCredential credential = GetCredential(options);
            bool useDefaultCredentials = options.GetProperty("useDefaultCredentials").ToBoolOrDefault(true);
            var client = new System.Net.Http.HttpClient(new HttpClientHandler
            {
                UseDefaultCredentials = useDefaultCredentials,
                Credentials = credential
            });
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = new HttpMethod(method),
            };
            AddAuthenticationHeader(client, url);
            AddHeaders(client, options.GetProperty("headers") as IDictionary<string, object>);
            AddBody(request, options.GetProperty("body").ToJsonOrString());
            AddProperties(request, options.GetProperty("properties") as IDictionary<string, object>);

            var result = client.SendAsync(request).Result;

            return new HttpResponse
            {
                statusCode = (int)result.StatusCode,
                reasonPhrase = result.ReasonPhrase,
                body = result.Content.ReadAsStringAsync().Result,
                headers = ToDictionary(result.Headers)
            };
        }

        private NetworkCredential GetCredential(object options)
        {
            string userName = options.GetProperty("userName").ToStringOrDefault();
            string password = options.GetProperty("password").ToStringOrDefault();
            string domain = options.GetProperty("domain").ToStringOrDefault();
            if (userName != null && password != null)
            {
                return new NetworkCredential(userName, password, domain);
            }

            return null;
        }

        private void AddAuthenticationHeader(System.Net.Http.HttpClient client, string url)
        {
            Uri parsedUrl = new Uri(url);
            if (string.IsNullOrEmpty(parsedUrl.UserInfo) == false)
            {
                var byteArray = Encoding.ASCII.GetBytes(parsedUrl.UserInfo);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }
        }

        private static void AddHeaders(System.Net.Http.HttpClient client, IDictionary<string, object> headers)
        {
            if (headers != null)
            {
                foreach (var headerElement in headers)
                {
                    if (headerElement.Value is object[])
                        client.DefaultRequestHeaders.TryAddWithoutValidation(headerElement.Key,
                            ((object[]) headerElement.Value).Select(x => x.ToStringOrDefault()).ToArray());
                    else
                        client.DefaultRequestHeaders.TryAddWithoutValidation(headerElement.Key,
                            headerElement.Value.ToStringOrDefault());
                }
            }
        }

        private static void AddBody(HttpRequestMessage request, string body)
        {
            if (body != null)
            {
                request.Content = new StringContent(body);
            }
        }

        private static void AddProperties(HttpRequestMessage request, IDictionary<string, object> properties)
        {
            if (properties != null)
            {
                foreach (var property in properties)
                    request.Properties.Add(property.Key, property.Value);
            }
        }

        private object ToDictionary(HttpResponseHeaders headers)
        {
            Dictionary<string, object> headerObj = new Dictionary<string, object>();
            foreach (var header in headers)
            {
                object value = header.Value.Count() > 1 ? 
                    (object)header.Value.ToArray() :
                    (object)header.Value.FirstOrDefault();
                headerObj[header.Key] = value;
            }
            return headerObj;
        }
    }
}
