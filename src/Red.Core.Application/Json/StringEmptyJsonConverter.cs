using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Core.Application.Json
{
    public sealed class StringEmptyJsonConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert != typeof(string))
            {
                throw new ArgumentException("Invalid type");
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();

                return stringValue ?? string.Empty;
            }        
            else if (reader.TokenType == JsonTokenType.Null)
            {
                throw new ArgumentNullException();
            }

            throw new Exception("Unsupported TokenType");
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}