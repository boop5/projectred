using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Core.Application.Json
{
    public sealed class EnumToStringJsonConverter<T> : JsonConverter<T> where T : struct
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();

            if (string.IsNullOrWhiteSpace(s))
            {
                return default;
            }

            return Enum.Parse<T>(s);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Enum.GetName(typeof(T), value));
        }
    }
}