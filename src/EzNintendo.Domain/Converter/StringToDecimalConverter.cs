using System;
using System.Globalization;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Converter
{
    internal class StringToDecimalConverter : JsonConverter
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

            var deserializedNumber = serializer.Deserialize<string>(reader);
            var number = decimal.Parse(deserializedNumber, NumberStyles.Currency, CultureInfo.InvariantCulture);

            return number;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}