using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.NintendoApi.Json
{
    internal sealed class SalesSearchScreenshotsJsonConverter : JsonConverter<List<string>>
    {
        public override List<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new List<string>();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
                reader.Read();
                if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
                reader.Read();
                if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
                reader.Read();
                if (reader.TokenType != JsonTokenType.String) throw new JsonException();
                var s = reader.GetString();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    result.Add(s);
                }
                reader.Read();
                if (reader.TokenType != JsonTokenType.EndObject) throw new JsonException();
                reader.Read();
                if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
                reader.Read();
            }

            //reader.Read();
            //if (reader.TokenType != JsonTokenType.EndObject) throw new JsonException();


            return result;
        }

        public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}