using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Core.Application.Json
{
    public sealed class EnumToStringJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsEnum)
            {
                return true;
            }

            return false;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(EnumToStringJsonConverterInternal<>).MakeGenericType(typeToConvert);
            var converter = (JsonConverter?) Activator.CreateInstance(converterType);

            return converter;
        }

        #region Converter

        private sealed class EnumToStringJsonConverterInternal<T> : JsonConverter<T> where T : struct
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var s = reader.GetString();

                if (string.IsNullOrWhiteSpace(s))
                {
                    return default;
                }

                var parsed = Enum.Parse<T>(s);

                return parsed;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(Enum.GetName(typeof(T), value));
            }
        }

        #endregion
    }
}