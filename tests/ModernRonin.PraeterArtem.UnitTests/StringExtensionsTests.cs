using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace ModernRonin.PraeterArtem.UnitTests
{
    public sealed class StringExtensionsTests
    {
        [Theory]
        [InlineData("x", 0, "")]
        [InlineData("x", 1, "x")]
        [InlineData("xA", 1, "xA")]
        [InlineData("xA", 3, "xAxAxA")]
        [InlineData("x", 5, "xxxxx")]
        [InlineData("abc", 5, "abcabcabcabcabc")]
        public void Repeat(string what, int count, string expectedResult)
        {
            what.Repeat(count).Should().Be(expectedResult);
        }
        [Fact]
        public void ToMd5Hash()
        {
            "Alpha".ToMd5Hash()
                   .Should()
                   .Be("6132295fcf5570fb8b0a944ef322a598");
        }
        [Theory]
        [InlineData("abcde", 0, "abcde")]
        [InlineData("abcde", 1, "bcde")]
        [InlineData("abcde", 2, "cde")]
        [InlineData("abcde", 3, "de")]
        [InlineData("abcde", 4, "e")]
        public void From(string input, int from, string expected)
        {
            input.From(from).Should().Be(expected);
        }
        [Theory]
        [InlineData("abcde", 0, "")]
        [InlineData("abcde", 1, "a")]
        [InlineData("abcde", 2, "ab")]
        [InlineData("abcde", 3, "abc")]
        [InlineData("abcde", 4, "abcd")]
        [InlineData("abcde", 5, "abcde")]
        public void Until(string input, int until, string expected)
        {
            input.Until(until).Should().Be(expected);
        }
        [Theory]
        [InlineData("abcde", new[] {"a"}, "")]
        [InlineData("abcde", new[] {"b"}, "a")]
        [InlineData("abcde", new[] {"c"}, "ab")]
        [InlineData("abcde", new[] {"d"}, "abc")]
        [InlineData("abcde", new[] {"e"}, "abcd")]
        [InlineData("abcde", new[] {"cd"}, "ab")]
        [InlineData("abcde", new[] {"x"}, "abcde")]
        [InlineData("abcde", new[] {"x", "y", "d"}, "abc")]
        public void Before(string input, string[] patterns, string expected)
        {
            input.Before(patterns).Should().Be(expected);
        }
        [Theory]
        [InlineData("abcde", new[] {"a"}, "bcde")]
        [InlineData("abcde", new[] {"b"}, "cde")]
        [InlineData("abcde", new[] {"c"}, "de")]
        [InlineData("abcde", new[] {"d"}, "e")]
        [InlineData("abcde", new[] {"e"}, "")]
        [InlineData("abcde", new[] {"cd"}, "e")]
        [InlineData("abcde", new[] {"abcde"}, "")]
        [InlineData("abcde", new[] {"x"}, "")]
        [InlineData("abcde", new[] {"x", "y", "z", "b"}, "cde")]
        public void After(string input, string[] patterns, string expected)
        {
            input.After(patterns).Should().Be(expected);
        }
        [Theory]
        [InlineData("abcde", new[] {"a"}, 0)]
        [InlineData("abcde", new[] {"b"}, 1)]
        [InlineData("abcde", new[] {"c"}, 2)]
        [InlineData("abcde", new[] {"d"}, 3)]
        [InlineData("abcde", new[] {"e"}, 4)]
        [InlineData("abcde", new[] {"cd"}, 2)]
        [InlineData("abcde", new[] {"x"}, -1)]
        [InlineData("abcde", new[] {"x", "y", "d"}, 3)]
        public void StartIndexOfAny(string input, string[] patterns,
                                       int expected)
        {
            input.StartIndexOfAny(patterns).Should().Be(expected);
        }
        [Theory]
        [InlineData("abcde", new[] {"a"}, 1)]
        [InlineData("abcde", new[] {"b"}, 2)]
        [InlineData("abcde", new[] {"c"}, 3)]
        [InlineData("abcde", new[] {"d"}, 4)]
        [InlineData("abcde", new[] {"e"}, 5)]
        [InlineData("abcde", new[] {"cd"}, 4)]
        [InlineData("abcde", new[] {"abcde"}, 5)]
        [InlineData("abcde", new[] {"x"}, -1)]
        [InlineData("abcde", new[] {"x", "y", "z", "b"}, 2)]
        public void EndIndexOfAny(string input, string[] patterns,
                                     int expected)
        {
            input.EndIndexOfAny(patterns).Should().Be(expected);
        }
    }
}