using System;
using System.Collections.Generic;
using System.Linq;

namespace BlendedJS
{
    public class BlendedJSResult
    {
        public BlendedJSResult()
        {
            Console = new List<Log>();
        }

        public List<Log> Console { get; set; }
        public string ConsoleTest {
            get
            {
                var lines = Console.Select(x => string.Format("{0}: {1}", x.Line, x.Message));
                return string.Join(Environment.NewLine, lines);
            }
        }
        public object Value { get; set; }
    }
}