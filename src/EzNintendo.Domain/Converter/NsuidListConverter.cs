using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EzNintendo.Domain.Nintendo;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Converter
{
    public sealed class NsuidListConverter : JsonConverter
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

            var codes = serializer.Deserialize<List<string>>(reader);

            return codes.Any() 
                       ? (NsuId) long.Parse(codes[0], NumberStyles.Integer, CultureInfo.InvariantCulture) 
                       : default;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}