using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BlendedJS.Http
{
    //https://github.com/request/request
    public class HttpClient : Dictionary<string, object>
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
            var url = options.GetProperty("url").ToStringOrDefault();
            if (options is string s)
                url = s;
            if (method == null)
                method = options.GetProperty("method").ToStringOrDefault();
            var body = options.GetProperty("body").ToJsonOrString();
            var headers = method.GetProperty("headers") as IDictionary<string, object>;
            var properties = method.GetProperty("properties") as IDictionary<string, object>;

            var client = new System.Net.Http.HttpClient(
                new HttpClientHandler
                {
                    UseDefaultCredentials = true

                });
            if (headers != null)
            {
                foreach (var headerElement in headers)
                {
                    if (headerElement.Value is object[])
                        client.DefaultRequestHeaders.TryAddWithoutValidation(headerElement.Key, ((object[])headerElement.Value).Select(x => x.ToStringOrDefault()).ToArray());
                    else
                        client.DefaultRequestHeaders.TryAddWithoutValidation(headerElement.Key, headerElement.Value.ToStringOrDefault());
                }
            }
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = new HttpMethod(method)
            };
            if (body != null)
            {
                request.Content = new StringContent(body);
            }
            if (properties != null)
            {
                foreach (var property in properties)
                    request.Properties.Add(property.Key, property.Value);
            }

            var result = client.SendAsync(request).Result;

            return new HttpResponse
            {
                statusCode = (int)result.StatusCode,
                reasonPhrase = result.ReasonPhrase,
                body = result.Content.ReadAsStringAsync().Result,
                headers = ToDictionary(result.Headers)
            };
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
