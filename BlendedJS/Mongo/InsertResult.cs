using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo
{
    public class InsertResult :  Dictionary<string, object>
    {
        public object acknowledged
        {
            get { return this.GetValueOrDefault("acknowledged"); }
            set { this["acknowledged"] = value; }
        }
        public object insertedId
        {
            get { return this.GetValueOrDefault("insertedId"); }
            set { this["insertedId"] = value; }
        }
        public object insertedIds
        {
            get { return this.GetValueOrDefault("insertedIds"); }
            set { this["insertedIds"] = value; }
        }
    }
}
