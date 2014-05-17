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
            "Alpha".ToMd5Hash().Should().Be("6132295fcf5570fb8b0a944ef322a598");
        }
    }
}