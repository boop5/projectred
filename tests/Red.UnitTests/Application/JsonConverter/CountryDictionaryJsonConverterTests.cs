using System.Text.Json;
using Red.Core.Application.Json;
using Red.Core.Domain.Models;
using Xunit;

namespace Red.UnitTests.Application.JsonConverter
{
    public class CountryDictionaryJsonConverterTests
    {
        [Fact]
        public void Tests()
        {
            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new CountryDictionaryJsonConverter()
                }
            };
            var dict = new CountryDictionary<Price>

            {
                ["de"] = new() {Amount = 123.45f, Currency = "EUR"},
                ["us"] = new() {Amount = 234.56f, Currency = "USD"},
                ["mx"] = new() {Amount = 345.67f, Currency = "MXN"},
                [string.Empty] = new(),
                ["          "] = new(),
            };

            var serialized = JsonSerializer.Serialize(dict, options);
            var deserialized = JsonSerializer.Deserialize<CountryDictionary<Price>>(serialized, options);

            Assert.NotNull(deserialized);
            Assert.NotNull(deserialized["de"]);
            Assert.NotNull(deserialized["us"]);
            Assert.NotNull(deserialized["mx"]);
            Assert.Null(deserialized[string.Empty]);
            Assert.Null(deserialized["          "]);
            Assert.Null(deserialized["does-not-exist"]);
            Assert.Equal(123.45f, deserialized["de"]?.Amount);
            Assert.Equal("EUR", deserialized["de"]?.Currency);
            Assert.Equal(234.56f, deserialized["us"]?.Amount);
            Assert.Equal("USD", deserialized["us"]?.Currency);
            Assert.Equal(345.67f, deserialized["mx"]?.Amount);
            Assert.Equal("MXN", deserialized["mx"]?.Currency);
        }
    }
}