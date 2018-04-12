using System;
using Newtonsoft.Json;

namespace BlendedJS.Types
{
    public class TypesJsonConverter : JsonConverter
    {
        public TypesJsonConverter()
        {
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(BlendedJS.Types.ValueType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            BlendedJS.Types.ValueType valueType = (BlendedJS.Types.ValueType)value;
            writer.WriteRawValue(JsonConvert.ToString(valueType.GetValue()));
        }
    }
}