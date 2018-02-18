using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace BlendedJS.Mongo
{
    public class EvaluatedCursor<T> : IEnumerable<T> where T : class
    {
        private IAsyncCursor<T> _asyncCursor;
        private Queue<T> _currentBatch;

        public EvaluatedCursor(IAsyncCursor<T> asyncCursor)
        {
            this._asyncCursor = asyncCursor;
            this._currentBatch = new Queue<T>(this._asyncCursor.Current ?? new List<T>());
        }

        public bool hasNext()
        {
            if (this._currentBatch.Count == 0)
            {
                this._asyncCursor.MoveNext();
                this._currentBatch = new Queue<T>(this._asyncCursor.Current ?? new List<T>());
            }
            return this._currentBatch.Count > 0;
        }

        public object next()
        {
            return _currentBatch.Count > 0 ? _currentBatch.Dequeue() : null;
        }

        public int objsLeftInBatch()
        {
            return _currentBatch.Count;
        }

        public void forEach(Action<object> processor)
        {
            _asyncCursor.ForEachAsync(x => processor(x));
        }

        public object[] toArray()
        {
            return _asyncCursor.ToList().Select(x => (object)x).ToArray();
        }

        public object[] map(Func<object, object> processor)
        {
            return _asyncCursor.ToList().Select(x => processor(x)).ToArray();
        }

        public int itcount()
        {
            return this._asyncCursor.ToList().Count;
        }

        public void pretty()
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._asyncCursor.ToEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._asyncCursor.ToEnumerable().GetEnumerator();
        }
    }
}
