using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Core.Application.Json
{
    public sealed class NullableIntJsonConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                var stringValue = reader.GetString();

                if (int.TryParse(stringValue, out var n))
                {
                    return n;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }
            else if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            throw new ArgumentException("Invalid type");
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString(CultureInfo.InvariantCulture));
        }
    }
}