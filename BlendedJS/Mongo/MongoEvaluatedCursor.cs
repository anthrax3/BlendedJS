using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace BlendedJS.Mongo
{
    public class MongoEvaluatedCursor<T> : ICursor where T : class
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
            this._currentBatch = new Queue<T>();
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
            return next.ToJsObject();
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
                processor(x.ToJsObject());
            });
        }

        public object[] toArray()
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            return _asyncCursor.ToList().Select(x => x.ToJsObject()).ToArray();
        }

        public object[] map(Func<object, object> processor)
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            return _asyncCursor.ToList().Select(x => processor(x.ToJsObject())).ToArray();
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            return this._asyncCursor.ToEnumerable().Select(x => x.ToJsObject()).GetEnumerator();
        }

        public IEnumerator<object> GetEnumerator()
        {
            if (this._asyncCursor == null)
                this._asyncCursor = _asyncCursorFactory();

            return this._asyncCursor.ToEnumerable().Select(x => x.ToJsObject()).GetEnumerator();
        }
    }
}
