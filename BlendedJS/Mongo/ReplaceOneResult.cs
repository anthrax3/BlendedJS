using MongoDB.Bson;
using System.Collections.Generic;

namespace BlendedJS.Mongo
{
    public class ReplaceOneResult : Dictionary<string, object>
    {
        public ReplaceOneResult(MongoDB.Driver.ReplaceOneResult mongoUpdateResult)
        {
            acknowledged = mongoUpdateResult.IsAcknowledged;
            modifiedCount = mongoUpdateResult.ModifiedCount;
            matchedCount = mongoUpdateResult.MatchedCount;
            upsertedId = mongoUpdateResult.UpsertedId;
        }

        public object acknowledged
        {
            get { return this.GetValueOrDefault2("acknowledged"); }
            set { this["acknowledged"] = value; }
        }

        public object modifiedCount
        {
            get { return this.GetValueOrDefault2("modifiedCount"); }
            set { this["modifiedCount"] = value; }
        }

        public object matchedCount
        {
            get { return this.GetValueOrDefault2("matchedCount"); }
            set { this["matchedCount"] = value; }
        }

        public object upsertedId
        {
            get { return this.GetValueOrDefault2("upsertedId"); }
            set { this["upsertedId"] = value; }
        }
    }
    
}
