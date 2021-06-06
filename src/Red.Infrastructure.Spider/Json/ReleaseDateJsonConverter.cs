using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.Spider.Json
{
    internal sealed class ReleaseDateJsonConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var list = new List<DateTime>();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Not an array");
            }

            reader.Read(); // skip startArray

            while (reader.TokenType is not JsonTokenType.EndArray)
            {
                if (reader.TryGetDateTime(out var date))
                {
                    list.Add(date);
                }

                reader.Read();
            }

            if (list.Any())
            {
                return list.Min(); // returns the first ReleaseDate
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? date, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            if (date.HasValue)
            {
                writer.WriteStringValue(date.Value);
            }

            writer.WriteEndArray();
        }
    }
}