using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MongoDB.Bson.Serialization;
using BlendedJS.Mongo;
using BlendedJS.Types;
using System.Collections;
using StackExchange.Redis;
using System.Text;

namespace BlendedJS
{
    public static class Extensions
    {
        public static bool SafeEquals(this string stringOne, string stringTwo)
        {
            return string.Equals(stringOne.ToStringOrDefault(""), stringTwo.ToStringOrDefault(""), StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool SafeContains(this string stringOne, string stringTwo)
        {
            if (stringOne == null)
                stringOne = "";
            if (stringTwo == null)
                stringTwo = "";
            return stringOne.ToLower().Contains(stringTwo.ToLower());
        }

        public static bool SafeEndsWith(this string stringOne, string stringTwo)
        {
            if (stringOne == null)
                return false;
            return stringOne.EndsWith(stringTwo, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool SafeStartsWith(this string stringOne, string stringTwo)
        {
            if (stringOne == null)
                return false;
            return stringOne.StartsWith(stringTwo, StringComparison.CurrentCultureIgnoreCase);
        }

        public static TValue GetValueOrDefault2<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value = default(TValue);
            dictionary.TryGetValue(key, out value);
            return value;
        }

        public static TValue GetValueOrDefault2<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue @default)
        {
            TValue value = default(TValue);
            if (dictionary.TryGetValue(key, out value))
                return value;
            else
                return @default;
        }

        public static int? ToIntOrDefault(this object obj)
        {
            if (obj == null)
                return null;

            if (obj is int)
                return (int)obj;

            if (obj is double doubleValue)
                return Convert.ToInt32(doubleValue);

            int value = 0;
            if (int.TryParse(obj.ToString(), out value))
                return value;
            else
                return null;
        }

        public static int ToIntOrDefault(this object obj, int defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (obj is int)
                return (int)obj;

            if (obj is double doubleValue)
                return Convert.ToInt32(doubleValue);

            int value = 0;
            if (int.TryParse(obj.ToString(), out value))
                return value;
            else
                return defaultValue;
        }


        public static long ToLongOrDefault(this object obj, long defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (obj is int)
                return (int)obj;

            if (obj is long)
                return (long)obj;

            int value = 0;
            if (int.TryParse(obj.ToString(), out value))
                return value;
            else
                return defaultValue;
        }

        public static long? ToLongOrDefault(this object obj)
        {
            if (obj == null)
                return null;

            if (obj is int)
                return (int)obj;

            if (obj is long)
                return (long)obj;

            int value = 0;
            if (int.TryParse(obj.ToString(), out value))
                return value;
            else
                return null;
        }


        public static bool? ToBoolOrDefault(this object obj)
        {
            if (obj == null)
                return null;

            if (obj is bool)
                return (bool)obj;

            var str = obj.ToString();
            if (str == "0")
                return false;
            if (str == "1")
                return true;

            bool value = false;
            if (bool.TryParse(str, out value))
                return value;
            else
                return null;
        }

        public static bool ToBoolOrDefault(this object obj, bool defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (obj is bool)
                return (bool)obj;

            var str = obj.ToString();
            if (str == "0")
                return false;
            if (str == "1")
                return true;

            bool value = false;
            if (bool.TryParse(str, out value))
                return value;
            else
                return defaultValue;
        }

        public static DateTime ToDateOrDefault(this object obj, DateTime defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (obj is DateTime)
                return (DateTime)obj;

            DateTime value;
            if (DateTime.TryParseExact(obj.ToString(), "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out value))
                return value;
            else
                return defaultValue;
        }

        public static DateTime? ToDateOrDefault(this object obj)
        {
            if (obj == null)
                return null;

            if (obj is DateTime)
                return (DateTime)obj;

            DateTime value;
            if (DateTime.TryParseExact(obj.ToString(), "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out value))
                return value;
            else
                return null;
        }

        public static DateTime ToDateTimeOrDefault(this object obj, DateTime defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (obj is DateTime)
                return (DateTime)obj;

            DateTime value;
            if (DateTime.TryParseExact(obj.ToString(), "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out value))
                return value;
            else
                return defaultValue;
        }

        public static DateTime? ToDateTimeOrDefault(this object obj)
        {
            if (obj == null)
                return null;

            if (obj is DateTime)
                return (DateTime)obj;

            DateTime value;
            if (DateTime.TryParseExact(obj.ToString(), "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out value))
                return value;
            else
                return null;
        }

        public static TimeSpan? ToTimeSpanOrDefault(this object obj)
        {
            return ToTimeSpanOrDefault(obj, null);
        }

        public static TimeSpan? ToTimeSpanOrDefault(this object obj, TimeSpan? defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (obj is TimeSpan)
                return (TimeSpan)obj;

            TimeSpan value;
            if (TimeSpan.TryParse(obj.ToString(), out value))
                return value;
            else
                return defaultValue;
        }

        public static double? ToDoubleOrDefault(this object obj)
        {
            if (obj == null)
                return null;

            if (obj is double)
                return (double)obj;

            double value = 0;
            if (double.TryParse(obj.ToString(), out value))
                return value;
            else
                return null;
        }

        public static double ToDoubleOrDefault(this object obj, double defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (obj is double)
                return (double)obj;

            double value = 0;
            if (double.TryParse(obj.ToString(), out value))
                return value;
            else
                return defaultValue;
        }

        public static string ToStringOrDefault(this object obj, string defaultValue)
        {
            if (obj == null)
                return defaultValue;

            return obj.ToString();
        }

        public static string ToStringOrDefault(this object obj)
        {
            if (obj == null)
                return null;

            return obj.ToString();
        }

        public static DateTime ToToDateTimeFromUnixTimeStamp(this object unixTimeStamp)
        {
            if (unixTimeStamp is DateTime)
                return (DateTime)unixTimeStamp;

            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp.ToDoubleOrDefault(0)).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime ToToDateTimeFromUnixTimeStampInMiliSeconds(this object unixTimeStamp)
        {
            if (unixTimeStamp is DateTime)
                return (DateTime)unixTimeStamp;

            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp.ToDoubleOrDefault(0)).ToLocalTime();
            return dtDateTime;
        }

        public static byte[] ToByteArray(this object obj)
        {
            if (obj is byte[] byteArray)
                return byteArray;
            return Encoding.ASCII.GetBytes(obj.ToStringOrDefault());
        }

        public static string ToJsonOrString(this object obj)
        {
            if (obj == null)
                return null;

            string text = obj.ToStringOrDefault();
            if (obj is IDictionary<string, object> || obj is object[])
            {
                text = obj.ToJsonOrDefault();
            }

            return text;
        }

        public static string ToJsonOrDefault(this object obj)
        {
            if (obj == null)
                return null;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj,
                new BsonObjectIdConverter(),
                new BsonISODateConverter(),
                new DecimalJsonConverter(),
                new TypesJsonConverter());
        }

        public static BsonDocument ToBsonDocument(this object value)
        {
            if (value == null)
                return new BsonDocument();

            BsonDocument bsonDocument = null;
            if (value is string)
            {
                bsonDocument = BsonSerializer.Deserialize<BsonDocument>((string)value);
            }
            else
            {
                string bson = value.ToJsonOrDefault();
                bsonDocument = BsonSerializer.Deserialize<BsonDocument>(bson);
            }

            return bsonDocument;
        }

        public static bool HasProperty(this object obj, string key)
        {
            if (obj == null)
                return false;

            var dictionary = obj as IDictionary<string, object>;
            if (dictionary != null)
            {
                return dictionary.Any(x => x.Key.Equals(key));
            }
            return false;
        }

        public static object GetProperty(this object obj, string key)
        {
            if (obj == null)
                return null;

            var dictionary = obj as IDictionary<string, object>;
            if (dictionary != null)
            {
                if (dictionary.Any(x => x.Key.Equals(key)))
                    return dictionary.First(x => x.Key.Equals(key)).Value;
            }
            return null;
        }

        public static void SetProperty(this object obj, string key, object value)
        {
            if (obj == null)
                return;

            var dictionary = obj as IDictionary<string, object>;
            if (dictionary != null)
            {
                dictionary[key] = value;
            }
        }

        public static object ToJsObject(this object obj)
        {
            if (obj is null)
            {
                return null;
            }
            if (obj is JsObject)
            {
                return obj;
            }
            if (obj is IDictionary<string, object> dic)
            {
                return new JsObject(dic);
            }
            if (obj is IDictionary dic2)
            {
                return new JsObject(
                    dic2
                        .Cast<DictionaryEntry>()
                        .ToDictionary(x => x.Key.ToString(), x => x.Value));
            }
            if (obj is byte[])
            {
                return new String(
                    ((byte[])obj)
                    .Select(x => (char)x)
                    .ToArray());
            }
            if (obj is MongoDB.Bson.BsonDocument bson)
            {
                return new JsObject(bson.ToDictionary());
            }
            if (obj is RedisValue redisValue)
            {
                if (redisValue.IsInteger)
                    return (int)redisValue;
                if (redisValue.HasValue == false)
                    return null;
                if (redisValue.IsNull)
                    return null;
                if (redisValue.IsNullOrEmpty)
                    return null;
                

                return redisValue.ToStringOrDefault();
            }

            return obj;
        }

        //public static IDictionary<string, object> ToJsObject(this IDictionary<string, object> dictionary)
        //{
        //    return new JsObject(dictionary);
        //}

        public static object MapDotNetType(this object obj)
        {
            return obj is Types.ValueType ? ((Types.ValueType) obj).GetValue() : obj;
        }

        public static object[] MapDotNetTypes(this object[] array)
        {
            return array.Select(x => x.MapDotNetType()).ToArray();
        }
    }
}