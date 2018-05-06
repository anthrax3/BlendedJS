using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Mongo
{
    public class InsertResult : JsObject
    {
        public object acknowledged
        {
            get { return this.GetValueOrDefault2("acknowledged"); }
            set { this["acknowledged"] = value; }
        }
        public object insertedId
        {
            get { return this.GetValueOrDefault2("insertedId"); }
            set { this["insertedId"] = value; }
        }
        public object insertedIds
        {
            get { return this.GetValueOrDefault2("insertedIds"); }
            set { this["insertedIds"] = value; }
        }
    }
}
