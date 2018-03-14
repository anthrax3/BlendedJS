using System.Collections.Generic;

namespace BlendedJS
{
    public sealed class Object : BaseObject
    {
        public Object() { }
        public Object(IDictionary<string, object> dictionary) :base(dictionary)
        {
        }
    }
}
