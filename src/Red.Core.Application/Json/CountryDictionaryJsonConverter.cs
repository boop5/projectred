using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Json
{
    public sealed class CountryDictionaryJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            if (typeToConvert.GetGenericTypeDefinition() != typeof(CountryDictionary<>))
            {
                return false;
            }

            return true;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var type = typeToConvert.GetGenericArguments()[0];

            if (type == null)
            {
                return null;
            }

            var converterType = typeof(CountryDictionaryJsonConverterInternal<>).MakeGenericType(type);
            var converter = (JsonConverter?) Activator.CreateInstance(converterType);

            return converter;
        }

        #region Converter

        private sealed class CountryDictionaryJsonConverterInternal<T> : JsonConverter<CountryDictionary<T>>
        {
            public override CountryDictionary<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                CountryDictionary<T> result = new();

                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        return result;
                    }

                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException();
                    }

                    var propertyName = reader.GetString();

                    if (string.IsNullOrWhiteSpace(propertyName))
                    {
                        throw new JsonException();
                    }

                    reader.Read();

                    var v = JsonSerializer.Deserialize<T>(ref reader, options);
                    result[propertyName] = v;
                }

                return default;
            }

            public override void Write(Utf8JsonWriter writer, CountryDictionary<T> value, JsonSerializerOptions options)
            {
                var dict = value.ToDictionary();
                JsonSerializer.Serialize(writer, dict, options);
            }
        }

        #endregion
    }
}