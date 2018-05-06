using System.Collections.Generic;

namespace BlendedJS.Http
{
    public class HttpResponse : JsObject
    {
        public object statusCode
        {
            get { return this.GetValueOrDefault2("statusCode"); }
            set { this["statusCode"] = value; }
        }
        public object reasonPhrase
        {
            get { return this.GetValueOrDefault2("reasonPhrase"); }
            set { this["reasonPhrase"] = value; }
        }
        public object headers
        {
            get { return this.GetValueOrDefault2("headers"); }
            set { this["headers"] = value; }
        }

        public object body
        {
            get { return this.GetValueOrDefault2("body"); }
            set { this["body"] = value; }
        }

        public object error
        {
            get { return this.GetValueOrDefault2("error"); }
            set { this["error"] = value; }
        }
    }
}