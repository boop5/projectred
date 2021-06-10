using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.NintendoApi.Json
{
    internal sealed class CsvArrayJsonConverter : JsonConverter<List<string>?>
    {
        public override List<string>? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var list = new List<string>();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Not an array");
            }

            reader.Read(); // skip startArray

            while (reader.TokenType is not JsonTokenType.EndArray)
            {
                if (reader.TokenType is JsonTokenType.String)
                {
                    var value = reader.GetString();

                    // nintendo puts languages in as csv in an array [ "a, b, c" ]
                    var languages = value?.Split(",", StringSplitOptions.RemoveEmptyEntries);

                    if (languages is not null)
                    {
                        list.AddRange(languages);
                    }
                }

                reader.Read();
            }

            if (list.Any())
            {
                return list;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, List<string>? value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            if (value != null)
            {
                var languages = value.Where(x => !string.IsNullOrWhiteSpace(x));

                foreach (var language in languages)
                {
                    writer.WriteStringValue(language);
                }
            }

            writer.WriteEndArray();
        }
    }
}