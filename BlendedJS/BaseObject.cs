using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS
{
    public class BaseObject : Dictionary<string, object>
    {
        public BaseObject()
        {
        }

        public BaseObject(IDictionary<string, object> dictionary) : base(dictionary)
        {
        }

        public override string ToString()
        {
            return "[object " + this.GetType().Name + "]";
        }
    }
}
