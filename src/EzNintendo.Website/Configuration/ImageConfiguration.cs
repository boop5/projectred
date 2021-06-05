using System.Diagnostics.CodeAnalysis;

namespace EzNintendo.Website.Configuration
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated by Runtime.")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Set by Runtime.")]
    public class ImageConfiguration
    {
        public string BasePath { get; set; }
        public byte Quality { get; set; }
    }
}