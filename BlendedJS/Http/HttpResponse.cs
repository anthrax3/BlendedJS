using System.Collections.Generic;

namespace BlendedJS.Http
{
    public class HttpResponse : Dictionary<string, object>
    {
        public object statusCode
        {
            get { return this.GetValueOrDefault("statusCode"); }
            set { this["statusCode"] = value; }
        }
        public object reasonPhrase
        {
            get { return this.GetValueOrDefault("reasonPhrase"); }
            set { this["reasonPhrase"] = value; }
        }
        public object headers
        {
            get { return this.GetValueOrDefault("headers"); }
            set { this["headers"] = value; }
        }

        public object body
        {
            get { return this.GetValueOrDefault("body"); }
            set { this["body"] = value; }
        }

        public object error
        {
            get { return this.GetValueOrDefault("error"); }
            set { this["error"] = value; }
        }
    }
}