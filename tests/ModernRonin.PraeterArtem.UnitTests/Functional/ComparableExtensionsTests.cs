using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using Xunit.Extensions;

namespace ModernRonin.PraeterArtem.UnitTests.Functional
{
    public sealed class ComparableExtensionsTests
    {
		[Theory]
        [InlineData(1.2f, 0.3f, 300f, true)]
		[InlineData(0.3f, 0.3f, 300f, true)]
		[InlineData(300f, 0.3f, 300f, true)]
		[InlineData(0.3f, 0.4f, 300f, false)]
		[InlineData(400f, 0.4f, 300f, false)]
        public void IsInClosedInterval(float input, float lower, float upper,
                                       bool expectedResult)
        {
            input.IsInClosedInterval(lower, upper)
                 .Should()
                 .Be(expectedResult);
        }
		[Theory]
		[InlineData(1.2f, 0.3f, 300f, true)]
		[InlineData(0.3f, 0.3f, 300f, false)]
		[InlineData(300f, 0.3f, 300f, false)]
		[InlineData(0.3f, 0.4f, 300f, false)]
		[InlineData(400f, 0.4f, 300f, false)]
        public void IsInOpenInterval(float input, float lower, float upper,
                                     bool expectedResult)
        {
            input.IsInOpenInterval(lower, upper).Should().Be(expectedResult);
        }
    }
}