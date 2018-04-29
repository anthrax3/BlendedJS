using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace BlendedJS.Mongo
{
    public class MongoCursor : MongoEvaluatedCursor<BsonDocument>
    {
        private Console _console;
        private IFindFluent<BsonDocument, BsonDocument> _documents;

        public MongoCursor(IFindFluent<BsonDocument, BsonDocument> documents, Console console) 
            : base(() => documents.ToCursor())
        {
            _console = console;
            _documents = documents;
        }

        public MongoCursor limit(int limit)
        {
            return new MongoCursor(_documents.Limit(limit), _console);
        }

        public MongoCursor skip(int skip)
        {
            return new MongoCursor(_documents.Skip(skip), _console);
        }

        public MongoCursor sort(object sort)
        {
            var mongoFind = new MongoCursor(_documents.Sort(sort.ToBsonDocument()), _console);
            return mongoFind;
        }

        public object count()
        {
            return _documents.Count();
        }

        public MongoCursor collation(object collation)
        {
            string locale = collation.GetProperty("locale").ToStringOrDefault();

            bool? caseLevel = collation.GetProperty("caseLevel").ToBoolOrDefault();

            CollationCaseFirst? caseFirst = null;
            int? caseFirstInt = collation.GetProperty("caseFirst").ToIntOrDefault();
            if (caseFirstInt.HasValue)
                caseFirst = (CollationCaseFirst)caseFirstInt;

            CollationStrength? strenth = null;
            int? strengthInt = collation.GetProperty("strength").ToIntOrDefault();
            if (strengthInt.HasValue)
                strenth = (CollationStrength)strengthInt;
            
            bool? numericOrdering = collation.GetProperty("numericOrdering").ToBoolOrDefault();

            CollationAlternate? alternate = null;
            int? alternateInt = collation.GetProperty("alternate").ToIntOrDefault();
            if (alternateInt.HasValue)
                alternate = (CollationAlternate)alternateInt;

            CollationMaxVariable? maxVariable = null;
            int? maxVariableInt = collation.GetProperty("maxVariable").ToIntOrDefault();
            if (maxVariableInt.HasValue)
                maxVariable = (CollationMaxVariable)maxVariableInt;

            bool? normalization = collation.GetProperty("normalization").ToBoolOrDefault();

            bool? backwards = collation.GetProperty("backwards").ToBoolOrDefault();

            _documents.Options.Collation = new Collation(
                locale, 
                caseLevel, 
                caseFirst,
                strenth,
                numericOrdering,
                alternate,
                maxVariable,
                normalization,
                backwards);

            return this;
        }

        public void noCursorTimeout()
        {
            _documents.Options.NoCursorTimeout = false;
        }
    }
}
