using System;
using System.Globalization;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Converter
{
    internal class StringToByteConverter : JsonConverter
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

            var s = serializer.Deserialize<string>(reader);
            var number = byte.Parse(s, NumberStyles.Integer, CultureInfo.InstalledUICulture);

            return number;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }

    internal class StringToIntConverter : JsonConverter
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

            var s = serializer.Deserialize<string>(reader);
            var number = int.Parse(s, NumberStyles.Integer, CultureInfo.InstalledUICulture);

            return number;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}