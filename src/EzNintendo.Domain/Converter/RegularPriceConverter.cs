using System;
using System.Globalization;
using EzNintendo.Domain.Nintendo;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Converter
{
    internal class RegularPriceConverter : JsonConverter
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

            var regularPrice = serializer.Deserialize<RegularPrice>(reader);

            if (string.IsNullOrWhiteSpace(regularPrice.RawValue))
            {
                return null;
            }

            var price = float.Parse(regularPrice.RawValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            return price;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}