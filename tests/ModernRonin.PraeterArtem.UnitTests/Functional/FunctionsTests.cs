using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using NUnit.Framework;

namespace ModernRonin.PraeterArtem.UnitTests.Functional
{
    [TestFixture]
	public sealed class FunctionsTests
    {
        [Test]
        public void Identity_Is_A_Unary_Function_Which_Returns_Its_Argument()
        {
            Functions.Identity<int>()(13).Should().Be(13);
        }
    }
}