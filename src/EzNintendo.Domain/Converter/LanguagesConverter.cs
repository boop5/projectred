using System;
using System.Linq;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Converter
{
    internal class LanguagesConverter : JsonConverter
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

            var list = serializer.Deserialize<string[]>(reader);

            if (list.Any())
            {
                var languages = list.First().Split(",").Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                return languages;
            }

            return Enumerable.Empty<string>().ToList();
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}