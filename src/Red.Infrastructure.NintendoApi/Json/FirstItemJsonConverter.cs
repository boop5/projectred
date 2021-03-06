using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.NintendoApi.Json
{
    internal sealed class FirstItemJsonConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                    
                    if (value != null)
                    {
                        list.Add(value);
                    }
                }

                reader.Read();
            }

            if (list.Any())
            {
                return list.First();
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            if (value != null)
            {
                writer.WriteStringValue(value);
            }

            writer.WriteEndArray();
        }
    }
}