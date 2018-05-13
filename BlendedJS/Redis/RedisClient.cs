using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using StackExchange.Redis;

namespace BlendedJS.Redis
{
    //https://redis.io/commands
    public class RedisClient : JsObject, IDisposable
    {
        private IDatabase _db;
        private IServer _server;

        public RedisClient(string url)
        {
            BlendedJSEngine.Clients.Value.Add(this);

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(url);
            _db = redis.GetDatabase();
        }

        public object del(params object[] keys)
        {
            return _db.KeyDelete(SelectManyKeys(keys));
        }

        public object dump(object key)
        {
            return _db.KeyDump(key.ToStringOrDefault()).ToJsObject();
        }

        public object exists(params object[] keys)
        {
            return SelectManyKeys(keys).All(x => _db.KeyExists(x)) ? 1 : 0;
        }

        public object expire(object key)
        {
            return _db.KeyExpire(key.ToStringOrDefault(), (TimeSpan?)null) ? 1 : 0;
        }

        public object expire(object key, object seconds)
        {
            return _db.KeyExpire(key.ToStringOrDefault(), TimeSpan.FromSeconds(seconds.ToIntOrDefault(0))) ? 1 : 0;
        }

        public object expireat(object key, object dateTime)
        {
            return _db.KeyExpire(key.ToStringOrDefault(), dateTime.ToToDateTimeFromUnixTimeStamp()) ? 1 : 0;
        }

        public object pexpire(object key, object miliseconds)
        {
            return _db.KeyExpire(key.ToStringOrDefault(), TimeSpan.FromMilliseconds(miliseconds.ToIntOrDefault(0))) ? 1 : 0;
        }

        public object pexpireat(object key, object dateTime)
        {
            return _db.KeyExpire(key.ToStringOrDefault(), dateTime.ToToDateTimeFromUnixTimeStampInMiliSeconds()) ? 1 : 0;
        }

        public object keys(object pattern)
        {
            throw new Exception("method not implemented");
        }

        public object keys(object key, object endpoint)
        {
            throw new Exception("method not implemented");
        }

        public object move(object key, object database)
        {
            var databaseValue = database.ToIntOrDefault();
            if (databaseValue.HasValue == false)
                throw new Exception("database has to be provided and has to be number");

            return _db.KeyMove(key.ToStringOrDefault(), databaseValue.Value) ? 1 : 0;
        }

        public object persist(object key)
        {
            return _db.KeyPersist(key.ToStringOrDefault()) ? 1 : 0;
        }

        public object pttl(object key)
        {
            var ttl = _db.KeyTimeToLive(key.ToStringOrDefault());
            if (ttl.HasValue)
                return ttl.Value.TotalMilliseconds;
            return -1;
        }

        public object randomkey()
        {
            return _db.KeyRandom().ToStringOrDefault();
        }

        public object rename(object key, object newKey)
        {
            return _db.KeyRename(key.ToStringOrDefault(), newKey.ToStringOrDefault()) ? 1 : 0;
        }

        public object renamenx(object key, object newKey)
        {
            return _db.KeyRename(key.ToStringOrDefault(), newKey.ToStringOrDefault(), When.NotExists) ? 1 : 0;
        }

        public void restore(object key, object value)
        {
            _db.KeyRestore(key.ToStringOrDefault(), value.ToByteArray());
        }

        public void restore(object key, object expire, object value)
        {
            _db.KeyRestore(key.ToStringOrDefault(), value.ToByteArray(), TimeSpan.FromSeconds(value.ToDoubleOrDefault(0)));
        }

        public object sort(params object[] attributes)
        {
            var key = attributes.ElementAtOrDefault(0).ToStringOrDefault();
            Order order = Order.Ascending;
            SortType sortType = SortType.Numeric;
            long skip = 0;
            long take = -1;
            RedisValue by = default(RedisValue);
            List<RedisValue> get = new List<RedisValue>();
            for (int i = 0; i < attributes.Length; i++)
            {
                if (string.Equals(attributes[i].ToStringOrDefault(), "DESC", StringComparison.InvariantCultureIgnoreCase))
                {
                    order = Order.Descending;
                }
                if (string.Equals(attributes[i].ToStringOrDefault(), "ALPHA", StringComparison.InvariantCultureIgnoreCase))
                {
                    sortType = SortType.Alphabetic;
                }
                if (string.Equals(attributes[i].ToStringOrDefault(), "LIMIT", StringComparison.InvariantCultureIgnoreCase))
                {
                    skip = attributes.ElementAtOrDefault(i + 1).ToLongOrDefault(0);
                    take = attributes.ElementAtOrDefault(i + 2).ToLongOrDefault(-1);
                }
                if (string.Equals(attributes[i].ToStringOrDefault(), "BY", StringComparison.InvariantCultureIgnoreCase))
                {
                    by = attributes.ElementAtOrDefault(i + 1).ToStringOrDefault();
                }
                if (string.Equals(attributes[i].ToStringOrDefault(), "GET", StringComparison.InvariantCultureIgnoreCase))
                {
                    string val = attributes.ElementAtOrDefault(i + 1).ToStringOrDefault();
                    if (string.IsNullOrEmpty(val) == false)
                        get.Add(val);
                }
            }

            return _db
                .Sort(key,
                    skip: skip,
                    take: skip,
                    order: order,
                    sortType: sortType,
                    by: by,
                    get: get.ToArray())
                .Select(x => x.ToJsObject())
                .ToArray();
        }

        public object touch(object key)
        {
            throw new Exception("method not implemented");
        }

        public object ttl(object key)
        {
            var ttl = _db.KeyTimeToLive(key.ToStringOrDefault());
            if (ttl.HasValue)
                return ttl.Value.TotalSeconds.ToIntOrDefault();
            return -1;
        }

        public object type(object key)
        {
            return _db.KeyType(key.ToStringOrDefault()).ToString();
        }

        public object unlink(object key)
        {
            throw new Exception("method not implemented");
        }

        public object wait()
        {
            throw new Exception("method not implemented");
        }

        public object scan()
        {
            throw new Exception("method not implemented");
        }

        public void set(object key, object value)
        {
            _db.StringSet(key.ToStringOrDefault(), value.ToStringOrDefault());
        }

        public object get(object key)
        {
            return _db.StringGet(key.ToStringOrDefault()).ToJsObject();
        }

        public object getset(object key, object value)
        {
            return _db.StringGetSet(key.ToStringOrDefault(), value.ToStringOrDefault()).ToJsObject();
        }

        public object setex(object key, object seconds, object value)
        {
            return _db.StringSet(key.ToStringOrDefault(), value.ToStringOrDefault(), TimeSpan.FromSeconds(seconds.ToIntOrDefault(0))) ? 1 : 0;
        }

        public object psetex(object key, object miliSeconds, object value)
        {
            return _db.StringSet(key.ToStringOrDefault(), value.ToStringOrDefault(), TimeSpan.FromMilliseconds(miliSeconds.ToIntOrDefault(0))) ? 1 : 0;
        }

        public object setnx(object key, object value)
        {
            return _db.StringSet(key.ToStringOrDefault(), value.ToStringOrDefault(), null, When.NotExists) ? 1 : 0;
        }

        public object setrange(object key, object offset, object value)
        {
            var offsetValue = offset.ToLongOrDefault();
            if (offsetValue.HasValue == false)
                throw new Exception("offset has to be provided");

            return _db.StringSetRange(key.ToStringOrDefault(), offsetValue.Value, value.ToStringOrDefault()).ToJsObject();
        }

        public object getrange(object key, object start, object end)
        {
            var startValue = start.ToLongOrDefault();
            if (startValue.HasValue == false)
                throw new Exception("start has to be provided");

            var endValue = end.ToLongOrDefault();
            if (endValue.HasValue == false)
                throw new Exception("end has to be provided");

            return _db.StringGetRange(key.ToStringOrDefault(), startValue.Value, endValue.Value).ToJsObject();
        }

        public object mset(params object[] keys)
        {
            keys = SelectMany(keys);

            if (keys != null)
            {
                List<KeyValuePair<RedisKey, RedisValue>> keyValueList = new List<KeyValuePair<RedisKey, RedisValue>>();

                for(int i = 0; i < keys.Length-1; i = i + 2)
                {
                    keyValueList.Add(
                        new KeyValuePair<RedisKey, RedisValue>(
                            (RedisKey)keys[i].ToStringOrDefault(), 
                            (RedisValue)keys[i + 1].ToStringOrDefault()));
                }
                return _db.StringSet(keyValueList.ToArray()) ? 1 : 0;
            }

            return null;
        }

        public object msetnx(params object[] keys)
        {
            keys = SelectMany(keys);

            if (keys != null)
            {
                List<KeyValuePair<RedisKey, RedisValue>> keyValueList = new List<KeyValuePair<RedisKey, RedisValue>>();

                for (int i = 0; i < keys.Length - 1; i = i + 2)
                {
                    keyValueList.Add(
                        new KeyValuePair<RedisKey, RedisValue>(
                            (RedisKey)keys[i].ToStringOrDefault(),
                            (RedisValue)keys[i + 1].ToStringOrDefault()));
                }
                return _db.StringSet(keyValueList.ToArray(), When.NotExists) ? 1 : 0;
            }

            return null;
        }

        public object[] mget(params object[] keys)
        {
            keys = SelectMany(keys);
            if (keys != null)
            {
                var keysArray = ((object[])keys).Select(x => (RedisKey)x.ToStringOrDefault()).ToArray();
                return _db.StringGet(keysArray).Select(x => x.ToJsObject()).ToArray();
            }

            return null;
        }

        public object setbit(object key, object offset, object bit)
        {
            var offsetValue = offset.ToLongOrDefault();
            if (offsetValue.HasValue == false)
                throw new Exception("offset has to be provided");

            var bitValue = bit.ToBoolOrDefault();
            if (bitValue.HasValue == false)
                throw new Exception("bit has to be provided");
            
            return _db.StringSetBit(key.ToStringOrDefault(), offsetValue.Value, bitValue.Value) ? 1 : 0;
        }

        public object getbit(object key, object offset)
        {
            var offsetValue = offset.ToLongOrDefault();
            if (offsetValue.HasValue == false)
                throw new Exception("offset has to be provided");

            return _db.StringGetBit(key.ToStringOrDefault(), offsetValue.Value) ? 1 : 0;
        }

        public object bitcount(object key)
        {
            return _db.StringBitCount(key.ToStringOrDefault());
        }

        public object bitcount(object key, object start)
        {
            return _db.StringBitCount(key.ToStringOrDefault(), start.ToLongOrDefault(1));
        }

        public object bitcount(object key, object start, object end)
        {
            return _db.StringBitCount(key.ToStringOrDefault(), start.ToLongOrDefault(1), end.ToLongOrDefault(-1));
        }

        public object bitpos(object key, object bit)
        {
            var bitValue = bit.ToBoolOrDefault();
            if (bitValue.HasValue == false)
                throw new Exception("bit has to be provided");

            return _db.StringBitPosition(key.ToStringOrDefault(), bitValue.Value);
        }

        public object bitpos(object key, object bit, object start)
        {
            var bitValue = bit.ToBoolOrDefault();
            if (bitValue.HasValue == false)
                throw new Exception("bit has to be provided");

            return _db.StringBitPosition(key.ToStringOrDefault(), bitValue.Value, start.ToLongOrDefault(1));
        }

        public object bitpos(object key, object bit, object start, object end)
        {
            var bitValue = bit.ToBoolOrDefault();
            if (bitValue.HasValue == false)
                throw new Exception("bit has to be provided");

            return _db.StringBitPosition(key.ToStringOrDefault(), bitValue.Value, start.ToLongOrDefault(1), end.ToLongOrDefault(-1));
        }

        public object strlen(object key)
        {
            return _db.StringLength(key.ToStringOrDefault());
        }

        public object incr(object key)
        {
            return _db.StringIncrement(key.ToStringOrDefault());
        }

        public object incrby(object key, object value)
        {
            var valueValue = value.ToLongOrDefault();
            if (valueValue.HasValue == false)
                throw new Exception("value has to be provided");

            return _db.StringIncrement(key.ToStringOrDefault(), valueValue.Value);
        }

        public object incrbyfloat(object key, object value)
        {
            var valueValue = value.ToDoubleOrDefault();
            if (valueValue.HasValue == false)
                throw new Exception("value has to be provided");

            return _db.StringIncrement(key.ToStringOrDefault(), valueValue.Value);
        }

        public object decr(object key)
        {
            return _db.StringDecrement(key.ToStringOrDefault());
        }

        public object decrby(object key, object value)
        {
            var valueValue = value.ToLongOrDefault();
            if (valueValue.HasValue == false)
                throw new Exception("value has to be provided");

            return _db.StringDecrement(key.ToStringOrDefault(), valueValue.Value);
        }

        public object decrbyfloat(object key, object value)
        {
            var valueValue = value.ToDoubleOrDefault();
            if (valueValue.HasValue == false)
                throw new Exception("value has to be provided");

            return _db.StringDecrement(key.ToStringOrDefault(), valueValue.Value);
        }

        public object append(object key, object value)
        {
            return _db.StringAppend(key.ToStringOrDefault(), value.ToStringOrDefault());
        }

        public object hset(object key, object hashField, object value)
        {
            return _db.HashSet(key.ToStringOrDefault(), hashField.ToStringOrDefault(), value.ToStringOrDefault()) ? 1 : 0;
        }

        public object hget(object key, object hashField)
        {
            return _db.HashGet(key.ToStringOrDefault(), hashField.ToStringOrDefault()).ToJsObject();
        }

        public object hdel(object key, object hashField)
        {
            return _db.HashDelete(key.ToStringOrDefault(), hashField.ToStringOrDefault()) ? 1 : 0;
        }

        public object hincrby(object key, object hashField, object value)
        {
            var valueDouble = value.ToDoubleOrDefault();
            if (valueDouble.HasValue == false)
                throw new Exception("value has to be provided and has to be number");

            return _db.HashIncrement(key.ToStringOrDefault(), hashField.ToStringOrDefault(), valueDouble.Value);
        }

        public object hdecrby(object key, object hashField, object value)
        {
            var valueDouble = value.ToDoubleOrDefault();
            if (valueDouble.HasValue == false)
                throw new Exception("value has to be provided and has to be number");

            return _db.HashDecrement(key.ToStringOrDefault(), hashField.ToStringOrDefault(), valueDouble.Value);
        }

        public object hincrbyfloat(object key, object hashField, object value)
        {
            var valueDouble = value.ToDoubleOrDefault();
            if (valueDouble.HasValue == false)
                throw new Exception("value has to be provided and has to be number");

            return _db.HashIncrement(key.ToStringOrDefault(), hashField.ToStringOrDefault(), valueDouble.Value);
        }

        public object hdecrbyfloat(object key, object hashField, object value)
        {
            var valueDouble = value.ToDoubleOrDefault();
            if (valueDouble.HasValue == false)
                throw new Exception("value has to be provided and has to be number");

            return _db.HashDecrement(key.ToStringOrDefault(), hashField.ToStringOrDefault(), valueDouble.Value);
        }

        public object hkeys(object key)
        {
            return _db.HashKeys(key.ToStringOrDefault()).Select(x => x.ToJsObject()).ToArray();
        }

        public object hlen(object key)
        {
            return _db.HashLength(key.ToStringOrDefault());
        }

        public object hmget(params object[] keys)
        {
            keys = SelectMany(keys);
            var key = keys.ElementAtOrDefault(0).ToStringOrDefault();
            var fields = keys.Skip(1).Select(x => (RedisValue)x.ToStringOrDefault()).ToArray();
            return _db.HashGet(key, fields).Select(x => x.ToJsObject()).ToArray();
        }

        public void hmset(params object[] keys)
        {
            keys = SelectMany(keys);
            var key = keys.ElementAtOrDefault(0).ToStringOrDefault();
            List<HashEntry> keyValueList = new List<HashEntry>();
            if (keys != null)
            {
                for (int i = 1; i < keys.Length - 1; i = i + 2)
                {
                    keyValueList.Add(new HashEntry(
                        (RedisValue)keys[i].ToStringOrDefault(),
                        (RedisValue)keys[i + 1].ToStringOrDefault()
                        ));
                }
            }

            _db.HashSet(key, keyValueList.ToArray());
        }

        public object hexists(object key, object hashField)
        {
            return _db.HashExists(key.ToStringOrDefault(), hashField.ToStringOrDefault()) ? 1 : 0;
        }

        public object hgetall(object key)
        {
            JsObject obj = new JsObject();
            foreach(var entry in _db.HashGetAll(key.ToStringOrDefault()))
            {
                obj[entry.Name.ToStringOrDefault()] = entry.Value.ToStringOrDefault();
            }
            return obj;
        }

        //public void hset(object key, object value)
        //{
        //    _db.HashSet(key.ToStringOrDefault(), value.ToStringOrDefault());
        //}

        public object[] SelectMany(object[] keys)
        {
            if (keys != null)
            {
                return keys
                    .SelectMany(x =>
                    {
                        if (x is object[])
                            return (object[])x;
                        else
                            return new object[1] { x };
                    })
                    .ToArray();
            }
            return null;
        }

        public RedisKey[] SelectManyKeys(object[] keys)
        {
            if (keys != null)
            {
                return keys
                    .SelectMany(x =>
                    {
                        if (x is object[])
                            return ((object[])x).Select(z => (RedisKey)z.ToStringOrDefault());
                        else
                            return new RedisKey[1] { (RedisKey)x.ToStringOrDefault() };
                    })
                    .ToArray();
            }
            return null;
        }

        public void Dispose()
        {
            
        }
    }
}
