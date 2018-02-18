using MongoDB.Bson;
using System.Collections.Generic;

namespace BlendedJS.Mongo
{
    public class UpdateResult : Dictionary<string, object>
    {
        public UpdateResult(MongoDB.Driver.UpdateResult mongoUpdateResult)
        {
            acknowledged = mongoUpdateResult.IsAcknowledged;
            modifiedCount = mongoUpdateResult.ModifiedCount;
            matchedCount = mongoUpdateResult.MatchedCount;
            upsertedId = mongoUpdateResult.UpsertedId;
        }

        public object acknowledged
        {
            get { return this.GetValueOrDefault("acknowledged"); }
            set { this["acknowledged"] = value; }
        }

        public object modifiedCount
        {
            get { return this.GetValueOrDefault("modifiedCount"); }
            set { this["modifiedCount"] = value; }
        }

        public object matchedCount
        {
            get { return this.GetValueOrDefault("matchedCount"); }
            set { this["matchedCount"] = value; }
        }

        public object upsertedId
        {
            get { return this.GetValueOrDefault("upsertedId"); }
            set { this["upsertedId"] = value; }
        }
    }
    
}
