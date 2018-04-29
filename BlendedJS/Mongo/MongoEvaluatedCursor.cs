using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace BlendedJS.Mongo
{
    public class MongoEvaluatedCursor<T> : IEnumerable<T> where T : class
    {
        private Func<IAsyncCursor<T>> _asyncCursorFactory;
        private IAsyncCursor<T> _asyncCursor;
        private Queue<T> _currentBatch;

        public MongoEvaluatedCursor(Func<IAsyncCursor<T>> asyncCursorFactory)
        {
            this._asyncCursorFactory = asyncCursorFactory;
            this._currentBatch = new Queue<T>();
        }

        public MongoEvaluatedCursor(IAsyncCursor<T> asyncCursor)
        {
            this._asyncCursor = asyncCursor;
            this._currentBatch = new Queue<T>(this._asyncCursor.Current ?? new List<T>());
        }

        public bool hasNext()
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            if (this._currentBatch.Count == 0)
            {
                this._asyncCursor.MoveNext();
                this._currentBatch = new Queue<T>(this._asyncCursor.Current ?? new List<T>());
            }
            return this._currentBatch.Count > 0;
        }

        public object next()
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();


            if (this._currentBatch.Count == 0)
            {
                this._asyncCursor.MoveNext();
                this._currentBatch = new Queue<T>(this._asyncCursor.Current ?? new List<T>());
            }

            var next = _currentBatch.Count > 0 ? _currentBatch.Dequeue() : null;
            if (typeof(T) is IDictionary<string, object>)
                return ((IDictionary<string, object>)next).ToJsObject();
            else
                return next;
        }

        public int objsLeftInBatch()
        {
            return _currentBatch.Count;
        }

        public void forEach(Action<object> processor)
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            _asyncCursor.ForEachAsync(x => 
            {
                if (typeof(T) is IDictionary<string, object>)
                    processor(((IDictionary<string, object>)x).ToJsObject());
                else
                    processor(x);
            });
        }

        public object[] toArray()
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            if (typeof(T) is IDictionary<string, object>)
                return _asyncCursor.ToList().Select(x => (object)((IDictionary<string, object>)x).ToJsObject()).ToArray();
            else
                return _asyncCursor.ToList().Cast<object>().ToArray();
        }

        public object[] map(Func<object, object> processor)
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            if (typeof(T) is IDictionary<string, object>)
                return _asyncCursor.ToList().Select(x => processor(((IDictionary<string, object>)x).ToJsObject())).ToArray();
            else
                return _asyncCursor.ToList().Select(x => processor(x)).ToArray();
        }

        public int itcount()
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            return this._asyncCursor.ToList().Count ;
        }

        public void pretty()
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            return this._asyncCursor.ToEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            return this._asyncCursor.ToEnumerable().GetEnumerator();
        }
    }
}
