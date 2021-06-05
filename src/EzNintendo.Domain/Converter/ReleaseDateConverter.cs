using System;
using System.Linq;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Converter
{
    internal class ReleaseDateConverter : JsonConverter
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

            var dates = serializer.Deserialize<DateTime[]>(reader);

            if (dates.Length < 1)
            {
                return DateTime.MinValue;
            }

            var min = dates.Min(); // returns the first ReleaseDate

            return min;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}