using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using static MongoDB.Driver.WriteConcern;

namespace BlendedJS.Mongo
{
    public class MongoCollection
    {
        private string _name;
        private IMongoCollection<BsonDocument> _collection;
        private IConsole _console;

        public MongoCollection(string name, IMongoCollection<BsonDocument> collection, IConsole console)
        {
            _collection = collection;
            _console = console;
            _name = name;
        }

        public object find()
        {
            return find(null, null);
        }

        public object find(object filter)
        {
            return find(filter, null);
        }

        public object find(object filter, object projection)
        {
            return new Cursor(_collection.Find(filter.ToBsonDocument()).Project(projection.ToBsonDocument()), _console);
        }

        public object findOne()
        {
            return this.findOne(null, null);
        }

        public object findOne(object filter)
        {
            return this.findOne(filter, null);
        }

        public object findOne(object filter, object projection)
        {
            return _collection.Find(filter.ToBsonDocument()).Project(projection.ToBsonDocument()).FirstOrDefault();
        }

        public object findOneAndDelete(object filter)
        {
            return this._collection.FindOneAndDelete(filter.ToBsonDocument());
        }

        public object findOneAndDelete(object filter, object options)
        {
            FindOneAndDeleteOptions<BsonDocument, BsonDocument> findAndDeleteOptions = new FindOneAndDeleteOptions<BsonDocument, BsonDocument>();
            findAndDeleteOptions.Sort = options.GetProperty("sort").ToBsonDocument();
            findAndDeleteOptions.Projection = options.GetProperty("projection").ToBsonDocument();
            var maxTimeMS = options.GetProperty("maxTimeMS").ToIntOrDefault();
            if (maxTimeMS.HasValue)
                findAndDeleteOptions.MaxTime = TimeSpan.FromMilliseconds(maxTimeMS.Value);

            return this._collection.FindOneAndDelete(filter.ToBsonDocument(), findAndDeleteOptions);
        }

        public object findOneAndUpdate(object filter, object update, object options)
        {
            FindOneAndUpdateOptions<BsonDocument, BsonDocument> findAndUpdateOptions = new FindOneAndUpdateOptions<BsonDocument, BsonDocument>();
            findAndUpdateOptions.Sort = options.GetProperty("sort").ToBsonDocument();
            findAndUpdateOptions.Projection = options.GetProperty("projection").ToBsonDocument();
            var maxTimeMS = options.GetProperty("maxTimeMS").ToIntOrDefault();
            if (maxTimeMS.HasValue)
                findAndUpdateOptions.MaxTime = TimeSpan.FromMilliseconds(maxTimeMS.Value);
            
            return this._collection.FindOneAndUpdate(filter.ToBsonDocument(), update.ToBsonDocument());
        }

        public object findOneAndReplace(object filter, object replacment, object options)
        {
            FindOneAndReplaceOptions<BsonDocument, BsonDocument> findAndReplaceOptions = new FindOneAndReplaceOptions<BsonDocument, BsonDocument>();
            findAndReplaceOptions.Sort = options.GetProperty("sort").ToBsonDocument();
            findAndReplaceOptions.Projection = options.GetProperty("projection").ToBsonDocument();
            var maxTimeMS = options.GetProperty("maxTimeMS").ToIntOrDefault();
            if (maxTimeMS.HasValue)
                findAndReplaceOptions.MaxTime = TimeSpan.FromMilliseconds(maxTimeMS.Value);
            var isUpsert = options.GetProperty("isUpsert").ToBoolOrDefault();
            if (isUpsert.HasValue)
                findAndReplaceOptions.IsUpsert = isUpsert.Value;
            
            return this._collection.FindOneAndReplace(filter.ToBsonDocument(), replacment.ToBsonDocument());
        }

        public object insert(object document)
        {
            return insert(document, null);
        }

        public object insert(object document, ExpandoObject options)
        {
            if (document is object[])
                return insertMany((object[])document, options);
            else
                return insertOne(document, options);
        }

        public object insertOne(object document)
        {
            return insertOne(document, null);
        }

        public object insertOne(object document, ExpandoObject options)
        {
            InsertOneOptions insertOneOpitons = new InsertOneOptions();

            this._collection = withWriteConcernIfDefined(options.GetProperty("writeConcern"));

            var bsonDocument = document.ToBsonDocument();
            this._collection.InsertOne(bsonDocument, insertOneOpitons);
            return new InsertResult
            {
                acknowledged = true,
                insertedId = bsonDocument["_id"]
            };
        }

        public object insertMany(object[] documents)
        {
            return insertMany(documents, null);
        }

        public object insertMany(object[] documents, object options)
        {
            InsertManyOptions insertOneOpitons = new InsertManyOptions();
            bool? ordered = options.GetProperty("ordered").ToBoolOrDefault();
            if (ordered.HasValue)
                insertOneOpitons.IsOrdered = ordered.Value;

            this._collection = withWriteConcernIfDefined(options.GetProperty("writeConcern"));

            var bsonDocuments = documents.Select(x => x.ToBsonDocument()).ToList();
            this._collection.InsertMany(bsonDocuments, insertOneOpitons);
            return new InsertResult
            {
                acknowledged = true,
                insertedIds = bsonDocuments.Select(x => x["_id"]).ToArray()
            };
        }

        public object updateOne(object filter, object document)
        {
            return updateOne(filter, document, null);
        }

        public object updateOne(object filter, object document, object options)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            bool? upsert = options.GetProperty("upsert")?.ToBoolOrDefault();
            if (upsert.HasValue)
                updateOptions.IsUpsert = upsert.Value;
            withWriteConcernIfDefined(options);
            return new UpdateResult(this._collection.UpdateOne(filter.ToBsonDocument(), document.ToBsonDocument(), updateOptions));
        }

        public object updateMany(object filter, object document)
        {
            return updateMany(filter, document, null);
        }

        public object updateMany(object filter, object document, object options)
        {
            UpdateOptions updateOptions = new UpdateOptions();

            bool? upsert = options.GetProperty("upsert")?.ToBoolOrDefault();
            if (upsert.HasValue)
                updateOptions.IsUpsert = upsert.Value;

            return new UpdateResult(this._collection.UpdateMany(filter.ToBsonDocument(), document.ToBsonDocument(), updateOptions));
        }

        public object update(object filter, object document)
        {
            return update(filter, document, null);
        }

        public object update(object filter, object document, object options)
        {
            BsonDocument bsonDocument = document.ToBsonDocument();

            if (bsonDocument.FirstOrDefault().Name.StartsWith("$") == false) // is replace
                return replaceOne(filter, document, options);

            bool? multi = options.GetProperty("multi")?.ToBoolOrDefault();
            if (multi.HasValue && multi.Value)
                return updateMany(filter, document, options);
            else
                return updateOne(filter, document, options);
        }

        public object replaceOne(object filter, object replacement)
        {
            return replaceOne(filter, replacement, null);
        }

        public object replaceOne(object filter, object replacement, object options)
        {
            UpdateOptions updateOptions = new UpdateOptions();

            bool? upsert = options.GetProperty("upsert")?.ToBoolOrDefault();
            if (upsert.HasValue)
                updateOptions.IsUpsert = upsert.Value;

            return new ReplaceOneResult(this._collection.ReplaceOne(filter.ToBsonDocument(), replacement.ToBsonDocument(), updateOptions));
        }

        public object deleteOne(object filter)
        {
            return this._collection.DeleteOne(filter.ToBsonDocument(), null);
        }

         public object deleteOne(object filter, object options)
        {
            withWriteConcernIfDefined(options);

            return this._collection.DeleteOne(filter.ToBsonDocument());
        }

        public object deleteMany(object filter)
        {
            return this._collection.DeleteMany(filter.ToBsonDocument());
        }

        public object deleteMany(object filter, object options)
        {
            withWriteConcernIfDefined(options);
            
            return this._collection.DeleteMany(filter.ToBsonDocument());
        }

        public object remove(object filter)
        {
            return remove(filter, null);
        }

        public object remove(object filter, bool justOne)
        {
            if (justOne)
                return deleteOne(filter);
            else
                return deleteMany(filter);
        }
        public object remove(object filter, object options)
        {
            bool? justOne = options.GetProperty("justOne")?.ToBoolOrDefault();
            if (justOne.HasValue && justOne.Value)
                return deleteOne(filter, options);
            else
                return deleteMany(filter, options);
        }

        public object count()
        {
            return count(null, null);
        }

        public object count(object filter)
        {
            return count(filter, null);
        }
        public object count(object filter, object options)
        {
            CountOptions countOptions = new CountOptions();

            return this._collection.Count(filter.ToBsonDocument(), countOptions);
        }

        public object distinct(object field, object query)
        {
            return distinct(field, query, null);
        }

        public object distinct(object field, object query, object options)
        {
            DistinctOptions distinctOptions = new DistinctOptions();
            
            var fieldDefinition = new StringFieldDefinition<BsonDocument, object>(field.ToStringOrDefault());
            var cursor = this._collection.Distinct(
                fieldDefinition,
                query.ToBsonDocument(),
                distinctOptions);
            return new EvaluatedCursor<object>(cursor);
        }

        public object aggregate(object pipeline)
        {
            return aggregate(pipeline, null);
        }

        public object aggregate(object filter, object options)
        {
            AggregateOptions aggregateOptions = new AggregateOptions();
            //bool? explain = options.GetString("explain")?.To(x => Convert.ToBoolean(x));
            //if (explain.HasValue)
            //aggregateOptions.ex
            

            bool? allowDiskUse = options.GetProperty("allowDiskUse")?.ToBoolOrDefault();
            if (allowDiskUse.HasValue)
                aggregateOptions.AllowDiskUse = allowDiskUse.Value;

            bool? bypassDocumentValidation = options.GetProperty("bypassDocumentValidation")?.ToBoolOrDefault();
            if (bypassDocumentValidation.HasValue)
                aggregateOptions.BypassDocumentValidation = bypassDocumentValidation.Value;

            int? batchSize = options.GetProperty("cursor")?.GetProperty("batchSize")?.ToIntOrDefault();
            if (batchSize.HasValue)
            {
                aggregateOptions.UseCursor = true;
                aggregateOptions.BatchSize = batchSize.Value;
            }

            var stages = ((Array)filter).OfType<object>().Select(x => x.ToBsonDocument());
            var pipeline = new BsonDocumentStagePipelineDefinition<BsonDocument, BsonDocument>(stages);
            return new EvaluatedCursor<BsonDocument>(this._collection.Aggregate(pipeline, aggregateOptions));
        }

        public void drop()
        {
            this._collection.Database.DropCollection(this._name);
        }

        public void dropIndex(string index)
        {
            this._collection.Indexes.DropOne(index);
        }

        public void dropIndexes()
        {
            this._collection.Indexes.DropAll();
        }

        public string createIndex(object keys)
        {
            return createIndex(keys, null);
        }

        public string createIndex(object keys, object options)
        {
            BsonDocumentIndexKeysDefinition<BsonDocument> keysDefinitions = 
                new BsonDocumentIndexKeysDefinition<BsonDocument>(keys.ToBsonDocument());

            CreateIndexOptions createIndexOptions = new CreateIndexOptions();

            return this._collection.Indexes.CreateOne(keysDefinitions, createIndexOptions);
        }

        public object[] createIndexes(object keys)
        {
            return createIndexes(keys, null);
        }

        public object[] createIndexes(object keys, object options)
        {
            CreateIndexOptions createIndexOptions = new CreateIndexOptions();

            var keysBson = ((Array)keys).OfType<object>().Select(x => x.ToBsonDocument());
            var keysDefinitions = keysBson.Select(x => new BsonDocumentIndexKeysDefinition<BsonDocument>(x.ToBsonDocument()));
            var keysModel = keysDefinitions.Select(x => new CreateIndexModel<BsonDocument>(x, createIndexOptions));
            
            return this._collection.Indexes.CreateMany(keysModel).Select(x => (object)x).ToArray();
        }
        
        public object ensureIndex(object keys, object options)
        {
            return createIndex(keys, options);
        }

        public object getIndexes()
        {
            return null;
        }

        public object totalSize()
        {
            return null;
        }

        public object stats(object scaleOrOptions)
        {
            var request = new Dictionary<string, object>();
            request.Add("collStats", _name);
            var scale = scaleOrOptions.ToStringOrDefault().ToIntOrDefault();
            if (scale.HasValue)
            {
                request.Add("scale", scale);
            }
            else if (scaleOrOptions is ExpandoObject)
            {
                scale = scaleOrOptions.GetProperty("scale").ToIntOrDefault();
                if (scale.HasValue)
                    request.Add("scale", scale);
            }
            
            var command = new BsonDocumentCommand<BsonDocument>(request.ToBsonDocument(), null);
            var result = this._collection.Database.RunCommand<BsonDocument>(command);
            return result;
        }

        public object dataSize()
        {
            return ((IDictionary<string, object>)stats(null))
                .GetValueOrDefault("size")
                .ToIntOrDefault();
        }

        public object mapReduce(object map, object reduce, object options)
        {
            var mapReduceOptions = new MapReduceOptions<BsonDocument, BsonDocument>();

            object sort = options.GetProperty("sort");
            if (sort != null)
                mapReduceOptions.Sort = new BsonDocumentSortDefinition<BsonDocument>(sort.ToBsonDocument());

            object query = options.GetProperty("query");
            if (query != null)
                mapReduceOptions.Filter = new BsonDocumentFilterDefinition<BsonDocument>(query.ToBsonDocument());

            object scope = options.GetProperty("scope");
            if (scope != null)
                mapReduceOptions.Scope = scope.ToBsonDocument();

            object limit = options.GetProperty("limit");
            if (limit != null)
                mapReduceOptions.Limit = limit.ToIntOrDefault();

            object verbose = options.GetProperty("verbose");
            if (limit != null)
                mapReduceOptions.Verbose = verbose.ToBoolOrDefault();

            object jsMode = options.GetProperty("jsMode");
            if (jsMode != null)
                mapReduceOptions.JavaScriptMode = jsMode.ToBoolOrDefault();

            object bypassDocumentValidation = options.GetProperty("bypassDocumentValidation");
            if (bypassDocumentValidation != null)
                mapReduceOptions.BypassDocumentValidation = bypassDocumentValidation.ToBoolOrDefault();

            object finalize = options.GetProperty("finalize");
            if (finalize != null)
                mapReduceOptions.Finalize = new BsonJavaScript(finalize.ToStringOrDefault());

            object outCollection = options.GetProperty("out");
            if (outCollection is string)
                mapReduceOptions.OutputOptions = MapReduceOutputOptions.Replace(outCollection.ToStringOrDefault());
            if (outCollection is IDictionary<string,object>)
            {
                var outDicionary = (IDictionary<string, object>)outCollection;
                object inline = outDicionary.GetValueOrDefault("inline");
                if (inline != null)
                {
                    mapReduceOptions.OutputOptions = MapReduceOutputOptions.Inline;
                }
                else
                {
                    string replaceAction = outDicionary.GetValueOrDefault("replace").ToStringOrDefault();
                    string mergeAction = outDicionary.GetValueOrDefault("merge").ToStringOrDefault();
                    string reduceAction = outDicionary.GetValueOrDefault("reduce").ToStringOrDefault();

                    string db = outDicionary.GetValueOrDefault("db").ToStringOrDefault();
                    bool? nonAtomic = outDicionary.GetValueOrDefault("nonAtomic").ToBoolOrDefault();
                    bool? sharded = outDicionary.GetValueOrDefault("sharded").ToBoolOrDefault();

                    if (replaceAction != null)
                        mapReduceOptions.OutputOptions = MapReduceOutputOptions.Replace(replaceAction, db, sharded);
                    if (mergeAction != null)
                        mapReduceOptions.OutputOptions = MapReduceOutputOptions.Merge(mergeAction, db, sharded, nonAtomic);
                    if (reduceAction != null)
                        mapReduceOptions.OutputOptions = MapReduceOutputOptions.Reduce(reduceAction, db, sharded, nonAtomic);
                }
            }

                return new EvaluatedCursor<BsonDocument>(
                    _collection.MapReduce<BsonDocument>(
                        new BsonJavaScript(map.ToStringOrDefault()),
                        new BsonJavaScript(reduce.ToStringOrDefault()),
                        mapReduceOptions)
            );
        }

        public IMongoCollection<BsonDocument> withWriteConcernIfDefined(object options)
        {
            if (options != null)
            {
                string w = options.GetProperty("w").ToStringOrDefault();
                int? wtimeout = options.GetProperty("wtimeout").ToIntOrDefault();
                
                if (string.IsNullOrEmpty(w) == false && wtimeout.HasValue)
                {
                    this._collection = this._collection.WithWriteConcern(new WriteConcern(w, TimeSpan.FromMilliseconds(wtimeout.Value)));
                }
            }
            return this._collection;
        }
    }
}