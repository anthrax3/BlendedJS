using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS
{
    public class JsObject : Dictionary<string, object>
    {
        public JsObject()
        {
        }

        public JsObject(IDictionary<string, object> dictionary) : base(dictionary)
        {
        }

        public override string ToString()
        {
            if (this.GetType() == typeof(JsObject))
                return "[object Object]";

            return "[object " + this.GetType().Name + "]";
        }
    }
}
