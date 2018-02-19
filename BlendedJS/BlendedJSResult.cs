using System.Collections.Generic;

namespace BlendedJS
{
    public class BlendedJSResult
    {
        public BlendedJSResult()
        {
            Output = new List<Log>();
        }

        public List<Log> Output { get; set; }
        public object Value { get; set; }
    }
}