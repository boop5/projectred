using System.Collections.Generic;
using Red.Core.Domain.Models;
using Xunit;

namespace Red.UnitTests.Domain
{
    public class CountryDictionaryTests
    {
        [Fact]
        public void AccessingUnknownKey_ReturnsNull()
        {
            var dict = new CountryDictionary<SimpleDto>();
            Assert.Null(dict["foo"]);
            Assert.Null(dict["bar"]);
            Assert.Null(dict["lorem ipsum"]);
            Assert.Null(dict["2142384"]);
        }

        [Fact]
        public void KeyEmptyOrNull_ReturnsDefault()
        {
            var dict = new CountryDictionary<SimpleEnum>();
            Assert.Equal(default, dict[null!]);
            Assert.Equal(default, dict[""]);
            Assert.Equal(default, dict["         "]);

            var dictB = new CountryDictionary<int>();
            Assert.Equal(default, dictB[null!]);
            Assert.Equal(default, dictB[""]);
            Assert.Equal(default, dictB["         "]);

            var dictC = new CountryDictionary<string>();
            Assert.Equal(default, dictC[null!]);
            Assert.Equal(default, dictC[""]);
            Assert.Equal(default, dictC["         "]);
        }

        [Fact]
        public void KeyIsCaseInsensitive()
        {
            var fooValue = "Lorem Ipsum 123";
            var dict = new CountryDictionary<SimpleDto>
            {
                ["DE"] = new() {Foo = fooValue}
            };

            Assert.NotNull(dict["DE"]);
            Assert.NotNull(dict["De"]);
            Assert.NotNull(dict["dE"]);
            Assert.NotNull(dict["de"]);

            Assert.Equal(dict["DE"].Foo, fooValue);
            Assert.Equal(dict["De"].Foo, fooValue);
            Assert.Equal(dict["dE"].Foo, fooValue);
            Assert.Equal(dict["de"].Foo, fooValue);
        }

        [Fact]
        public void MergeAddsMissingKeys()
        {
            var keyA = "key1";
            var keyB = "key2";
            var valueA = "lorem";
            var valueB = "ipsum";
            var dictA = new CountryDictionary<SimpleDto>
            {
                [keyA] = new() {Foo = valueA}
            };
            var dictB = new CountryDictionary<SimpleDto>
            {
                [keyB] = new() {Foo = valueB}
            };

            var dictC = dictA.Merge(dictB);

            Assert.NotNull(dictC[keyB]);
            Assert.Equal(dictC[keyB].Foo, valueB);
        }

        [Fact]
        public void MergeDoesNotRemoveKeys()
        {
            var keyA = "key1";
            var keyB = "key2";
            var valueA = "lorem";
            var valueB = "ipsum";
            var dictA = new CountryDictionary<SimpleDto>
            {
                [keyA] = new() {Foo = valueA}
            };
            var dictB = new CountryDictionary<SimpleDto>
            {
                [keyB] = new() {Foo = valueB}
            };

            var dictC = dictA.Merge(dictB);

            Assert.NotNull(dictC[keyA]);
            Assert.Equal(dictC[keyA].Foo, valueA);
        }

        [Fact]
        public void MergeOverridesValueIfDifferent()
        {
            var dictA = new CountryDictionary<List<int>> {["a"] = new() {1, 2, 3}};
            var dictB = new CountryDictionary<List<int>> {["a"] = new() {4, 5, 6}};
            var dictC = dictA.Merge(dictB);

            Assert.Equal(3, dictC["a"]!.Count);
            Assert.Equal(4, dictC["a"]![0]);
            Assert.Equal(5, dictC["a"]![1]);
            Assert.Equal(6, dictC["a"]![2]);
        }

        [Fact]
        public void MergeReturnsNewInstance()
        {
            var dictA = new CountryDictionary<SimpleDto>();
            var dictB = new CountryDictionary<SimpleDto>();

            dictA["key"] = new SimpleDto {Foo = "Lorem Ipsum 123"};

            Assert.NotEqual(dictA, dictB);
            var dictC = dictA.Merge(dictB);
            Assert.Equal(dictA, dictC);
            Assert.NotSame(dictA, dictC);
            Assert.NotEqual(dictB, dictC);
        }

        [Fact]
        public void TestEquality()
        {
            Assert.True(new CountryDictionary<int> {["a"] = 1}.Equals(new CountryDictionary<int> {["a"] = 1}));
            Assert.False(new CountryDictionary<int> {["a"] = 1}.Equals(new CountryDictionary<int> {["a"] = 2}));
            Assert.False(new CountryDictionary<int> {["a"] = 1}.Equals(new CountryDictionary<int> {["b"] = 2}));
            Assert.False(new CountryDictionary<int> {["a"] = 1}.Equals(null));

            var dict = new CountryDictionary<int>();
            Assert.True(dict.Equals(dict));

            // hashcode

            var dictA = new CountryDictionary<int>(){["a"] = 1};
            var dictB = new CountryDictionary<int>(){["a"] = 1};

            Assert.NotEqual(dictA.GetHashCode(), dictB.GetHashCode());
        }

        private sealed class SimpleDto
        {
            public string Foo { get; init; }
        }

        private enum SimpleEnum
        {
            A = 4,
            B = 3,
            C = 2,
            D = 1,
            E = 0
        }
    }
}