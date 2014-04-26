using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using NUnit.Framework;

namespace ModernRonin.PraeterArtem.UnitTests.Functional
{
    [TestFixture]
    public class ComparableExtensionsTests
    {
        [Test]
        [TestCase(1.2f, 0.3f, 300f, true)]
        [TestCase(0.3f, 0.3f, 300f, true)]
        [TestCase(300f, 0.3f, 300f, true)]
        [TestCase(0.3f, 0.4f, 300f, false)]
        [TestCase(400f, 0.4f, 300f, false)]
        public void IsInClosedInterval(float input, float lower, float upper,
                                       bool expectedResult)
        {
            input.IsInClosedInterval(lower, upper)
                 .Should()
                 .Be(expectedResult);
        }
        [Test]
        [TestCase(1.2f, 0.3f, 300f, true)]
        [TestCase(0.3f, 0.3f, 300f, false)]
        [TestCase(300f, 0.3f, 300f, false)]
        [TestCase(0.3f, 0.4f, 300f, false)]
        [TestCase(400f, 0.4f, 300f, false)]
        public void IsInOpenInterval(float input, float lower, float upper,
                                     bool expectedResult)
        {
            input.IsInOpenInterval(lower, upper).Should().Be(expectedResult);
        }
    }
}