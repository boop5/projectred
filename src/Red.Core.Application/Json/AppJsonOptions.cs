using System.Text.Json;

namespace Red.Core.Application.Json
{
    public static class AppJsonOptions
    {
        public static JsonSerializerOptions Default { get; } = new()
        {
            AllowTrailingCommas = false,
            WriteIndented = false,
            ReadCommentHandling = JsonCommentHandling.Skip,
            Converters =
            {
                new EnumToStringJsonConverter(),
                new CountryDictionaryJsonConverter()
            }
        };
    }
}