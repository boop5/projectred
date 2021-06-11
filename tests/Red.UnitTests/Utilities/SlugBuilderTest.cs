using System.Diagnostics.CodeAnalysis;
using Red.Infrastructure.Utilities;
using Xunit;

namespace Red.UnitTests.Utilities
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class SlugBuilderTest
    {
        [Fact]
        public void TestBuilder()
        {
            var sut = new SlugBuilder();

            Assert.Equal("doom-4-classic", sut.Build("DOOM IV (Classic)"));
            Assert.Equal(
                "monster-energy-supercross-the-official-videogame",
                sut.Build("Monster Energy Supercross - The Official Videogame"));
            Assert.Equal("foo-bar", sut.Build("foo bar"));
            Assert.Equal("foo-bar", sut.Build("fôo% bar"));
            Assert.Equal("foo-bar", sut.Build("fóo bar"));
            Assert.Equal("foo-bar", sut.Build("fòo bar"));
            Assert.Equal("foo-bar", sut.Build("foo bar"));
            Assert.Equal("foo-bar", sut.Build("foo    -   bar"));
            Assert.Equal("foo-bar", sut.Build("foo       bar"));
            Assert.Equal("foo-bar", sut.Build("foo: bar"));
            Assert.Equal("foo-bar", sut.Build("--- foo --- bar ---"));
            Assert.Equal("foo-bar", sut.Build("foo:bar"));
            Assert.Equal("foo-bar", sut.Build("Foo Bar"));
            Assert.Equal("foo-bar", sut.Build("Foo Bar!"));
            Assert.Equal("foo-bar", sut.Build("Foo: Bar"));
            Assert.Equal("foo-bar", sut.Build("Foo-Bar"));
            Assert.Equal("foo-bar", sut.Build("Foo Bar   "));
            Assert.Equal("foo-bar", sut.Build("   Foo Bar   "));
            Assert.Equal("foo-bar", sut.Build("   Foo Bar"));
            Assert.Equal("foo-bar", sut.Build("Foo Bar ™℠®©"));
            Assert.Equal("foo-bar", sut.Build("Fôô Bar"));
            Assert.Equal("foo-bar-123", sut.Build("Foo Bar 123"));
            Assert.Equal(
                "assassins-creed-the-rebel-collection",
                sut.Build("Assassin’s Creed: The Rebel Collection"));
            Assert.Equal("doom-2-classic", sut.Build("DOOM II (Classic)"));
            Assert.Equal("foo-bar-2", sut.Build("Foo Bar II"));
            Assert.Equal("foo-bar-2", sut.Build(" Foo Bar II"));
            Assert.Equal("foo-bar-3", sut.Build("Foo Bar III"));
            Assert.Equal("foo-bar-4", sut.Build("Foo Bar IV"));
            Assert.Equal("foo-bar-5", sut.Build("Foo Bar V"));
            Assert.Equal("foo-bar-6", sut.Build("Foo Bar VI"));
            Assert.Equal("foo-bar-7", sut.Build("Foo Bar VII"));
            Assert.Equal("foo-bar-8", sut.Build("Foo Bar VIII"));
            Assert.Equal("foo-bar-9", sut.Build("Foo Bar IX"));
            Assert.Equal("foo-bar-10", sut.Build("Foo Bar X"));
            Assert.Equal("foo-bar-11", sut.Build("Foo Bar XI"));
            Assert.Equal("foo-bar-12", sut.Build("Foo Bar XII"));
            Assert.Equal("foo-bar-13", sut.Build("Foo Bar XIII"));
            Assert.Equal("foo-bar-14", sut.Build("Foo Bar XIV"));
            Assert.Equal("foo-bar-15", sut.Build("Foo Bar XV"));
            Assert.Equal("foo-bar-16", sut.Build("Foo Bar XVI"));
            Assert.Equal("foo-bar-17", sut.Build("Foo Bar XVII"));
            Assert.Equal("foo-bar-18", sut.Build("Foo Bar XVIII"));
            Assert.Equal("foo-bar-19", sut.Build("Foo Bar XIX"));
            Assert.Equal("1234", sut.Build("1234"));
            Assert.Equal("1234", sut.Build("   1234   "));
            Assert.Equal("1234", sut.Build("- - - 1234 - - -"));

            Assert.Null(sut.Build("        "));
            Assert.Null(sut.Build(""));
            Assert.Null(sut.Build(null));
            Assert.Null(sut.Build("------"));
            Assert.Null(sut.Build("*****"));
            Assert.Null(sut.Build("+++++"));
            Assert.Null(sut.Build("______"));
            Assert.Null(sut.Build("______"));
        }
    }
}