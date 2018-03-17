using System;
using System.Collections.Generic;
using System.Linq;

namespace BlendedJS
{
    public class BlendedJSResult
    {
        public BlendedJSResult()
        {
            Logs = new List<Log>();
        }

        public List<Log> Logs { get; set; }
        public string ConsoleTest {
            get
            {
                var lines = Logs.Select(x => string.Format("Line {0}: {1}", x.Line, x.Arg1));
                return string.Join(Environment.NewLine, lines);
            }
        }
        public object Value { get; set; }
        public Exception Exception { get; internal set; }
        public int LastExecutedLine { get; internal set; }
    }
}