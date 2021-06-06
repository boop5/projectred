using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Core.Application.Json
{
    public sealed class NullableLongJsonConverter : JsonConverter<long?>
    {
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                var stringValue = reader.GetString();

                if (long.TryParse(stringValue, out var n))
                {
                    return n;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64();
            }
            else if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            throw new ArgumentException("Invalid type");
        }

        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString(CultureInfo.InvariantCulture));
        }
    }
}