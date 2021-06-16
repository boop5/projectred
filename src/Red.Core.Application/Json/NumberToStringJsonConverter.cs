using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Core.Application.Json
{
    public sealed class NumberToStringJsonConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt16(out var n16))
                {
                    return n16.ToString(CultureInfo.InvariantCulture);
                }

                if (reader.TryGetInt32(out var n32))
                {
                    return n32.ToString(CultureInfo.InvariantCulture);
                }

                if (reader.TryGetInt64(out var n64))
                {
                    return n64.ToString(CultureInfo.InvariantCulture);
                }

                if (reader.TryGetDouble(out var d))
                {
                    return d.ToString(CultureInfo.InvariantCulture);
                }

                if (reader.TryGetDecimal(out var dec))
                {
                    return dec.ToString(CultureInfo.InvariantCulture);
                }
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();

                if (short.TryParse(s, out var n16))
                {
                    return n16.ToString(CultureInfo.InvariantCulture);
                }

                if (int.TryParse(s, out var n32))
                {
                    return n32.ToString(CultureInfo.InvariantCulture);
                }

                if (long.TryParse(s, out var n64))
                {
                    return n64.ToString(CultureInfo.InvariantCulture);
                }

                if (double.TryParse(s, out var d))
                {
                    return d.ToString(CultureInfo.InvariantCulture);
                }

                if (decimal.TryParse(s, out var dec))
                {
                    return dec.ToString(CultureInfo.InvariantCulture);
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (int.TryParse(value, out var n))
            {
                writer.WriteNumberValue(n);
            }
        }
    }
}
