using System.Collections.Generic;
using System.Text.Json;
using Red.Core.Application.Json;
using Red.Core.Domain.Models;
using Xunit;

namespace Red.UnitTests.Application.JsonConverter
{
    public class EnumToStringConverterTests
    {
        [Fact]
        public void Tests()
        {
            var options = new JsonSerializerOptions {Converters = {new EnumToStringJsonConverter()}};

            var testCases = new List<KeyValuePair<EshopSalesStatus, string>>
            {
                new(EshopSalesStatus.PreOrder, "PreOrder"),
                new(EshopSalesStatus.SalesTermination, "SalesTermination"),
                new(EshopSalesStatus.NotFound, "NotFound"),
                new(EshopSalesStatus.Unreleased, "Unreleased"),
                new(EshopSalesStatus.OnSale, "OnSale"),
                new(EshopSalesStatus.Unknown, "Unknown"),
            };

            foreach (var (input, expectation) in testCases)
            {
                var serialized = JsonSerializer.Serialize(input, options);
                var deserialized = JsonSerializer.Deserialize<EshopSalesStatus>(serialized, options);
                
                Assert.NotNull(serialized);
                Assert.NotNull(deserialized);
                Assert.Equal($"\"{expectation}\"", serialized);
                Assert.Equal(input, deserialized);
            }
        }
    }
}