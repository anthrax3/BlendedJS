using System;
using System.Collections.Generic;

namespace BlendedJS
{
    public class Result
    {
        public Result()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
            Info = new List<string>();
        }

        public bool HasErrors { get { return Errors.Count > 0; } }
        public List<string> Errors { get; set; }
        public bool HasWarnings { get { return Warnings.Count > 0; } }
        public List<string> Warnings { get; set; }
        public bool HasInfo { get { return Info.Count > 0; } }
        public List<string> Info { get; set; }
        public bool HasAny { get { return HasErrors || HasWarnings || HasInfo; } }

        public void AppendResults(Result results)
        {
            Errors.AddRange(results.Errors);
            Warnings.AddRange(results.Warnings);
            Info.AddRange(results.Info);
        }
    }
}
