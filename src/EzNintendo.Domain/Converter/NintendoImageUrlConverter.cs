using System;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Converter
{
    internal sealed class NintendoImageUrlConverter : JsonConverter
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

            var url = serializer.Deserialize<string>(reader);
            
            return url.StartsWith("//") 
                       ? $"https:{url}" 
                       : url;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}