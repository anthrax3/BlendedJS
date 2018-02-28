using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MongoDB.Bson.Serialization;
using BlendedJS.Mongo;

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

            int value = 0;
            if (int.TryParse(obj.ToString(), out value))
                return value;
            else
                return defaultValue;
        }

        public static bool? ToBoolOrDefault(this object obj)
        {
            if (obj == null)
                return null;

            if (obj is bool)
                return (bool)obj;

            bool value = false;
            if (bool.TryParse(obj.ToString(), out value))
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

            bool value = false;
            if (bool.TryParse(obj.ToString(), out value))
                return value;
            else
                return defaultValue;
        }

        public static DateTime? ToDateOrDefault(this object obj, DateTime? defaultValue)
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

        public static string ToJsonOrString(this object obj)
        {
            if (obj == null)
                return null;

            string text = obj.ToStringOrDefault();
            if (obj is IDictionary<string, object>)
                text = JsonConvert.SerializeObject(obj);
            if (obj is object[])
                text = JsonConvert.SerializeObject(obj);
            return text;
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
                string bson = Newtonsoft.Json.JsonConvert.SerializeObject(value, new BsonObjectIdConverter(), new BsonISODateConverter());
                bsonDocument = BsonSerializer.Deserialize<BsonDocument>(bson);
            }

            return bsonDocument;
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
    }
}