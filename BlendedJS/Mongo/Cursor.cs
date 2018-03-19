using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace BlendedJS.Mongo
{
    public class Cursor : IEnumerable<BsonDocument>
    {
        private Console _console;
        private IFindFluent<BsonDocument, BsonDocument> _documents;
        public Cursor(IFindFluent<BsonDocument, BsonDocument> documents, Console console)
        {
            _console = console;
            _documents = documents;
        }

        public Cursor limit(int limit)
        {
            return new Cursor(_documents.Limit(limit), _console);
        }

        public Cursor skip(int skip)
        {
            return new Cursor(_documents.Skip(skip), _console);
        }

        public Cursor sort(object sort)
        {
            var mongoFind = new Cursor(_documents.Sort(sort.ToBsonDocument()), _console);
            return mongoFind;
        }

        public object count()
        {
            return _documents.Count();
        }

        public Cursor collation(object collation)
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

        public void forEach(Action<object> processor)
        {
            _documents.ToCursor().ForEachAsync(x =>
            {
                processor((object)x.ToDictionary().ToJsObject());
            });
        }

        private IEnumerator<BsonDocument> _cursor;
        public bool hasNext()
        {
            if (_cursor == null)
                _cursor = _documents.ToCursor().ToEnumerable().GetEnumerator();
            return _cursor.MoveNext();
        }

        public object next()
        {
            return _cursor?.Current.ToDictionary().ToJsObject();
        }

        public object[] toArray()
        {
            return _documents.ToList().Select(x => (object)x.ToDictionary().ToJsObject()).ToArray();
        }

        public void noCursorTimeout()
        {
            _documents.Options.NoCursorTimeout = false;
        }

        public IEnumerator<BsonDocument> GetEnumerator()
        {
            return _documents.ToEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _documents.ToEnumerable().GetEnumerator();
        }
    }
}
