using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Converter
{
    internal class ListToStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var list = serializer.Deserialize<List<object>>(reader);
            var txt = string.Join("|", list);

            return txt;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}