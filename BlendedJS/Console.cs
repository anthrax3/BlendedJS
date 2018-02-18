using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedJS
{
    public interface IConsole
    {
        void Log(object message);
        void Log(object message, object json);
        List<Log> Logs { get; }
        int CurrentLine { get; set; }
    }

    public class Console : IConsole
    {
        public int CurrentLine { get; set; }
        public List<Log> Logs { get; private set; }

        public Console()
        {
            Logs = new List<Log>();
        }

        public void Log(object arg1)
        {
            Log(arg1, null);
        }

        public void Log(object arg1, object arg2)
        {
            Logs.Add(new Log()
            {
                Line = CurrentLine,
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
