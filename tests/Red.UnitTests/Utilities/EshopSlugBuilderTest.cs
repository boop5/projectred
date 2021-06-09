using Red.Infrastructure.Utilities;
using Xunit;

namespace Red.UnitTests.Utilities
{
    public class EshopSlugBuilderTest
    {
        [Fact]
        public void TestBuilder()
        {
            var sut = new EshopSlugBuilder();

            Assert.Equal("dragon-blaze", sut.Build("Dragon Blaze for nintendo switch"));
            Assert.Equal("dragon-blaze", sut.Build("Dragon Blaze For Nintendo Switch"));
            Assert.Equal("dragon-blaze", sut.Build("  Dragon   Blaze   fOr   NiNteNdo   SWitCH  "));
        }
    }
}