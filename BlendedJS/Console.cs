using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedJS
{
    public class Console : Dictionary<string,object>
    {
        public int currentLine { get; set; }
        public List<Log> logs { get; private set; }

        public Console()
        {
            logs = new List<Log>();
        }

        public void log(object arg1)
        {
            log(arg1, null);
        }

        public void log(object arg1, object arg2)
        {
            logs.Add(new Log()
            {
                Line = currentLine,
                Arg1 = arg1.ToJsonOrString(),
                Arg2 = arg2.ToJsonOrString()
            });
        }
    }

    public class Log
    {
        public int Line { get; set; }
        public object Arg1 { get; set; }
        public object Arg2 { get; set; }
    }
}
