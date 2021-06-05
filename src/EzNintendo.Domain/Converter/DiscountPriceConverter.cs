using System;
using System.Globalization;
using EzNintendo.Domain.Nintendo;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Converter
{
    internal class DiscountPriceConverter : JsonConverter
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

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var discountPrice = serializer.Deserialize<DiscountPrice>(reader);
            var price = float.Parse(discountPrice.RawValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            return price;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}