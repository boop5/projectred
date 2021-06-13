using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.NintendoApi.Models
{
    internal abstract class ExtensionsObject
    {
        [JsonExtensionData]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Dictionary<string, JsonElement> _extensionData_ { get; init; } = new();
    }
}