using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using Xunit;

namespace ModernRonin.PraeterArtem.UnitTests.Functional
{
	public sealed class NullTests
    {
        [Fact]
        public void Enumerable_Is_Empty()
        {
            Null.Enumerable<int>().Should().BeEmpty();
        }
        [Fact]
        public void Action_Does_Nothing()
        {
            // actually, I don't see any way to prove that the following call does nothing ;)
            Null.Action()();
        }
        [Fact]
        public void Action_With_Argument_Does_Nothing()
        {
            // actually, I don't see any way to prove that the following call does nothing ;)
            Null.Action<int>()(3);
        }
    }
}