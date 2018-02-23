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
                Message = arg1.ToJsonOrString(),
                Json = arg2.ToJsonOrString()
            });
        }
    }

    public class Log
    {
        public int Line { get; set; }
        public string Message { get; set; }
        public object Json { get; set; }
    }
}
