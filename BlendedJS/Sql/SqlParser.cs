using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BlendedJS.Sql
{
    public class SqlParser
    {
        public List<string> FindParameters(string sql)
        {
            List<string> parameters = new List<string>();
            var matches = new Regex("(@[A-Za-z1-9_]*)", RegexOptions.IgnoreCase).Matches(sql);
            foreach (var match in matches)
            {
                parameters.Add(match.ToString().Substring(1));
            }
            return parameters;
        }
    }
}
