using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests.Functional
{
	public sealed class FunctionsTests
    {
        [Fact]
        public void Identity_Is_A_Unary_Function_Which_Returns_Its_Argument()
        {
            Functions.Identity<int>()(13).Should().Be(13);
        }
    }
}